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
            StartCoroutine(FadeInOut());
            other.transform.position = StartPoint.transform.position;
            TextGUIManager.Instance.FallInDarkText();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInOut());
            other.transform.position = StartPoint.transform.position;
            TextGUIManager.Instance.FallInDarkText();
        }
    }

    IEnumerator FadeInOut()
    {
        FadeController.instance.FadeOut();
        yield return new WaitForSeconds(1f);
        FadeController.instance.FadeIn();
    }
}
