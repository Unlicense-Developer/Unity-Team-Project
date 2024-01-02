using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PentagramGate : MonoBehaviour
{
    public float openHeight = 4.5f; // ����Ʈ�� ���� ����
    public float openDuration = 10f; // ����Ʈ ������ �ð�

    public int ActivatedDevices = 0; // �۵��� ��ġ ��

    private Vector3 initialPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    public GameObject dust;


    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // ���� ��ġ ����
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
        transform.DOMove(targetPosition, openDuration);     // DoTween�� ����Ͽ� ����Ʈ ����
    }

}
