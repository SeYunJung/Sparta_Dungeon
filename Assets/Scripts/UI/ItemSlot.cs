using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Slot창 하나에 들어갈 데이터들")]
    public ItemData itemData; // 아이템 정보 
    public int slotIndex; // 몇번째 아이템 슬롯인지 
    public bool isEquipped; // 창작된 건지 아닌지 
    public int quantity; // 아이템 개수 
    public Button button; // 버튼
    public Image icon; // 이미지(아이콘)
    public TextMeshProUGUI quantityText; // 몇개를 가지고 있는지 (Text)
    private Outline outline; // 외곽선 => 아이템을 클릭했을 때 외곽선이 표시될 수 있게 하려고 

    [Header("참조 변수들")]
    public UIInventory inventory; // 최상위 부모 

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // 아이템 슬롯(ItemSlot)이 켜질 때(활성화 될때) 
    private void OnEnable()
    {
        // 장착중인 아이템이면 외곽선 보이게 하기 
        outline.enabled = isEquipped;
    }

    // 슬롯 세팅 
    public void Set()
    {
        icon.gameObject.SetActive(true); // 아이템 아이콘 보이게 하기 
        icon.sprite = itemData.itemImage; // 아이템 이미지 넣어줘서 보이게 하기
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 아이템 개수 설정 (하나도 없으면 "")

        // 아이템을 장착했는데 아직 외각선이 안 보이면 (방어 코드) 
        if (outline != null)
        {
            // 외곽선 보이게 하기 (장착했는데(isEquipped = true) 외곽선이 안 보인다면)
            outline.enabled = isEquipped;
        }
    }

    // 슬롯에 있는 아이템 비우기 
    public void Clear()
    {
        itemData = null; // 아이템 데이터(ItemData)를 비워주고 
        icon.gameObject.SetActive(false); // 이미지 오브젝트 꺼주고 
        quantityText.text = string.Empty; // 양을 나타내는 UI도 비워주기 
    }

    // 아이템 클릭했을 때 실행되는 메서드 -> 인벤토리 UI에 아이템 정보를 세팅해야함 -> UIInventory에 세팅 메서드 있어야함. 
    public void OnClickButton()
    {
        inventory.SelectItem(slotIndex); // 몇번째 슬롯인지 넘겨서 
    }
}
