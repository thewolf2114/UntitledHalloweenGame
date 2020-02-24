using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");

        if (moveForward != 0 || moveRight != 0)
        {
            playerAnimator.SetFloat("Speed_f", 1);
        }
        else
        {
            playerAnimator.SetFloat("Speed_f", 0);
        }
    }
}
