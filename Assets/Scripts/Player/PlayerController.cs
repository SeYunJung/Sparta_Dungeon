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
    public float moveSpeed;
    public float jumpPower;
    public LayerMask groundLayer;
    private Rigidbody _rigid;
    private Vector2 _inputMovementDirection;
    private Vector3 _movementDirection;

    [Header("레이(Ray) 변수")]
    public float heightOffset = 0.5f; // 레이 위치 변수
    public float horizontalOffset = 0.2f; // 레이 위치 변수
    public float groundCheckRayDistance = 0.6f; // 레이 길이 

    // 캐릭터 회전에 필요한 변수들 
    [Header("카메라 변수들")]
    public Transform cameraContainer;
    public float minXLook; // x회전 최소 범위 
    public float maxXLook; // x회전 최대 범위 
    private float camCurXRot; // 카메라 위아래 회전값 
    public float lookSensitivity; // 회전 민감도
    private Vector2 mouseDelta; // 마우스의 델타값(이동변화량)

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 게임이 시작되면 커서를 보이지 않게 설정 
        Cursor.lockState = CursorLockMode.Locked;

        // 게임이 처음 시작하면 카메라가 땅을 보고있다.
        // 정면을 보게 하기 위해서 카메라 초기값을 세팅해주자. 
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

    // 마우스가 회전되면 실행되는 메서드
    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스는 시작하고 진행중 취소를 따질 필요없다. 
        // 그냥 마우스 좌표값만 읽어오면 된다. 
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        /*
         * mouseDelta = 마우스 변화량 = 마우스 좌표
         * mouseDelta.x = 마우스 좌우 좌표
         * mouseDelta.y = 마우스 위아래 좌표 
         */

        camCurXRot += mouseDelta.y * lookSensitivity; // 카메라 위아래 회전값 설정 
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 회전값이 minXLook ~ maxXLook에서 벗어나지 않게 제한

        // 카메라에 위아래 회전 적용 (마우스를 위아래로 이동하면 카메라가 위아래로 회전) 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        /*
         * 왜 -camCurXRot일까? 
         * 유니티에서는 x축 회전을 증가시키면 카메라가 아래를 보도록 설정되어 있다.
         * 카메라를 위로 회전해야 한다면 (-camCurXRot, 0, 0) 으로 회전을 해야한다. 
         */

        // 플레이어 좌우 회전 적용 (마우스를 좌우로 이동하면 플레이어가 좌우로 회전)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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
