using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [Header("Slotâ �ϳ��� �� �����͵�")]
    public ItemData item; // ������ ���� 
    public int slotIndex; // ���° ������ �������� 
    public bool isEquipped; // â�۵� ���� �ƴ��� 
    public int quantity; // ������ ���� 

    [Header("���� ������")]
    public UIInventory inventory; // �ֻ��� �θ� 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
