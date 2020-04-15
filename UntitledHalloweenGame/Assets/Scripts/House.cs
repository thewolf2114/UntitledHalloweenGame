using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    GameObject candy, candySpawn, porchLight;

    public int NumCandy { get; private set; }

    void Start()
    {
        NumCandy = Random.Range(0, 6);
        if (NumCandy == 0)
        {
            porchLight.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < NumCandy; i++)
                Instantiate(candy, candySpawn.transform.position, Quaternion.identity);

            GetComponent<SphereCollider>().enabled = false;
            porchLight.SetActive(false);
        }
    }
}
