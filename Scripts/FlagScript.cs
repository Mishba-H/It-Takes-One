using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagScript : MonoBehaviour
{
    [SerializeField] private GameObject droneMother;
    [SerializeField] private GameObject[] npcs;
    internal bool levelPassed = false;
    internal bool allnpcDestroyed = false;
    private int flag = 0;

    void Update()
    {
        foreach (GameObject npc in npcs)
        {
            if (npc != null) 
            {
                flag = 0;
                break;
            }
            flag = 1;
        }

        if (flag == 1) allnpcDestroyed = true;
        if (flag == 0) allnpcDestroyed = false;

        if (allnpcDestroyed && levelPassed)
        {
            Debug.Log("Level Cleared");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (allnpcDestroyed && !levelPassed)
        {
            Debug.Log("Level Failed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (droneMother == null)
        {
            Debug.Log("Level Failed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("NPC"))
        {
            levelPassed = true;
            Destroy(collider.gameObject);
        }    
    }
}
