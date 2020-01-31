using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Transform candySpawn;
    public GameObject candyPickUp;
    float candyTimer = 0.2f;
    int candyAmount;
    bool dispenseCandy = false;
    bool hasCandy = true;

    // Start is called before the first frame update
    void Start()
    {
        candyAmount = Random.Range(1, 5);

        Debug.Log(candyAmount);
    }

    void Update()
    {
        if(dispenseCandy && hasCandy)
        {
            dispenseCandy = false;
            Instantiate(candyPickUp, candySpawn.position, Quaternion.identity);
            candyAmount--;

            if (candyAmount == 0)
            {
                hasCandy = false;
            }

            StartCoroutine("CandyTimer");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            dispenseCandy = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            dispenseCandy = false;
    }

    IEnumerator CandyTimer()
    {
        yield return new WaitForSeconds(candyTimer);

        dispenseCandy = true;
    }
}
