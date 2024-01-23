using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class MouseCursor : MonoBehaviour
    {
        public static MouseCursor instance;
        [SerializeField]
        Texture2D cursorIcon;//커서이미지
        public CursorMode cursorMode = CursorMode.Auto;

        void Awake()
        {
            if (null == instance)
                instance = this;
        }
        public void CursorChange()//커서 집게로 변경
        {
            Cursor.lockState = CursorLockMode.Confined; //커서 화면밖으로 안나가게 하기(잠금)
            Cursor.SetCursor(cursorIcon, new Vector2(cursorIcon.width / 6, 0), CursorMode.ForceSoftware); //커서 인식 위치 
        }
        public void CursorDefault()//커서 기본으로 변경
        {
            Cursor.lockState = CursorLockMode.None;//커서 화면밖으로 나가게 하기(잠금해제)
            Cursor.SetCursor(null, Vector2.zero, cursorMode);//커서 기본으로 돌려놓기
        }
    }
}


