using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Players shooting mechanic
/// </summary>
public class PlayerShoot : MonoBehaviour
{
    public Transform projectileStart;
    public Transform reticle;
    public GameObject projectile;
    public int distance = 5;
    public int projectileSpeed = 50;
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
        Vector3 direction = Camera.main.transform.forward;
        spawnedProjectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootTimer);

        canShoot = true;
    }
}
