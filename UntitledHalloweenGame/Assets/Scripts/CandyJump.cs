using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyJump : MonoBehaviour
{
    [SerializeField]
    Animator candyAnim;

    Vector3 currVelocity;
    Vector3 startVelocity;
    Vector3 gravity = Vector3.down * 9.81f;

    float lerpTimer = 0;
    float timeMultiplyer = 2.5f;
    int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        startVelocity = transform.forward;
        startVelocity = Quaternion.Euler(60, 0, 0) * startVelocity;
        startVelocity = startVelocity.normalized;
        gravity = gravity.normalized;
        currVelocity = startVelocity;

        Debug.DrawLine(transform.position, transform.position + (startVelocity * speed), Color.red);

        candyAnim.enabled = false;

        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        bool jumping = true;

        while (jumping)
        {
            currVelocity = Vector3.Slerp(startVelocity, gravity, lerpTimer);
            lerpTimer += (Time.deltaTime * timeMultiplyer);

            transform.position += currVelocity * speed * Time.deltaTime;

            if (lerpTimer >= 1)
                jumping = false;

            yield return new WaitForEndOfFrame();
        }


        candyAnim.enabled = true;
    }
}
