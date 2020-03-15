using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadConnections : MonoBehaviour
{
    [SerializeField]
    List<GameObject> connections;

    public List<GameObject> Connections { get { return connections; } }
}
