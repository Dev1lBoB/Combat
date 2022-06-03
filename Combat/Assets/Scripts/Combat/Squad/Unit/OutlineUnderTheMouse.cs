using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutlineController))]
public class OutlineUnderTheMouse : MonoBehaviour
{
    private OutlineController outlineController;

    private bool _isActive = false;

    public bool isActive
    {
        get => _isActive;
        set
        {
        // Remove the outline from the object if it was ON at the time of disabling
            if (value == false)
                outlineController.TurnOffOutline();
            _isActive = value;
        }
    }

    private void Awake()
    {
        outlineController = GetComponent<OutlineController>();
    }

    private void OnMouseEnter()
    {
        if (isActive)
            outlineController.BecomeSelected();
    }

    private void OnMouseExit()
    {
        if (isActive)
            outlineController.BecomeTarget();
    }
}
