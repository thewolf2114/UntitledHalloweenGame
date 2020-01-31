using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bully : MonoBehaviour
{
    public Transform projectileStart;
    public GameObject player;
    public GameObject projectile;
    public GameObject candyPickup;
    public float firingRate = 0.5f;
    public int projectileSpeed = 50;
    public int shootRadius = 50;
    public int health = 100;
    public int projectileDamage = 25;
    float hitTimeer = 0.1f;
    bool isHit = false;
    bool canShoot = true;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (!dead && distance < shootRadius && canShoot)
        {
            canShoot = false;
            transform.LookAt(player.transform);
            Shoot();
            StartCoroutine("ShootTimer");
        }
    }

    void Shoot()
    {
        GameObject spawnedProjectile = Instantiate(projectile, projectileStart.position, Quaternion.identity) as GameObject;
        Vector3 direction = projectileStart.forward;
        spawnedProjectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(firingRate);

        canShoot = true;
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimeer);

        isHit = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!dead && !isHit && collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            isHit = true;
            health -= projectileDamage;

            Instantiate(candyPickup, transform.position, Quaternion.identity);

            if (health <= 0)
            {
                dead = true;

                transform.Rotate(Vector3.right, 90);
                transform.position = new Vector3(transform.position.x, transform.position.y - GetComponent<CapsuleCollider>().height / 2, transform.position.z);
                GetComponent<Rigidbody>().isKinematic = true;
            }

            Destroy(collision.gameObject);
            StartCoroutine("HitTimer");
        }
    }
}
