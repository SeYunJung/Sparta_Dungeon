using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("���� ������")]
    public UICondition uiCondition;

    public Condition health
    {
        get { return uiCondition.health; }
    }

    [Header("ü�°��� ����")]
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
        Debug.Log("�׾���");
    }

    // ü�� ȸ�� 
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 
    //public void Eat(float amount)
    //{

    //}
}
