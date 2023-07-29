using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : MonoBehaviour
{
    internal Rigidbody2D rb;
    private GameObject droneMother;
    internal bool isFacingRight = true;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float acceleration;
    private Vector2 moveDirection;

    void Awake() 
    {
        droneMother = GameObject.FindGameObjectWithTag("DroneMother");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        moveDirection = droneMother.GetComponent<InputManager>().moveDirection;
        Flip();
        Move();
    }

    void Flip()
    {
        if (moveDirection.x > 0) isFacingRight = true;
        else if (moveDirection.y < 0) isFacingRight = false;

        if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Move()
    {
        if (rb.velocity.magnitude >= moveSpeed && moveDirection != Vector2.zero)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        else if (moveDirection != Vector2.zero)
        {
            rb.velocity += new Vector2(acceleration * moveDirection.x * Time.fixedDeltaTime, 0f);
            rb.velocity += new Vector2(0f, acceleration * moveDirection.y * Time.fixedDeltaTime);
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity -= new Vector2(acceleration * Time.fixedDeltaTime, 0f);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0f, moveSpeed), rb.velocity.y);
            }
            else if (rb.velocity.x < 0)
            {
                rb.velocity -= new Vector2(-acceleration * Time.fixedDeltaTime, 0f);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, 0f), rb.velocity.y);
            }
            if (rb.velocity.y > 0)
            {
                rb.velocity -= new Vector2(0f, acceleration * Time.fixedDeltaTime);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0f, moveSpeed));
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity -= new Vector2(0f, -acceleration * Time.fixedDeltaTime);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -moveSpeed, 0f));
            }
        }
    }
}
