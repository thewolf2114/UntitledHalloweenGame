using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Pausable
{
    [SerializeField]
    GameObject levelGenerator, player;

    [SerializeField]
    GameObject gameTimer;

    public static GameManager Instance { get; private set; }

    private NavigationBaker baker;

    Text enemyText;
    string startText;
    int enemyCount;

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

        GameObject timer = Instantiate(gameTimer);
        timer.GetComponent<GameTimer>().Time = gameTime;

        enemyText = GameObject.FindGameObjectWithTag("EnemyText").GetComponent<Text>();
        startText = enemyText.text;

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
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyText.text = startText + enemyCount.ToString();
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

    void CalculateScore()
    {

    }
}
