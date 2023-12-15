using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public GameObject arrow;

    float fireTime = 0.1f;

    float fireForce = 70.0f;
    float throwUpwardForce;

    bool readyToFire = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0) && readyToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        if ( !DefenceGameManager.instance.isPlaying )
            return;

        readyToFire = false;

        Quaternion arrowQuat = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y - 90.0f, -Camera.main.transform.rotation.eulerAngles.x - 90.0f));

        //�ν��Ͻ�ȭ
        GameObject arrowTemp = Instantiate(arrow, attackPoint.position, arrowQuat);
        Rigidbody arrowRigid = arrowTemp.GetComponent<Rigidbody>();

        //Raycast�� ȭ���� ���⺤�͸� ���
        Vector3 forceDirection = Camera.main.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //ȭ�쿡 ������ ��
        Vector3 forceToAdd = forceDirection * fireForce + transform.up * throwUpwardForce;

        //ȭ���� Rigidbody�� ���� ����
        arrowRigid.AddForce(forceToAdd, ForceMode.Impulse);

        Invoke(nameof(ResetFire), fireTime);
    }

    void ResetFire()
    {
        readyToFire = true;
    }
}
