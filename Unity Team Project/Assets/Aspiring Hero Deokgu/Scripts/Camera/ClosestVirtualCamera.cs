using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClosestVirtualCamera : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾� ��ġ
    private CinemachineClearShot clearShotCamera;

    public CharacterController controller;

    void Start()
    {
        clearShotCamera = GetComponent<CinemachineClearShot>();
        if (clearShotCamera == null)
        {
            Debug.LogError("CinemachineClearShot ������Ʈ�� �ʿ��մϴ�.");
        }

        if (playerTransform == null)
        {
            Debug.LogError("�÷��̾� Transform�� �Ҵ�Ǿ�� �մϴ�.");
        }
    }

    void Update()
    {
        if (clearShotCamera != null && playerTransform != null)
        {
            SelectClosestVirtualCamera();
        }
    }

    void SelectClosestVirtualCamera()
    {
        float closestDistance = float.MaxValue;
        CinemachineVirtualCamera closestCamera = null;

        foreach (CinemachineVirtualCamera vcam in clearShotCamera.ChildCameras)
        {
            float distance = Vector3.Distance(playerTransform.position, vcam.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCamera = vcam;
            }
        }

        if (closestCamera != null)
        {
            foreach (CinemachineVirtualCamera vcam in clearShotCamera.ChildCameras)
            {
                vcam.Priority = (vcam == closestCamera) ? 1 : 0;
            }
        }
    }
    /*public void PlayerStop()
    {
        StartCoroutine(EnableCharacterController(0.5f));
    }*/

    IEnumerator EnableCharacterController(float duration)
    {
        if (controller != null)
        {
            controller.enabled = false;
            yield return new WaitForSeconds(duration);
            controller.enabled = true;
        }
    }
}