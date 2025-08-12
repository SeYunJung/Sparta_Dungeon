using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("체력 변수들")]
    public float curValue; // 현재 체력 
    public float startValue; // 시작 체력 
    public float maxValue; // 최대 체력
    public float passiveValue; // 주기적으로 변하는 값 (몇씩 체력이 줄어들지)
    public Image uiBar; // 이미지에 있는 FillAmount를 컨트롤하려면 image가 필요함.
    
    void Start()
    {
        // 처음에는 시작체력부터 시작 
        curValue = startValue; 
    }

    void Update()
    {
        // 체력바 UI 업데이트 
        uiBar.fillAmount = GetPercentage();
    }

    // FillAmount 값을 계산해서 반환해주는 메서드 
    float GetPercentage()
    {
        return curValue / maxValue; // curValue만 수정되면 Update에서 자동으로 UI가 업디이트된다. 
    }

    // curValue 업데이트 메서드들 -> 외부에서 호출 -> Update에서 현재 체력이 업데이트됨. 
    public void Add(float value)
    {
        // 변환된 체력(curValue + value)가 최대 체력(maxValue)보다 클 경우에는 maxValue로 업데이트 
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        // 변환된 체력(curValue - value)가 최소 체력(0)보다 작을 경우 0으로 업데이트 
        curValue = Mathf.Max(curValue - value, 0);
    }

    // 스피드 업
    public void SpeedUp(float value, PlayerController playerController)
    {
        StartCoroutine(Coroutine_SpeedUp(value, playerController));
    }

    private IEnumerator Coroutine_SpeedUp(float value, PlayerController playerController)
    {
        playerController.moveSpeed += value;
        yield return new WaitForSeconds(3.0f);
        playerController.moveSpeed -= value;
    }
}
