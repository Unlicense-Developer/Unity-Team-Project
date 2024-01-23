using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dungeon;


public class SpearTrap : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveDuration = 0.2f;
    public float delayBetweenMovements = 2f;

    public PlayerStatus playerStatus;
    public PlayerBattleController controller;

    private void Awake()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        controller = FindObjectOfType<PlayerBattleController>();
    }

    void Start()
    {
        StartCoroutine(MoveRepeatedly());
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

    IEnumerator MoveRepeatedly()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(delayBetweenMovements);

                transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
                    .SetEase(Ease.Linear);

                yield return new WaitForSeconds(2f); // 1초 대기

                transform.DOMoveY(transform.position.y - moveDistance, moveDuration)
                    .SetEase(Ease.Linear);
            }
        }
    }

    void playerDamage(int damageAmount)
    {
        
    }
}
