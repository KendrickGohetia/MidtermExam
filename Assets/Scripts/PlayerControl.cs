using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float upForce = 200.0f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckerRadius = 0.15f;
    [SerializeField] private Transform groundChecker;

    private Rigidbody2D rBody2d;
    private bool onGround = false;
    private bool isDucking = false;
    private bool isFacingRight = true;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rBody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horiz = Input.GetAxis("Horizontal");
        onGround = OnGroundChecker();

        if (onGround && Input.GetAxis("Jump") > 0)
        {
            rBody2d.AddForce(new Vector2(0.0f, upForce));
            //rBody2d.velocity = new Vector2(0.0f, upForce);
            onGround = false;
        }
        
        rBody2d.velocity = new Vector2(horiz * speed, rBody2d.velocity.y);

        if (onGround && rBody2d.velocity.x == 0 && Input.GetAxis("Vertical") < 0)
        {
            isDucking = true;
        }
        else
        {
            isDucking = false;
        }

        if (isFacingRight && rBody2d.velocity.x < 0)
        {
            Flip();
        }
        else if (!isFacingRight && rBody2d.velocity.x > 0)
        {
            Flip();
        }

        anim.SetFloat("xSpeed", Mathf.Abs(rBody2d.velocity.x));
        anim.SetFloat("ySpeed", rBody2d.velocity.y);
        anim.SetBool("isGrounded", onGround);
        anim.SetBool("isDucking", isDucking);
    }

    private bool OnGroundChecker()
    {
        return Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround); ;
    }

    private void Flip()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;

        isFacingRight = !isFacingRight;
    }
}
