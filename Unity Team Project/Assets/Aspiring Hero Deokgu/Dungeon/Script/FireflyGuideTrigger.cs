using UnityEngine;

public class FireflyGuideTrigger : MonoBehaviour
{
    public FireflyGuide fireflyGuide; // �ݵ����� ��ũ��Ʈ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fireflyGuide.TriggerMovement();
        }
    }
}