using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("아이템 관련 참조 변수들")]
    public ItemSlot[] slots; // 아이템 슬롯들 정보
    public GameObject inventoryWindow; // 인벤토리 창 (껐다 켰다하기)
    public Transform slotPanel; // ? -> 아마도 slots 오브젝트 Transform인듯 

    [Header("선택된 아이템 정보 관련 변수들")]
    public TextMeshProUGUI selectedItemName; // 선택한 아이템 이름
    public TextMeshProUGUI selectedItemDescription; // 선택한 아이템 설명
    public TextMeshProUGUI selectedStatName; // 선택한 아이템 이름
    public TextMeshProUGUI selectedStatValue; // 
    public GameObject useButton; // 버튼 게임 오브젝트 => 껐다 켜주기만 할 용도 
    public GameObject equipButton; 
    public GameObject unequipButton; 
    public GameObject dropButton;
    private ItemData _selectedItem;
    private int _selectedItemIndex = 0;

    [Header("플레이어 관련 참조 변수들")]
    private CharacterManager _characterManager;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;
    public Transform dropItemPosition;

    void Start()
    {
        // 플레이어 관련 참조 변수들 가져오기 
        _characterManager = CharacterManager.Instance;
        _playerController = _characterManager.Player.playerController;
        _playerCondition = _characterManager.Player.playerCondition;
        dropItemPosition = _characterManager.Player.dropItemPosition;

        _playerController.inventoryAction += Toggle;
        _characterManager.Player.addItem += AddItem; // Player의 델리게이트. -> 아이템을 먹으면 인벤토리에 등록해주는 델리게이트.

        // 인벤토리 창은 처음에는 끄기 
        inventoryWindow.SetActive(false);

        // slots 오브젝트 하위 오브젝트 개수크기(슬롯 개수만큼)로 slots(아이템 슬롯 정보들) 배열 생성
        slots = new ItemSlot[slotPanel.childCount];

        // 슬롯 정보 초기화 
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>(); // 부모(slotPanel)로부터 자식(ItemSlot) 가져오기
            slots[i].slotIndex = i; // 몇번째 슬롯인지 세팅 
            slots[i].inventory = this; // 슬롯(slots[i])에게 부모(UIInventory)를 알려주기 => ItemSlot은 부모를 알도록 설계했음 
        }

        // 아이템 정보는 빈 정보로, 버튼은 비활성화로 초기화 => 초기 환경 세팅 
        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    // 아이템을 클릭했을 때 표시되는 정보들에 대한 초기화 -> 너무 많아서 함수로 만들자. 
    private void ClearSelectedItemWindow()
    {
        // 표시되는 정보들은 모두 빈 문자열로 초기화 
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 버튼은 모두 비활성화 
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 인벤토리를 끄거나 켜주는 메서드 
    public void Toggle()
    {
        // 인벤토리 창이 열린 상태에서 인벤토리 키를 누르면 인벤토리 창이 꺼져야 하고
        // 인벤토리 창이 꺼진 상태에서 인벤토리 키를 누르면 인벤토리 창이 켜져야 한다. 
        // 따라서 인벤토리 창이 열렸는지 꺼졌는지 확인을 해야한다. 
        // 확인 메서드를 만들자. 

        // 하이어라키에서 인벤토리가 켜져있으면 
        if(IsOpen())
        {
            // 인벤토리 끄기 
            inventoryWindow.SetActive(false);
        }
        // 인벤토리가 꺼져있으면
        else
        {
            // 인벤토리 키기
            inventoryWindow.SetActive(true);
        }
    }

    // 인벤토리가 열여 있는지 아닌지 확인하는 메서드 
    public bool IsOpen()
    {
        // 인벤토리 오브젝트가 하이어라키에서 활성화되어 있으면 true를 아니면 false를 반환
        return inventoryWindow.activeInHierarchy; 
    }

    // 인벤토리에 아이템 추가 메서드 
    private void AddItem()
    {
        // 플레이어가 가지고 있는 아이템 정보 받아오기 
        ItemData itemData = _characterManager.Player.itemData;

        // 아이템이 중복 가능한지 canStack으로 체크
        if (itemData.canStack)
        {
            // 아이템 슬롯 가져오기 
            ItemSlot itemSlot = GetItemStack(itemData);

            // 아이템 슬롯이 있으면 -> 넣을 공간이 있으면 
            if(itemSlot != null)
            {
                itemSlot.quantity++; // 슬롯에서 아이템 양을 늘리고 
                UpdateUI();
                _characterManager.Player.itemData = null; // 플레이어의 아이템 데이터는 null로 => 아이템 슬롯에 넣었으니
                return;
            }
        }

        // 중복가능한 아이템이 아니라면 비어있는 슬롯을 가져온다. 
        ItemSlot emptySlot = GetEmptySlot(); // 빈 슬롯이 없으면 null반환 

        // 비어있는 슬롯이 있다면 
        if(emptySlot != null)
        {
            // 빈 슬롯에 아이템데이터 넣기 
            emptySlot.itemData = itemData;

            // 아이템 양은 1로
            emptySlot.quantity = 1;

            // 인벤토리 UI 업데이트 
            UpdateUI();

            // 플레이어의 아이템 데이터는 null로 => 아이템 슬롯에 넣었으니
            _characterManager.Player.itemData = null;

            return;
        }

        // 비어있는 슬롯이 없다면 -> 파밍한 아이템을 버려야 함. 
        ThrowItem(itemData);

        // 플레이어의 아이템 정보는 null로 만들어서 아이템 정보를 초기화 
        _characterManager.Player.itemData = null;
    }

    // 
    private void UpdateUI()
    {
        // 인벤토리에 있는 슬롯들을 하나씩 순회해서 
        for(int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 데이터가 있으면 
            if (slots[i].itemData != null)
            {
                // 슬롯 세팅 (보이게 하거나 외곽선 보이게 하는 것 등)
                slots[i].Set();
            }

            // 슬롯에 데이터가 없으면 
            else
            {
                // 슬롯 비우기
                slots[i].Clear();
            }
        }
    }

    // 슬롯에서 itemData(아이템) 가져오는 메서드 -> 아이템을 인벤토리에 추가하기 전에 그 아이템을 인벤토리
    // 에 넣을 수 있는지 검사. -> ItemSlot을 반환하면 슬롯에 해당 아이템을 넣을 공간이 있다는 것.
    // null을 반환하면 슬롯에 해당 아이템을 넣을 공간이 없다는 것 => 최대 소지량을 가지고 있기 때문 
    private ItemSlot GetItemStack(ItemData itemData)
    {
        // 슬롯을 하나씩 순회해서 
        for(int i = 0; i < slots.Length; i++)
        {
            // 해당 슬롯에 있는 아이템이 내가 추가하려는 아이템(itemData)이면서 
            // 슬롯에 있는 아이템의 양이 최대소지량을 벗어나지 않으면 
            if (slots[i].itemData == itemData && slots[i].quantity < itemData.maxStackAmount)
            {
                // 해당 아이템 슬롯을 반환해줘서, 슬롯에 아이템 수를 늘릴 수 있게 해준다. 
                return slots[i];
            }
        }

        // 슬롯 아이템과 가져오려는 아이템은 같은데 슬롯에 있는 해당 아이템이 최대량을 가지고 있으면
        return null;
    }

    // 아이템을 넣을 수 있는 빈 슬롯을 찾아서 반환해주는 메서드 
    private ItemSlot GetEmptySlot()
    {
        // 슬롯들을 순회해서 
        for(int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템이 없으면 
            if (slots[i].itemData == null)
            {
                // 아이템이 없는 빈 슬롯을 반환 
                return slots[i];
            }
        }

        // 여기로 왔다는 건 빈 아이템 슬롯이 없다는 것
        // 빈 아이템 슬롯이 없다는 의미로 null을 반환 
        return null;
    }

    // 아이템을 먹는 순간 Destroy로 아이템이 없어짐.
    // 아이템을 먹었는데 빈 슬롯이 없으면 버려야 함. 그 아이템을 원래 위치로 위치시켜야 함.
    // 사라진 아이템을 다시 생성해서 원래 자리로 위치시켜주는 메서드 
    private void ThrowItem(ItemData itemData)
    {
        // 프리팹으로 버릴 아이템 만들고, 위치는 dropPosition,
        // 360도 * (0 ~ 1사이 랜덤값) 각도로. 랜덤 회전값을 적용시켜서 프리팹을 생성
        Instantiate(itemData.dropPrefab, dropItemPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // 아이템 선택하면 슬롯 번호에 접근해서 가져오기
    public void SelectItem(int index)
    {
        // index번째 슬롯에 아이템이 없으면 종료
        if (slots[index].itemData == null) return;

        _selectedItem = slots[index].itemData; // 선택된 아이템을 가져오기 
        _selectedItemIndex = index; // 아이템 번호 저장

        selectedItemName.text = _selectedItem.displayName; // 이름과
        selectedItemDescription.text = _selectedItem.description; // 아이템 설명 저장 

        // 스탯(체력, 마나 같은 것들)정보 저장
        // 모든 아이템이 스탯을 가지고 있지 않음. 
        // 그래서 일단 스탯은 빈문자열로 저장
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 선택된 아이템이 소비가능한 아이템이면, 어떤 소비 아이템인지 얼만큼 회복시켜주는 아이템인지를
        // UI에 업데이트 해준다. -> 선택된 아이템의 정보를 표시 
        for (int i = 0; i < _selectedItem.consumables.Length; i++)
        {
            // 아이템이 타입(체력 회복, 배고픔 회복)이 여러 개일 수 있음.
            // 그래서 consumables 배열에 여러 아이템 타입과 값을 저장해둠. 
            // 여러 타입의 아이템이 있는 경우 텍스트를 여러개 붙여주기 위해 +=연산자를 사용
            selectedStatName.text += _selectedItem.consumables[i].consumableType.ToString() + "\n";
            selectedStatValue.text += _selectedItem.consumables[i].value.ToString() + "\n";
        }

        // (버튼 활성화/비활성화)
        // 사용하는 아이템은 사용 버튼을 활성화
        useButton.SetActive(_selectedItem.itemType == ItemType.Consumable);
        // 선택한 아이템이 장착 타입이고 장착이 안 되어 있을때는 장착 버튼을 활성화
        equipButton.SetActive(_selectedItem.itemType == ItemType.Equipable && !slots[index].isEquipped);
        // 선택한 아이템을 장착 타입이고 장착이 되어 있을 때는 장착 버튼 비활성화
        unequipButton.SetActive(_selectedItem.itemType == ItemType.Equipable && slots[index].isEquipped);
        // 버리기 버튼은 활성화 
        dropButton.SetActive(true);
    }

    // 사용하기
    public void OnUseButton()
    {
        // 선택된 아이템이 소비 타입일 때 => 왜 소비 타입일때만 따짐? 소비 아이템만 "사용"할 수 있으니까 
        if (_selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < _selectedItem.consumables.Length; i++)
            {
                // 그 아이템의 소비 타입에 따라 체력을 회복하거나 배고픔을 회복한다. 
                switch (_selectedItem.consumables[i].consumableType)
                {
                    case ConsumableType.Health:
                        _playerCondition.Heal(_selectedItem.consumables[i].value); // 체력 회복 
                        break;
                    case ConsumableType.Hunger:
                        //_playerCondition.Eat(_selectedItem.consumables[i].value); // 배고픔 회복 
                        break;
                    case ConsumableType.Speed:
                        _playerCondition.SpeedUp(_selectedItem.consumables[i].value, _characterManager.Player.playerController); // 스피드 업 
                        break;
                }
            }

            // 소비 아이템을 사용했으면 인벤토리 정보창(인벤토리창)에서 없애준다. 
            RemoveSelectedItem();
        }
    }

    // 버리기
    public void OnDropButton()
    {
        // 버리기 메서드 이전에 구현한거 호출 
        ThrowItem(_selectedItem);
        // 선택한 아이템 삭제
        RemoveSelectedItem();
    }

    // 아이템을 버리면 UI도 업데이트 해줘야함. 
    void RemoveSelectedItem()
    {
        slots[_selectedItemIndex].quantity--; // 슬롯에서 해당 아이템 양 줄이기 => 버리니까. 

        // 양이 0이거나 그보다 작아지면
        if (slots[_selectedItemIndex].quantity <= 0)
        {
            _selectedItem = null; // 선택 아이템을 null
            slots[_selectedItemIndex].itemData = null; // 슬롯(내가 선택한 인벤토리 슬롯)에 있는 아이템을 null로
            _selectedItemIndex = -1; // 아이템 인덱스도 -1로 설정 => 아마 없는 아이템의 인덱스를 -1로 구분하려고 -1로 초기화한듯
            ClearSelectedItemWindow(); // 아이템 클릭시 표시되는 정보들 초기화
        }

        UpdateUI(); // 인벤토리 UI 업데이트 
    }
}
