using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶� �÷��̾� ��ġ �̵�
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform camPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camPos.position;
    }
}
