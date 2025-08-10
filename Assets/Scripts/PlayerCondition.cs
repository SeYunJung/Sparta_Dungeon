using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    public Condition health
    {
        get { return uiCondition.health; }
    }

    public float noHunberHealthDecay;

    void Update()
    {
        health.Subtract(noHunberHealthDecay * Time.deltaTime);

        if(health.curValue == 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("ав╬З╢ы");
    }
}
