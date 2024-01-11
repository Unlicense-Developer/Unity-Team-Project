using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]
    GameObject uiPanel;

    void Start()
    {
        uiPanel.SetActive(false);//처음 닫은상태
    }
    void ToggleAchievement() //인벤토리 열기,닫기
    {
        if (WinInput.GetKeyDown(KeyCode.J))
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    } 


    void Update()
    {
        ToggleAchievement();
    }
}
