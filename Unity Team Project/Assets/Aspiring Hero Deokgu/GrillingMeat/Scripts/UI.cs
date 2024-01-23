using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GrillingMeatGame
{
    public class UI : MonoBehaviour
    {

        public static UI instance;
        float limitTime = 180.0f; //제한시간 //180
        [SerializeField]
        TMP_Text scoreText;

        [SerializeField]
        TMP_Text timeText;

        [SerializeField]
        Slider timeSlider; //Value값 조절해서 시간 줄이기

        //점수에 따라서 [동별, 은별, 금별] SetActive(true)로 켜서 이미지 바꾸기
        [SerializeField]
        Image brownStar;
        [SerializeField]
        Image silverStar;
        [SerializeField]
        Image goldStar;

        //하트 5개 배열? 리스트? 선언후 MeatColor.MeatAniPlayingScoreStep()에서 
        //else if 에 하트 1개씩 빼는 동작 추가
        [SerializeField]
        Image heartLife;//하트 이미지
        float maxHeart = 100f; //하트이미지 전체사이즈
        float heart; //기준 숫자
        Animator meatColorChange; //고기 익는 애니메이션
        [SerializeField]
        GameObject gameOverPanel; //게임 오버 창
        [SerializeField]
        GameObject timeOverPanel; //타임 오버 창 
        [SerializeField]
        GameObject missionClearPanel;//클리어 창
        [SerializeField]
        GameObject gameRuleGuidePanel;// 게임 가이드 창

        [SerializeField]
        GameObject pausePanel;// 일시정지 창
        public bool activeGameRuleGuide = false; // 게임가이드 창 조건(끄기)기본
        public bool activePause = false; // 일시정지 창 조건(끄기)기본
        void Awake()
        {
            if (null == instance)
                instance = this;

        }
        void Start()
        {
            StartUISetting();
            meatColorChange = GameObject.FindWithTag("MEAT").GetComponent<Animator>();//고기 애니 
        }
        void StartUISetting()
        {
            Time.timeScale = 0f;
            HideStars();
            heart = maxHeart;
            //상태에 따라 변경하는 패널들 처음에 끄기
            gameRuleGuidePanel.SetActive(true);//게임 가이드 창
            gameOverPanel.SetActive(false);//게임 오버 창 
            timeOverPanel.SetActive(false);//타임 오버 창
            missionClearPanel.SetActive(false);//클리어 창
            pausePanel.SetActive(false);//일시정지 창
            timeSlider.interactable = false;//타임슬라이더 멈춤 
            brownStar.enabled = true; //처음 점수에는 갈색별 보이게 
                                      //UnityEditor.EditorApplication.isPaused = false;//게임 일시정지 끄기
        }
        public void OnPauseKey()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                activePause = !activePause;
                pausePanel.SetActive(activePause);
                if ((pausePanel.activeSelf == true) || (gameRuleGuidePanel.activeSelf == true))
                {
                    Time.timeScale = 0f;//게임 시간 멈추기
                }
                else
                {
                    Time.timeScale = 1.0f;//게임 시간 흐르게
                }
            }
        }

        public void OffPause()//게임창으로 다시 돌아가기
        {
            pausePanel.SetActive(false);//일시정지 창 끄기
            Time.timeScale = 1.0f;//게임 시간 흐르게
        }

        //게임 재시작
        public void OnReStart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1.0f;//게임 시간 흐르게
        }


        public void OnSceneChangeWorldMap() //게임끝나고 나가기 버튼으로 월드맵으로 돌아간다
        {
            StarsCount();//고기를 월드로 넘긴다
            Time.timeScale = 1.0f;
            MouseCursor.instance.CursorDefault();//커서를 원상태로 
            SceneManager.LoadScene("WorldMap");//월드맵으로 이동 
        }
        public void OnGameRuleGuideKey()
        {
            if (Input.GetKeyDown(KeyCode.F1)) //F1 키 누르거나 버튼 누르면 켜진다
            {
                activeGameRuleGuide = !activeGameRuleGuide;//누르면 true로 변경 
                gameRuleGuidePanel.SetActive(activeGameRuleGuide);
                //true면 false로 false면 true로 변경(끄기,켜기 반복사용)
                if ((gameRuleGuidePanel.activeSelf == true) || (pausePanel.activeSelf == true)) //pausePanel.activeSelf == true였을때 킬때도 
                {
                    Time.timeScale = 0f;//게임 시간 멈추기

                }
                else
                {
                    Time.timeScale = 1.0f;//게임 시간 흐르게
                }
            }
        }
        public void OnGameRuleGuide()//게임가이드창 켜기
        {
            gameRuleGuidePanel.SetActive(true);
            Time.timeScale = 0f;

        }
        public void OffGameRuleGuide()//게임가이드창 끄기
        {
            gameRuleGuidePanel.SetActive(false);
            if (((pausePanel.activeSelf == false) && (gameRuleGuidePanel.activeSelf == true))
            || ((pausePanel.activeSelf == false) && (gameRuleGuidePanel.activeSelf == false))) //pausePanel.activeSelf == false 조건일때 끄면  
            {
                Time.timeScale = 1.0f;//시간흐르기

            }
        }

        void HeartBar() // heart 1칸씩 깎기
        {
            heartLife.fillAmount = heart / maxHeart;
        }
        public void HideHearts()//하트 깎임
        {
            heart -= 20f; //1칸씩 깎이게 
            if (heart <= 0f) //하트가 0이면 
            {
                GameOverSign();//게임오버
            }
        }

        public void GameOverSign() //게임 오버 상태
        {
            GameManager.instance.SoundMute.Invoke(); //고기 소리 멈춤
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; //게임 시간 멈추기
            MouseCursor.instance.CursorDefault();//커서를 원상태로 
        }

        void HideStars() //동별 제외하고 나머지 안보이게
        {
            silverStar.enabled = false;
            goldStar.enabled = false;
        }
        void TimerSlider() //제한시간 바 이미지 
        {
            if (timeSlider.value > 0.0f)
            {
                timeSlider.value -= Time.deltaTime; //시간이 흐를수록 타임 슬라이더도 줄어듬
            }
        }

        void NowLimitTime() //제한시간 숫자
        {
            limitTime -= Time.deltaTime; //제한시간 흘러가게
            timeText.text = Mathf.Round(limitTime).ToString(); //제한시간 글씨로 변경
            TimeZero();
        }

        void TimeZero()
        {
            //시간초 0이 이하로 안떨어진다
            if (limitTime < 0)
            {
                limitTime = 0f; // 방어코드 
                TimeOverSign(); //타임오버 패널 켜기
            }
        }
        void TimeOverSign() //타임오버 상태
        {
            GameManager.instance.SoundMute.Invoke(); //고기 소리 멈춤
            timeOverPanel.SetActive(true); //타임오버 패널 ON
            Time.timeScale = 0f; //게임 시간 멈춤
            MouseCursor.instance.CursorDefault();//커서를 원상태로 
        }
        //점수 UI 연결 
        public void NowScoreDisplay(int score) //현재 점수
        {
            ViewStars(score);
            scoreText.text = score.ToString(); //점수를 숫자로 변환
        }
        void ViewStars(int score) //점수에 따라서 별색상 변경
        {
            //점수 대략 정하기
            if (score >= 2200)
            {
                goldStar.enabled = true; //골드 
                silverStar.enabled = false;
                Invoke("MissionClear", 2f);
            }
            else if (score >= 1200)
            {
                silverStar.enabled = true; //실버
                brownStar.enabled = false;
            }
        }
        void StarsCount() //고기를 월드로 넘긴다 (점수에 따라서 고기갯수 다르게)
        {
            if (ScoreManager.instance.score >= 2200)//금별
            {
                InventoryManager.Instance.AddMultipleItem("Meat", 2);//고기갯수 2개
            }
            else if (ScoreManager.instance.score >= 1200)//은별
            {
                InventoryManager.Instance.AddItem("Meat");//고기갯수 1개
            }
        }
        void MissionClear()
        {
            missionClearPanel.SetActive(true);
            Time.timeScale = 0f; //게임 시간 멈춤
            MouseCursor.instance.CursorDefault();//커서를 원상태로 
        }
        void Update()
        {
            OnGameRuleGuideKey();//F1키 = 게임가이드창ON
            OnPauseKey();//ESC키 = 게임옵션창ON 
            NowLimitTime();//제한시간 숫자
            TimerSlider();//제한시간 바
            HeartBar();//하트 줄어듬
        }
    }
}






































