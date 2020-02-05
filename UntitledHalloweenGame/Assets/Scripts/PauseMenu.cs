using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnResumeButtonClick()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().Dead = false;
        player.GetComponent<PlayerShoot>().Dead = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
