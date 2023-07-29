using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private bool startMovingAutomatically = true;

    private Vector3 targetPosition;
    private bool movingToA;
    private Transform playerTransform;

    void Start()
    {
        targetPosition = startMovingAutomatically ? pointA.position : transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            if (movingToA)
            {
                targetPosition = pointB.position;
            }
            else
            {
                targetPosition = pointA.position;
            }

            movingToA = !movingToA;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bot") || other.CompareTag("NPC"))
        {
            
            playerTransform = other.transform;
            playerTransform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Bot") || other.CompareTag("NPC")) && other.transform == playerTransform)
        {
            playerTransform.parent = null;
        }
    }
}

