using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [Header("Slot창 하나에 들어갈 데이터들")]
    public ItemData item; // 아이템 정보 
    public int slotIndex; // 몇번째 아이템 슬롯인지 
    public bool isEquipped; // 창작된 건지 아닌지 
    public int quantity; // 아이템 개수 

    [Header("참조 변수들")]
    public UIInventory inventory; // 최상위 부모 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
