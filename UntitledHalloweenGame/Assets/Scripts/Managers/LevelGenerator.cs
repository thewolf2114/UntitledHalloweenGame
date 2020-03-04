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

        // place the starting boarder at 0, 0, 0
        currPosition = Vector3.zero;
        PlaceStartingBoarder(currPosition);

        // move the current position to prepare for road generation.
        currPosition += new Vector3(X_INCREMENT / 2, 0, Z_INCREMENT / 2);
        BuildRoads(currPosition);
    }

    private void PlaceStartingBoarder(Vector3 position)
    {
        Instantiate(startingBoarder, position, Quaternion.identity);
    }

    private void BuildRoads(Vector3 currPosition)
    {
        for (int i = 0; i < LEVEL_HEIGHT; i++)
        {
            for (int j = 0; j < LEVEL_WIDTH; j++)
            {
                int randRoad = Random.Range(0, roads.Count);

                Instantiate(roads[randRoad], currPosition, Quaternion.identity);
                currPosition += new Vector3(X_INCREMENT, 0, 0);
            }
            currPosition = new Vector3(X_INCREMENT / 2, 0, currPosition.z + Z_INCREMENT);
        }
    }
}
