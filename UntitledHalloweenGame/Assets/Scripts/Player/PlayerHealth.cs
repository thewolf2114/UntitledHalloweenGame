using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    GameObject deadMenu, pauseMenu;
    Slider healthBar;

    LayerMask damageLayer;

    string originalText;
    int currHealth, maxHealth, damage = 10;
    float hitTimer = 0.8f;
    bool isHit = false;

    void Start()
    {
        deadMenu = GameObject.FindGameObjectWithTag("GameOver");
        pauseMenu = GameObject.FindGameObjectWithTag("Pause");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();

        deadMenu.SetActive(false);
        pauseMenu.SetActive(false);

        maxHealth = 100;
        currHealth = maxHealth;
        healthBar.value = ((float)currHealth / maxHealth);

        damageLayer = LayerMask.NameToLayer("Melee");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            Pausable[] pausables = FindObjectsOfType<Pausable>();
            for (int i = 0; i < pausables.Length; i++)
            {
                pausables[i].IsPaused = !pausables[i].IsPaused;

                if (pausables[i].gameObject.GetComponent<Animator>())
                {
                    pausables[i].gameObject.GetComponent<Animator>().enabled = !pausables[i].gameObject.GetComponent<Animator>().enabled;
                }

            }
            Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void Damage()
    {
        if (!isHit)
        {
            isHit = true;
            currHealth -= damage;

            if (currHealth < 0)
                currHealth = 0;

            healthBar.value = ((float)currHealth / maxHealth);

            if (currHealth <= 0)
            {
                Pausable[] pausables = FindObjectsOfType<Pausable>();
                for (int i = 0; i < pausables.Length; i++)
                {
                    pausables[i].IsPaused = !pausables[i].IsPaused;

                    if (pausables[i].gameObject.GetComponent<Animator>())
                    {
                        pausables[i].gameObject.GetComponent<Animator>().enabled = !pausables[i].gameObject.GetComponent<Animator>().enabled;
                    }

                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                deadMenu.SetActive(true);
            }
            StartCoroutine(HitTimer());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickUp"))
        {
            int pickUpAmount = other.gameObject.GetComponentInParent<CandyPickup>().PickUpAmount;
            GameManager.Instance.AddCurrCandy(pickUpAmount);
            Destroy(other.transform.root.gameObject);
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimer);

        isHit = false;
    }
}
