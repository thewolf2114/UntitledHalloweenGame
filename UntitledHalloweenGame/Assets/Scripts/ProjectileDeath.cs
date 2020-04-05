using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeath : MonoBehaviour
{
    float deathTimer = 5;
    Vector3 currPosition;
    Vector3 prevPosition;
    LayerMask projAndEnemy;

    // Start is called before the first frame update
    void Start()
    {
        int proj = 8;
        int enem = 18;
        LayerMask projLayer = LayerMask.NameToLayer("Projectile");
        LayerMask enemyLayer = LayerMask.NameToLayer("Enemy");
        projAndEnemy = projLayer | enemyLayer;

        currPosition = transform.position;
        prevPosition = currPosition;

        StartCoroutine("DeathTimer");
    }

    void Update()
    {
        currPosition = transform.position;

        RaycastHit hit;
        Vector3 dir = prevPosition - currPosition;
        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, projAndEnemy))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Projectile"))
            {
                hit.transform.gameObject.GetComponent<Skeleton>().Hit();

                Destroy(gameObject);
            }
        }

        prevPosition = currPosition;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTimer);

        Destroy(gameObject);
    }
}
