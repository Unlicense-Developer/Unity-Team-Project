using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;

public class ChestController : MonoBehaviour
{
    public GameObject interactionUI;
    public PlayerStatus playerStatus;

    Animator ani;
    public bool isOpen = false;
    public bool isGet = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    public void ToggleChest()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            
            if (playerInteract != null && isOpen == true)
            {
                
                playerInteract.DisableAllInteractUIs();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpen == false)
            {
                WorldSoundManager.Instance.PlaySFX("ChestOpen");
                ani.SetBool("IsOpen", true);
                Debug.Log("상자가 열렸습니다.");
                isOpen = true;

                if (isGet == false)
                {
                    isGet = true;
                    InventoryManager.Instance.AddMultipleItem("Potion", 6);
                    playerStatus.UpdatePotionCount();
                    PotionController.instance.GetPotions();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WorldSoundManager.Instance.PlaySFX("ChestClose");
            ani.SetBool("IsOpen", false);
            Debug.Log("상자가 닫혔습니다.");
            isOpen = false;
        }
    }
}