using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;


public class SawTrap : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public PlayerBattleController controller;

    private void Awake()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        controller = FindObjectOfType<PlayerBattleController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStatus.ReceiveDamage(10);
            Debug.Log("PlayerController: 플레이어 피격!");
            controller.PlayerHittedOther();
        }
    }

    void playerDamage(int damageAmount)
    {
       
    }
}
