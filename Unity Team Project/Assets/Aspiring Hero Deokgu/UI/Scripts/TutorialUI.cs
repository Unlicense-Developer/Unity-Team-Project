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

    void CloseTutorial() //�κ��丮 ����,�ݱ�
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
