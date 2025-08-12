using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("������ ���� ���� ������")]
    public ItemSlot[] slots; // ������ ���Ե� ����
    public GameObject inventoryWindow; // �κ��丮 â (���� �״��ϱ�)
    public Transform slotPanel; // ? -> �Ƹ��� slots ������Ʈ Transform�ε� 

    [Header("���õ� ������ ���� ���� ������")]
    public TextMeshProUGUI selectedItemName; // ������ ������ �̸�
    public TextMeshProUGUI selectedItemDescription; // ������ ������ ����
    public TextMeshProUGUI selectedStatName; // ������ ������ �̸�
    public TextMeshProUGUI selectedStatValue; // 
    public GameObject useButton; // ��ư ���� ������Ʈ => ���� ���ֱ⸸ �� �뵵 
    public GameObject equipButton; 
    public GameObject unequipButton; 
    public GameObject dropButton;

    [Header("�÷��̾� ���� ���� ������")]
    private CharacterManager _characterManager;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;

    void Start()
    {
        // �÷��̾� ���� ���� ������ �������� 
        _characterManager = CharacterManager.Instance;
        _playerController = _characterManager.Player.playerController;
        _playerCondition = _characterManager.Player.playerCondition;

        _playerController.inventoryAction += Toggle;

        // �κ��丮 â�� ó������ ���� 
        inventoryWindow.SetActive(false);

        // slots ������Ʈ ���� ������Ʈ ����ũ��(���� ������ŭ)�� slots(������ ���� ������) �迭 ����
        slots = new ItemSlot[slotPanel.childCount];

        // ���� ���� �ʱ�ȭ 
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>(); // �θ�(slotPanel)�κ��� �ڽ�(ItemSlot) ��������
            slots[i].slotIndex = i; // ���° �������� ���� 
            slots[i].inventory = this; // ����(slots[i])���� �θ�(UIInventory)�� �˷��ֱ� => ItemSlot�� �θ� �˵��� �������� 
        }

        // ������ ������ �� ������, ��ư�� ��Ȱ��ȭ�� �ʱ�ȭ => �ʱ� ȯ�� ���� 
        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    // �������� Ŭ������ �� ǥ�õǴ� �����鿡 ���� �ʱ�ȭ -> �ʹ� ���Ƽ� �Լ��� ������. 
    private void ClearSelectedItemWindow()
    {
        // ǥ�õǴ� �������� ��� �� ���ڿ��� �ʱ�ȭ 
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // ��ư�� ��� ��Ȱ��ȭ 
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // �κ��丮�� ���ų� ���ִ� �޼��� 
    public void Toggle()
    {
        // �κ��丮 â�� ���� ���¿��� �κ��丮 Ű�� ������ �κ��丮 â�� ������ �ϰ�
        // �κ��丮 â�� ���� ���¿��� �κ��丮 Ű�� ������ �κ��丮 â�� ������ �Ѵ�. 
        // ���� �κ��丮 â�� ���ȴ��� �������� Ȯ���� �ؾ��Ѵ�. 
        // Ȯ�� �޼��带 ������. 

        // ���̾��Ű���� �κ��丮�� ���������� 
        if(IsOpen())
        {
            // �κ��丮 ���� 
            inventoryWindow.SetActive(false);
        }
        // �κ��丮�� ����������
        else
        {
            // �κ��丮 Ű��
            inventoryWindow.SetActive(true);
        }
    }

    // �κ��丮�� ���� �ִ��� �ƴ��� Ȯ���ϴ� �޼��� 
    public bool IsOpen()
    {
        // �κ��丮 ������Ʈ�� ���̾��Ű���� Ȱ��ȭ�Ǿ� ������ true�� �ƴϸ� false�� ��ȯ
        return inventoryWindow.activeInHierarchy; 
    }
}
