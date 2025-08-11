using UnityEngine;

// ��� ItemObject�� �� �������̽��� �����ϵ��� �ϱ� 
public interface IInteractable
{
    public string GetInteractPrompt(); // ȭ�鿡 ����� ������Ʈ �޼��� 
    public void OnInteract(); // ��ȣ�ۿ� �Ǿ��� �� � ȿ���� �߻���ų ������ 
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    /*
     * ���� ItemObject���� ������ �ϳ��ϳ��� Ŭ������ ����� ���?
     * �� ��� Ŭ������ �����ϱⰡ �����. ���ǹ��� ��û ��������. 
     * �������� 100������� 100�� Ŭ������ ���ǹ����� �˻��غ��ٰ� �����غ���.. 
     * �׷��� ���⼭ �������̽��� ����ؾ� �Ѵ�. 
     */

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // ��ȣ�ۿ��� �Ǹ� 
    public void OnInteract()
    {
        // �÷��̾�� ������ ������ �Ѱ��ش�. 
        CharacterManager.Instance.Player.itemData = data;

        // ��������Ʈ ����? 
        CharacterManager.Instance.Player.addItem?.Invoke();

        // �ʿ��� ������� �ϱ�
        Destroy(this.gameObject);
    }
}