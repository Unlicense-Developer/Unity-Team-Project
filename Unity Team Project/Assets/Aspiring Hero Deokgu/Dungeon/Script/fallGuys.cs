using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallGuys : MonoBehaviour
{
    public GameObject BackPoint;

    public GameObject PlayerT;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController playerController = PlayerT.GetComponent<CharacterController>();

        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInOut());
            playerController.enabled = false;
            other.transform.position = BackPoint.transform.position;
            playerController.enabled = true;
            TextGUIManager.Instance.FallInDarkText();
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInOut());
            other.transform.position = BackPoint.transform.position;
            TextGUIManager.Instance.FallInDarkText();
        }
    }*/

    IEnumerator FadeInOut()
    {
        FadeController.instance.FadeOut();
        yield return new WaitForSeconds(1f);
        FadeController.instance.FadeIn();
    }
}
