using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;


public class ButtonController : MonoBehaviour
{
    ButtonManager buttonManager;
    //public bool isActivated = false; // 활성화 여부

    private void Start()
    {
        buttonManager = GetComponentInParent<ButtonManager>();
    }
    public void PushButton()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && this.gameObject.activeSelf) // this.gameObject가 활성화 상태일 때만 조건문 실행
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                buttonManager.ButtonActivated();
                this.gameObject.SetActive(false);
                playerInteract.DisableAllInteractUIs();
            }
        }
    }
}
