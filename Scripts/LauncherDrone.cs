using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherDrone : MonoBehaviour
{
    [SerializeField] internal bool isFacingRight = true; 
    private GameObject droneMother;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private GameObject launchPoint;
    [SerializeField] private float throwStrength;
    internal bool npcLoaded;
    [SerializeField] private float range;
    [SerializeField] private LayerMask npcLayer;
    private GameObject npc;
    private Vector2 moveDirection;
    private TrajectoryScript trajectoryScript;

    [SerializeField] private AudioSource source1;
    [SerializeField] private AudioSource source2;

    void Awake()
    {
        droneMother = GameObject.FindGameObjectWithTag("DroneMother");
        rb = GetComponent<Rigidbody2D>();
        trajectoryScript = GetComponent<TrajectoryScript>();
        trajectoryScript.enabled = false;
    }

    void FixedUpdate()
    {
        InputCheck();
        Movement();
        Flip();
        LoadNPC();

        SetTrajectory();
    }

    void Flip()
    {
        if (moveDirection.x > 0) isFacingRight = true;
        else if (moveDirection.x < 0) isFacingRight = false;

        if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void InputCheck()
    {
        moveDirection = droneMother.GetComponent<InputManager>().moveDirection;

        if(droneMother.GetComponent<InputManager>().actionA == 1 && npcLoaded)
        {
            Launch();
        }
    }

    private void Movement()
    {
        if (npcLoaded)
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * 0.25f, rb.velocity.y);
        else
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void Launch()
    {
        npcLoaded = false;
        Vector2 launchDirection = droneMother.GetComponent<InputManager>().aimDirection;
        npc.SetActive(true);
        npc.transform.position = launchPoint.transform.position;
        npc.GetComponent<Rigidbody2D>().velocity = launchDirection.normalized * throwStrength;
        trajectoryScript.enabled = false;

        source2.Play();
    }

    private void LoadNPC()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, range, npcLayer);
        if (collider !=null && collider.CompareTag("NPC") && droneMother.GetComponent<InputManager>().actionB == 1 && !npcLoaded)
        {
            npc = collider.gameObject;
            npc.SetActive(false);
            npcLoaded = true;
            trajectoryScript.enabled = true;

            source1.Play();
        }
    }

    void SetTrajectory()
    {
        if (trajectoryScript.isActiveAndEnabled)
        {
            trajectoryScript.directionVector = droneMother.GetComponent<InputManager>().aimDirection;
            trajectoryScript.launchPosition = launchPoint.transform.position;
            trajectoryScript.launchSpeed = throwStrength;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
} 
