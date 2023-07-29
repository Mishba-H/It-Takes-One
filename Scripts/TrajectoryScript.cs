using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryScript : MonoBehaviour
{
    internal LineRenderer lr;
    private float collisionCheckRadius = 0.1f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float simulateForDuration = 5f;//simulate for 5 secs in the furture
    [SerializeField] private float simulationStep = 0.1f;//Will add a point every 0.1 secs.
    private int steps;
    internal Vector2 directionVector = new Vector2(0.5f,0.5f);//You plug you own direction here this is just an example
    internal Vector2 launchPosition = Vector2.zero;//Position where you launch from
    internal float launchSpeed = 10f;//Example speed per secs.

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        steps = (int)(simulateForDuration/simulationStep);
    }

    void OnEnable()
    {
        lr.enabled = true;
    }

    void OnDisable()
    {
        lr.enabled = false;
    }

    void FixedUpdate()
    {
        SimulateArc();
    }

    private void SimulateArc()
    {
        Vector2[] lineRendererPoints = new Vector2[steps];
        Vector2 calculatedPosition;
        int activePoints = 0;
        for(int i = 0; i < steps; ++i)
        {
            calculatedPosition.x = launchSpeed * directionVector.x * i * simulationStep;
            calculatedPosition.y = launchSpeed * directionVector.y * i * simulationStep + 
                (Physics2D.gravity.y * i * simulationStep * i * simulationStep)/2;
            calculatedPosition += launchPosition;
            lineRendererPoints[i] = calculatedPosition;
            activePoints += 1;
            if(CheckForCollision(calculatedPosition))//if you hit something
            {
                break;//stop adding positions
            }
        }

        lr.positionCount = activePoints;
        //Assign all the positions to the line renderer.
        for (int i = 0; i < activePoints; i ++)
        {
            lr.SetPosition(i, lineRendererPoints[i]);
        }
    }
    private bool CheckForCollision(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, collisionCheckRadius, layerMask);
        if(hits.Length > 0)
        {
            //We hit something 
            //check if its a wall or seomthing
            //if its a valid hit then return true
            return true;
        }
        return false;
    }
}
