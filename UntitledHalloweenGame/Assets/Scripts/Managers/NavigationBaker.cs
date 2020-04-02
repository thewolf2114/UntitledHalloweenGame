using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public List<NavMeshSurface> Surfaces { get; set; }

    public List<List<GameObject>> RoadGrid { get; set; }

    /// <summary>
    /// Rebuilds the NavMesh
    /// </summary>
    public void BuildNavMesh()
    {
        //if (RoadGrid.Count > 0)
        //{
        //    for (int i = RoadGrid.Count - 1; i > 0; i--)
        //    {
        //        for (int j = RoadGrid[0].Count - 1; j > 0; j--)
        //        {
        //            if (i == Constants.LEVEL_WIDTH && j == Constants.LEVEL_HEIGHT)
        //                continue;

        //            if (i == Constants.LEVEL_WIDTH)
        //            {
        //                NavMeshLink link = RoadGrid[i][j].AddComponent<NavMeshLink>();
        //                link.startPoint = Vector3.right * Constants.LINK_START_POINT_OFFSET;
        //                link.endPoint = Vector3.right * Constants.LINK_END_POINT_OFFSET;
        //                link.width = Constants.LINK_WIDTH;
        //                continue;
        //            }
        //            else if (j == Constants.LEVEL_HEIGHT)
        //            {
        //                NavMeshLink link = RoadGrid[i][j].AddComponent<NavMeshLink>();
        //            }
        //        }
        //    }
        //}
        if (Surfaces.Count > 0)
        {
            foreach (NavMeshSurface surface in Surfaces)
            {
                surface.BuildNavMesh();
            }
        }
        else
            Debug.LogError("No NavMesh loaded.");
    }
}
