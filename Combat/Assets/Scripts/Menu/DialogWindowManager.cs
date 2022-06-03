using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogWindowManager : MonoBehaviour
{
    [SerializeField]
    private GameObject  dialogPanelPrefab;
    [SerializeField]
    private GameObject  mainCanvas;

    private CanvasGroup[]   canvasGroups;
    private bool[]          canvasGroupStatuses;

    private bool isActive = false;

    private void Awake()
    {
        canvasGroups = FindObjectsOfType(typeof(CanvasGroup)) as CanvasGroup[];
        canvasGroupStatuses = new bool[canvasGroups.Length];
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroupStatuses[i] = canvasGroups[i].interactable;
        }
    }

    private void ExitYesPressed()
    {
        Application.Quit();
    }

    private void ExitNoPressed()
    {
        isActive = false;

        PauseGame.Instance.Unpause();
        EnableCanvasGroups();
    }

    private void RestartPressed()
    {
        isActive = false;
        
        PauseGame.Instance.Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        if (isActive == true) // Prevent multiple dialog windows to open at the same time
            return ;
        isActive = true;

        DisableCanvasGroups();

        YesNoDialog.ShowDialog
        (
            Instantiate(dialogPanelPrefab, mainCanvas.transform),
            null,
            "Are you sure want to quit?",

            "Yes",
            () => ExitYesPressed(),

            "No",
            () => ExitNoPressed()
        );

        PauseGame.Instance.Pause();
    }

    public void GameEnded(string winTeamName)
    {
        if (isActive == true) // Prevent multiple dialog windows to open at the same time
            return ;
        isActive = true;

        DisableCanvasGroups();

        YesNoDialog.ShowDialog
        (
            Instantiate(dialogPanelPrefab, mainCanvas.transform),
            null,
            "The " + winTeamName + " team won!",

            "Restart",
            () => RestartPressed(),

            "Exit",
            () => ExitYesPressed()
        );

        PauseGame.Instance.Pause();
    }

    private void DisableCanvasGroups()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].interactable = false; // Disable canvas groups, so they won't work while the dialog window is opened
        }
    }

    private void EnableCanvasGroups()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].interactable = canvasGroupStatuses[i]; // Set canvas groups as there were before pause
        }
    }


}
