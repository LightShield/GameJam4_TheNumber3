using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void onStartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void onExitClicked()
    {
        Debug.Log("exit clicked");
        Application.Quit();
    }
}
