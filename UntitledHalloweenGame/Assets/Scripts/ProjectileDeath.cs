using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeath : MonoBehaviour
{
    float deathTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DeathTimer");
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTimer);

        Destroy(gameObject);
    }
}
