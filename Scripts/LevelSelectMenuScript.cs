using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectMenuScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        string scene = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(scene);
    }
}
