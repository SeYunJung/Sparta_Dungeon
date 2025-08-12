using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("�����ӿ� �ʿ��� ������")]
    public float moveSpeed;
    public float minZPos = 4;
    public float maxZPos = 12;
    private Vector3 _movementDirection;
    private Rigidbody _rigid;
    private BoxCollider _collider;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _movementDirection = Vector3.forward;
        _movementDirection *= moveSpeed;
    }

    private void FixedUpdate()
    {
        _rigid.velocity = _movementDirection;

        // +�������� ���� ������ maxZPos�� �Ѿ�� 
        if (_movementDirection.z > 0 && transform.position.z >= maxZPos)
        {
            // �̵����� ���� 
            _movementDirection *= -1.0f;
        }

        // -�������� ���� ������ minZPos�� �Ѿ��  
        else if (_movementDirection.z < 0 && transform.position.z <= minZPos)
        {
            _movementDirection *= -1.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }
}
