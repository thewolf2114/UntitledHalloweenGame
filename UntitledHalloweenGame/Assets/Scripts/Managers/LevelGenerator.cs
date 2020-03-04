using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> roads, cornerHouses, straightHouses;

    [SerializeField]
    GameObject startingBoarder;

    Vector3 currPosition;
    List<GameObject> roadsInGame;
    LayerMask cornerLayer;
    LayerMask straightLayer;

    const int X_INCREMENT = 60;
    const int Z_INCREMENT = 60;
    const int LEVEL_WIDTH = 5;
    const int LEVEL_HEIGHT = 5;

    // Start is called before the first frame update
    void Start()
    {
        int seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        Debug.Log("Seed: " + seed);

        roadsInGame = new List<GameObject>();
        cornerLayer = LayerMask.NameToLayer("Corner");
        straightLayer = LayerMask.NameToLayer("Straight");

        // place the starting boarder at 0, 0, 0
        currPosition = Vector3.zero;
        PlaceStartingBoarder();

        // move the current position to prepare for road generation.
        currPosition += new Vector3(X_INCREMENT / 2, 0, Z_INCREMENT / 2);
        BuildRoads();

        currPosition = Vector3.zero;
        BuildHouses();
    }

    private void PlaceStartingBoarder()
    {
        Instantiate(startingBoarder, currPosition, Quaternion.identity);
    }

    private void BuildRoads()
    {
        for (int i = 0; i < LEVEL_HEIGHT; i++)
        {
            for (int j = 0; j < LEVEL_WIDTH; j++)
            {
                int randRoad = Random.Range(0, roads.Count);
                GameObject newRoad = roads[randRoad];
                roadsInGame.Add(newRoad);

                Instantiate(roads[randRoad], currPosition, Quaternion.identity);
                currPosition += new Vector3(X_INCREMENT, 0, 0);
            }
            currPosition = new Vector3(X_INCREMENT / 2, 0, currPosition.z + Z_INCREMENT);
        }
    }

    private void BuildHouses()
    {
        for (int i = 0; i < roadsInGame.Count; i++)
        {
            if (roadsInGame[i].layer == LayerMask.NameToLayer("DeadEnd"))
                continue;

            for (int j = 1; j <= 4; j++)
            {
                string plotName = "HousePlot_0" + j;
                GameObject plot = roadsInGame[i].transform.Find(plotName).gameObject;

                SpawnHouse(plot.transform.position, plot.layer);
            }
        }
    }

    private void SpawnHouse(Vector3 location, LayerMask layer)
    {
        layer = layer >> 14;
        List<List<GameObject>> houses = new List<List<GameObject>>() { cornerHouses, straightHouses };
        int randHouse = Random.Range(0, houses[layer].Count);

        GameObject house = Instantiate(houses[layer][randHouse], location, Quaternion.identity) as GameObject;
    }
}
