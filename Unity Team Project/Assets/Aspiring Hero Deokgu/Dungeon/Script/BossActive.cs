using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActive : MonoBehaviour
{
    public GameObject sitBoss;
    public GameObject stendBoss;
    public GameObject ActiveEffect;

    public static BossActive instance;

    public int ActivatedDevices = 0;

    public bool bossActive = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        sitBoss.SetActive(true);
        stendBoss.SetActive(false);
        ActiveEffect.SetActive(false);
    }

    public void DeviceActivated()
    {
        ActivatedDevices++;
        Checked();
    }

    private void Checked()
    {
        if (ActivatedDevices >= 2)
        {
            bossActive = true;
            sitBoss.SetActive(false);
            EventCameraController.Instacne.OtherEvnetOn();
            stendBoss.SetActive(true);
            StartCoroutine(SummonEffect());

            Debug.Log("보스 활성화");
        }
        else
        {
            Debug.Log("한개 남았습니다");
        }
    }

    IEnumerator SummonEffect()
    {
        ActiveEffect.SetActive(true);

        yield return new WaitForSeconds(7f);
        WorldSoundManager.Instance.PlaySFX("Summon");

        ActiveEffect.SetActive(false);
    }

    void Update()
    {
        
    }
}
