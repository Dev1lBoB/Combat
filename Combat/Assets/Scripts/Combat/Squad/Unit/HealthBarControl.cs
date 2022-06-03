using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarControl : MonoBehaviour
{
    [SerializeField]
    private Health  health;
    [SerializeField]
    private Image   healthBar;

    [SerializeField]
    private Color   maxHealthColor = Color.green;
    [SerializeField]
    private Color   minHealthColor = Color.red;

    void OnEnable()
    {
        health.DamageTaken += UpdateBarInfo;
    }

    void UpdateBarInfo()
    {
        float status = health.GetHealthStatus();
        healthBar.fillAmount = status;
        healthBar.color = Color.Lerp(minHealthColor, maxHealthColor, status); // Calculate the color of the healthbar
    }
}
