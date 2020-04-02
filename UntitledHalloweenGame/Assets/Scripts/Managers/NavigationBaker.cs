using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }

    /// <summary>
    /// Rebuilds the NavMesh
    /// </summary>
    public void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }
}
