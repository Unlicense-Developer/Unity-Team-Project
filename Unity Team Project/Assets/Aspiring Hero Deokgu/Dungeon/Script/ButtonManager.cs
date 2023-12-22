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

        }
    }
}
