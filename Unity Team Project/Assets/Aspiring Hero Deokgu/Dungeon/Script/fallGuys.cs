using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallGuys : MonoBehaviour
{
    public GameObject StartPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = StartPoint.transform.position;
            Debug.Log("����� �߶������� ��°���ΰ� �Ա��� �� �ֽ��ϴ�.");
        }
    }

}
