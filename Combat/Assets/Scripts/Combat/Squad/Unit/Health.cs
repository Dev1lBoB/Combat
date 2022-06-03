using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float  maxHealth;

    [HideInInspector]
    public float  curHealth;

    public delegate void HealthStatusChanged();
    public event HealthStatusChanged DamageTaken;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        curHealth -= amount;
        if (curHealth < 0)
            curHealth = 0;
        DamageTaken();
    }

    public float GetHealthStatus()
    {
        return curHealth / maxHealth;
    }
}
