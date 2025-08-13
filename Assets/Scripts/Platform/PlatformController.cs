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

    public Transform playerTransform;
    public Vector3 playerPrePos;
    public float diff;

    public bool onPlatform;

    //public PlayerController playerController;
    //public Rigidbody playerRigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _movementDirection = Vector3.forward;
        _movementDirection *= moveSpeed;
    }

    private void Update()
    {
        //_rigid.velocity = _movementDirection;
        //transform.position += _movementDirection;
        transform.position += _movementDirection * Time.deltaTime;

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

        //if (onPlatform)
        //{
        //    diff = Vector3.Distance(playerPrePos, transform.position); // 플레이어와 발판 사이 거리를 구해서
        //    // 이 차이 거리만큼으로 플레이어의 위치를 계속 이동

        //    //if(playerTransform != null)
        //    //{
        //    //    playerTransform.position = playerPrePos + new Vector3(0, 0, diff);
        //    //    //playerRigid.velocity = playerPrePos + new Vector3(0, 0, diff);
        //    //}
        //    playerTransform.position = playerPrePos + new Vector3(0, 0, diff);
        //}


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //// 이전위치 저장
            //// 현재위치랑 비교해서 플레이어를 계속 이동. 
            ////collision.gameObject.transform.SetParent(transform);

            //onPlatform = true;
            //playerTransform = collision.transform;
            ////playerController = collision.gameObject.GetComponent<PlayerController>();
            ////playerRigid = playerController.Rigid;
            //playerPrePos = collision.gameObject.transform.position;

            ////diff = Vector3.Distance(playerPrePos, transform.position);
            ////playerTransform.position = playerPrePos + new Vector3(0, 0, diff);
            collision.gameObject.transform.SetParent(this.gameObject.transform);
            //Debug.Log($"{transform.localScale} {transform.lossyScale}");
            //Debug.Break();
            //collision.gameObject.transform.localScale = Vector3.one; // scale 은 곱셉으로 
            //collision.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            //collision.gameObject.transform.localPosition = new Vector3(0, 0, 5.0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //onPlatform = false;
            collision.gameObject.transform.SetParent(null);
            collision.gameObject.transform.localScale = Vector3.one;
        }
    }
}
