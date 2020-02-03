using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public void OnRetryButtonClick()
    {
        gameObject.SetActive(false);

        SceneManager.LoadScene("PlayerTestScene");
    }
}
