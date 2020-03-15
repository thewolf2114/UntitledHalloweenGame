using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Closes all the pause menus and resumes the game
    /// </summary>
    public void OnResumeButtonClick()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().Dead = false;
        player.GetComponent<PlayerShoot>().Dead = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void OnQuitButtonClick()
    {
        SceneManager.LoadScene(0);
    }
}
