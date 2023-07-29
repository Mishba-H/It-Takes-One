using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleDrone : MonoBehaviour
{
    private GameObject droneMother;
    internal bool isActive = false;
    internal Rigidbody2D rb;
    internal bool isFacingRight = true;
    [SerializeField] internal float moveSpeed = 4f;
    private float xInput;
    private float yInput;
    private Vector2 moveDirection;
    private bool slowTime = false;
    private bool canControlTime = true;
    [SerializeField] private float slowedTimeScale;
    [SerializeField] private float timeControlDuration;
    private float timeControlCounter;
    [SerializeField] private float timeControlInterval;

    private AudioSource source;

    void Awake() 
    {
        droneMother = GameObject.FindGameObjectWithTag("DroneMother");
        rb = GetComponent<Rigidbody2D>();
        timeControlCounter = timeControlDuration;

        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        GetInput();
        Flip();
        TimeControl();
    }

    void LateUpdate()
    {
        Move();
    }

    void GetInput()
    {
        moveDirection = droneMother.GetComponent<InputManager>().moveDirection;
        slowTime = droneMother.GetComponent<InputManager>().actionA == 1;
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

    void Move()
    {
        if (moveDirection != Vector2.zero && !isActive)
        {
            rb.transform.position += new Vector3(moveDirection.x, moveDirection.y, 0f) * moveSpeed * Time.unscaledDeltaTime;
        }
        else 
        {
            rb.velocity = Vector2.zero;
        }
    }

    void TimeControl()
    {
        if (timeControlCounter > 0f && slowTime && canControlTime)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowedTimeScale, 0.05f);
            timeControlCounter -= Time.unscaledDeltaTime;

            if (!source.isPlaying) source.Play();
        }
        else if (timeControlCounter <= 0f)
        {
            canControlTime = false;
            timeControlCounter = timeControlDuration;
            StartCoroutine(TimeControlCoroutine());
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.1f);
        }
    }

    IEnumerator TimeControlCoroutine()
    {
        yield return new WaitForSeconds(timeControlInterval);
        canControlTime = true;
    }
}