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
    [Header("������ ���� ������")]
    public string displayName; // ������ �̸�
    public string description; // ������ ����
    public ItemType itemType; // ������ Ÿ��
    public Sprite itemImage; // ������ �̹���
    public GameObject dropPrefab; // �ڿ� ä��� �������� ������ ������Ʈ ������

    [Header("������ �ߺ� ���� ���������� ���� ������")]
    public bool canStack; // ������ ���� �� �ִ°�
    public int maxStackAmount; // �ִ� ���� ���� 

    // �� �������� � ȸ��Ÿ���� �����°�? (���������� ó���ϱ� ���� �迭�� ���)
    [Header("������ ȿ����")]
    public ItemDataConsumable[] consumables;
}
