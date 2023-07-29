using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    void Awake()
    {   
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else 
        {
            Destroy(instance);
        }
    }
}