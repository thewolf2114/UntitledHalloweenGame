using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : Pausable
{
    private Animator playerAnimator;
    private float groundCheckDistance = 1f;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused)
            return;

        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");
        float moveVal = Mathf.Abs(moveForward) + Mathf.Abs(moveRight);
        bool jumping = Input.GetKey(KeyCode.Space);

        playerAnimator.SetFloat("Speed_f", moveVal);
        playerAnimator.SetBool("Jump_b", jumping);
        playerAnimator.SetBool("Grounded", !jumping);
    }
}
