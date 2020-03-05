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

                GameObject road = Instantiate(roads[randRoad], currPosition, roads[randRoad].transform.rotation) as GameObject;
                roadsInGame.Add(road);
                currPosition += new Vector3(X_INCREMENT, 0, 0);
            }
            currPosition = new Vector3(X_INCREMENT / 2, 0, currPosition.z + Z_INCREMENT);
        }
    }

    private void BuildHouses()
    {
        List<string> plotNames = new List<string>();
        for (int i = 1; i <= 4; i++)
        {
            string plotName = "HousePlot_0" + i;
            plotNames.Add(plotName);
        }

        for (int i = 0; i < roadsInGame.Count; i++)
        {
            if (roadsInGame[i].layer == LayerMask.NameToLayer("DeadEnd"))
                continue;

            List<GameObject> houses = new List<GameObject>();
            GameObject road = roadsInGame[i];

            for (int j = 0; j < 4; j++)
            {
                GameObject plot = road.transform.Find(plotNames[j]).gameObject;

                houses.Add(SpawnHouse(road, plot.layer));
            }

            for (int j = 0; j < 4; j++)
            {
                GameObject plot = road.transform.Find(plotNames[j]).gameObject;
                GameObject house = houses[j];

                house.transform.localPosition = plot.transform.localPosition;
                Quaternion houseRotation = house.transform.localRotation;

                if (house.layer == cornerLayer)
                    houseRotation *= Quaternion.Euler(0, (-90 * i), 0); // add -90 degrees per plot space
                else
                {
                    Quaternion rotationAmount = (j > 1) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
                    houseRotation *= rotationAmount;
                }
                house.transform.localRotation = houseRotation;
            }

            houses.Clear();
        }
    }

    private GameObject SpawnHouse(GameObject parent, LayerMask layer)
    {
        int houseType = (layer == cornerLayer) ? 0 : 1;
        List<List<GameObject>> houses = new List<List<GameObject>>() { cornerHouses, straightHouses };
        int randHouse = Random.Range(0, houses[houseType].Count);

        GameObject house = Instantiate(houses[houseType][randHouse], parent.transform) as GameObject;
        return house;
    }
}
