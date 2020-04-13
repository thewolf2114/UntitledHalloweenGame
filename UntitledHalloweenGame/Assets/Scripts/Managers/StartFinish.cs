using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinish : MonoBehaviour
{
    [SerializeField]
    private Material idleMat, startMat;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = idleMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().material = startMat;
    }
}
