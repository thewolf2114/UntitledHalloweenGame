using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : Pausable
{
    public float Time { get; set; }
    float minutes, seconds;
    Text m_timerText;

    bool timerPaused = false;

    // Start is called before the first frame update
    override protected void Start()
    {
        minutes = Mathf.Floor(Time / 60);
        seconds = Time % 60;

        m_timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        m_timerText.text = minutes.ToString() + " : " + seconds.ToString("F2");
    }

    IEnumerator Timer()
    {
        while (Time > 0)
        {
            Time -= UnityEngine.Time.deltaTime;
            minutes = Mathf.Floor(Time / 60);
            seconds = Time % 60;

            m_timerText.text = minutes.ToString() + " : " + seconds.ToString("F2");

            if (IsPaused || timerPaused)
                yield return new WaitUntil(() => !IsPaused);

            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.EndGame();
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
}
