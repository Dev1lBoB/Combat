using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField]
    private DialogWindowManager dialogWindowManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dialogWindowManager.Exit();
        }
    }
}
