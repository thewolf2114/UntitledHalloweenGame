using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Pausable
{
    [SerializeField]
    GameObject levelGenerator, player;

    [SerializeField]
    GameObject gameTimerObject;

    public static GameManager Instance { get; private set; }

    private NavigationBaker baker;

    GameTimer gameTimerScript;
    Text enemyText;
    Text candyText;
    string enemyStartText;
    string candyStartText;
    int startEnemyCount;
    int currEnemyCount;
    int startCandyCount;
    int currCandyCount;

    float fogTimer = 15;
    float gameTime = 600f;
    float densityIncrease = 0.0001f;
    float currDensity = 0.0f;
    float finalDensity = 0.015f;

    float score = 0;
    int startingNumEnemies;
    int endNumEnemies;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

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

    void CalculateScore()
    {

    }
}
