using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    GameObject candy, candySpawn, porchLight;

    float candySpacingTimer = 0.3f;

    public int NumCandy { get; private set; }

    void Start()
    {
        NumCandy = Random.Range(0, 6);
        if (NumCandy == 0)
        {
            porchLight.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
        }

        if (GameManager.Instance) GameManager.Instance.AddCandy(NumCandy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(GiveCandy());
        }
    }

    IEnumerator GiveCandy()
    {
        while (NumCandy > 0)
        {
            GameObject candySpawned = Instantiate(candy, candySpawn.transform.position, Quaternion.identity);
            candySpawned.transform.rotation = candySpawn.transform.rotation;
            NumCandy--;

            yield return new WaitForSeconds(candySpacingTimer);
        }

        GetComponent<SphereCollider>().enabled = false;
        porchLight.SetActive(false);
    }
}
