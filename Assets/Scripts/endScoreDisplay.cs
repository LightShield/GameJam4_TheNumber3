using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class endScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("updating score");
        int score = 0;
        if (PlayerPrefs.HasKey("score"))
        {
            Debug.Log("has key score");
            score = (int)PlayerPrefs.GetFloat("score");
        }
        scoreText.text += "\n" + score;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
