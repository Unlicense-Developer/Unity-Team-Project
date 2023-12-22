using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine;

namespace Dungeon
{
    public class BattleManager : MonoBehaviour
    {
        public UIManager uiManager;
        private GameObject player;
        private GameObject currentEnemy;

        public GameObject weapon;
        public GameObject shield;

        public PlayerStatus playerStatus; // ���� ���� ���� �÷��̾�
        public PlayerBattleController playerBattleController;
        public EnemyStatus currentEnemyStatus; // ���� ���� ���� ��
        public EnemyBattleController currentEnemyBattleController; // ���� ���� ���� ���� EnemyController

        public CinemachineVirtualCamera battleCamera; // ������ ���� ī�޶� ����

        public enum BattleState { Start, EnemyTurn, PlayerTurn, Won, Lost }

        public BattleState state;
        public int turnCount = 0; // �� ī��Ʈ
        public int paturnCount = 0; // 0 == �⺻��, 1 == ����, 2 == ����
        public bool paturnSuccess = false;
        public float delayTime;
        public bool playerTurn = false;
        public bool enemyTurn = false;

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
            playerStatus = FindObjectOfType<PlayerStatus>();
            playerBattleController = FindObjectOfType<PlayerBattleController>();
        }

        void Start()
        {
            state = BattleState.Start;
            // �ʱ�ȭ ����...
        }

        void Update()
        {
            switch (state)
            {
                case BattleState.EnemyTurn:
                    // ���� ���� Ȯ��
                    CheckEndBattle();
                    // �� �� ó�� ����
                    enemyTurn = true;
                    currentEnemyBattleController.EnemyTurn();
                    break;

                case BattleState.PlayerTurn:
                    // �÷��̾� �� ó�� ����
                    //playerController.playerTurn();
                    break;

                    // ... ��Ÿ ���µ��� ó��
            }
        }

        // ���� ���� �޼ҵ�
        public void SetUpBattle(GameObject currentEnemy)
        {
            currentEnemyStatus = currentEnemy.GetComponent<EnemyStatus>();
            currentEnemyBattleController = currentEnemy.GetComponent<EnemyBattleController>();

            if (currentEnemyBattleController == null)
            {
                Debug.LogError("EnemyController not found on the current enemy");
                return;
            }

            // �� ���� UI �ִϸ��̼� ���
            uiManager.BattleStartEffect();

            // ���� BGM Ȱ��ȭ
            DungeonSoundManager.Instance.StartBattleMode();

            // �� �±� ����
            currentEnemy.tag = "EnemyInBattle";

            // �÷��̾�� ���� ���ֺ��� ��ġ ����
            OrientCharactersForBattle();

            // ī�޶� ��ȯ ���� (������ ī�޶�� ��ȯ)
            SwitchToBattleCamera();

            // �÷��̾� �غ� �ð� ���� (��: �� �ʰ��� ī��Ʈ�ٿ�)
            StartCoroutine(ProvidePreparationTime(currentEnemy));
        }

        private void OrientCharactersForBattle()
        {
            // "Player" �±׸� ���� ��ü�� ã�� player ������ �Ҵ�
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player object not found");
                return;
            }

            // "Enemy" �±׸� ���� ��ü�� ã�� currentEnemy ������ �Ҵ�
            currentEnemy = GameObject.FindWithTag("EnemyInBattle");
            if (currentEnemy == null)
            {
                Debug.LogError("Enemy object not found");
                return;
            }

            // �÷��̾� �ִϸ����� ���������� ��ȯ
            if (playerBattleController != null)
            {
                playerBattleController.SwitchToBattleAnimator();
            }

            // �÷��̾�� ���� ���θ� �ٶ󺸰� ����
            player.transform.LookAt(currentEnemy.transform.position);
            currentEnemy.transform.LookAt(player.transform.position);

            // �÷��̾�� �� ������ �Ÿ��� ����
            float desiredDistance = 3.0f; // ���ϴ� �Ÿ� ����
            Vector3 directionToEnemy = (currentEnemy.transform.position - player.transform.position).normalized;

            // �÷��̾��� ��ġ�� �����Լ� ���ϴ� �Ÿ���ŭ �������� ����
            player.transform.position = currentEnemy.transform.position - directionToEnemy * desiredDistance;
            // ���� ��ġ�� �÷��̾�Լ� ���ϴ� �Ÿ���ŭ �������� ����
            currentEnemy.transform.position = player.transform.position + directionToEnemy * desiredDistance;

            // ���� �ٽ� ����
            player.transform.LookAt(currentEnemy.transform.position);
            currentEnemy.transform.LookAt(player.transform.position);

            // �÷��̾� ��� Ȱ��ȭ
            weapon.SetActive(true);
            shield.SetActive(true);
            playerStatus.CalculateTotalAttackDamage();
            playerStatus.CalculateTotalBreakDamage();
        }

        private void SwitchToBattleCamera()
        {
            // ���� ī�޶��� Follow�� Look At ����
            if (battleCamera != null)
            {
                battleCamera.Follow = player.transform;
                battleCamera.LookAt = currentEnemy.transform;

                // ���� ī�޶� Ȱ��ȭ
                battleCamera.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Battle camera not assigned");
            }
        }

        private IEnumerator ProvidePreparationTime(GameObject currentEnemy)
        {
            // �غ� �ð� ī��Ʈ�ٿ�
            yield return new WaitForSeconds(4); // ����: 2�ʰ� ���

            // ���� UI Ȱ��ȭ
            uiManager.BattleStart();

            // �������� ���� ����
            StartBattleWithEnemy(currentEnemy);
        }

        public void StartBattleWithEnemy(GameObject currentEnemy)
        {
            if (currentEnemy != null)
            {
                state = BattleState.EnemyTurn;
                Debug.Log("BattleManager: ���� ����");
            }
        }

        public void ProcessPlayerReaction(EnemyBattleController.EnemyAttack enemyAttack, string playerAction)
        {
            if ((enemyAttack == EnemyBattleController.EnemyAttack.Smash && playerAction == "Evade") ||
                (enemyAttack == EnemyBattleController.EnemyAttack.Swipe && playerAction == "CounterAttack") ||
                (enemyAttack == EnemyBattleController.EnemyAttack.Stab && playerAction == "Defend"))
            {
                // �������� ����
                SuccessfulReaction();
            }
            else
            {
                // ������ ����
                FailedReaction();
            }
        }

        public void SuccessfulReaction()
        {
            paturnCount = 1;
            paturnSuccess = true;
            Debug.Log("BattleManager: ���� ���� ����");
            // ���� �и� ���� ���� �� ���
            int parryVariant = Random.Range(1, 4); // 1���� 3���� ���� ���� ����
            string parrySoundName = $"Parrying({parryVariant})";
            DungeonSoundManager.Instance.PlaySFX(parrySoundName);
            // ����ȭ üũ
            currentEnemyStatus.ReceiveBreak(playerStatus.TotalBreakDamage);
            currentEnemyBattleController.CheckBreak();
        }

        public void FailedReaction()
        {
            paturnCount = 2;
            Debug.Log("BattleManager: ���� ���� ����");
            playerStatus.ReceiveDamage(currentEnemyStatus.AttackDamage);
        }

        public IEnumerator EndPlayerTurnRoutine()
        {
            Debug.Log("BattleManager: �÷��̾� �� ���� ���");
            float delayTime = Random.Range(2.0f, 3.0f);
            yield return new WaitForSeconds(delayTime);

            // �÷��̾��� ���� ���� �� ȣ��Ǵ� �޼ҵ�
            turnCount++; // �÷��̾� �� ī��Ʈ ����

            // ���� �� ������ ���� ����
            state = BattleState.EnemyTurn;
            Debug.Log("BattleManager: ���� �� ����");
        }

        // ���� ���� Ȯ�� �� ����
        public void CheckEndBattle()
        {
            if (playerStatus.CurrentHealth <= 0 || (currentEnemyStatus != null && currentEnemyStatus.CurrentHealth <= 0))
            {
                EndBattle();
            }
        }

        // ���� ���� ó��
        private void EndBattle()
        {
            if (playerStatus.CurrentHealth <= 0)
            {
                state = BattleState.Lost;
                DungeonSoundManager.Instance.PlaySFX("GameOver");
            }
            else
            {
                state = BattleState.Won;
            }

            // ��Ʋ UI ��Ȱ��ȭ
            uiManager.BattleOver();
            // ��Ʋ BGM ��Ȱ��ȭ
            DungeonSoundManager.Instance.EndBattleMode();
            // ���� ī�޶� ��Ȱ��ȭ
            Invoke("CameraReset", 2.0f);

            if (playerStatus.isDeath == false)
            {
                // ���� �ʱ�ȭ ����
                Invoke("ResetForNextBattle", 0.1f);

                if (currentEnemy != null)
                {
                    currentEnemy.gameObject.tag = "Enemy"; // �±� ����
                    currentEnemy.gameObject.SetActive(false); // �� ��Ȱ��ȭ
                }

                // �÷��̾� �ִϸ����� ������ȭ
                if (playerBattleController != null)
                {
                    playerBattleController.SwitchToOriginalAnimator();
                }
            }
        }

        // ���� ������ ���� �ʱ�ȭ �޼ҵ�
        private void ResetForNextBattle()
        {
            // ����, �� ī��Ʈ, ���� ī��Ʈ ���� �ʱ�ȭ�մϴ�.
            state = BattleState.Start;
            turnCount = 0;
            paturnCount = 0;
            playerTurn = false;
            enemyTurn = false;

            // �÷��̾�� ���� ���¸� �ʱ�ȭ�մϴ�. (��: ü��, ����ȭ ���� ��)
            // �÷��̾� ��� ��Ȱ��ȭ
            weapon.SetActive(false);
            shield.SetActive(false);
            // playerStatus.ResetStatusForNewBattle();
            // ���� ������ ���� �� ���� �Ǵ� ���� ������ �ʿ��� ��� ���⿡ �߰��մϴ�.

            Debug.Log("���� �ý��� �ʱ�ȭ �Ϸ�. ���� ���� �غ� �Ϸ�.");
        }

        void CameraReset()
        {
            battleCamera.gameObject.SetActive(false);
        }
    }
}
