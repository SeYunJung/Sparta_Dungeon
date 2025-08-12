using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("ü���̳� ���ǵ� ������")]
    public float curValue; // ���� ü��/���ǵ� 
    public float startValue; // ���� ü��/���ǵ� 
    public float maxValue; // �ִ� ü��/���ǵ�
    public float passiveValue; // �ֱ������� ���ϴ� �� (� ü���� �پ����)
    public Image uiBar; // �̹����� �ִ� FillAmount�� ��Ʈ���Ϸ��� image�� �ʿ���.
    
    void Start()
    {
        // ó������ ����ü��/���ǵ���� ���� 
        curValue = startValue; 
    }

    void Update()
    {
        // ü��/���ǵ�� UI ������Ʈ 
        uiBar.fillAmount = GetPercentage();
    }

    // FillAmount ���� ����ؼ� ��ȯ���ִ� �޼��� 
    float GetPercentage()
    {
        return curValue / maxValue; // curValue�� �����Ǹ� Update���� �ڵ����� UI�� ������Ʈ�ȴ�. 
    }

    // curValue ������Ʈ �޼���� -> �ܺο��� ȣ�� -> Update���� ���� ü���� ������Ʈ��. 
    public void Add(float value)
    {
        // ��ȯ�� ü��(curValue + value)�� �ִ� ü��(maxValue)���� Ŭ ��쿡�� maxValue�� ������Ʈ 
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        // ��ȯ�� ü��(curValue - value)�� �ּ� ü��(0)���� ���� ��� 0���� ������Ʈ 
        curValue = Mathf.Max(curValue - value, 0);
    }

    // ���ǵ� ��
    public void SpeedUp(float value, PlayerController playerController)
    {
        StartCoroutine(Coroutine_SpeedUp(value, playerController));
    }

    private IEnumerator Coroutine_SpeedUp(float value, PlayerController playerController)
    {
        playerController.moveSpeed += value;
        curValue = curValue + value;
        yield return new WaitForSeconds(3.0f);
        playerController.moveSpeed -= value;
        curValue = curValue - value;
    }
}
