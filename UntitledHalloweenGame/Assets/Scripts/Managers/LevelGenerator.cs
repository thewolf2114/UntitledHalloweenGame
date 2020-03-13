using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roads, cornerHouses, straightHouses;

    [SerializeField]
    private GameObject startingBoarder, emptySection;

    private enum Roads { CROSS, DEAD_END, STRAIGHT, TEE };

    List<GameObject> roadsInGame;
    LayerMask cornerLayer;

    const int X_INCREMENT = 60;
    const int Z_INCREMENT = 60;
    const int LEVEL_WIDTH = 9;  // range from 0 to 4
    const int LEVEL_HEIGHT = 9; // range from 0 to 4

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
        // initializing the starting dead end position and setting its random rotation
        GameObject deadEnd = Instantiate(roads[1], LocalToGlobal(currPosition), roads[1].transform.rotation) as GameObject;
        int randomRotation = Random.Range(0, 2);
        deadEnd.transform.rotation *= (randomRotation == 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 180, 0);

        // getting all the connection points that are on the starting section and initialize the road grid
        Queue<GameObject> connectionPoints = new Queue<GameObject>(deadEnd.GetComponent<RoadConnections>().Connections);
        List<List<GameObject>> roadGrid = new List<List<GameObject>>();
        for (int i = 0; i <= LEVEL_WIDTH; i++)
        {
            List<GameObject> emptyList = new List<GameObject>();
            for (int j = 0; j <= LEVEL_HEIGHT; j++)
            {
                emptyList.Add(new GameObject());
            }
            roadGrid.Add(emptyList);
        }
        GameObject temp = roadGrid[0][0];
        roadGrid[0].Remove(temp);
        Destroy(temp);
        roadGrid[0].Insert(0, deadEnd);
        roadsInGame.Add(deadEnd);

        GameObject point;
        List<GameObject> possibleRoads;
        int roadIndex;
        GameObject road;
        int looped = 300;

        // while there are still available connection points
        while (connectionPoints.Count > 0 && looped > 0)
        {
            // pop of the top connection point from the queue and adjust our current position
            point = connectionPoints.Dequeue();
            currPosition = GlobalToLocal(point.transform.parent.position);

            // move to the cell that would connect to connection point
            FindGridPointFromConnection(point, ref currPosition);

            // initialize possible roads that can be placed at the current position
            possibleRoads = SpawnableRoads(currPosition, roadGrid);

            // randomize the available road segments
            if (possibleRoads.Count > 0)
            {
                roadIndex = Random.Range(0, possibleRoads.Count);
                road = Instantiate(possibleRoads[roadIndex], LocalToGlobal(currPosition), possibleRoads[roadIndex].transform.rotation) as GameObject;
                possibleRoads.RemoveAt(roadIndex);
            }
            else
            {
                road = Instantiate(emptySection, LocalToGlobal(currPosition), emptySection.transform.rotation);
            }

            RotateRoad(point, ref road);

            RemoveConnectionPoints(ref point, ref road, currPosition, ref connectionPoints);

            foreach (GameObject connection in road.GetComponent<RoadConnections>().Connections)
            {
                connectionPoints.Enqueue(connection);
            }

            temp = roadGrid[(int)currPosition.z][(int)currPosition.x];
            roadGrid[(int)currPosition.z].Remove(temp);
            Destroy(temp);
            roadGrid[(int)currPosition.z].Insert((int)currPosition.x, road);

            roadsInGame.Add(road);

            //if (connectionPoints.Count == 0)
            //{
            //    int numberOfRoadsInScene = roadsInGame.Count;
            //    int numberOfPossibleRoads = roadGrid.Count * roadGrid[0].Count;

            //    float roadRatio = numberOfRoadsInScene / numberOfPossibleRoads;

            //    if (roadRatio < 0.8f)
            //    {
            //        GameObject straightSec = roadsInGame.Find(x => x.name.Contains(roads[(int)Roads.STRAIGHT].name));
            //    }
            //}

            looped--;
        }

        for (int i = 0; i < roadGrid.Count; i++)
        {
            for (int j = 0; j < roadGrid[0].Count; j++)
            {
                if (roadGrid[i][j].name == "New Game Object")
                {
                    currPosition = new Vector3(j, 0, i);
                    GameObject blank = Instantiate(emptySection, LocalToGlobal(currPosition), emptySection.transform.rotation) as GameObject;

                    temp = roadGrid[i][j];
                    roadGrid[i].Remove(temp);
                    Destroy(temp);
                    roadGrid[i].Insert(j, blank);
                }
            }
        }
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
    /// <param name="roadGrid">The road grid as it currently stands</param>
    /// <returns>A list of possible road segments</returns>
    private List<GameObject> SpawnableRoads(Vector3 currPosition, List<List<GameObject>> roadGrid)
    {
        List<GameObject> spawnableRoads = new List<GameObject>(roads);
        Vector3 right = currPosition + Vector3.right;
        Vector3 left = currPosition.x > 0 ? currPosition + Vector3.left : Vector3.one;
        Vector3 up = currPosition + Vector3.forward;
        Vector3 back = currPosition.z > 0 ? currPosition + Vector3.back : Vector3.one;

        //GameObject rightRoad = roadGrid[(int)right.z][(int)right.x];
        //GameObject leftRoad = roadGrid[(int)left.z][(int)left.x];
        //GameObject upRoad = roadGrid[(int)up.z][(int)up.x];
        //GameObject downRoad = roadGrid[(int)back.z][(int)back.x];

        if ((currPosition.x < 1 || currPosition.x >= LEVEL_WIDTH) && (currPosition.z < 1 || currPosition.z >= LEVEL_HEIGHT))
        {
            spawnableRoads.Remove(roads[(int)Roads.CROSS]);
            spawnableRoads.Remove(roads[(int)Roads.STRAIGHT]);
            spawnableRoads.Remove(roads[(int)Roads.TEE]);
        }
        else if ((currPosition.x < 1 || currPosition.x >= LEVEL_WIDTH) || (currPosition.z < 1 || currPosition.z >= LEVEL_HEIGHT))
        {
            spawnableRoads.Remove(roads[(int)Roads.CROSS]);
            spawnableRoads.Remove(roads[(int)Roads.DEAD_END]);
        }



        return spawnableRoads;
    }

    /// <summary>
    /// Finds the point on the road grid that corresponds with that connection point
    /// </summary>
    /// <param name="connectionPoint">the connnection point to find connecting grid</param>
    /// <returns>The grid point that connects to connectionPoint</returns>
    private void FindGridPointFromConnection(GameObject connectionPoint, ref Vector3 currPosition)
    {
        Vector3 gridPoint = connectionPoint.transform.position - connectionPoint.transform.parent.position;
        gridPoint.x /= 30;
        gridPoint.z /= 30;

        currPosition += gridPoint;
    }

    /// <summary>
    /// Rotates the road until it is aligned with the connection point
    /// </summary>
    /// <param name="connectionPoint">the point you want the road to connect to</param>
    /// <param name="road">the road you want to connect to connectionPoint</param>
    private void RotateRoad(GameObject connectionPoint, ref GameObject road)
    {
        bool connected = isConnected(connectionPoint, road);
        bool noEnds = NoAbruptEnds(road);
        int maxLoop = 5;

        while ((!connected || !noEnds) && maxLoop > 0)
        {
            road.transform.rotation *= Quaternion.Euler(0, 90, 0);
            connected = isConnected(connectionPoint, road);
            noEnds = NoAbruptEnds(road);
            maxLoop--;
        }
    }

    /// <summary>
    /// Determines if the road segment is connected to the connectionpoint
    /// </summary>
    /// <param name="connectionPoint">Position to connect road to</param>
    /// <param name="road">Road you want connected to connectionPoint</param>
    /// <returns>true if they are connected; false otherwise</returns>
    private bool isConnected(GameObject connectionPoint, GameObject road)
    {
        bool connected = false;

        List<GameObject> connections = road.GetComponent<RoadConnections>().Connections;
        foreach (GameObject connection in connections)
        {
            if ((connection.transform.position - connectionPoint.transform.position).magnitude < 0.1f)
            {
                connected = true;
            }
        }

        return connected;
    }

    private bool NoAbruptEnds(GameObject road)
    {
        bool noEnds = true;
        float tolerence = 0.1f;

        Vector3 roadPosition = GlobalToLocal(road.transform.position);

        foreach(GameObject connection in road.GetComponent<RoadConnections>().Connections)
        {
            if ((connection.transform.position.x - 0) < tolerence && (roadPosition.x - 0) < tolerence ||
                (connection.transform.position.x - (LEVEL_WIDTH * 60) + 60) < tolerence && roadPosition.x == LEVEL_WIDTH ||
                (connection.transform.position.z - 0) < tolerence && (roadPosition.z - 0) < tolerence ||
                (connection.transform.position.z - (LEVEL_HEIGHT * 60) + 60) < tolerence && roadPosition.z == LEVEL_HEIGHT)
            {
                noEnds = false;
            }
        }

        return noEnds;
    }

    /// <summary>
    /// Removes the connection points that are no longer useful,
    /// like connections on an edge
    /// </summary>
    /// <param name="point">the current point used to spawn the road</param>
    /// <param name="road">the road that was spawned</param>
    /// <param name="currPosition">the current tile position</param>
    private void RemoveConnectionPoints(ref GameObject point, ref GameObject road, Vector3 currPosition, ref Queue<GameObject> connectionQueue)
    {
        List<GameObject> removedPoints = new List<GameObject>();
        List<GameObject> connections = road.GetComponent<RoadConnections>().Connections;
        float tolerance = 0.1f;

        // add all the connection points that need to be removed to a list
        foreach (GameObject connectionPoint in connections)
        {
            if (Vector3.Distance(point.transform.position, connectionPoint.transform.position) < 10)
            {
                removedPoints.Add(point);
                removedPoints.Add(connectionPoint);
                continue;
            }

            // if the connection point is on the right or left edge, destroy it
            if ((currPosition.x - 0) < tolerance && (connectionPoint.transform.position.x - 0) < tolerance)
            {
                removedPoints.Add(connectionPoint);
                continue;
            }
            else if (currPosition.x == LEVEL_WIDTH && (connectionPoint.transform.localPosition.x - 30) < tolerance)
            {
                removedPoints.Add(connectionPoint);
                continue;
            }
            // if the connection point is on the top or bottom egde, destroy it
            else if ((currPosition.z - 0) < tolerance && (connectionPoint.transform.position.z - 0) < tolerance)
            {
                removedPoints.Add(connectionPoint);
                continue;
            }
            else if (currPosition.z == LEVEL_HEIGHT && (connectionPoint.transform.localPosition.z - 30) < tolerance)
            {
                removedPoints.Add(connectionPoint);
                continue;
            }

            // if there is a connection with any other road
            for (int i = 0; i < roadsInGame.Count; i++)
            {
                foreach(GameObject connection in roadsInGame[i].GetComponent<RoadConnections>().Connections)
                {
                    if (Vector3.Distance(connection.transform.position, connectionPoint.transform.position) < 10)
                    {
                        removedPoints.Add(connection);
                        removedPoints.Add(connectionPoint);

                        List<GameObject> connectionNonQueue = new List<GameObject>(connectionQueue.ToArray());
                        if (connectionNonQueue.Contains(connection))
                        {
                            connectionNonQueue.Remove(connection);
                            connectionQueue.Clear();

                            connectionQueue = new Queue<GameObject>(connectionNonQueue.ToArray());
                        }
                    }
                }
            }
        }

        // remove the connection points
        foreach (GameObject obj in removedPoints)
        {
            if (connections.Contains(obj))
                connections.Remove(obj);
            else
                obj.transform.parent.gameObject.GetComponent<RoadConnections>().Connections.Remove(obj);
        }
    }

    /// <summary>
    /// Spawns a house as a child of the given parent obect
    /// </summary>
    /// <param name="parent">the object parent</param>
    /// <param name="layer">layer used to determine house type</param>
    /// <returns>Returns the house object that was spawned</returns>
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
        float newX = Mathf.Round((local.x * 60) + 30);
        float newZ = Mathf.Round((local.z * 60) + 30);
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
        float newX = Mathf.Round((global.x - 30) / 60);
        float newZ = Mathf.Round((global.z - 30) / 60);
        global = new Vector3(newX, 0, newZ);

        return global;
    }
}
