using UnityEngine;

namespace Dungeon
{
    public class EnemyStatus : MonoBehaviour
    {
        public int Health;
        public int CurrentHealth;
        public int BreakPoint;
        public int CurrentBreakPoint { get; private set; }
        public int AttackDamage;
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            CurrentHealth = Health;

            CurrentBreakPoint = BreakPoint;

            uiManager = FindObjectOfType<UIManager>();

            uiManager.UpdateEnemyHealth(CurrentHealth); // 초기 체력 UI 업데이트
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                CurrentHealth -= damage;
                uiManager.UpdateEnemyHealth(CurrentHealth); // 체력 변경 시 UI 업데이트

                if (CurrentHealth <= 0)
                {
                    isDeath = true;
                    // 사망 모션 재생
                    GetComponent<Animator>().SetTrigger("Death");
                }
            }
        }

        public void ReceiveBreak(int damage)
        {
            CurrentBreakPoint -= damage;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // 체력 변경 시 UI 업데이트
        }
        public void RestoreBreak()
        {
            CurrentBreakPoint = BreakPoint;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // 체력 변경 시 UI 업데이트
        }

        // 추가 적 관련 메소드
    }
}