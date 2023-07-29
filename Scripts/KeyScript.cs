using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.6f, 0f);

    public AudioSource audioA;
    public AudioSource audioB;

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("NPC"))
        {
            transform.position = collider.transform.position + offset;
            transform.SetParent(collider.gameObject.transform);

            audioA.Play();
        }    

        if (collider.CompareTag("Door"))
        {
            audioB.Play();

            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
