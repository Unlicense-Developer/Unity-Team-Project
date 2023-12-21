using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public static ButtonManager instance;

    private int activeButtons = 0;

    public void ButtonActivated()
    {
        activeButtons++;
        Debug.Log(activeButtons);

        if (activeButtons >= 4)
        {
            BossActive.instance.DeviceActivated();
            Debug.Log("당신은 어디선가 사악하고 탐욕스러운 기운을 느꼈다!");
        }
    }
}
