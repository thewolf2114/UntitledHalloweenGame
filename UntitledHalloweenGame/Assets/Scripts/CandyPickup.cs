using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    public int PickUpAmount
    { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        PickUpAmount = /*Random.Range(5, 10)*/ 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
