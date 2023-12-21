using UnityEngine;

namespace DungeonBattle
{
    public class PlayerStatus : MonoBehaviour
    {
        public int Health { get; private set; }
        public int BaseAttackDamage { get; private set; } // 기본 공격력
        public int AdditionalAttackDamage { get; private set; } // 추가 공격력 (장비 등)
        public int TotalAttackDamage { get; private set; } // 최종 공격력
        public int BaseBreakDamage { get; private set; } // 기본 무력화
        public int AdditionalBreakDamage { get; private set; } // 추가 무력화 (장비 등)
        public int TotalBreakDamage { get; private set; } // 최종 무력화
        public int ShieldCount { get; private set; }
        public int Defense { get; private set; } // 방어력 스탯 추가
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            Health = 100; // 초기 체력 설정
            BaseAttackDamage = 10;
            BaseBreakDamage = 10;
            ShieldCount = 3; // 초기 방패 횟수 설정
            Defense = 0; // 방어력 설정

            uiManager = FindObjectOfType<UIManager>();

            uiManager.UpdatePlayerHealth(Health); // 초기 체력 UI 업데이트
        }

        // 총 공격력 계산
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
                int finalDamage = Mathf.Max(damage - Defense, 0); // 방어력 적용
                Health -= finalDamage;
                uiManager.UpdatePlayerHealth(Health); // 체력 변경 시 UI 업데이트

                if (Health <= 0)
                {
                    isDeath = true;
                    // 사망 모션 재생
                    GetComponent<Animator>().SetTrigger("Death");
                }
            }
        }

        public void UseShield()
        {
            if (ShieldCount > 0)
            {
                ShieldCount--;
                // 방패 사용 로직
                // UI 업데이트 로직 필요한 경우 여기에 추가
            }
        }


        // 추가 플레이어 관련 메소드
    }
}