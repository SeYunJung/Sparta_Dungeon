using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("������ �������� ������")]
    public float checkRate = 0.05f; // �󸶳� ���� Update�ؼ� �������� -> 0.05�ʸ��� Ray�� ���� �������� �����Ѵ�. 
    public float maxCheckDistance; // �󸶳� �ָ� �ִ� ���� üũ���� -> �÷��̾� �þ߿��� ������ ���� �ִ�Ÿ� 
    public LayerMask layerMask; // � ���̾ �������� -> 
    private float lastCheckTime; // ���������� üũ�� �ð�  -> Ray�� ���������� �������� ������ �ð�
    // -> ������ ���� �ð��� ����ð� ���̰� checkRate���� ũ�� Ray�� �� �� �ְ� �Ѵ�. 

    [Header("���� ��ȣ�ۿ� ������ ���� ������Ʈ�� �������̽��� ������ ������Ʈ")]
    public GameObject curInteractGameObject; // �÷��̾ �ٶ󺸰� �ִ� ��ȣ�ۿ� ������ ������Ʈ
    private IInteractable curInteractable; // IInteractable �������̽��� ������ ����(ItemObject). ���� ��ȣ�ۿ��ϰ� �ִ� ������ 

    [Header("�ؽ�Ʈ UI ������")]
    public TextMeshProUGUI promptText; // ȭ�鿡 ��� Text UI

    private Camera camera; // ī�޶� 
    // => Camera���� ScreenPointToRay��� �޼��尡 �ִ�. �� �޼���� ī�޶󿡼� Ray�� �� �� �ִ�. 
    // �׷��� Camera�� ����ؾ� ��. 

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ī�޶� �������� Ray ���. ��ũ���� �ʺ��� ����, ��ũ�� ���� ����(���߾����� ����). �߽����� ������. ī�޶� ��� ������ �⺻������ ����Ǿ� �־ ������ �������ص� �ȴ�. 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            // �ε��� ������Ʈ ������ ���� RaycastHit
            RaycastHit hit;

            // ���̸� ���� �浹�� ��ü�� ������ hit�� ������ �ѱ��. ���̴� maxChee... ���̾��ũ ���� 
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // �浹�Ǹ� 
            {
                // �浹�� ������Ʈ�� ���� ��ȣ�ۿ� ���� ������Ʈ�� ���� ������ -> ���ο� �����۰� �浹�ϸ� 
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // ���ο� ������ �ִ´�. 
                    curInteractGameObject = hit.collider.gameObject; // ���� ��ȣ�ۿ� ���� ������Ʈ�� �浹�� ������Ʈ�� ���� 
                    curInteractable = hit.collider.GetComponent<IInteractable>(); // �浹�� ������Ʈ�� ������ ����?�� ����

                    // ������Ʈ�� ���
                    SetPromptText();
                }
            }

            // �浹�Ǵ� �������� ������ 
            else
            {
                // ���� ���ֱ� 
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
        
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); // UI�� Ű�� 
        promptText.text = curInteractable.GetInteractPrompt(); // ItemObject���� ������ �̸�, ������ ��ȯ 
    }

    // EŰ�� ������ ����Ǵ� �޼��� 
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // ������ ��, ���� ������ �������� �ٶ󺸰� ���� ��(���� ��ȣ�ۿ� ������ ������ ������)
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // �÷��̾�� ������ ���� �Ѱ��ֱ�, ��������Ʈ ����, �ʿ��� ������� �ϱ� 
            curInteractable.OnInteract();
            curInteractGameObject = null; // ��ȣ�ۿ�� ���ӿ�����Ʈ ���� 
            curInteractable = null; // ���� ��ȣ�ۿ��ϰ� �ִ� ������ ���� 
            promptText.gameObject.SetActive(false);
        }
    }
}
