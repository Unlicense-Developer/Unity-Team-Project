using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class MeatColor : MonoBehaviour
    {
        Vector3 originalPos;//오브젝트의 최초 위치를 저장할 변수
        public float delay = 10f; //원래 위치로 돌아가기 전 대기 시간
        Animator meatColorChange; //고기 애니메이터
        BoxCollider2D meatCollider; //고기 Collider
        [SerializeField]
        GameObject[] Meats; //생고기들

        //MeatAniPlayingScoreStep의 4가지값
        [SerializeField]
        float addScoreStartAniTime = 0.3f;//0.4
        [SerializeField]
        float addScoreEndAniTime = 0.5f;
        [SerializeField]
        float MinusScoreStartAniTime = 0.51f;
        [SerializeField]
        float MinusScoreEndAniTime = 1.0f;

        public Action RemoveHeartsEvent;//목숨(하트) 삭제 

        void Awake()
        {
            meatColorChange = GetComponent<Animator>();
            meatCollider = GetComponent<BoxCollider2D>();
        }
        void Start()
        {
            meatCollider.enabled = true; //처음에 고기 잡을수 있게 Collider 켜기
            MeatStarPosSave();
        }
        void MeatStarPosSave()
        {
            originalPos = transform.position;  // 최초 위치와 회전을 저장
        }

        IEnumerator ReturnToOriginalPositionAfterDelay()
        {
            yield return new WaitForSeconds(delay); // 설정된 시간만큼 대기
            meatCollider.enabled = true;//콜라이더 켜기
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;//고기이미지 켜기
            transform.position = originalPos; // 원래 위치로 복귀

        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MEATGRILL"))
            {
                GameManager.instance.OnSmokeParticle.Invoke();//파티클 큰 연기
                meatColorChange.SetBool("Color", true); //고기 익히는 애니 재생
            }

            if (other.gameObject.CompareTag("ENDPLATE"))
            {
                if (other != null)
                {
                    GameManager.instance.SoundEatPlay.Invoke(); // 고기 먹는소리 재생
                    meatColorChange.SetBool("Color", false); //고기 익히는 애니 멈춤
                    meatCollider.enabled = false; // 선택 안됨(Collider 끄기)
                    MeatAniPlayingScoreStep();// 고기 익는단계에 따라서 점수 다르게 계산 함수 실행
                    this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false; //고기이미지 끄기 
                    meatColorChange.SetBool("Spawn", true); //생고기
                    if (this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == false)//고기 이미지 꺼진상태 확인
                    {

                        StartCoroutine(ReturnToOriginalPositionAfterDelay()); // 원래 위치로 돌아가는 코루틴 실행
                    }
                }
            }
        }
        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MEATGRILL"))
            {
                GameManager.instance.OnSmokeParticle.Invoke(); //연기 켜기
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MEATGRILL"))
            {
                meatColorChange.SetBool("Color", false); //고기 익히는 애니 멈춤
                GameManager.instance.OffSmokeParticle.Invoke(); //연기 끄기
            }
        }

        void MeatAniPlayingScoreStep() // 고기 익는단계에 따라서 점수 다르게 계산
        {
            if (meatColorChange.GetCurrentAnimatorStateInfo(0).normalizedTime >= addScoreStartAniTime  // 고기 익는애니메이션 단계가 40퍼~
            && meatColorChange.GetCurrentAnimatorStateInfo(0).normalizedTime <= addScoreEndAniTime) // 50퍼 였을때  
            {
                GameManager.instance.ScoreAdd.Invoke();//점수 추가
            }

            else if (meatColorChange.GetCurrentAnimatorStateInfo(0).normalizedTime >= MinusScoreStartAniTime // 고기 익는애니메이션 단계가 51퍼
            && meatColorChange.GetCurrentAnimatorStateInfo(0).normalizedTime <= MinusScoreEndAniTime) // ~100퍼 였을때 
            {
                GameManager.instance.ScoreMinus.Invoke();//점수 빼기

                UI.instance.HideHearts(); //하트 1개씩 사라지게
            }
            else //그외 조건(0~39퍼)에 해당하면 실행 ==>(덜익는다)
            {
                GameManager.instance.ScoreMinus.Invoke();//점수 빼기

                UI.instance.HideHearts(); //하트 1개씩 사라지게
            }
        }
    }
}



















