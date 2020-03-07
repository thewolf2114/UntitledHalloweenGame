using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roads, cornerHouses, straightHouses;

    [SerializeField]
    private GameObject startingBoarder;

    private enum Roads { CROSS, DEAD_END, STRAIGHT, TEE };

    List<GameObject> roadsInGame;
    LayerMask cornerLayer;

    const int X_INCREMENT = 60;
    const int Z_INCREMENT = 60;
    const int LEVEL_WIDTH = 4;  // range from 0 to 4
    const int LEVEL_HEIGHT = 4; // range from 0 to 4

    // Start is called before the first frame update
    void Start()
    {
        Vector3 currPosition;
        int seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        Debug.Log("Seed: " + seed);

        roadsInGame = new List<GameObject>();
        cornerLayer = LayerMask.NameToLayer("Corner");

        // place the starting boarder at 0, 0, 0
        currPosition = Vector3.zero;
        PlaceStartingBoarder(ref currPosition);

        // road generation.
        BuildRoads(ref currPosition);

        // house generation
        BuildHouses();
    }

    /// <summary>
    /// Places the Starting boarder and updates the the curretn position
    /// </summary>
    /// <param name="currPosition">a reference to the current spot</param>
    private void PlaceStartingBoarder(ref Vector3 currPosition)
    {
        Instantiate(startingBoarder, currPosition, Quaternion.identity);
        currPosition = GlobalToLocal(new Vector3(X_INCREMENT / 2, 0, Z_INCREMENT / 2));
    }

    /// <summary>
    /// Randomly generates the road system for the level
    /// </summary>
    /// <param name="currPosition">a reference to the current spot</param>
    private void BuildRoads(ref Vector3 currPosition)
    {
        GameObject deadEnd = Instantiate(roads[1], currPosition, roads[1].transform.rotation) as GameObject;
        deadEnd.transform.rotation *= Quaternion.Euler(0, 90, 0);

        //for (int i = 0; i < LEVEL_HEIGHT; i++)
        //{
        //    for (int j = 0; j < LEVEL_WIDTH; j++)
        //    {
        //        int randRoad = Random.Range(0, roads.Count);

        //        GameObject road = Instantiate(roads[randRoad], LocalToGlobal(currPosition), roads[randRoad].transform.rotation) as GameObject;
        //        roadsInGame.Add(road);
        //        currPosition += new Vector3(1, 0, 0);
        //    }
        //    currPosition = new Vector3(0, 0, currPosition.z + 1);
        //}

        //currPosition = Vector3.zero;
    }

    /// <summary>
    /// Takes the list of current roads in the game and generates the 
    /// houses for those roads. 
    /// </summary>
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
                // get the current plot for layer checking
                GameObject plot = road.transform.Find(plotNames[j]).gameObject;

                // spawn a house and add it to the houses list
                houses.Add(SpawnHouse(road, plot.layer));
            }

            for (int j = 0; j < 4; j++)
            {
                // get the current plot and house
                GameObject plot = road.transform.Find(plotNames[j]).gameObject;
                GameObject house = houses[j];

                // change the current houses position to the plots position. includes rotation
                house.transform.localPosition = plot.transform.localPosition;
                Quaternion houseRotation = house.transform.localRotation;

                // determine rotation calculation depending on house type
                if (house.layer == cornerLayer) 
                    houseRotation *= Quaternion.Euler(0, (-90 * j), 0); // add -90 degrees per plot space
                else
                {
                    Quaternion rotationAmount = (j > 1) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
                    houseRotation *= rotationAmount;
                }
                house.transform.localRotation = houseRotation;
            }

            // clear the house list for the next road segment
            houses.Clear();
        }
    }

    /// <summary>
    /// Developes a list of possible road segments that can be placed at the current position
    /// </summary>
    /// <param name="currPosition">The position used to spawn the next road</param>
    /// <returns>A list of possible road segments</returns>
    private List<GameObject> SpawnableRoads(Vector3 currPosition)
    {
        List<GameObject> spawnableRoads = new List<GameObject>();

        return spawnableRoads;
    }

    /// <summary>
    /// Spawns a house as a child of the given parent obect
    /// </summary>
    /// <param name="parent">the object parent</param>
    /// <param name="layer">layer used to determine house type</param>
    /// <returns></returns>
    private GameObject SpawnHouse(GameObject parent, LayerMask layer)
    {
        int houseType = (layer == cornerLayer) ? 0 : 1;
        List<List<GameObject>> houses = new List<List<GameObject>>() { cornerHouses, straightHouses };
        int randHouse = Random.Range(0, houses[houseType].Count);

        GameObject house = Instantiate(houses[houseType][randHouse], parent.transform) as GameObject;
        return house;
    }

    /// <summary>
    /// Translates a local position into a global positino
    /// </summary>
    /// <param name="local">the position to translate</param>
    /// <returns>The local position in global scale</returns>
    private Vector3 LocalToGlobal(Vector3 local)
    {
        float newX = (local.x * 60) + 30;
        float newZ = (local.z * 60) + 30;
        local = new Vector3(newX, 0, newZ);

        return local;
    }

    /// <summary>
    /// Translates a Global position into a local position
    /// </summary>
    /// <param name="global">the position to translate</param>
    /// <returns>The global position in local space</returns>
    private Vector3 GlobalToLocal(Vector3 global)
    {
        float newX = (global.x - 30) / 60;
        float newZ = (global.z - 30) / 60;
        global = new Vector3(newX, 0, newZ);

        return global;
    }
}
