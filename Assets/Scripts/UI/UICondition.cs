using UnityEngine;

// UI �Ŵ��� 
public class UICondition : MonoBehaviour
{
    [Header("���� UI������")]
    public Condition health;

    [Header("���� ������")]
    public CharacterManager characterManager;
    public Player player;

    void Start()
    {
        characterManager = CharacterManager.Instance;
        player = characterManager.Player;

        // player�� playerCondition�� �ִ� uiCondition�� �ڱ��ڽ�(UICondition)�� �ֱ�
        // PlayerCondition���� Condition�� �����Ϸ��� �߰� �ٸ��� �ʿ���.
        // �� �߰� �ٸ��� UICondition�̴�. �׷��� UICondition ��ü���� �ڱ��ڽ��� �߰��ٸ����� 
        // �˷���� �Ѵ�. �ڱ��ڽ��� �߰��ٸ����� �˷��ִ� �ڵ尡 �̰� 
        player.playerCondition.uiCondition = this;
        // �׷��� Player�� PlayerCondition�� UICondition�� ���ؼ� Condition�� ������ �� �ְ� �ȴ�. 
        // Player�� �ڽ��� ���ϴ� ü�� ������ Condition�� ���޸� ���ָ� ü�¹� UI�� ü�¿� �°� �پ��ų� �þ�� �ȴ�. 
    }
}
