using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("참조 변수들")]
    public UICondition uiCondition;

    public Condition health { get { return uiCondition.health; } }
    public Condition speed { get { return uiCondition.speed; } }
    public Condition stamina { get { return uiCondition.stamina; } }

    [Header("체력감소 변수")]
    public float noHungerHealthDecay;

    void Update()
    {
        health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if(health.curValue == 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("죽었다");
    }

    // 체력 회복 
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 
    //public void Eat(float amount)
    //{

    //}

    // 스피드 업
    public void SpeedUp(float amount, PlayerController playerController)
    {
        //speed.Add(amount);
        speed.SpeedUp(amount, playerController);
    }

    // 스태미나 업데이트
    public void UpdateStamina(float value)
    {
        stamina.UpdateStamina(value);
    }
}
