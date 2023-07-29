using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberDrone : MonoBehaviour
{
    [SerializeField] internal bool isFacingRight = true; 
    private GameObject droneMother;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private GameObject launchPoint;
    [SerializeField] private float throwStrength;
    internal bool isLoaded;
    [SerializeField] private float range;
    [SerializeField] private LayerMask objectsLayer;
    private GameObject obj;
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
        Load();

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

        if(droneMother.GetComponent<InputManager>().actionA == 1 && isLoaded)
        {
            Launch();
        }
    }

    private void Movement()
    {
        if (isLoaded)
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * 0.25f, rb.velocity.y);
        else
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void Launch()
    {
        isLoaded = false;
        Vector2 launchDirection = droneMother.GetComponent<InputManager>().aimDirection;
        obj.SetActive(true);
        obj.transform.position = launchPoint.transform.position;
        obj.GetComponent<Rigidbody2D>().velocity = launchDirection.normalized * throwStrength;
        if (obj.GetComponent<GrenadeScript>() != null)
        {
            obj.GetComponent<GrenadeScript>().Explode();
        }

        trajectoryScript.enabled = false;

        source2.Play();
    }

    private void Load()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, range, objectsLayer);
        if (collider !=null && collider.CompareTag("Object") && droneMother.GetComponent<InputManager>().actionB == 1 && !isLoaded)
        {
            obj = collider.gameObject;
            obj.SetActive(false);
            isLoaded = true;
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