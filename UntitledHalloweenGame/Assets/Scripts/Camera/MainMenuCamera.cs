using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField]
    GameObject cowboy, skeleton;

    Vector3 betweenPoint;
    Vector3 movementVector;
    float movementSpeed = 0.1f;
    bool moveLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        // finding the halfway point between the cowboy and skeleton
        Vector3 direction = cowboy.transform.position - skeleton.transform.position;
        float halfDistance = direction.magnitude / 2;
        betweenPoint = skeleton.transform.position + direction.normalized * halfDistance;
        betweenPoint += Vector3.up;

        // have the camera look at the spot between the characters
        transform.LookAt(betweenPoint);

        movementVector = Vector3.right;

        StartCoroutine(MoveRight());
    }

    IEnumerator MoveRight()
    {
        movementVector = Vector3.right;

        while (transform.position.x < -6.6f)
        {
            transform.position += movementVector * movementSpeed * Time.deltaTime;
            transform.LookAt(betweenPoint);

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MoveLeft());
    }

    IEnumerator MoveLeft()
    {
        movementVector = Vector3.left;

        while (transform.position.x > -8)
        {
            transform.position += movementVector * movementSpeed * Time.deltaTime;
            transform.LookAt(betweenPoint);

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MoveRight());
    }
}
