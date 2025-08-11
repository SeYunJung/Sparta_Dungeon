using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("아이템 감지관련 변수들")]
    public float checkRate = 0.05f; // 얼마나 자주 Update해서 검출할지 -> 0.05초마다 Ray를 쏴서 아이템을 감지한다. 
    public float maxCheckDistance; // 얼마나 멀리 있는 것을 체크할지 -> 플레이어 시야에서 아이템 감지 최대거리 
    public LayerMask layerMask; // 어떤 레이어를 감지할지 -> 
    private float lastCheckTime; // 마지막으로 체크한 시간  -> Ray가 마지막으로 아이템을 감지한 시간
    // -> 마지막 감지 시간과 현재시간 차이가 checkRate보다 크면 Ray를 쏠 수 있게 한다. 

    [Header("현재 상호작용 가능한 게임 오브젝트와 인터페이스를 구현한 오브젝트")]
    public GameObject curInteractGameObject; // 플레이어가 바라보고 있는 상호작용 가능한 오브젝트
    private IInteractable curInteractable; // IInteractable 인터페이스를 구현한 무언가(ItemObject). 현재 상호작용하고 있는 아이템 

    [Header("텍스트 UI 변수들")]
    public TextMeshProUGUI promptText; // 화면에 띄울 Text UI

    private Camera camera; // 카메라 
    // => Camera에는 ScreenPointToRay라는 메서드가 있다. 이 메서드로 카메라에서 Ray를 쏠 수 있다. 
    // 그래서 Camera를 사용해야 함. 

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 카메라 기준으로 Ray 쏘기. 스크린의 너비의 절반, 스크린 높이 절반(정중앙으로 세팅). 중심점만 세팅함. 카메라가 찍는 방향이 기본적으로 적용되어 있어서 방향을 설정안해도 된다. 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            // 부딪힐 오브젝트 정보를 담을 RaycastHit
            RaycastHit hit;

            // 레이를 쏴서 충돌된 물체가 있으면 hit에 정보를 넘기기. 길이는 maxChee... 레이어마스크 설정 
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // 충돌되면 
            {
                // 충돌한 오브젝트와 현재 상호작용 중인 오브젝트가 같지 않으면 -> 새로운 아이템과 충돌하면 
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // 새로운 정보를 넣는다. 
                    curInteractGameObject = hit.collider.gameObject; // 현재 상호작용 중인 오브젝트를 충돌한 오브젝트로 변경 
                    curInteractable = hit.collider.GetComponent<IInteractable>(); // 충돌한 오브젝트의 아이템 정보?를 저장

                    // 프롬프트에 출력
                    SetPromptText();
                }
            }

            // 충돌되는 아이템이 없으면 
            else
            {
                // 정보 없애기 
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
        
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); // UI를 키고 
        promptText.text = curInteractable.GetInteractPrompt(); // ItemObject에서 아이템 이름, 설명을 반환 
    }

    // E키를 누르면 실행되는 메서드 
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // 눌렸을 때, 현재 에임이 아이템을 바라보고 있을 때(현재 상호작용 아이템 정보가 있을때)
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // 플레이어에게 아이템 정보 넘겨주기, 델리게이트 실행, 맵에서 사라지게 하기 
            curInteractable.OnInteract();
            curInteractGameObject = null; // 상호작용된 게임오브젝트 비우기 
            curInteractable = null; // 현재 상호작용하고 있는 아이템 비우기 
            promptText.gameObject.SetActive(false);
        }
    }
}
