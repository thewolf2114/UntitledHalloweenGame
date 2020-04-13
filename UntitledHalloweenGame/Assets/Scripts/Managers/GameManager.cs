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
    string startText;
    int startEnemyCount;
    int currEnemyCount;

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
        startText = enemyText.text;
        enemyText.text = startText + startEnemyCount.ToString();

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

    public void StartGameTimer()
    {
        startEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currEnemyCount = startEnemyCount;
        enemyText.text = startText + currEnemyCount.ToString() + " / " + startEnemyCount.ToString();
        gameTimerScript.StartTimer();
    }

    public void EnemyDied()
    {
        currEnemyCount--;
        enemyText.text = startText + currEnemyCount.ToString() + " / " + startEnemyCount.ToString();
    }

    void CalculateScore()
    {

    }
}
