using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    public static PotionController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void GetPotions()
    {
        this.gameObject.SetActive(false);
        WorldSoundManager.Instance.PlaySFX("GetPotionSound");
    }
}
