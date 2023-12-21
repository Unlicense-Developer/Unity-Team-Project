using UnityEngine;

namespace DungeonBattle
{
    public class EnemyStatus : MonoBehaviour
    {
        public int Health;
        public int BreakPoint;
        public int CurrentBreakPoint { get; private set; }
        public int AttackDamage;
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            CurrentBreakPoint = BreakPoint;

            uiManager = FindObjectOfType<UIManager>();

            uiManager.UpdateEnemyHealth(Health); // 초기 체력 UI 업데이트
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                Health -= damage;
                uiManager.UpdateEnemyHealth(Health); // 체력 변경 시 UI 업데이트

                if (Health <= 0)
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