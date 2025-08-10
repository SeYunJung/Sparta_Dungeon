using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     * 플레이어 이동
     * 
     * 필요한 것들 
     * - 리지드바디
     * - 이동속도
     * - 이동 애니메이션 -> 애니메이션만 관리하는 클래스를 만들어서 애니메이션을 동작시키자. (중요 X)
     * - 이동 방향 벡터 
     */
    [Header("이동 변수들")]
    private Rigidbody _rigid;
    public float moveSpeed;
    public float jumpPower;
    private Vector2 _inputMovementDirection;
    private Vector3 _movementDirection;
    public LayerMask groundLayer;

    [Header("레이(Ray) 변수")]
    public float heightOffset = 0.5f; // 아래로 쏘는 레이 길이
    public float horizontalOffset = 0.2f; // 앞으로 쏘는 레이 길이 
    public float groundCheckRayDistance = 0.6f; // 레이 길이 

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // 이동방향 설정 
        _movementDirection = transform.forward * _inputMovementDirection.y + transform.right * _inputMovementDirection.x;

        // 이동속도 설정 
        _movementDirection *= moveSpeed;

        // 중력 설정 
        _movementDirection.y = _rigid.velocity.y;

        // 이동
        _rigid.velocity = _movementDirection;
    }

    // WASD키를 누르면 실행되는 메서드 
    public void OnMove(InputAction.CallbackContext context)
    {
        /*
         * InputActionPhase.Started : WASD키를 눌렀을 때 (한번)
         * InputActionPhase.Performed : WASD키를 누르고 있을 때
         * InputActionPhase.Canceled : WASD키를 뗐을 때 
         */

        // WASD키를 누르고 있으면 
        if(context.phase == InputActionPhase.Performed)
        {
            _inputMovementDirection = context.ReadValue<Vector2>();
        }

        // WASD키를 뗐을 때 
        else if(context.phase == InputActionPhase.Canceled)
        {
            _inputMovementDirection = Vector2.zero;
        }
    }

    // Space bar를 누르면 실행되는 메서드
    public void OnJump(InputAction.CallbackContext context)
    {
        // Space bar를 누르고, 땅에 닿지 않으면
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            // 플레이어 위치에서 위쪽 방향으로 순간적인 힘을 줘서 점프하게 하기 
            _rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    // 플레이어가 땅에 있는지 없는지 확인하는 bool 메서드 
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // 플레이어 앞뒤좌우 기준으로 아래쪽으로 레이 쏘기
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (transform.forward * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (-transform.forward * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (transform.right * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (-transform.right * horizontalOffset), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // ray를 0.1크기로 쏜다. groundLayerMask만 감지한다. 탐지되면 true 반환
            if (Physics.Raycast(rays[i], groundCheckRayDistance, groundLayer))
            {
                return true;
            }
        }

        return false;
    }
}
