using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Slotâ �ϳ��� �� �����͵�")]
    public ItemData itemData; // ������ ���� 
    public int slotIndex; // ���° ������ �������� 
    public bool isEquipped; // â�۵� ���� �ƴ��� 
    public int quantity; // ������ ���� 
    public Button button; // ��ư
    public Image icon; // �̹���(������)
    public TextMeshProUGUI quantityText; // ��� ������ �ִ��� (Text)
    private Outline outline; // �ܰ��� => �������� Ŭ������ �� �ܰ����� ǥ�õ� �� �ְ� �Ϸ��� 

    [Header("���� ������")]
    public UIInventory inventory; // �ֻ��� �θ� 

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // ������ ����(ItemSlot)�� ���� ��(Ȱ��ȭ �ɶ�) 
    private void OnEnable()
    {
        // �������� �������̸� �ܰ��� ���̰� �ϱ� 
        outline.enabled = isEquipped;
    }

    // ���� ���� 
    public void Set()
    {
        icon.gameObject.SetActive(true); // ������ ������ ���̰� �ϱ� 
        icon.sprite = itemData.itemImage; // ������ �̹��� �־��༭ ���̰� �ϱ�
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ������ ���� ���� (�ϳ��� ������ "")

        // �������� �����ߴµ� ���� �ܰ����� �� ���̸� (��� �ڵ�) 
        if (outline != null)
        {
            // �ܰ��� ���̰� �ϱ� (�����ߴµ�(isEquipped = true) �ܰ����� �� ���δٸ�)
            outline.enabled = isEquipped;
        }
    }

    // ���Կ� �ִ� ������ ���� 
    public void Clear()
    {
        itemData = null; // ������ ������(ItemData)�� ����ְ� 
        icon.gameObject.SetActive(false); // �̹��� ������Ʈ ���ְ� 
        quantityText.text = string.Empty; // ���� ��Ÿ���� UI�� ����ֱ� 
    }

    // ������ Ŭ������ �� ����Ǵ� �޼��� -> �κ��丮 UI�� ������ ������ �����ؾ��� -> UIInventory�� ���� �޼��� �־����. 
    public void OnClickButton()
    {
        inventory.SelectItem(slotIndex); // ���° �������� �Ѱܼ� 
    }
}
