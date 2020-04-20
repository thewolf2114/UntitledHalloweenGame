using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausable : MonoBehaviour
{
    public bool IsPaused
    { get; set; }

    virtual protected void Start()
    {
        IsPaused = false;
    }
}
