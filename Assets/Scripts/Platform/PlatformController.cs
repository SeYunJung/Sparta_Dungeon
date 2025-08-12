using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("움직임에 필요한 변수들")]
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

        // +방향으로 가고 있으며 maxZPos를 넘어서면 
        if (_movementDirection.z > 0 && transform.position.z >= maxZPos)
        {
            // 이동방향 반전 
            _movementDirection *= -1.0f;
        }

        // -방향으로 가고 있으며 minZPos를 넘어서면  
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
