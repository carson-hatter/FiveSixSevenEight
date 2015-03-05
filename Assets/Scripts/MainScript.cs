using UnityEngine;

using System.Collections.Generic;
using System;
using System.Linq;

using AssemblyCSharp;

public class MainScript : MonoBehaviour {

	public GameObject FloorPrefab;
	private GameObject floorInstantiated = null;

	public GameObject [] NumberPrefabs;

	public Vector2 GridDimensions;

	public float DropYCoord;

	public float DropPauseInSeconds;

	private System.Random random;

	// Use this for initialization
	void Start () 
	{
		try
		{
			random = new System.Random();

			if(NumberPrefabs.Length <= 0)
				throw new NullReferenceException("No number prefabs found");

			float numberPrefabWidth = NumberPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

			floorInstantiated = Instantiate(FloorPrefab) as GameObject;
//
//			numbersInstantiated = new List<List<GameObject>>();
//			float xOffset = -(GridDimensions.x * numberPrefabWidth) / 2;
//			float lastDrop = Time.realtimeSinceStartup;
//			for(int i = 0; i < GridDimensions.x; i++)
//			{
//				numbersInstantiated.Add(new List<GameObject>());
//
//				for(int j = 0; j < GridDimensions.y; j++)
//				{
//					numbersInstantiated[i].Add (Instantiate(NumberPrefabs[random.Next() % NumberPrefabs.Length]) as GameObject);
//
//					numbersInstantiated[i].Last<GameObject>().transform.position = new Vector3(xOffset, DropYCoord);
//				}
//
//				while(Time.realtimeSinceStartup > DropPauseInSeconds)
//				{
//					// do nothing
//				}
//
//				lastDrop = Time.realtimeSinceStartup;
//
//				xOffset += numberPrefabWidth;
//			}
		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}



	// Update is called once per frame
	void Update () 
	{
		try
		{
			if(Grid.AnyOpenSpaces)
			{
				GameObject toAdd = Instantiate(NumberPrefabs[random.Next() % NumberPrefabs.Count()]) as GameObject;
				Grid.Add(toAdd);
			}
		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
