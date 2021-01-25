using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float expectedRatio = 1920f / 1080f;
        float diffRatio = screenRatio / expectedRatio;

        if (screenRatio < expectedRatio)
        {
            Camera.main.orthographicSize = (1080f / 200f) / diffRatio;
        }
        else
        {
            Camera.main.orthographicSize = (1080f / 200f);
        }
    }
}
