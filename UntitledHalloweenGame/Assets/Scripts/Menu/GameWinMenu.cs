using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWinMenu : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    public void OnRetryButtonClicked()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitButtonClicked()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }
}
