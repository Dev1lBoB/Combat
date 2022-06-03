using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowHealhBarUnderTheMouse : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    private void OnMouseEnter()
    {
        healthBar.enabled = true;
    }

    private void OnMouseExit()
    {
        healthBar.enabled = false;
    }
}
