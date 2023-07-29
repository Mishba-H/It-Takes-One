using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float explodeInterval;
    [SerializeField] private float blastRadius;
    [SerializeField] private LayerMask objectsLayer;

    private AudioSource source;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        source = GetComponent<AudioSource>();
    }

    internal void Explode()
    {
        StartCoroutine(ExplodeCoroutine());
    }

    IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(explodeInterval);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius, objectsLayer);
        foreach (Collider2D obj in colliders)
        {
            Destroy(obj.gameObject);
        }

        source.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
