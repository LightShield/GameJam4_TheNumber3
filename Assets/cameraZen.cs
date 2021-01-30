using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZen : MonoBehaviour
{

    private Camera cam;

    public Color regularColor;
    public Color zenColor;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_START_ZEN_MODE,startZenMode);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_STOP_ZEN_MODE,stopZenMode);
    }

    private void startZenMode(object obj)
    {
        cam.backgroundColor = zenColor;
    }

    private void stopZenMode(object obj)
    {
        cam.backgroundColor = regularColor;
    }

    
}
