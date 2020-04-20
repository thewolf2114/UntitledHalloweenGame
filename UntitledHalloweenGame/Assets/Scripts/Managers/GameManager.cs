using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Pausable
{
    [SerializeField]
    GameObject levelGenerator, player, gameWin;

    [SerializeField]
    GameObject gameTimerObject;

    public static GameManager Instance { get; private set; }

    NavigationBaker baker;

    // game timer
    GameTimer gameTimerScript;

    // UI manipulation
    Text enemyText;
    Text candyText;
    string enemyStartText;
    string candyStartText;

    // score calculation
    int startEnemyCount;
    int currEnemyCount;
    int startCandyCount;
    int currCandyCount;
    float score;

    // timers
    float fogTimer = 15;
    float gameTime = 600f;

    // fog
    float densityIncrease = 0.0001f;
    float currDensity = 0.0f;
    float finalDensity = 0.015f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (levelGenerator)
            Instantiate(levelGenerator);
        if (player)
            Instantiate(player, new Vector3(30, 0, 30), player.transform.rotation);

        baker = GetComponent<NavigationBaker>();

        GameObject timer = Instantiate(gameTimerObject);
        gameTimerScript = timer.GetComponent<GameTimer>();
        gameTimerScript.Time = gameTime;

        enemyText = GameObject.FindGameObjectWithTag("EnemyText").GetComponent<Text>();
        startEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyStartText = enemyText.text;
        enemyText.text = enemyStartText + startEnemyCount.ToString();

        candyText = GameObject.FindGameObjectWithTag("Candy").GetComponent<Text>();
        candyStartText = candyText.text;
        candyText.text = candyStartText + startCandyCount.ToString();

        gameWin.SetActive(false);

        StartCoroutine(Fog());
    }

    /// <summary>
    /// Returns the NavMeshBaker
    /// </summary>
    public NavigationBaker NavBaker
    {
        get { return baker; }
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Timer for fog to initiate
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fog()
    {
        yield return new WaitForSeconds(fogTimer);

        if (IsPaused)
            yield return new WaitUntil(() => !IsPaused);

        StartCoroutine(Density());
    }

    /// <summary>
    /// Increases the density of the fog
    /// </summary>
    /// <returns></returns>
    private IEnumerator Density()
    {
        while (currDensity < finalDensity)
        {
            yield return new WaitForEndOfFrame();
            RenderSettings.fogDensity += densityIncrease;
            currDensity = RenderSettings.fogDensity;
            Debug.Log(currDensity);
        }

    }

    public void AddCandy(int amount)
    {
        startCandyCount += amount;
    }

    public void AddCurrCandy(int amount)
    {
        currCandyCount += amount;
        candyText.text = candyStartText + currCandyCount.ToString() + " / " + startCandyCount.ToString();
    }

    public void StartGameTimer()
    {
        startEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currEnemyCount = startEnemyCount;
        enemyText.text = enemyStartText + currEnemyCount.ToString() + " / " + startEnemyCount.ToString();

        currCandyCount = 0;
        candyText.text = candyStartText + currCandyCount.ToString() + " / " + startCandyCount.ToString();

        gameTimerScript.StartTimer();
    }

    public void EnemyDied()
    {
        currEnemyCount--;
        enemyText.text = enemyStartText + currEnemyCount.ToString() + " / " + startEnemyCount.ToString();
    }

    public void EndGame()
    {
        score = CalculateScore();
        Destroy(gameTimerScript);

        Pausable[] pausables = FindObjectsOfType<Pausable>();
        for (int i = 0; i < pausables.Length; i++)
        {
            pausables[i].IsPaused = !pausables[i].IsPaused;

            if (pausables[i].gameObject.GetComponent<Animator>())
            {
                pausables[i].gameObject.GetComponent<Animator>().enabled = !pausables[i].gameObject.GetComponent<Animator>().enabled;
            }

        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameWin.SetActive(true);
        gameWin.GetComponent<GameWinMenu>().SetScoreText("Score: " + Mathf.Floor(score));
    }

    float CalculateScore()
    {
        // calculate the score for the enemies
        float enemyScore = ((1 - ((float)currEnemyCount / startEnemyCount))) * Constants.ENEMY_TOTAL_SCORE;

        // calculate the score for the candy
        float candyScore = ((float)currCandyCount / startCandyCount) * Constants.CANDY_TOTAL_SCORE;

        // calculate total score based on time remaining
        float totalScore = (enemyScore + candyScore) * (1 - (gameTimerScript.Time / gameTime));

        return totalScore;
    }

    
}
