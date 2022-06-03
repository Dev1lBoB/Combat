using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField]
    private Color activeColor = Color.white;
    [SerializeField]
    private Color canBeTargetColor = Color.red;
    [SerializeField]
    private Color selectedColor = Color.green;


    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void TurnOnOutline()
    {
        rend.material.SetFloat("_OutlineReferenceTexWidth", 1024);
    }

    public void TurnOffOutline()
    {
        rend.material.SetFloat("_OutlineReferenceTexWidth", 0);
    }

    public void BecomeTarget()
    {
        TurnOnOutline();
        rend.material.SetColor("_OutlineColor", canBeTargetColor);
    }

    public void BecomeActive()
    {
        TurnOnOutline();
        rend.material.SetColor("_OutlineColor", activeColor);
    }

    public void BecomeSelected()
    {
        TurnOnOutline();
        rend.material.SetColor("_OutlineColor", selectedColor);
    }

    public void SetCustomOutlineColor(Color color)
    {
        TurnOnOutline();
        rend.material.SetColor("_OutlineColor", color);
    }
}
