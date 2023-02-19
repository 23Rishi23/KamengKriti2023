using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Animator anim;
    [SerializeField] private float movementSpeed = 10f;
    private Vector3 playerVelocity;
    private float speedMultiplier = 1f;
    private bool facingRight;
    private float gravityValue;

    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    private float initialVelocity;
    private bool isJumping;
    [SerializeField] private GameObject feetPosition;

    [SerializeField] private GameObject handPosition;
    [SerializeField] private LayerMask wallLayer;
    private bool isSliding;
    private bool canSlide;
    [SerializeField] private float slideSpeed;
    private bool wallJumping;
    void Start()
    {
        SetupJumpVariables();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        facingRight = true;
    }

    void FixedUpdate()
    {
        Move();
        transform.SetPositionAndRotation(new Vector3(0, transform.position.y, transform.position.z), transform.rotation);
        playerVelocity.y -= gravityValue * Time.deltaTime;
        if (Physics.CheckSphere(feetPosition.transform.position, 0.1f, groundLayer))
        {
            isGrounded = true;
        }
        canSlide = Physics.CheckSphere(handPosition.transform.position, 0.1f, wallLayer);
        if (canSlide && !isGrounded)
        {
            anim.SetBool("isSliding", true);
            isSliding = true;
        } else
        {
            anim.SetBool("isSliding", false);
            if (isJumping) anim.SetBool("isJumping", true);
            isSliding = false;
        }

        if(canSlide && !isGrounded && ((facingRight && Input.GetAxis("Horizontal") < 0) || (!facingRight && Input.GetAxis("Horizontal") > 0)))
        {
            wallJumping = true;
        } else
        {
            wallJumping = false;
        }
    }

    private void Move()
    {
        //Player Movement Horizontally and Animation Control
        anim.SetBool("isWalking", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isCrouching", false);
        anim.SetBool("isFalling", false);
        isJumping = false;
        if(Input.GetAxis("Horizontal") != 0)
        {
            if(playerVelocity.z > 0)
            {
                facingRight = true;
            } else if (playerVelocity.z < 0)
            {
                facingRight = false;
            }
            if(facingRight)
            {
                transform.forward = new Vector3(0, 0, 1);
            } else
            {
                transform.forward = new Vector3(0, 0, -1);
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                speedMultiplier = 1.5f;
                if(isGrounded) anim.SetBool("isRunning", true);
            } else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speedMultiplier = 1f;
                anim.SetBool("isRunning", false);
            }
            if (isGrounded)
            {
                anim.SetBool("isWalking", true);
            }
            if (!isSliding && !wallJumping)
            {
                playerVelocity = new Vector3(0, playerVelocity.y, Input.GetAxis("Horizontal") * movementSpeed * speedMultiplier);
            }
        }

        //Jump and Crouch Behaviours
        if ((Input.GetAxis("Vertical") > 0 || Input.GetKeyDown(KeyCode.Space)) && isGrounded && !isJumping)
        {
            StartJumpIdle();
        }

        if (Input.GetAxis("Vertical") < 0 && isGrounded && Input.GetAxis("Horizontal") == 0)
        {
            anim.SetBool("isCrouching", true);
        }

        //Sliding against the wall
        if(canSlide || true)
        {
            SlidingAndJumping();
        }
        //Debug.Log(playerVelocity);
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void StartJumpIdle()
    {
        anim.SetBool("isJumping", true);
        isJumping = true;
        isGrounded = false;
        Invoke("JumpIdle", 0.15f);
    }

    private void JumpIdle()
    {
        playerVelocity.y = initialVelocity;
    }

    private void SetupJumpVariables()
    {
        float timeToApex = jumpTime / 2;
        gravityValue = 2 * jumpHeight / (timeToApex * timeToApex);
        initialVelocity = 2 * jumpHeight / timeToApex;
    }

    private void SlidingAndJumping()
    {
        if (isSliding)
        {
            playerVelocity = new Vector3(0, -slideSpeed, 0);
        }

        if (wallJumping)
        {
            anim.SetBool("isSliding", false);
            anim.SetBool("isJumping", true);
            isSliding = false;
            playerVelocity = new Vector3(0, 2*initialVelocity, -Mathf.Sign(transform.localScale.z) * initialVelocity);
        }
    }
}
