using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;


public class ButtonController : MonoBehaviour
{
    ButtonManager buttonManager;
    //public bool isActivated = false; // Ȱ��ȭ ����

    private void Start()
    {
        buttonManager = GetComponentInParent<ButtonManager>();
    }
    public void PushButton()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && this.gameObject.activeSelf) // this.gameObject�� Ȱ��ȭ ������ ���� ���ǹ� ����
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
