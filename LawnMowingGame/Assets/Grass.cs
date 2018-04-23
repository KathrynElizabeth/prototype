using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour 
{
	private const float MIN_POSITION_Y = -0.5f;
	private const float MAX_POSITION_Y = 0.5f;

	private const float GRASS_CUT_SPEED = 0.01f;

	public bool Mowed { get; private set; }

	// Use this for initialization
	void Start () 
	{
		Mowed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Mow()
	{
		Vector3 localPosition = transform.localPosition;

		if (localPosition.y > MIN_POSITION_Y)
			SetGrassHeight (localPosition.y - GRASS_CUT_SPEED);
		else 
		{
			SetGrassHeight (MIN_POSITION_Y);
			Mowed = true;
		}
	}

	private void SetGrassHeight(float newGrassHeight)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, newGrassHeight, transform.localPosition.z);
	}
}
