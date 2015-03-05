using UnityEngine;

using System.Collections.Generic;
using System;
using System.Linq;

using AssemblyCSharp;

public class MainScript : MonoBehaviour {

	public GameObject FloorPrefab;
	private GameObject floorInstantiated = null;

	public GameObject WallPrefab;
	private List<GameObject> wallsInstantiated = null;

	public GameObject [] NumberPrefabs;

	public Vector2 GridDimensions;

	//public float DropYCoord;

	public float DropPauseInSeconds;

	private float lastDrop;

	private System.Random random;

	// Use this for initialization
	void Start () 
	{
		try
		{
			lastDrop = Time.realtimeSinceStartup;

			random = new System.Random();

			if(NumberPrefabs.Length <= 0)
				throw new NullReferenceException("No number prefabs found");

			float numberPrefabWidth = NumberPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x; // number prefab sprite will have to be square

			floorInstantiated = Instantiate(FloorPrefab) as GameObject;

			floorInstantiated.transform.position = new Vector3(0, -((GridDimensions.y + 1) * numberPrefabWidth) / 2);
			floorInstantiated.transform.localScale = new Vector3(((GridDimensions.x + 2) * numberPrefabWidth), floorInstantiated.transform.localScale.y); // sprite width will have to equal number prefab widths

			wallsInstantiated = new List<GameObject>();
			wallsInstantiated.Add (Instantiate(WallPrefab) as GameObject);
			wallsInstantiated[0].transform.position = new Vector3(-((GridDimensions.x * numberPrefabWidth) / 2) - (numberPrefabWidth / 2), 0);
			wallsInstantiated[0].transform.localScale = new Vector3(wallsInstantiated[0].transform.localScale.x, numberPrefabWidth * GridDimensions.y);
			wallsInstantiated.Add (Instantiate(WallPrefab) as GameObject);
			wallsInstantiated[1].transform.position = new Vector3(((GridDimensions.x * numberPrefabWidth) / 2) + (numberPrefabWidth / 2), 0);
			wallsInstantiated[1].transform.localScale = new Vector3(wallsInstantiated[0].transform.localScale.x, numberPrefabWidth * GridDimensions.y);

			Grid.Resize(GridDimensions, ((GridDimensions.y + 1) * numberPrefabWidth) / 2, numberPrefabWidth);


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
			if(Time.realtimeSinceStartup > lastDrop + DropPauseInSeconds)
			{
				if(Grid.AnyOpenSpaces)
				{
					Debug.Log("test1");
					GameObject toAdd = Instantiate(NumberPrefabs[random.Next() % NumberPrefabs.Count()]) as GameObject;
					Grid.Add(toAdd);
					lastDrop = Time.realtimeSinceStartup;
				}
			}
		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
