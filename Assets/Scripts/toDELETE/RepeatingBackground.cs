using UnityEngine;
using System.Collections;

public class RepeatingBackground : MonoBehaviour 
{
	private float groundHorizontalLength;		//A float to store the x-axis length of the collider2D attached to the Ground GameObject.

	//Awake is called before Start.
	private void Awake ()
	{
		//Store the size of the collider along the x axis (its length in units).
		groundHorizontalLength = 14.39f;
	}

	//Update runs once per frame
	private void Update()
	{
		//Check if the difference along the x axis between the main Camera and the position of the object this is attached to is greater than groundHorizontalLength.
		if (transform.position.x < -2f * groundHorizontalLength)
		{
			//If true, this means this object is no longer visible and we can safely move it forward to be re-used.
			RepositionBackground ();
		}
	}

	//Moves the object this script is attached to right in order to create our looping background effect.
	private void RepositionBackground()
	{
		//This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
		Vector2 groundOffSet = new Vector2(groundHorizontalLength * 4f , 0);

		//Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
		transform.position = (Vector2) transform.position + groundOffSet;
	}
}