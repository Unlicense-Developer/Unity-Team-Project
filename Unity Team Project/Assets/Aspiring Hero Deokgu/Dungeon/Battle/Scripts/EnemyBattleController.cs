using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

namespace DungeonBattle
{
    public class EnemyBattleController : MonoBehaviour
    {
        private int consecutivePlayerHits = 0;
        private bool specialAttackPerformed = false; // 특수 공격 실행 여부를 추적
        public bool isBreak = false;

        private ParticleSystem hittedEffectParticleSystem;
        private ParticleSystem auraEffectParticleSystem; // 오라 이펙트 파티클 시스템

        public enum EnemyAttack { Smash, Swipe, Stab }

        private Animator animator;
        private Renderer[] childRenderers;

        private BattleManager battleManager;
        private DamageScript damageScript;
        private EnemyStatus enemy;
        private PlayerStatus player;
        private PlayerBattleController playerBattleController;
        private List<string> playerRecentActions = new List<string>();

        private void Awake()
        {
            animator = GetComponent<Animator>();
            childRenderers = GetComponentsInChildren<Renderer>();
            battleManager = FindObjectOfType<BattleManager>();
            damageScript = FindObjectOfType<DamageScript>();
            enemy = GetComponent<EnemyStatus>();

            Transform hittedEffectTransform = transform.Find("HittedEffect");
            if (hittedEffectTransform != null)
            {
                GameObject hittedEffect = hittedEffectTransform.gameObject;
                hittedEffectParticleSystem = hittedEffect.GetComponent<ParticleSystem>();
            }

            Transform auraEffectTransform = transform.Find("AuraEffect");
            if (auraEffectTransform != null)
            {
                GameObject auraEffect = auraEffectTransform.gameObject;
                auraEffectParticleSystem = auraEffect.GetComponent<ParticleSystem>();
            }
        }

        void Start()
        {
            playerBattleController = FindObjectOfType<PlayerBattleController>();
            player = FindObjectOfType<PlayerStatus>();
        }

        public void EnemyTurn()
        {
            if (battleManager.enemyTurn && battleManager.playerStatus.isDeath == false)
            {
                // 첫 번째 특수 공격 조건 검사: 체력 30% 이하 또는 turnCount 10 이상
                if (!specialAttackPerformed && (enemy.Health <= 30 || battleManager.turnCount >= 10))
                {
                    PerformSpecialAttack();
                    specialAttackPerformed = true; // 특수 공격 실행 표시
                    Debug.Log("EnemyController: 특수 공격 활성화");
                }
                // 이후 턴에서는 30% 확률로 특수 공격
                // else if (specialAttackPerformed && Random.Range(0, 100) < 30)
                // {
                //     PerformSpecialAttack();
                // }
                else
                {
                    // 일반 공격 로직
                    EnemyAttack attack = ChooseAttack();

                    switch (attack)
                    {
                        case EnemyAttack.Smash:
                            PlayRandomAnimation("Smash", 2);
                            break;
                        case EnemyAttack.Swipe:
                            PlayRandomAnimation("Swipe", 3);
                            break;
                        case EnemyAttack.Stab:
                            PlayRandomAnimation("Stab", 2);
                            break;
                    }

                    playerBattleController.ReactToEnemyAttack(attack);
                }

                battleManager.enemyTurn = false;
                Debug.Log("EnemyController: 적 턴 종료");
                battleManager.playerTurn = true;

                // 플레이어 턴으로 전환
                battleManager.state = BattleManager.BattleState.PlayerTurn;
            }
        }

        private EnemyAttack ChooseAttack()
        {
            // 무작위성을 추가하기 위해 일정 확률로 무작위 공격을 선택
            if (Random.Range(0, 100) < 30) // 예: 30% 확률로 무작위 공격
            {
                return (EnemyAttack)Random.Range(0, 3);
            }

            return ChooseAttackBasedOnPlayerBehavior();
        }

        private EnemyAttack ChooseAttackBasedOnPlayerBehavior()
        {
            if (playerRecentActions.Count == 0)
            {
                return DefaultStrategy();
            }

            var mostCommonAction = AnalyzePlayerActions();

            switch (mostCommonAction)
            {
                case "CounterAttack":
                    return EnemyAttack.Smash;
                case "ShieldAttack":
                    return EnemyAttack.Stab;
                case "Guard":
                    return EnemyAttack.Swipe;
                default:
                    return DefaultStrategy();
            }
        }

        private EnemyAttack DefaultStrategy()
        {
            return (EnemyAttack)Random.Range(0, 3);
        }

        private void PerformSpecialAttack()
        {
            if (!gameObject.activeInHierarchy)
                return;

            // 오라 이펙트 활성화
            if (auraEffectParticleSystem != null)
            {
                auraEffectParticleSystem.Play();
            }
            else
            {
                Debug.LogError("Aura effect particle system not found!");
            }

            // 캐릭터 색상을 붉은색으로 변경
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
            }

            // 애니메이션 속도 증가
            animator.speed = 1.5f; // 예: 속도를 1.5배로 증가

            StartCoroutine(battleManager.EndPlayerTurnRoutine());
        }

        private void PlayRandomAnimation(string attackType, int variations)
        {
            int variant = Random.Range(1, variations + 1); // 1부터 variations까지의 랜덤 숫자
            string triggerName = attackType + variant.ToString(); // 예: "Smash1"
            animator.SetTrigger(triggerName);
            Debug.Log("EnemyController: " + triggerName + " 공격");
        }

        public void OnPlayerAttackSuccess()
        {
            consecutivePlayerHits++;
            if (consecutivePlayerHits >= 3) // 플레이어가 연속 3번 공격 성공 시
            {
                ActivateDefenseMode();
                consecutivePlayerHits = 0;
            }
        }

        public void AttackPlayer()
        {
            playerBattleController.PlayerHitted();
        }

        public void EnemyHitted()
        {
            // 오브젝트가 활성화되어 있지 않으면 아무것도 하지 않음
            if (!gameObject.activeInHierarchy)
                return;

            if (battleManager.paturnCount == 1 || isBreak == true)
            {
                Debug.Log("EnemyController: 적 피격!");

                // 피격 이펙트 활성화
                hittedEffectParticleSystem.Play();

                // 피격 데미지 적용
                battleManager.currentEnemyStatus.ReceiveDamage(battleManager.playerStatus.TotalAttackDamage);

                battleManager.paturnCount = 0;

                // 랜덤 피격 애니메이션 실행
                if (battleManager.currentEnemyStatus.isDeath == false)
                {
                    int hitVariant = Random.Range(1, 4); // 1부터 3까지 랜덤 숫자 생성
                    animator.SetTrigger($"Hitted{hitVariant}"); // 예: "Hitted1", "Hitted2", "Hitted3"

                    if (isBreak == true) animator.SetTrigger("Stunned");
                }

                // 플레이어 턴 종료 및 대기 시간 후 적 턴 시작
                if (isBreak == false) StartCoroutine(battleManager.EndPlayerTurnRoutine());
            }
        }

        public void CheckBreak()
        {
            if (battleManager.paturnCount == 1 && battleManager.currentEnemyStatus.CurrentBreakPoint <= 0)
            {
                Debug.Log("EnemyController: 적 무력화!");
                isBreak = true;
                animator.SetTrigger("Stunned");
                battleManager.uiManager.PressF.gameObject.SetActive(true);
                Invoke("ActivateDefenseMode", 5.0f);
            }
        }

        // 방어 모드 활성화 로직
        private void ActivateDefenseMode()
        {
            // 오브젝트가 활성화되어 있지 않으면 아무것도 하지 않음
            if (!gameObject.activeInHierarchy)
                return;

            isBreak = false;
            if (battleManager.currentEnemyStatus.isDeath == false)
            {
                animator.SetTrigger("ActivateDefense");
                battleManager.currentEnemyStatus.RestoreBreak();
                Debug.Log("EnemyController: 방어 모드 활성화");
            }
            battleManager.uiManager.PressF.gameObject.SetActive(false);
            StartCoroutine(battleManager.EndPlayerTurnRoutine());
        }

        private string AnalyzePlayerActions()
        {
            if (playerRecentActions.Count == 0)
                return null;

            var frequency = new Dictionary<string, int>();
            foreach (var action in playerRecentActions)
            {
                if (!frequency.ContainsKey(action))
                {
                    frequency[action] = 0;
                }
                frequency[action]++;
            }

            string mostCommonAction = null;
            int maxFrequency = 0;
            foreach (var pair in frequency)
            {
                if (pair.Value > maxFrequency)
                {
                    mostCommonAction = pair.Key;
                    maxFrequency = pair.Value;
                }
            }

            return mostCommonAction;
        }

        public void RecordPlayerAction(string action)
        {
            if (playerRecentActions.Count >= 5)
            {
                playerRecentActions.RemoveAt(0);
            }
            playerRecentActions.Add(action);
        }
    }
}
