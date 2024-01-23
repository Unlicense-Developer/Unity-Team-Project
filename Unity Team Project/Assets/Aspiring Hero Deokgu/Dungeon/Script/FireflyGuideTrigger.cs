using UnityEngine;

public class FireflyGuideTrigger : MonoBehaviour
{
    public GameObject fireFlyObj;

    public FireflyGuide fireflyGuide; // �ݵ����� ��ũ��Ʈ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fireFlyObj.SetActive(true);

            fireflyGuide.TriggerMovement();
        }
    }
}