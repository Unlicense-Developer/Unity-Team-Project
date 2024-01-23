using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class Drag : MonoBehaviour
    {
        void OnMouseDrag()//마우스 드래그로 고기를 잡는다
        {
            bool isPanelActivePause = UI.instance.activePause; //일시정지창 bool 가져옴
            bool isPanelActiveGameRuleGuide = UI.instance.activeGameRuleGuide; //게임방법창 bool 가져옴 
            if ((!isPanelActiveGameRuleGuide || !isPanelActivePause) && //두개의 창이 켜있을때 혹은 1개만 켜있을때
            (!isPanelActiveGameRuleGuide) && (!isPanelActivePause))
            {
                //카메라 z값 삭제용 
                Vector3 cameraZpos = Vector3.back * 10; // Z값을 -10으로 만든다
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);//마우스 위치값 
                Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos); // 마우스 위치값의 스크린 좌표를 월드값으로 변환 
                objPos -= cameraZpos; // z값 -1로 만들기위해서 뺀다 (-11)-(-10)(카메라 z값) = -1
                                      //objPos.z = -1; //한줄로 사용할때 강제주입 방식
                transform.position = objPos;// 계산된 z값 = -1 넣음 
            }
        }
    }
}