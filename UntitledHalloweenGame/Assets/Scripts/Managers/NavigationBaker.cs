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
        if (RoadGrid.Count > 0)
        {
            Vector3 startLeftOffset = Vector3.right * Constants.LINK_START_POINT_OFFSET;
            Vector3 endLeftOffset = Vector3.right * Constants.LINK_END_POINT_OFFSET;
            Vector3 startBottomOffset = Vector3.forward * Constants.LINK_START_POINT_OFFSET;
            Vector3 endBottomOffset = Vector3.forward * Constants.LINK_END_POINT_OFFSET;

            // sew up the nav mesh
            for (int i = RoadGrid.Count - 1; i >= 0; i--)
            {
                for (int j = RoadGrid[0].Count - 1; j >= 0; j--)
                {
                    if (i == Constants.LEVEL_HEIGHT && j == Constants.LEVEL_WIDTH)
                        continue;

                    if (i != Constants.LEVEL_HEIGHT)
                    {
                        NavMeshLink link = RoadGrid[i][j].AddComponent<NavMeshLink>();
                        link.startPoint = RoadGrid[i][j].transform.InverseTransformPoint(RoadGrid[i][j].transform.position + startBottomOffset);
                        link.endPoint = RoadGrid[i][j].transform.InverseTransformPoint(RoadGrid[i][j].transform.position + endBottomOffset);
                        link.width = Constants.LINK_WIDTH;
                        link.costModifier = 1;
                    }
                    if (j != Constants.LEVEL_WIDTH)
                    {
                        NavMeshLink link = RoadGrid[i][j].AddComponent<NavMeshLink>();
                        link.startPoint = RoadGrid[i][j].transform.InverseTransformPoint(RoadGrid[i][j].transform.position + startLeftOffset);
                        link.endPoint = RoadGrid[i][j].transform.InverseTransformPoint(RoadGrid[i][j].transform.position + endLeftOffset);
                        link.width = Constants.LINK_WIDTH;
                        link.costModifier = 1;
                    }
                }
            }
        }
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
