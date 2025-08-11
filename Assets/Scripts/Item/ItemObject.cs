using UnityEngine;

// 모든 ItemObject는 이 인터페이스를 구현하도록 하기 
public interface IInteractable
{
    public string GetInteractPrompt(); // 화면에 띄워줄 프롬프트 메서드 
    public void OnInteract(); // 상호작용 되었을 때 어떤 효과를 발생시킬 것인지 
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    /*
     * 만약 ItemObject말고 아이템 하나하나를 클래스로 만들면 어떨까?
     * 이 모든 클래스를 대응하기가 힘들다. 조건문이 엄청 많아진다. 
     * 아이템이 100가지라면 100개 클래스를 조건문으로 검사해본다고 생각해봐라.. 
     * 그래서 여기서 인터페이스를 사용해야 한다. 
     */

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // 상호작용이 되면 
    public void OnInteract()
    {
        // 플레이어에게 아이템 정보를 넘겨준다. 
        CharacterManager.Instance.Player.itemData = data;

        // 델리게이트 실행? 
        CharacterManager.Instance.Player.addItem?.Invoke();

        // 맵에서 사라지게 하기
        Destroy(this.gameObject);
    }
}