using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_B_Script : MonoBehaviour
{
    [SerializeField] internal bool isFacingRight = true;
    internal Rigidbody2D rb;
    private LineRenderer lr;
    private DistanceJoint2D dj;
    [SerializeField] internal float moveSpeed = 3f;
    [SerializeField] private float groundDetectRadius = 2f;
    [SerializeField] private Transform detector; 
    internal bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    internal RaycastHit2D wallRay;
    [SerializeField] private float wallRayLength;
    [SerializeField] private LayerMask wallLayer;
    private bool isGrappling = false;
    [SerializeField] private float grappleRadius = 2f;
    [SerializeField] private LayerMask grappleLayer;
    private Vector3 grappleHookPosition;
    [SerializeField] internal PhysicsMaterial2D bouncy;
    [SerializeField] internal PhysicsMaterial2D nonBouncy;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();

        lr.enabled = false;
        dj.enabled = false;
    }

    private void OnEnable() {
        isGrounded = false;
    }

    void FixedUpdate() 
    {
        Flip();
        Move();
        GroundCheck();
        WallCheck();
        AutoGrapple();
        DrawRope();
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
        if (isGrounded)
            rb.velocity = new Vector2(isFacingRight? moveSpeed : -moveSpeed, rb.velocity.y);
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(detector.position, groundDetectRadius, groundLayer);
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

    void AutoGrapple()
    {
        Collider2D collider = Physics2D.OverlapCircle(detector.position, grappleRadius, grappleLayer);
        if (collider != null && collider.CompareTag("Bot") && !isGrappling &&
            Mathf.Abs(transform.position.y - collider.transform.position.y) < 0.2f && !isGrounded)
        {
            isGrappling = true;
            dj.enabled = true;
            dj.connectedAnchor = collider.transform.position;
            grappleHookPosition = collider.transform.position;
            collider.GetComponentInParent<GrappleDrone>().isActive = true;
        }
        if ((isGrappling && collider != null && collider.CompareTag("Bot") && 
            Mathf.Abs(transform.position.x - collider.transform.position.x) < 0.2f))
        {
            isGrappling = false;
            dj.enabled = false;
            collider.GetComponentInParent<GrappleDrone>().isActive = false;
        }
        if (isGrounded)
        {
            isGrappling = false;
            dj.enabled = false;
        }
    }

    void DrawRope()
    {
        if (isGrappling) 
        {
            lr.enabled = true;
            lr.SetPosition(0, detector.position);
            lr.SetPosition(1, grappleHookPosition);
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(detector.position, groundDetectRadius);
        Gizmos.DrawLine(detector.position, detector.position + new Vector3(wallRayLength, 0, 0));
        Gizmos.DrawWireSphere(detector.position, grappleRadius);
    }
}