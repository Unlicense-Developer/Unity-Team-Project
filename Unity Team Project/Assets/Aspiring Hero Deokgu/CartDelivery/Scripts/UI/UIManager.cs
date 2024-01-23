using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CartDelivery
{
    public class UIManager : MonoBehaviour
    {

        // 타이틀창에서 Start버튼 누르면 ==> 코루틴으로   
        float startCount = 0;
        bool gameStartSign;
        Player player; //Player스크립트
        WheelController wheelController; //wheelController스크립트
        CameraManager cameraManager;//cameraManager 스크립트
        public static UIManager instance;

        const string productPriceText = "Product Price : "; //UI 앞에 첫 글자
        [SerializeField]
        TMP_Text productPriceTmp;//물건 값어치 글자

        //등급 별 이미지(동별,은별,금별)
        [SerializeField]
        GameObject scoreStarBrown;
        [SerializeField]
        GameObject scoreStarSliver;
        [SerializeField]
        GameObject scoreStarGold;

        [SerializeField]
        GameObject clearPanel;//클리어창
        [SerializeField]
        GameObject gameOverPanel;//게임오버창
        [SerializeField]
        GameObject gameStartCountPanel;//게임스타트 카운트창
        [SerializeField]
        GameObject gameReadyPanel;//게임레디 텍스트
        [SerializeField]
        TMP_Text startCountTmp;//게임카운트 텍스트

        [SerializeField]
        GameObject startCountThree;//게임카운트 3
        [SerializeField]
        GameObject startCountTwo;//게임카운트 2
        [SerializeField]
        GameObject startCountOne;//게임카운트 1

        [SerializeField]
        GameObject gameGoPanel;//게임고 패널
        [SerializeField]
        GameObject TitleKeyGuideUIPanel;//게임키 가이드창
        bool activeKeyGuide = false;
        void Awake()
        {
            if (null == instance)
                instance = this;
        }
        void Start()
        {
            WorldSoundManager.Instance.bgmSource.Stop();//월드 BGM사운드 끄기
            WorldSoundManager.Instance.PlayBGM("CartDelivery");//BGM사운드 켜기
            OffStart();
            player = GameObject.Find("Horse").GetComponent<Player>();
            wheelController = GameObject.Find("Cart").GetComponent<WheelController>();
            cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        }

        void OffStart() //시작할때 셋팅
        {
            Cursor.visible = true;//마우스커서 보이게
            gameStartCountPanel.SetActive(false);//게임 카운트창 켜기
            gameReadyPanel.SetActive(false);//게임레디창 켜기
            startCountThree.SetActive(false);//숫자 3 끄기 
            startCountTwo.SetActive(false);//숫자 2 끄기
            startCountOne.SetActive(false);//숫자 1 끄기
            gameGoPanel.SetActive(false);//게임go이미지창 끄기
            clearPanel.SetActive(false);//게임클리어창 끄기
            gameOverPanel.SetActive(false);//게임오버창 끄기
            OnKeyGuideUIButton();//타이틀 켜기

            //등급별 이미지 끄기 
            scoreStarBrown.SetActive(false);//동별
            scoreStarSliver.SetActive(false);//은별
            scoreStarGold.SetActive(false);//금별
        }
        public void ScoreStarChangeSign()//스코어에 따라서 다른색 별 켜기
        {
            int ProdutPrice = ScoreManager.instance.productPrice;
            if (ProdutPrice >= 1000) //1000점이상 
            {
                scoreStarGold.SetActive(true);//금별
            }
            else if (ProdutPrice >= 500)//500점이상 
            {
                scoreStarSliver.SetActive(true);//은별
            }
            else//그외 == 0점이상~499점이하
            {
                scoreStarBrown.SetActive(true);//동별
            }
        }
        void UIproductPrice()//TMP_Text productPrice 글씨를  ScoreManager.MinusProductPrice에 있는 productPrice값을 보여준다
        {
            int ProdutPrice = ScoreManager.instance.productPrice;
            productPriceTmp.text = productPriceText + Convert.ToString(ProdutPrice);
        }
        public IEnumerator StartCount()
        {
            startCountThree.SetActive(true); //숫자 3 켜기
            yield return new WaitForSeconds(1f); // 1초 대기

            startCountThree.SetActive(false); //숫자 3 끄기 
            startCountTwo.SetActive(true); //숫자 2 켜기
            yield return new WaitForSeconds(1f); // 1초 대기

            startCountTwo.SetActive(false); //숫자 2 끄기
            startCountOne.SetActive(true); //숫자 1 켜기
            yield return new WaitForSeconds(1f); // 1초 대기

            startCountOne.SetActive(false); //숫자 1 끄기
            gameReadyPanel.SetActive(false); //게임레디창 끄기
            gameGoPanel.SetActive(true); //게임go이미지 켜기
            yield return new WaitForSeconds(1f); // 1초 대기

            GameStartSign(); //게임 시작 신호
            gameGoPanel.SetActive(false); //게임go이미지 끄기
            gameStartSign = true;
            player.gamePlaying = true;
            cameraManager.gamePlaying = true;
            wheelController.gamePlaying = true;
        }
        public void OnStartButton()
        {
            Invoke("CameraSign", 5f);

            OffKeyGuideUIButton();//스타트창 끄기
            gameStartCountPanel.SetActive(true); //게임 카운트창 켜기
            gameReadyPanel.SetActive(true); //게임레디창 켜기
            StartCoroutine(StartCount());//스타트 카운트
            Cursor.visible = false;//마우스 커서 안보이게 
        }
        public void OnReStartButton() //재시작 버튼
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void OnReStartKey()//재시작 키
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        public void OnExitButton()
        {
            Cursor.visible = true;//마우스커서 보이게
            SceneManager.LoadScene("WorldMap");
        }
        public void OnProductPriceSendButton()
        {
            PlayerData.Instance.AddGold(ScoreManager.instance.productPrice);
            //현재 productPrice값을 월드 인벤토리 골드로 넘긴다
            Cursor.visible = true;//마우스커서 보이게
            SceneManager.LoadScene("WorldMap");
        }


        void CameraSign()
        {
            cameraManager.OnCameraSign();
        }

        void GameStartSign()
        {
            gameGoPanel.SetActive(false);//게임go이미지 끄기
        }
        public void OnGameOver()
        {
            Cursor.visible = true;//마우스커서 보이게
            gameOverPanel.SetActive(true);
        }

        public void OnGameClear()
        {
            Cursor.visible = true;//마우스커서 보이게
            clearPanel.SetActive(true);
        }

        public void OnKeyGuideUIButton()
        {
            TitleKeyGuideUIPanel.SetActive(true);
        }
        public void OffKeyGuideUIButton()
        {
            TitleKeyGuideUIPanel.SetActive(false);
        }
        void FixedUpdate()
        {
            UIproductPrice();
            ScoreStarChangeSign();
        }
        void Update()
        {
            OnReStartKey();
        }

    }
}















