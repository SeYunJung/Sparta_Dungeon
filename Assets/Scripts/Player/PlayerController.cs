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
    public float moveSpeed;
    public float jumpPower;
    public LayerMask groundLayer;
    private Rigidbody _rigid;
    private Vector2 _inputMovementDirection;
    private Vector3 _movementDirection;

    [Header("����(Ray) ����")]
    public float heightOffset = 0.5f; // ���� ��ġ ����
    public float horizontalOffset = 0.2f; // ���� ��ġ ����
    public float groundCheckRayDistance = 0.6f; // ���� ���� 

    // ĳ���� ȸ���� �ʿ��� ������ 
    [Header("ī�޶� ������")]
    public Transform cameraContainer;
    public float minXLook; // xȸ�� �ּ� ���� 
    public float maxXLook; // xȸ�� �ִ� ���� 
    private float camCurXRot; // ī�޶� ���Ʒ� ȸ���� 
    public float lookSensitivity; // ȸ�� �ΰ���
    private Vector2 mouseDelta; // ���콺�� ��Ÿ��(�̵���ȭ��)

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ������ ���۵Ǹ� Ŀ���� ������ �ʰ� ���� 
        Cursor.lockState = CursorLockMode.Locked;

        // ������ ó�� �����ϸ� ī�޶� ���� �����ִ�.
        // ������ ���� �ϱ� ���ؼ� ī�޶� �ʱⰪ�� ����������. 
        //cameraContainer.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void LateUpdate()
    {
        CameraLook();
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

    // ���콺�� ȸ���Ǹ� ����Ǵ� �޼���
    public void OnLook(InputAction.CallbackContext context)
    {
        // ���콺�� �����ϰ� ������ ��Ҹ� ���� �ʿ����. 
        // �׳� ���콺 ��ǥ���� �о���� �ȴ�. 
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        /*
         * mouseDelta = ���콺 ��ȭ�� = ���콺 ��ǥ
         * mouseDelta.x = ���콺 �¿� ��ǥ
         * mouseDelta.y = ���콺 ���Ʒ� ��ǥ 
         */

        camCurXRot += mouseDelta.y * lookSensitivity; // ī�޶� ���Ʒ� ȸ���� ���� 
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // ȸ������ minXLook ~ maxXLook���� ����� �ʰ� ����

        // ī�޶� ���Ʒ� ȸ�� ���� (���콺�� ���Ʒ��� �̵��ϸ� ī�޶� ���Ʒ��� ȸ��) 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        /*
         * �� -camCurXRot�ϱ�? 
         * ����Ƽ������ x�� ȸ���� ������Ű�� ī�޶� �Ʒ��� ������ �����Ǿ� �ִ�.
         * ī�޶� ���� ȸ���ؾ� �Ѵٸ� (-camCurXRot, 0, 0) ���� ȸ���� �ؾ��Ѵ�. 
         */

        // �÷��̾� �¿� ȸ�� ���� (���콺�� �¿�� �̵��ϸ� �÷��̾ �¿�� ȸ��)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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
