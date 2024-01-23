using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class FireflyGuide : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform lookAtTarget;
    public CinemachineVirtualCamera lookAtCamera; // lookAtTarget�� �ٶ󺸴� ���ο� ī�޶�
    public float speed = 5f;
    private int currentPointIndex = 0;
    private bool isMoving = false;

    public CinemachineVirtualCamera followCamera;

    public Image blackoutImage;

    void Start()
    {
        if (blackoutImage != null)
        {
            blackoutImage.color = new Color(0, 0, 0, 0);
        }

        //gameObject.SetActive(false); // �ݵ����� ������Ʈ ��Ȱ��ȭ
    }

    public void TriggerMovement()
    {
        if (!isMoving && currentPointIndex < waypoints.Length)
        {
            gameObject.SetActive(true);
            followCamera.Priority = 100;
            followCamera.LookAt = transform; // ī�޶� �ݵ����̸� �ٶ󺸰� ����
            WorldSoundManager.Instance.PlaySFX("FireFly");

            StartCoroutine(FollowWaypoints());
            isMoving = true;
        }
    }

    IEnumerator FollowWaypoints()
    {
        while (currentPointIndex < waypoints.Length)
        {
            Transform targetPoint = waypoints[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPoint.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }

        // ������ waypoint�� �����ϸ� lookAtCamera�� ��ȯ
        if (lookAtTarget != null)
        {
            lookAtCamera.LookAt = lookAtTarget;
            lookAtCamera.Priority = 101; // ���� ī�޶󺸴� ���� �켱���� ����
        }

        yield return new WaitForSeconds(2f); // ���� ���� ���
        FadeOutEffect();

        yield return new WaitForSeconds(1f); // �ٽ� �÷��̾� ī�޶�� ��ȯ ���
        followCamera.Priority = 0;
        lookAtCamera.Priority = 0;

        yield return new WaitForSeconds(2f); // ���� �� ���
        FadeInEffect();

        gameObject.SetActive(false);
    }

    // ȭ�� ���� ȿ�� ����
    void FadeOutEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(1f, 1f); // 1�� ���� ȭ���� ��Ӱ� ����
        }
    }

    // ȭ�� ���� ȿ�� ����
    void FadeInEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(0f, 1f); // 1�� ���� ȭ���� ��� ����
        }
    }
}