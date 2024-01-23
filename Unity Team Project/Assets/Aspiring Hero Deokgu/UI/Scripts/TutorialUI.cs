using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;

public class TutorialUI : MonoBehaviour
{
    bool tutorialUiCheck;

    public GameObject tutorialUiPanel;

    void Start()
    {

    }

    void CloseTutorial() //인벤토리 열기,닫기
    {
        if (WinInput.GetKeyDown(KeyCode.Escape) && tutorialUiCheck == false)
        {
            tutorialUiPanel.SetActive(false);
            tutorialUiCheck = true;
        }
        else if (tutorialUiCheck == true)
        {
            tutorialUiPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CloseTutorial();
    }
}
