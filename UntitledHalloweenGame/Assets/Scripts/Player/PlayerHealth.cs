using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    string originalText;
    int currCandy;
    int maxCandy = 100;
    int damage = 10;
    float hitTimer = 0.5f;
    bool isHit = false;

    void Start()
    {
        currCandy = maxCandy;

        originalText = healthText.text;
        healthText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isHit && collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            isHit = true;
            currCandy -= damage;
            healthText.text = originalText + currCandy.ToString() + " / " + maxCandy.ToString();

            if (currCandy <= 0)
            {
                Debug.LogError("You Are Dead!!!");
            }

            Destroy(collision.gameObject);
            StartCoroutine("HitTimer");
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(hitTimer);

        isHit = false;
    }
}
