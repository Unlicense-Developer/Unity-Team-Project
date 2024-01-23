using UnityEngine;

public class FireflyGuideTrigger : MonoBehaviour
{
    public GameObject fireFlyObj;

    public FireflyGuide fireflyGuide; // 반딧불이 스크립트 참조

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fireFlyObj.SetActive(true);

            fireflyGuide.TriggerMovement();
        }
    }
}