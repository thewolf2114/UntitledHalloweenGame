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
        Pausable[] pausables = FindObjectsOfType<Pausable>();
        for (int i = 0; i < pausables.Length; i++)
        {
            pausables[i].IsPaused = false;

            if (pausables[i].gameObject.GetComponent<Animator>())
            {
                pausables[i].gameObject.GetComponent<Animator>().enabled = true;
            }
        }
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<PlayerController>().Dead = false;
        //player.GetComponent<PlayerShoot>().Dead = false;

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
