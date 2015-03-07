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

			//NumberPrefabs.OrderByDescending(npf => npf.GetComponent<NumberScript>().Probability);

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

		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	GameObject GetNumberPrefab()
	{
		int totalWeight = NumberPrefabs.Sum (np => np.GetComponent<NumberScript> ().Weight);
		int randomNumber = random.Next (0, totalWeight);

		GameObject toReturn = null;
		foreach(GameObject go in NumberPrefabs)
		{
			if(randomNumber < go.GetComponent<NumberScript>().Weight)
			{
				toReturn = go;
				break;
			}
			randomNumber -= go.GetComponent<NumberScript>().Weight;
		}

		return toReturn;
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
					GameObject toAdd = Instantiate(GetNumberPrefab()) as GameObject;
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
