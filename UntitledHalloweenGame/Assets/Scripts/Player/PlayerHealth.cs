using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int currCandy;
    int maxCandy = 100;
    int damage = 10;
    float hitTimer = 0.5f;
    bool isHit = false;

    void Start()
    {
        currCandy = maxCandy;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isHit && collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            isHit = true;
            currCandy -= damage;

            if (currCandy <= 0)
            {
                Debug.LogError("You Are Dead!!!");
            }

            StartCoroutine("HitTimer");
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimer);

        isHit = false;
    }
}
