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
    private ItemData _selectedItem;
    private int _selectedItemIndex = 0;

    [Header("�÷��̾� ���� ���� ������")]
    private CharacterManager _characterManager;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;
    public Transform dropItemPosition;

    void Start()
    {
        // �÷��̾� ���� ���� ������ �������� 
        _characterManager = CharacterManager.Instance;
        _playerController = _characterManager.Player.playerController;
        _playerCondition = _characterManager.Player.playerCondition;
        dropItemPosition = _characterManager.Player.dropItemPosition;

        _playerController.inventoryAction += Toggle;
        _characterManager.Player.addItem += AddItem; // Player�� ��������Ʈ. -> �������� ������ �κ��丮�� ������ִ� ��������Ʈ.

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

    // �κ��丮�� ������ �߰� �޼��� 
    private void AddItem()
    {
        // �÷��̾ ������ �ִ� ������ ���� �޾ƿ��� 
        ItemData itemData = _characterManager.Player.itemData;

        // �������� �ߺ� �������� canStack���� üũ
        if (itemData.canStack)
        {
            // ������ ���� �������� 
            ItemSlot itemSlot = GetItemStack(itemData);

            // ������ ������ ������ -> ���� ������ ������ 
            if(itemSlot != null)
            {
                itemSlot.quantity++; // ���Կ��� ������ ���� �ø��� 
                UpdateUI();
                _characterManager.Player.itemData = null; // �÷��̾��� ������ �����ʹ� null�� => ������ ���Կ� �־�����
                return;
            }
        }

        // �ߺ������� �������� �ƴ϶�� ����ִ� ������ �����´�. 
        ItemSlot emptySlot = GetEmptySlot(); // �� ������ ������ null��ȯ 

        // ����ִ� ������ �ִٸ� 
        if(emptySlot != null)
        {
            // �� ���Կ� �����۵����� �ֱ� 
            emptySlot.itemData = itemData;

            // ������ ���� 1��
            emptySlot.quantity = 1;

            // �κ��丮 UI ������Ʈ 
            UpdateUI();

            // �÷��̾��� ������ �����ʹ� null�� => ������ ���Կ� �־�����
            _characterManager.Player.itemData = null;

            return;
        }

        // ����ִ� ������ ���ٸ� -> �Ĺ��� �������� ������ ��. 
        ThrowItem(itemData);

        // �÷��̾��� ������ ������ null�� ���� ������ ������ �ʱ�ȭ 
        _characterManager.Player.itemData = null;
    }

    // 
    private void UpdateUI()
    {
        // �κ��丮�� �ִ� ���Ե��� �ϳ��� ��ȸ�ؼ� 
        for(int i = 0; i < slots.Length; i++)
        {
            // ���Կ� �����Ͱ� ������ 
            if (slots[i].itemData != null)
            {
                // ���� ���� (���̰� �ϰų� �ܰ��� ���̰� �ϴ� �� ��)
                slots[i].Set();
            }

            // ���Կ� �����Ͱ� ������ 
            else
            {
                // ���� ����
                slots[i].Clear();
            }
        }
    }

    // ���Կ��� itemData(������) �������� �޼��� -> �������� �κ��丮�� �߰��ϱ� ���� �� �������� �κ��丮
    // �� ���� �� �ִ��� �˻�. -> ItemSlot�� ��ȯ�ϸ� ���Կ� �ش� �������� ���� ������ �ִٴ� ��.
    // null�� ��ȯ�ϸ� ���Կ� �ش� �������� ���� ������ ���ٴ� �� => �ִ� �������� ������ �ֱ� ���� 
    private ItemSlot GetItemStack(ItemData itemData)
    {
        // ������ �ϳ��� ��ȸ�ؼ� 
        for(int i = 0; i < slots.Length; i++)
        {
            // �ش� ���Կ� �ִ� �������� ���� �߰��Ϸ��� ������(itemData)�̸鼭 
            // ���Կ� �ִ� �������� ���� �ִ�������� ����� ������ 
            if (slots[i].itemData == itemData && slots[i].quantity < itemData.maxStackAmount)
            {
                // �ش� ������ ������ ��ȯ���༭, ���Կ� ������ ���� �ø� �� �ְ� ���ش�. 
                return slots[i];
            }
        }

        // ���� �����۰� ���������� �������� ������ ���Կ� �ִ� �ش� �������� �ִ뷮�� ������ ������
        return null;
    }

    // �������� ���� �� �ִ� �� ������ ã�Ƽ� ��ȯ���ִ� �޼��� 
    private ItemSlot GetEmptySlot()
    {
        // ���Ե��� ��ȸ�ؼ� 
        for(int i = 0; i < slots.Length; i++)
        {
            // ���Կ� �������� ������ 
            if (slots[i].itemData == null)
            {
                // �������� ���� �� ������ ��ȯ 
                return slots[i];
            }
        }

        // ����� �Դٴ� �� �� ������ ������ ���ٴ� ��
        // �� ������ ������ ���ٴ� �ǹ̷� null�� ��ȯ 
        return null;
    }

    // �������� �Դ� ���� Destroy�� �������� ������.
    // �������� �Ծ��µ� �� ������ ������ ������ ��. �� �������� ���� ��ġ�� ��ġ���Ѿ� ��.
    // ����� �������� �ٽ� �����ؼ� ���� �ڸ��� ��ġ�����ִ� �޼��� 
    private void ThrowItem(ItemData itemData)
    {
        // ���������� ���� ������ �����, ��ġ�� dropPosition,
        // 360�� * (0 ~ 1���� ������) ������. ���� ȸ������ ������Ѽ� �������� ����
        Instantiate(itemData.dropPrefab, dropItemPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // ������ �����ϸ� ���� ��ȣ�� �����ؼ� ��������
    public void SelectItem(int index)
    {
        // index��° ���Կ� �������� ������ ����
        if (slots[index].itemData == null) return;

        _selectedItem = slots[index].itemData; // ���õ� �������� �������� 
        _selectedItemIndex = index; // ������ ��ȣ ����

        selectedItemName.text = _selectedItem.displayName; // �̸���
        selectedItemDescription.text = _selectedItem.description; // ������ ���� ���� 

        // ����(ü��, ���� ���� �͵�)���� ����
        // ��� �������� ������ ������ ���� ����. 
        // �׷��� �ϴ� ������ ���ڿ��� ����
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // ���õ� �������� �Һ񰡴��� �������̸�, � �Һ� ���������� ��ŭ ȸ�������ִ� ������������
        // UI�� ������Ʈ ���ش�. -> ���õ� �������� ������ ǥ�� 
        for (int i = 0; i < _selectedItem.consumables.Length; i++)
        {
            // �������� Ÿ��(ü�� ȸ��, ����� ȸ��)�� ���� ���� �� ����.
            // �׷��� consumables �迭�� ���� ������ Ÿ�԰� ���� �����ص�. 
            // ���� Ÿ���� �������� �ִ� ��� �ؽ�Ʈ�� ������ �ٿ��ֱ� ���� +=�����ڸ� ���
            selectedStatName.text += _selectedItem.consumables[i].consumableType.ToString() + "\n";
            selectedStatValue.text += _selectedItem.consumables[i].value.ToString() + "\n";
        }

        // (��ư Ȱ��ȭ/��Ȱ��ȭ)
        // ����ϴ� �������� ��� ��ư�� Ȱ��ȭ
        useButton.SetActive(_selectedItem.itemType == ItemType.Consumable);
        // ������ �������� ���� Ÿ���̰� ������ �� �Ǿ� �������� ���� ��ư�� Ȱ��ȭ
        equipButton.SetActive(_selectedItem.itemType == ItemType.Equipable && !slots[index].isEquipped);
        // ������ �������� ���� Ÿ���̰� ������ �Ǿ� ���� ���� ���� ��ư ��Ȱ��ȭ
        unequipButton.SetActive(_selectedItem.itemType == ItemType.Equipable && slots[index].isEquipped);
        // ������ ��ư�� Ȱ��ȭ 
        dropButton.SetActive(true);
    }

    // ����ϱ�
    public void OnUseButton()
    {
        // ���õ� �������� �Һ� Ÿ���� �� => �� �Һ� Ÿ���϶��� ����? �Һ� �����۸� "���"�� �� �����ϱ� 
        if (_selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < _selectedItem.consumables.Length; i++)
            {
                // �� �������� �Һ� Ÿ�Կ� ���� ü���� ȸ���ϰų� ������� ȸ���Ѵ�. 
                switch (_selectedItem.consumables[i].consumableType)
                {
                    case ConsumableType.Health:
                        _playerCondition.Heal(_selectedItem.consumables[i].value); // ü�� ȸ�� 
                        break;
                    case ConsumableType.Hunger:
                        //_playerCondition.Eat(_selectedItem.consumables[i].value); // ����� ȸ�� 
                        break;
                    case ConsumableType.Speed:
                        _playerCondition.SpeedUp(_selectedItem.consumables[i].value, _characterManager.Player.playerController); // ���ǵ� �� 
                        break;
                }
            }

            // �Һ� �������� ��������� �κ��丮 ����â(�κ��丮â)���� �����ش�. 
            RemoveSelectedItem();
        }
    }

    // ������
    public void OnDropButton()
    {
        // ������ �޼��� ������ �����Ѱ� ȣ�� 
        ThrowItem(_selectedItem);
        // ������ ������ ����
        RemoveSelectedItem();
    }

    // �������� ������ UI�� ������Ʈ �������. 
    void RemoveSelectedItem()
    {
        slots[_selectedItemIndex].quantity--; // ���Կ��� �ش� ������ �� ���̱� => �����ϱ�. 

        // ���� 0�̰ų� �׺��� �۾�����
        if (slots[_selectedItemIndex].quantity <= 0)
        {
            _selectedItem = null; // ���� �������� null
            slots[_selectedItemIndex].itemData = null; // ����(���� ������ �κ��丮 ����)�� �ִ� �������� null��
            _selectedItemIndex = -1; // ������ �ε����� -1�� ���� => �Ƹ� ���� �������� �ε����� -1�� �����Ϸ��� -1�� �ʱ�ȭ�ѵ�
            ClearSelectedItemWindow(); // ������ Ŭ���� ǥ�õǴ� ������ �ʱ�ȭ
        }

        UpdateUI(); // �κ��丮 UI ������Ʈ 
    }
}
