using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    Vector3 startVector;
    int jumpForce = 5;
    int moveForce = 5;
    int maxHoverHeight = 100;
    float minHoverHeight = .2f;
    bool hover = false;
    bool floatUp = true;

    public int PickUpAmount
    { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        float xAngle = .5f;
        float zAngle = Mathf.Sqrt(3) / 2;
        startVector = new Vector3(Mathf.Cos(xAngle), 1, Mathf.Sin(zAngle));
        GetComponent<Rigidbody>().AddForce(startVector.normalized * jumpForce, ForceMode.Impulse);

        PickUpAmount = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down * Mathf.Infinity, Color.yellow, 1);

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.distance < 2)
                hover = true;

            if (hover)
            {
                if (floatUp)
                {
                    transform.position += Vector3.up * moveForce * Time.deltaTime;

                    if (hit.distance > maxHoverHeight)
                        floatUp = false;
                }

                if (hit.distance < minHoverHeight)
                    floatUp = true;
            }
        }
    }
}
