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

            uiManager.UpdateEnemyHealth(Health); // �ʱ� ü�� UI ������Ʈ
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                Health -= damage;
                uiManager.UpdateEnemyHealth(Health); // ü�� ���� �� UI ������Ʈ

                if (Health <= 0)
                {
                    isDeath = true;
                    // ��� ��� ���
                    GetComponent<Animator>().SetTrigger("Death");
                }
            }
        }

        public void ReceiveBreak(int damage)
        {
            CurrentBreakPoint -= damage;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // ü�� ���� �� UI ������Ʈ
        }
        public void RestoreBreak()
        {
            CurrentBreakPoint = BreakPoint;
            uiManager.UpdateEnemyBreakPoint(CurrentBreakPoint); // ü�� ���� �� UI ������Ʈ
        }

        // �߰� �� ���� �޼ҵ�
    }
}