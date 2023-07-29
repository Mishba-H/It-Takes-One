using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMother : MonoBehaviour
{
    [SerializeField] private GameObject[] drones;
    private int i = 0;
    private GameObject drone;
    
    private AudioSource source;

    void Awake()
    {
        drone = Instantiate(drones[0], transform.position, Quaternion.identity);
        source = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        FollowDrone();
    }

    internal void SwapDrone(int a)
    {
        if (drone.GetComponent<LauncherDrone>() != null)
            if (drone.GetComponent<LauncherDrone>().npcLoaded) return;
            
        if (drone.GetComponent<GrappleDrone>() == null) Time.timeScale = 1;
        

        i += a;
        if (i == -1)
        {
            i = drones.Length - 1;
        }
        if (i == drones.Length)
        {
            i = 0;
        }
        Vector3 pos = drone.transform.position;
        Destroy(drone);
        drone = Instantiate(drones[i], pos, Quaternion.identity);

        source.Play();
    }

    void FollowDrone()
    {
        transform.position = drone.transform.position;
    }
}
