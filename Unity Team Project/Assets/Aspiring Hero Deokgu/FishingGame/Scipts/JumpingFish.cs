using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFish : MonoBehaviour
{
    public bool delay; // ������ ����
    public Vector2 TrandomMinMax = new Vector2(3, 7); // ������ ������ �ð� ����
    public float randomTime; // ���� ���� ������

    public Vector2 xMinMax = new Vector2(0, 0); // x ��ǥ �̵� ����
    public Vector2 zMinMax = new Vector2(0, 0); // z ��ǥ �̵� ����
    public Vector3 randomPos; // ������ ��ġ


    void Start()
    {
        // �ʱ� ����
        randomTime = Random.Range(1.5f, (int) TrandomMinMax.y);

        randomPos = transform.position;
        randomPos.x = Random.Range(xMinMax.x, xMinMax.y);
        randomPos.z = Random.Range(zMinMax.x, zMinMax.y);

        transform.position = randomPos;

        delay = true;
    }


    void Update()
    {
        if (delay)
        {
            // ������ ���� ��
            if (randomTime >= 0)
            {
                randomTime -= Time.deltaTime;
            }
            else
            {
                // �����̰� ������ ���ο� ������ ��ġ�� ������ ����
                randomTime = Random.Range((int) TrandomMinMax.x, (int) TrandomMinMax.y);
                if (randomTime < 1)
                {
                    randomTime = Random.Range((int) TrandomMinMax.x, (int) TrandomMinMax.y);
                }

                randomPos.x = Random.Range(xMinMax.x, xMinMax.y);
                randomPos.z = Random.Range(zMinMax.x, zMinMax.y);
                transform.position = randomPos;

                Vector3 euler = transform.eulerAngles;
                euler.y = Random.Range(0f, 360f);
                transform.eulerAngles = euler;

                // �ִϸ��̼� Ʈ���� Ȱ��ȭ �� ������ ���� ��ȯ
                transform.GetComponent<Animator>().SetBool("Jumping", true);
                delay = false;
            }
        }
    }

    public void EndAnimation()
    {
        // �ִϸ��̼� ���� �� ������ ���� ��ȯ
        transform.GetComponent<Animator>().SetBool("Jumping", false);
        delay = true;
    }
}
