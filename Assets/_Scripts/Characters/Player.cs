using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float jumpForce = 30, maxJump = 4, moveForce = 10, moveSpeed = 10;
    float moveAmount;
    bool canJump = true;
    Rigidbody2D rb;
    Resources resources;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        resources = GetComponent<Resources>();
    }

    private void Update()
    {
        float speedFactor;
        if (resources.currentSizeValue < 1f)
        {
            speedFactor = ((resources.currentSizeValue - 2) * (resources.currentSizeValue - 2) + 2) / 3;
        }
        else
        {
            speedFactor = 1 - Mathf.Sqrt(resources.currentSizeValue - 1) / 5;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(jumpForce * speedFactor * Vector2.up, ForceMode2D.Impulse);
            if (rb.velocity.y > maxJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJump);
            }
        }
        moveAmount = Input.GetAxis("Horizontal") * speedFactor;
    }
    void FixedUpdate()
    {
        rb.AddForce(moveAmount * moveSpeed * Vector2.right);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
