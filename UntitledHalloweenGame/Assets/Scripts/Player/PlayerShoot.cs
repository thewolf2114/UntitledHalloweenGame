using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Players shooting mechanic
/// </summary>
public class PlayerShoot : MonoBehaviour
{
    public Transform projectileStart;
    public GameObject projectile;
    public int distance = 5;
    public int projectileSpeed = 25;
    public float shootTimer = 0.25f;
    bool canShoot = true;

    public bool Dead
    { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
            return;

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            canShoot = false;
            SpawnAndShoot();
            StartCoroutine("ShootTimer");
        }
    }

    void SpawnAndShoot()
    {
        GameObject spawnedProjectile = Instantiate(projectile, projectileStart.position, Quaternion.identity) as GameObject;

        RaycastHit hit;
        Vector3 direction = Camera.main.transform.forward;
        if (Physics.Raycast(Camera.main.gameObject.transform.position, direction, out hit, Mathf.Infinity))
        {
            Vector3 projectileDirection = hit.point - projectileStart.position;
            spawnedProjectile.GetComponent<Rigidbody>().AddForce(projectileDirection.normalized * projectileSpeed, ForceMode.Impulse);
        }
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootTimer);

        canShoot = true;
    }
}
