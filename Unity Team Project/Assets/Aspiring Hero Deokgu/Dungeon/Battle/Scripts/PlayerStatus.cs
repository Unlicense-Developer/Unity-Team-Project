using GrillingMeatGame;
using UnityEngine;

namespace Dungeon
{
    public class PlayerStatus : MonoBehaviour
    {
        public int Health { get; private set; }
        public int CurrentHealth { get; private set; }
        public int BaseAttackDamage { get; private set; } // 기본 공격력
        public int AdditionalAttackDamage { get; private set; } // 추가 공격력 (장비 등)
        public int TotalAttackDamage { get; private set; } // 최종 공격력
        public int BaseBreakDamage { get; private set; } // 기본 무력화
        public int AdditionalBreakDamage { get; private set; } // 추가 무력화 (장비 등)
        public int TotalBreakDamage { get; private set; } // 최종 무력화
        public int PotionCount { get; private set; }
        public int Defense { get; private set; } // 방어력 스탯 추가
        public bool isDeath { get; private set; }

        private UIManager uiManager;

        void Start()
        {
            Health = 100; // 초기 체력 설정
            CurrentHealth = Health;
            BaseAttackDamage = 10;
            BaseBreakDamage = 10;
            PotionCount = 100; // 포션 횟수 설정
            Defense = 0; // 방어력 설정

            uiManager = FindObjectOfType<UIManager>();
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
                CurrentHealth -= finalDamage;
                uiManager.UpdatePlayerHealth(CurrentHealth); // 체력 변경 시 UI 업데이트

                if (CurrentHealth <= 0)
                {
                    isDeath = true;
                    // 사망 모션 재생
                    GetComponent<Animator>().SetTrigger("Death");
                    // 사망 사운드 재생
                    WorldSoundManager.Instance.PlaySFX("MaleScream");
                }
            }
        }

        public void UsePotion()
        {
            if (PotionCount > 0 && !isDeath)
            {
                PotionCount--;
                // 포션 사용 로직
                CurrentHealth = Health;
                // 사운드 재생
                WorldSoundManager.Instance.PlaySFX("Drink");
            }
        }

        public void Revive(int health)
        {
            CurrentHealth = health;
            // 필요한 경우 추가 로직...
            isDeath = false;
        }
    }
}