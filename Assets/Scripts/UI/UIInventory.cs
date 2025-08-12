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

    [Header("플레이어 관련 참조 변수들")]
    private CharacterManager _characterManager;
    private PlayerController _playerController;
    private PlayerCondition _playerCondition;

    void Start()
    {
        // 플레이어 관련 참조 변수들 가져오기 
        _characterManager = CharacterManager.Instance;
        _playerController = _characterManager.Player.playerController;
        _playerCondition = _characterManager.Player.playerCondition;

        _playerController.inventoryAction += Toggle;

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
}
