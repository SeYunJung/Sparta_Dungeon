using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("���� ���� ������")]
    public PlayerController playerController;
    public PlayerCondition playerCondition;

    [Header("������ ���� ������")]
    public ItemData itemData;
    public Action addItem;

    [Header("������ ������ ��ġ")]
    public Transform dropItemPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }
}
