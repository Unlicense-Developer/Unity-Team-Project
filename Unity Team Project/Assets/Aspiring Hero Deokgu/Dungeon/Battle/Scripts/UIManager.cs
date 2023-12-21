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
        public GameObject battleWonGroup; // 승리 UI
        public GameObject battleLostGroup; // 패배 UI

        public Image PressF;
        public float fadeDuration = 3.0f; // 페이드 지속 시간 (초)
        private float currentAlpha = 1.0f; // 현재 알파 값 (1.0은 완전 불투명, 0.0은 완전 투명)
        private bool isFadeIn = false; // 페이드 인 여부
        private bool isFading = false; // 페이드 중인지 여부
        private Color originalColor; // 초기 색상을 저장하기 위한 변수

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

        // 전투 시작 효과
        public void BattleStartEffect()
        {
            Debug.Log("UIManager: battleStartImage 이미지 활성화");
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
                // 전투 승리 UI
                StartFadeEffect(battleWonGroup, true, 1.0f);
                Invoke("FadeOutBattleWonGroup", 2.0f);
            }
            else if (battleManager.state == BattleManager.BattleState.Lost)
            {
                // 전투 패배 UI
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

        // 페이드 아웃 효과 시작
        private void StartFadeEffect(GameObject group, bool fadeIn, float duration)
        {
            // 그룹 활성화
            group.SetActive(true);

            Image[] images = group.GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                float endValue = fadeIn ? 1.0f : 0.0f; // 페이드 인이면 1, 페이드 아웃이면 0
                image.DOFade(endValue, duration).SetUpdate(true);
            }

            // 페이드 아웃의 경우, 페이드가 완료되면 그룹을 비활성화
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
                // 페이드 인 또는 아웃에 따라 알파 값을 조정
                currentAlpha += Time.deltaTime / fadeDuration * (isFadeIn ? 1 : -1);
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color newColor = image.color;
                newColor.a = currentAlpha;
                image.color = newColor;

                // 페이드 아웃 완료 시 이미지 비활성화
                if (!isFadeIn && currentAlpha <= 0)
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (!isFadeIn && currentAlpha <= 0)
            {
                isFading = false;
                // 모든 이미지의 페이드 아웃 완료 후 그룹 비활성화
                battleWonGroup.SetActive(false);
                Debug.Log("UIManager: 페이드 아웃 완료 및 이미지 비활성화");
            }

            // 페이드 인 완료 시
            if (isFadeIn && currentAlpha >= 1)
            {
                isFading = false;
                // 모든 이미지의 페이드 인 완료 후 추가 작업
                Debug.Log("UIManager: 페이드 인 완료");
            }
        }

        // UI 이미지의 투명도를 초기화하는 함수
        public void ResetAlpha(Image image)
        {
            if (image != null)
            {
                currentAlpha = isFadeIn ? 1.0f : 0.0f;
                image.color = originalColor; // 초기 색상으로 되돌림
            }
        }
    }
}