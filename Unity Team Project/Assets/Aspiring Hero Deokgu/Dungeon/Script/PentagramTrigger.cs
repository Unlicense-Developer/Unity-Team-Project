using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PentagramTrigger : MonoBehaviour
{
    public GameObject PlayerT;

    public GameObject TeleportPoint;

    public GameObject ring;

    public GameObject mineCart;

    private bool isTriggerActivated = false;


    void Start()
    {
        ring.SetActive(false);
        // PlayerT = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggerActivated)
        {
            WorldSoundManager.Instance.PlaySFX("PentagramTrigger");
            BossActive.instance.DeviceActivated();
            StartCoroutine(ringEffect());
            isTriggerActivated = true;
            StartCoroutine(TeleportPlayer());
        }
        else
        {
            Debug.Log("더 이상 작동하지 않는다");
        }
    }

    IEnumerator ringEffect()
    {
        ring.SetActive(true);

        yield return new WaitForSeconds(30f);

        ring.SetActive(false);
    }

    IEnumerator TeleportPlayer()
    {
        CharacterController playerController = PlayerT.GetComponent<CharacterController>();

        mineCart.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeInOut());
        yield return new WaitForSeconds(1f); // FadeInOut 코루틴이 완료될 때까지 대기
        playerController.enabled = false;
        PlayerT.transform.position = TeleportPoint.transform.position; // 위치 변경
        playerController.enabled = true;
        TextGUIManager.Instance.PentaTrigger();
    }

    IEnumerator FadeInOut()
    {
        FadeController.instance.FadeOut();
        yield return new WaitForSeconds(2f);
        FadeController.instance.FadeIn();
    }
}
