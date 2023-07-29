using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("NPC"))
        {
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("Bot"))
        {
            GameObject droneMother = GameObject.FindGameObjectWithTag("DroneMother");
            Destroy(droneMother);
        }
    }
}
