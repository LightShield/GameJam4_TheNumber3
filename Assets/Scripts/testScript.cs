using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update

    public SpriteRenderer l, m, s, x;
    public int layerCounter = 4;
    SpriteRenderer[] layers;
    void Start()
    {
        layers = new SpriteRenderer[layerCounter];
        layers[0] = x;
        layers[1] = s;
        layers[2] = m;
        layers[3] = l;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            --layerCounter;
            layers[layerCounter].enabled = false;
        }
    }
}
