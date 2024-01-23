using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GobsAI : MonoBehaviour
{
    public enum STATE
    {
        PATROL,
        CHASE,
        DIE
    }

    public STATE state = STATE.PATROL;

    Animator anim;
    Transform PlayerT;
    Transform GobsT;
    GobsMoveAgent moveAgent;
    EnemyFOV enemyFOV;

    public bool isDie = false;
    public float chaseDis = 6.0f;
    public float rechaseDis = 5.0f;

    readonly int hashIsMove = Animator.StringToHash("IsMove");
    readonly int hashDie = Animator.StringToHash("IsDaed");

    private void Awake()
    {
        PlayerT = GameObject.FindWithTag("Player").GetComponent<Transform>();
        GobsT = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        moveAgent = GetComponent<GobsMoveAgent>();
        enemyFOV = GetComponent<EnemyFOV>();
    }

    void Start()
    {
        StartCoroutine(UpdateState());
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == STATE.DIE)
            {
                yield break;    //coroutine ����
            }

            float distance = Vector3.Distance(PlayerT.position, GobsT.position);
            //float distance = (PlayerT.position - GobsT.position).sqrMagnitude;   //���� ����� ������� ����ȭ�� ���

            if (distance <= chaseDis)
            {
                if (enemyFOV.IsChasePlayer())
                    state = STATE.CHASE;
            }
            else
            {
                state = STATE.PATROL;
            }
            yield return 0.3f;
        }
    }

    void patrol()
    {
        moveAgent.IsPatrol = true;
        anim.SetBool("IsMove", false);
    }

    void chase()
    {
        moveAgent.ChaseTarget = PlayerT.position;
        anim.SetBool("IsMove", true);
    }

    public void die()
    {
        anim.SetBool("IsMove", false);
        anim.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
    }

    IEnumerator UpdateState()
    {
        while (!isDie)
        {
            switch (state)
            {
                case STATE.PATROL:
                    patrol();
                    break;
                case STATE.CHASE:
                    chase();
                    break;
                case STATE.DIE:
                    die();
                    break;
            }
            yield return 0.3f;
        }
    }
}
