using UnityEngine;

// UI 매니저 
public class UICondition : MonoBehaviour
{
    [Header("상태 UI변수들")]
    public Condition health;

    [Header("참조 변수들")]
    public CharacterManager characterManager;
    public Player player;

    void Start()
    {
        characterManager = CharacterManager.Instance;
        player = characterManager.Player;

        // player의 playerCondition에 있는 uiCondition에 자기자신(UICondition)을 넣기
        // PlayerCondition에서 Condition에 접근하려면 중간 다리가 필요함.
        // 그 중간 다리가 UICondition이다. 그래서 UICondition 자체에서 자기자신이 중간다리임을 
        // 알려줘야 한다. 자기자신이 중간다리임을 알려주는 코드가 이것 
        player.playerCondition.uiCondition = this;
        // 그러면 Player는 PlayerCondition의 UICondition을 통해서 Condition에 접근할 수 있게 된다. 
        // Player는 자신의 변하는 체력 정보를 Condition에 전달만 해주면 체력바 UI가 체력에 맞게 줄어들거나 늘어나게 된다. 
    }
}
