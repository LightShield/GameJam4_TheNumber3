using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{

    public GameObject startButton, quitButton;

    private void Start()
    {                    
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void onStartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void onExitClicked()
    {
        Debug.Log("exit clicked");
        Application.Quit();
    }

    public void onQuitClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
