using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D playerBody;

    [SerializeField]
    private float playerSpeed;

    private Animator playerAnimator;
    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private bool aircontrol;
	// Use this for initialization

    
	void Start ()
    {
        facingRight = true;
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
	}

    void Update()
    {
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
        flip(horizontal);
        ResetValues();

    }

    private void HandleMovement(float horizontal)
    {
        if (playerBody.velocity.y < 0)
        {
            //playerAnimator.SetBool("land", true);
        }
        if (isGrounded || aircontrol)
        {
            playerBody.velocity = new Vector2(horizontal * playerSpeed, playerBody.velocity.y);
            playerAnimator.SetFloat("speed", Mathf.Abs(horizontal));
            

        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            playerBody.AddForce(new Vector2(0, jumpForce));
            playerAnimator.SetLayerWeight(1, 1);
            playerAnimator.SetBool("jump",true);
        }
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }
    private void flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 thescale = transform.localScale;

            thescale.x *= -1;

            transform.localScale = thescale;
        }

    }

    private void ResetValues()
    {
        jump = false;
    }
    private bool IsGrounded()
    {
        if (playerBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        playerAnimator.SetBool("jump", false);
                        playerAnimator.SetLayerWeight(1, 0);
                        return true;
                    }
                }
            }
        }
        return false;
    }


}
