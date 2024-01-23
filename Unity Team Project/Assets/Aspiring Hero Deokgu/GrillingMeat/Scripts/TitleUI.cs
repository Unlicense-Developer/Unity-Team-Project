using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace GrillingMeatGame
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField]
        GameObject gameTitlePanel;
        [SerializeField]
        GameObject gameRuleGuidePanel;
        bool activeGameRuleGuide = false;
        void Start()
        {
            WorldSoundManager.Instance.bgmSource.Stop();
            WorldSoundManager.Instance.PlayBGM("GrillingMeat_Game_Title");
            StartUISetting();
        }
        void StartUISetting()
        {
            //상태에 따라 변경하는 패널들 처음에 끄기
            gameTitlePanel.SetActive(true); //게임 타이틀 창 
            gameRuleGuidePanel.SetActive(false);//게임 가이드 창
        }
        public void OnGameStartSceneChangeInGame()
        {
            SceneManager.LoadScene("GrillingMeat_Game_InGame");
            Time.timeScale = 1.0f;            //게임시간 흐르게 
            MouseCursor.instance.CursorChange();//게임 시작하면 마우스커서가 고기집게로 변경
        }
        public void Quit() //게임 나가기
        {
            SceneManager.LoadScene("WorldMap");
        }
        public void OnGameRuleGuideKey()
        {
            if (Input.GetKeyDown(KeyCode.F1)) //F1 키 누르거나 버튼 누르면 켜진다
            {
                activeGameRuleGuide = !activeGameRuleGuide;//누르면 true로 변경 
                gameRuleGuidePanel.SetActive(activeGameRuleGuide); //true면 false로 false면 true로 변경(끄기,켜기 반복사용)
            }
        }
        void Update()
        {
            OnGameRuleGuideKey();
        }
    }
}