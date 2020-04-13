using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : Pausable
{
    public float Time { get; set; }
    Text m_timerText;

    // Start is called before the first frame update
    override protected void Start()
    {
        m_timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        m_timerText.text = ((int)Time / 60).ToString() + " : " + (Time % 60).ToString("F2");
    }

    IEnumerator Timer()
    {
        while(Time > 0)
        {
            Time -= UnityEngine.Time.deltaTime;
            m_timerText.text = ((int)Time / 60).ToString() + " : " + (Time % 60).ToString("F2");

            if (IsPaused)
                yield return new WaitUntil(() => !IsPaused);

            yield return new WaitForEndOfFrame();
        }
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
}
