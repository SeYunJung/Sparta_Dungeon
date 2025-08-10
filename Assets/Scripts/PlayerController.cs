using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     * �÷��̾� �̵�
     * 
     * �ʿ��� �͵� 
     * - ������ٵ�
     * - �̵��ӵ�
     * - �̵� �ִϸ��̼� -> �ִϸ��̼Ǹ� �����ϴ� Ŭ������ ���� �ִϸ��̼��� ���۽�Ű��. (�߿� X)
     * - �̵� ���� ���� 
     */
    [Header("�̵� ������")]
    private Rigidbody _rigid;
    public float moveSpeed;
    public float jumpPower;
    private Vector2 _inputMovementDirection;
    private Vector3 _movementDirection;
    public LayerMask groundLayer;

    [Header("����(Ray) ����")]
    public float heightOffset = 0.5f; // �Ʒ��� ��� ���� ����
    public float horizontalOffset = 0.2f; // ������ ��� ���� ���� 
    public float groundCheckRayDistance = 0.6f; // ���� ���� 

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
        // �̵����� ���� 
        _movementDirection = transform.forward * _inputMovementDirection.y + transform.right * _inputMovementDirection.x;

        // �̵��ӵ� ���� 
        _movementDirection *= moveSpeed;

        // �߷� ���� 
        _movementDirection.y = _rigid.velocity.y;

        // �̵�
        _rigid.velocity = _movementDirection;
    }

    // WASDŰ�� ������ ����Ǵ� �޼��� 
    public void OnMove(InputAction.CallbackContext context)
    {
        /*
         * InputActionPhase.Started : WASDŰ�� ������ �� (�ѹ�)
         * InputActionPhase.Performed : WASDŰ�� ������ ���� ��
         * InputActionPhase.Canceled : WASDŰ�� ���� �� 
         */

        // WASDŰ�� ������ ������ 
        if(context.phase == InputActionPhase.Performed)
        {
            _inputMovementDirection = context.ReadValue<Vector2>();
        }

        // WASDŰ�� ���� �� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            _inputMovementDirection = Vector2.zero;
        }
    }

    // Space bar�� ������ ����Ǵ� �޼���
    public void OnJump(InputAction.CallbackContext context)
    {
        // Space bar�� ������, ���� ���� ������
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            // �÷��̾� ��ġ���� ���� �������� �������� ���� �༭ �����ϰ� �ϱ� 
            _rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    // �÷��̾ ���� �ִ��� ������ Ȯ���ϴ� bool �޼��� 
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // �÷��̾� �յ��¿� �������� �Ʒ������� ���� ���
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (transform.forward * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (-transform.forward * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (transform.right * horizontalOffset), Vector3.down),
            new Ray(transform.position + new Vector3(0, heightOffset, 0) + (-transform.right * horizontalOffset), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // ray�� 0.1ũ��� ���. groundLayerMask�� �����Ѵ�. Ž���Ǹ� true ��ȯ
            if (Physics.Raycast(rays[i], groundCheckRayDistance, groundLayer))
            {
                return true;
            }
        }

        return false;
    }
}
