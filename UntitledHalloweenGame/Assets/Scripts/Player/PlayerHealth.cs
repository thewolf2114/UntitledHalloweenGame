using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deadMenu;
    public GameObject pauseMenu;
    public Text healthText;
    string originalText;
    int currCandy = 15;
    int maxCandy = 100;
    int damage = 10;
    float hitTimer = 0.1f;
    bool isHit = false;

    void Start()
    {
        originalText = healthText.text;
        healthText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetAxis("Cancel") != 0)
        {
            GetComponent<PlayerController>().Dead = true;
            GetComponent<PlayerShoot>().Dead = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (!isHit && collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            isHit = true;
            currCandy -= damage;

            if (currCandy < 0)
                currCandy = 0;

            healthText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();

            if (currCandy <= 0)
            {
                GetComponent<PlayerController>().Dead = true;
                GetComponent<PlayerShoot>().Dead = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                deadMenu.SetActive(true);
            }

            Destroy(collision.gameObject);
            StartCoroutine("HitTimer");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickUp"))
        {
            int pickUpAmount = other.gameObject.GetComponentInParent<CandyPickup>().PickUpAmount;
            currCandy = (currCandy + pickUpAmount) >= maxCandy ? maxCandy : currCandy + pickUpAmount;
            healthText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();
            Destroy(other.transform.root.gameObject);
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimer);

        isHit = false;
    }
}
