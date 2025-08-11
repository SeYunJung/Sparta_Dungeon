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
        // 처음 실행시 _instance = null
        if(_instance == null)
        {
            // 객체를 만들어주고 DontDestroyOnLoad로 유지 
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // 이미 _instance가 있는 경우(씬 전환이 되고 Awake가 호출되는 경우)
        else
        {
            // 기존 _instance와 현재 인스턴스(this)가 같으면. 그러니까 이미 인스턴스가 있으면 
            if(_instance == this)
            {
                // 현재 인스턴스 파괴하기 -> 기존 것으로 유지하기 
                Destroy(this.gameObject);
            }
        }
    }
}
