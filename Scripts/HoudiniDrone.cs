using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoudiniDrone : MonoBehaviour
{
    private GameObject droneMother;
    private Rigidbody2D rb;
    private bool canSwap = true;
    private GameObject npc;
    [SerializeField] private float checkRadius = 1f;
    [SerializeField] private LayerMask npcLayer;
    [SerializeField] internal float moveSpeed = 2.5f;
    [SerializeField] private float acceleration = 10;
    private Vector2 moveDirection;

    private AudioSource source;

    void Awake() 
    {
        droneMother = GameObject.FindGameObjectWithTag("DroneMother");
        rb = GetComponent<Rigidbody2D>();

        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        moveDirection = droneMother.GetComponent<InputManager>().moveDirection;
        Move();

        Collider2D collider = Physics2D.OverlapCircle(transform.position, checkRadius, npcLayer);
        if (collider != null && collider.CompareTag("NPC") && droneMother.GetComponent<InputManager>().actionA == 1 && canSwap)
        {
            npc = collider.gameObject;
            Swap();
        }
        if (droneMother.GetComponent<InputManager>().actionA == 0)
        {
            canSwap = true;
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

    private void Swap(){
        if (npc == null)
        {
            return;
        }
        canSwap = false;
        Vector3 tempPosition = transform.position;
        transform.position = npc.transform.position;
        npc.transform.position = tempPosition;

        source.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}