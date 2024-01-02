using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PentagramGate : MonoBehaviour
{
    public float openHeight = 4.5f; // 게이트가 열린 높이
    public float openDuration = 10f; // 게이트 열리는 시간

    public int ActivatedDevices = 0; // 작동한 장치 수

    private Vector3 initialPosition; // 초기 위치
    private Vector3 targetPosition; // 목표 위치

    public GameObject dust;


    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // 열린 위치 설정
        dust.SetActive(false);
    }

    public void DeviceTriggered()
    {
        ActivatedDevices++;
        CheckAndOpenGate();
    }

    public void CheckAndOpenGate()
    {
        if (ActivatedDevices >= 4)
        {
            GateMoveUp();
            EventCameraController.instacne.EventOn();
            StartCoroutine(DustFall());
        }
    }

    IEnumerator DustFall()
    {
        dust.SetActive(true);

        yield return new WaitForSeconds(openDuration);

        dust.SetActive(false);
    }

    private void GateMoveUp()
    {
        transform.DOMove(targetPosition, openDuration);     // DoTween을 사용하여 게이트 열기
    }

}
