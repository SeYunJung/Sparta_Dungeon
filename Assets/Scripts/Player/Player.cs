using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("제어 관련 변수들")]
    public PlayerController playerController;
    public PlayerCondition playerCondition;

    [Header("아이템 관련 변수들")]
    public ItemData itemData;
    public Action addItem;

    [Header("아이템 버리는 위치")]
    public Transform dropItemPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }
}
