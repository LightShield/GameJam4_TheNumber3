using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingEnemy : MonoBehaviour 
{

	public float speed = 1;
	private playerMovement mPlayerMovement;

	private void Start()
	{
		mPlayerMovement = GameObject.Find("player").GetComponent<playerMovement>();
	}

	void Update()
	{
		if (mPlayerMovement._horizontalMove >= 0.1f)
		{
			transform.Translate(Time.deltaTime * speed  * -1 *GameControl.instance.globalSpeed, 0, 0,
				Camera.main.transform);
			// If the game is over, stop scrolling.
		}
		else
		{
			transform.Translate(Time.deltaTime * speed * 0.5f * -1 * GameControl.instance.globalSpeed, 0, 0,
				Camera.main.transform);
		}
		if (GameControl.instance.gameOver == true) speed = 0;


	}
}


