using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject interactionUI;
    Animator ani;
    public bool isOpen = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void ToggleChest()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            
            if (playerInteract != null)
            {
                if (isOpen)
                {
                    
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpen)
            {
                WorldSoundManager.Instance.PlaySFX("ChestOpen");
                ani.SetBool("IsOpen", true);
                Debug.Log("���ڰ� ���Ƚ��ϴ�.");
                isOpen = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WorldSoundManager.Instance.PlaySFX("ChestClose");
            ani.SetBool("IsOpen", false);
            Debug.Log("���ڰ� �������ϴ�.");
            isOpen = !isOpen;
        }
    }
}