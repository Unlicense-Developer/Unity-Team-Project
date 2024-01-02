using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonController : MonoBehaviour
{
    ButtonManager buttonManager;
    //public bool isActivated = false; // 활성화 여부

    private void Start()
    {
        buttonManager = GetComponentInParent<ButtonManager>();

    }

    public void Enter()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                buttonManager.ButtonActivated();
                gameObject.SetActive(false);
                playerInteract.DisableAllInteractUIs();
            }
        }
        
    }
}
