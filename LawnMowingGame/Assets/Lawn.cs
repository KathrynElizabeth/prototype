using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawn : MonoBehaviour 
{
	[SerializeField]
	private GameObject grassPrefab;

	[SerializeField]
	private GameObject trimmedGrassPrefab;

	[SerializeField]
	private int gridWidth;

	[SerializeField]
	private int gridHeight;

	private float trimmedGrassWidth;
	private float trimmedGrassDepth;

	private float grassWidth;
	private float grassDepth;

	void Awake()
	{
		trimmedGrassWidth = trimmedGrassPrefab.transform.localScale.x;
		trimmedGrassDepth = trimmedGrassPrefab.transform.localScale.z;

		grassWidth = grassPrefab.transform.localScale.x;
		grassDepth = grassPrefab.transform.localScale.z;

		GenerateGrassGrid ();
	}

	private void GenerateGrassGrid()
	{
		for (int i = 0; i < gridWidth; i++) 
		{
			for (int j = 0; j < gridHeight; j++) 
			{
				Transform trimmedGrass = GameObject.Instantiate (trimmedGrassPrefab, transform).transform;
				trimmedGrass.localPosition = new Vector3 (trimmedGrassWidth * i, 0.1f, trimmedGrassDepth * j);

				Transform grass = GameObject.Instantiate (grassPrefab, transform).transform;
				grass.localPosition = new Vector3 (grassWidth * i, 0.5f, grassDepth * j);
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
