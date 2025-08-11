using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        // ó�� ����� _instance = null
        if(_instance == null)
        {
            // ��ü�� ������ְ� DontDestroyOnLoad�� ���� 
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // �̹� _instance�� �ִ� ���(�� ��ȯ�� �ǰ� Awake�� ȣ��Ǵ� ���)
        else
        {
            // ���� _instance�� ���� �ν��Ͻ�(this)�� ������. �׷��ϱ� �̹� �ν��Ͻ��� ������ 
            if(_instance == this)
            {
                // ���� �ν��Ͻ� �ı��ϱ� -> ���� ������ �����ϱ� 
                Destroy(this.gameObject);
            }
        }
    }
}
