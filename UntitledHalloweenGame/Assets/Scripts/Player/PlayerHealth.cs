using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    GameObject deadMenu, pauseMenu;
    Text candyText;
    Slider healthBar;

    LayerMask damageLayer;

    string originalText;
    int currHealth, maxHealth, currCandy = 0, maxCandy = 100, damage = 10;
    float hitTimer = 0.8f;
    bool isHit = false;

    void Start()
    {
        deadMenu = GameObject.FindGameObjectWithTag("GameOver");
        pauseMenu = GameObject.FindGameObjectWithTag("Pause");
        candyText = GameObject.FindGameObjectWithTag("Candy").GetComponent<Text>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();

        deadMenu.SetActive(false);
        pauseMenu.SetActive(false);

        maxHealth = 100;
        currHealth = maxHealth;
        healthBar.value = ((float)currHealth / maxHealth);

        originalText = candyText.text;
        candyText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();

        damageLayer = LayerMask.NameToLayer("Melee");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            GetComponent<PlayerController>().Dead = !GetComponent<PlayerController>().Dead;
            GetComponent<PlayerShoot>().Dead = !GetComponent<PlayerShoot>().Dead;

            Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void Damage()
    {
        if (!isHit)
        {
            Debug.Log("Hit Player");
            isHit = true;
            currHealth -= damage;

            if (currHealth < 0)
                currHealth = 0;

            healthBar.value = ((float)currHealth / maxHealth);

            if (currHealth <= 0)
            {
                GetComponent<PlayerController>().Dead = true;
                GetComponent<PlayerShoot>().Dead = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                deadMenu.SetActive(true);
            }
            StartCoroutine(HitTimer());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        //if (!isHit && collision.gameObject.layer == damageLayer)
        //{
        //    Debug.Log("Hit Player");
        //    isHit = true;
        //    currHealth -= damage;

        //    if (currHealth < 0)
        //        currHealth = 0;

        //    healthBar.value = ((float)currHealth / maxHealth);

        //    if (currHealth <= 0)
        //    {
        //        GetComponent<PlayerController>().Dead = true;
        //        GetComponent<PlayerShoot>().Dead = true;

        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //        deadMenu.SetActive(true);
        //    }
        //    StartCoroutine(HitTimer());
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickUp"))
        {
            int pickUpAmount = other.gameObject.GetComponentInParent<CandyPickup>().PickUpAmount;
            currCandy = (currCandy + pickUpAmount) >= maxCandy ? maxCandy : currCandy + pickUpAmount;
            candyText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();
            Destroy(other.transform.root.gameObject);
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimer);

        isHit = false;
    }
}
