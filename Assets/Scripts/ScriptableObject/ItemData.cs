using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType consumableType;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("아이템 정보 변수들")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType itemType; // 아이템 타입
    public Sprite itemImage; // 아이템 이미지
    public GameObject dropPrefab; // 자원 채취시 떨어지는 아이템 오브젝트 프리팹

    [Header("아이템 중복 소지 가능한지에 대한 변수들")]
    public bool canStack; // 여러개 가질 수 있는가
    public int maxStackAmount; // 최대 소지 개수 

    // 이 아이템이 어떤 회복타입을 가지는가? (여러가지를 처리하기 위해 배열을 사용)
    [Header("아이템 효과들")]
    public ItemDataConsumable[] consumables;
}
