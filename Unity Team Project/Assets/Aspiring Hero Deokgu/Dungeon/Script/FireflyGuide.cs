using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class FireflyGuide : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform lookAtTarget;
    public CinemachineVirtualCamera lookAtCamera; // lookAtTarget을 바라보는 새로운 카메라
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

        //gameObject.SetActive(false); // 반딧불이 오브젝트 비활성화
    }

    public void TriggerMovement()
    {
        if (!isMoving && currentPointIndex < waypoints.Length)
        {
            gameObject.SetActive(true);
            followCamera.Priority = 100;
            followCamera.LookAt = transform; // 카메라가 반딧불이를 바라보게 설정
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

        // 마지막 waypoint에 도달하면 lookAtCamera로 전환
        if (lookAtTarget != null)
        {
            lookAtCamera.LookAt = lookAtTarget;
            lookAtCamera.Priority = 101; // 현재 카메라보다 높은 우선순위 설정
        }

        yield return new WaitForSeconds(2f); // 암전 전에 대기
        FadeOutEffect();

        yield return new WaitForSeconds(1f); // 다시 플레이어 카메라로 전환 대기
        followCamera.Priority = 0;
        lookAtCamera.Priority = 0;

        yield return new WaitForSeconds(2f); // 암전 후 대기
        FadeInEffect();

        gameObject.SetActive(false);
    }

    // 화면 암전 효과 시작
    void FadeOutEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(1f, 1f); // 1초 동안 화면을 어둡게 만듦
        }
    }

    // 화면 암전 효과 해제
    void FadeInEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(0f, 1f); // 1초 동안 화면을 밝게 만듦
        }
    }
}