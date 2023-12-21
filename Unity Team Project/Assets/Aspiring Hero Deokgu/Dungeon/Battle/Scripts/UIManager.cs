using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


namespace DungeonBattle
{
    public class UIManager : MonoBehaviour
    {
        public GameObject battleUiGroup;
        public GameObject battleStartGroup;
        public GameObject battleWonGroup; // �¸� UI
        public GameObject battleLostGroup; // �й� UI

        public Image PressF;
        public float fadeDuration = 3.0f; // ���̵� ���� �ð� (��)
        private float currentAlpha = 1.0f; // ���� ���� �� (1.0�� ���� ������, 0.0�� ���� ����)
        private bool isFadeIn = false; // ���̵� �� ����
        private bool isFading = false; // ���̵� ������ ����
        private Color originalColor; // �ʱ� ������ �����ϱ� ���� ����

        public TextMeshProUGUI playerHealthText;
        public TextMeshProUGUI enemyHealthText;
        public TextMeshProUGUI enemyBreakPointText;
        public TextMeshProUGUI shieldCountText;

        public Button evadeButton;
        public Button counterAttackButton;
        public Button shieldButton;

        private BattleManager battleManager;
        private PlayerStatus player;

        void Start()
        {
            battleManager = FindObjectOfType<BattleManager>();
            player = FindObjectOfType<PlayerStatus>();
        }

        void Update()
        {
            IsFading();
        }

        // ���� ���� ȿ��
        public void BattleStartEffect()
        {
            Debug.Log("UIManager: battleStartImage �̹��� Ȱ��ȭ");
            StartFadeEffect(battleStartGroup, true, 1.0f);
            Invoke("FadeOutBattleStartGroup", 2.0f);
            UpdateUI();
        }
        private void FadeOutBattleStartGroup()
        {
            StartFadeEffect(battleStartGroup, false, 1.0f);
        }

        public void BattleStart()
        {
            battleUiGroup.SetActive(true);
        }

        public void BattleOver()
        {
            battleUiGroup.SetActive(false);

            if (battleManager.state == BattleManager.BattleState.Won)
            {
                // ���� �¸� UI
                StartFadeEffect(battleWonGroup, true, 1.0f);
                Invoke("FadeOutBattleWonGroup", 2.0f);
            }
            else if (battleManager.state == BattleManager.BattleState.Lost)
            {
                // ���� �й� UI
                StartFadeEffect(battleLostGroup, true, 1.0f);
                Invoke("FadeOutBattleLostGroup", 2.0f);
            }
        }

        private void FadeOutBattleWonGroup()
        {
            StartFadeEffect(battleWonGroup, false, 1.0f);
        }
        private void FadeOutBattleLostGroup()
        {
            StartFadeEffect(battleLostGroup, false, 1.0f);
        }

        public void UpdateUI()
        {
            playerHealthText.text = "Player Health: " + player.Health;
            enemyHealthText.text = "Enemy Health: " + battleManager.currentEnemyStatus.Health;
            enemyBreakPointText.text = "Break Point: " + battleManager.currentEnemyStatus.BreakPoint;
            shieldCountText.text = "Shields: " + player.ShieldCount;
        }

        public void OnShieldButtonClicked()
        {
            player.UseShield();
            UpdateUI();
        }

        public void UpdatePlayerHealth(int health)
        {
            if (health <= 0) health = 0;
            playerHealthText.text = "Player Health: " + health;
        }

        public void UpdateEnemyHealth(int health)
        {
            if (health <= 0) health = 0;
            enemyHealthText.text = "Enemy Health: " + health;
        }

        public void UpdateEnemyBreakPoint(int breakPoint)
        {
            if (breakPoint <= 0) breakPoint = 0;
            enemyBreakPointText.text = "Break Point: " + breakPoint;
        }

        // ���̵� �ƿ� ȿ�� ����
        private void StartFadeEffect(GameObject group, bool fadeIn, float duration)
        {
            // �׷� Ȱ��ȭ
            group.SetActive(true);

            Image[] images = group.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                float endValue = fadeIn ? 1.0f : 0.0f; // ���̵� ���̸� 1, ���̵� �ƿ��̸� 0
                image.DOFade(endValue, duration).SetUpdate(true);
            }

            // ���̵� �ƿ��� ���, ���̵尡 �Ϸ�Ǹ� �׷��� ��Ȱ��ȭ
            if (!fadeIn)
            {
                DOVirtual.DelayedCall(duration, () => group.SetActive(false));
            }
        }

        private void IsFading()
        {
            if (!isFading)
            {
                return;
            }

            Image[] images = battleWonGroup.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                // ���̵� �� �Ǵ� �ƿ��� ���� ���� ���� ����
                currentAlpha += Time.deltaTime / fadeDuration * (isFadeIn ? 1 : -1);
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color newColor = image.color;
                newColor.a = currentAlpha;
                image.color = newColor;

                // ���̵� �ƿ� �Ϸ� �� �̹��� ��Ȱ��ȭ
                if (!isFadeIn && currentAlpha <= 0)
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (!isFadeIn && currentAlpha <= 0)
            {
                isFading = false;
                // ��� �̹����� ���̵� �ƿ� �Ϸ� �� �׷� ��Ȱ��ȭ
                battleWonGroup.SetActive(false);
                Debug.Log("UIManager: ���̵� �ƿ� �Ϸ� �� �̹��� ��Ȱ��ȭ");
            }

            // ���̵� �� �Ϸ� ��
            if (isFadeIn && currentAlpha >= 1)
            {
                isFading = false;
                // ��� �̹����� ���̵� �� �Ϸ� �� �߰� �۾�
                Debug.Log("UIManager: ���̵� �� �Ϸ�");
            }
        }

        // UI �̹����� ������ �ʱ�ȭ�ϴ� �Լ�
        public void ResetAlpha(Image image)
        {
            if (image != null)
            {
                currentAlpha = isFadeIn ? 1.0f : 0.0f;
                image.color = originalColor; // �ʱ� �������� �ǵ���
            }
        }
    }
}