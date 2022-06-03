using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame
{
    private PauseGame()
    {

    }

    private static PauseGame _Instance;
    public static PauseGame Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PauseGame();
            }
            return _Instance;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;

        Squad[] squads = UnityEngine.Object.FindObjectsOfType(typeof(Squad)) as Squad[];

        ChangeUnitsLayer(squads, 2); // Ignore raycast layer
    }

    public void Unpause()
    {
        Time.timeScale = 1;

        Squad[] squads = UnityEngine.Object.FindObjectsOfType(typeof(Squad)) as Squad[];

        ChangeUnitsLayer(squads, 0); // Default layer
    }

    private void ChangeUnitsLayer(Squad[] squads, int newLayer)
    {
        foreach (Squad s in squads)
        {
            foreach (Unit u in s.GetAllUnits())
            {
                u.gameObject.layer = newLayer;
            }
        }
    }
}
