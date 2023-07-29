using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_A_Script : MonoBehaviour
{
    [SerializeField] internal bool isFacingRight = true;
    internal Rigidbody2D rb;
    [SerializeField] internal float moveSpeed = 3f;
    [SerializeField] private float detectRadius = 2f;
    [SerializeField] private Transform detector; 
    internal bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    internal RaycastHit2D wallRay;
    [SerializeField] private float wallRayLength;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] internal PhysicsMaterial2D bouncy;
    [SerializeField] internal PhysicsMaterial2D nonBouncy;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        isGrounded = false;
    }

    void FixedUpdate() 
    {
        Flip();
        if (isGrounded) Move();
        GroundCheck();
        WallCheck();
    }

    void Flip()
    {
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
        rb.velocity = new Vector2(isFacingRight? moveSpeed : -moveSpeed, rb.velocity.y);
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(detector.position, detectRadius, groundLayer);
    }

    void WallCheck()
    {
        wallRay = Physics2D.Raycast(detector.position, isFacingRight ? Vector2.right : Vector2.left, wallRayLength, wallLayer);
        
        //Turn From Walls
        if (wallRay.collider != null)
        {
            rb.sharedMaterial = bouncy;
            isFacingRight = !isFacingRight;
        }
        else
        {
            rb.sharedMaterial = nonBouncy;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(detector.position, detectRadius);
        Gizmos.DrawLine(detector.position, detector.position + new Vector3(wallRayLength, 0, 0));
    }
}