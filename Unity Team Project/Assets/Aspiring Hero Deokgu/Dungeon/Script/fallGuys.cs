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
            Debug.Log("당신은 추락했지만 어째서인가 입구에 와 있습니다.");
        }
    }

}
