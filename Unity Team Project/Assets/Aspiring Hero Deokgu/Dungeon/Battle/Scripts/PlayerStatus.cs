using UnityEngine;

namespace DungeonBattle
{
    public class PlayerStatus : MonoBehaviour
    {
        public int Health { get; private set; }
        public int BaseAttackDamage { get; private set; } // �⺻ ���ݷ�
        public int AdditionalAttackDamage { get; private set; } // �߰� ���ݷ� (��� ��)
        public int TotalAttackDamage { get; private set; } // ���� ���ݷ�
        public int BaseBreakDamage { get; private set; } // �⺻ ����ȭ
        public int AdditionalBreakDamage { get; private set; } // �߰� ����ȭ (��� ��)
        public int TotalBreakDamage { get; private set; } // ���� ����ȭ
        public int ShieldCount { get; private set; }
        public int Defense { get; private set; } // ���� ���� �߰�
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            Health = 100; // �ʱ� ü�� ����
            BaseAttackDamage = 10;
            BaseBreakDamage = 10;
            ShieldCount = 3; // �ʱ� ���� Ƚ�� ����
            Defense = 0; // ���� ����

            uiManager = FindObjectOfType<UIManager>();

            uiManager.UpdatePlayerHealth(Health); // �ʱ� ü�� UI ������Ʈ
        }

        // �� ���ݷ� ���
        public void CalculateTotalAttackDamage()
        {
            TotalAttackDamage = BaseAttackDamage + AdditionalAttackDamage;
        }

        public void CalculateTotalBreakDamage()
        {
            TotalBreakDamage = BaseBreakDamage + AdditionalBreakDamage;
        }

        public void ReceiveDamage(int damage)
        {
            if (!isDeath)
            {
                int finalDamage = Mathf.Max(damage - Defense, 0); // ���� ����
                Health -= finalDamage;
                uiManager.UpdatePlayerHealth(Health); // ü�� ���� �� UI ������Ʈ

                if (Health <= 0)
                {
                    isDeath = true;
                    // ��� ��� ���
                    GetComponent<Animator>().SetTrigger("Death");
                }
            }
        }

        public void UseShield()
        {
            if (ShieldCount > 0)
            {
                ShieldCount--;
                // ���� ��� ����
                // UI ������Ʈ ���� �ʿ��� ��� ���⿡ �߰�
            }
        }


        // �߰� �÷��̾� ���� �޼ҵ�
    }
}