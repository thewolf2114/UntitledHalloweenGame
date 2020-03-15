using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject startQuit, loading, instructions;

    /// <summary>
    /// Begins loading the level scene
    /// </summary>
    public void OnStartButtonClicked()
    {
        startQuit.SetActive(false);
        SceneManager.LoadSceneAsync(1);
        loading.SetActive(true);
    }

    /// <summary>
    /// Opens the instructins page
    /// </summary>
    public void OnInstructionsButtonClicked()
    {
        startQuit.SetActive(false);
        instructions.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        instructions.SetActive(false);
        startQuit.SetActive(true);
    }

    /// <summary>
    /// Closes the application
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
