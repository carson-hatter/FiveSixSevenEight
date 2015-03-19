using UnityEngine;

using System.Collections.Generic;
using System;
using System.Linq;

using AssemblyCSharp;

public class MainScript : MonoBehaviour {

	public GameObject FloorPrefab;
	private GameObject floorInstantiated = null;

	public GameObject ScoreboardPrefab;
	private GameObject scoreboardInstantiated = null;

	public GameObject MultiplierPrefab;
	private GameObject multiplierBoardInstantiated = null;

	public GameObject WallPrefab;
	private List<GameObject> wallsInstantiated = null;

	public GameObject [] NumberPrefabs;

	public Vector2 GridOffset;
	public Vector2 GridDimensions;

	public float DropPauseInSeconds;

	private float lastDrop;

	private System.Random random;

	private List<GameObject> selectedNumbers = new List<GameObject>();

	private bool isTouchDevice;

	private int score;

	private int multiplier;

	void Start () 
	{
		try
		{
			score = 0;
			multiplier = 1;

#if UNITY_IOS || UNITY_ANDROID
			isTouchDevice = true;
#else
			isTouchDevice = false;
#endif

			lastDrop = Time.realtimeSinceStartup;

			random = new System.Random();

			if(NumberPrefabs.Length <= 0)
				throw new NullReferenceException("No number prefabs found");

			scoreboardInstantiated = Instantiate(ScoreboardPrefab) as GameObject;
			multiplierBoardInstantiated = Instantiate(MultiplierPrefab) as GameObject;

			float numberPrefabWidth = NumberPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x; // number prefab sprite will have to be square

			floorInstantiated = Instantiate(FloorPrefab) as GameObject;

			floorInstantiated.transform.position = new Vector3(GridOffset.x, -((GridDimensions.y + 1) * numberPrefabWidth) / 2 + GridOffset.y);
			floorInstantiated.transform.localScale = new Vector3(((GridDimensions.x + 2) * numberPrefabWidth), floorInstantiated.transform.localScale.y); // sprite width will have to equal number prefab widths

			wallsInstantiated = new List<GameObject>();
			wallsInstantiated.Add (Instantiate(WallPrefab) as GameObject);
			wallsInstantiated[0].transform.position = new Vector3(-((GridDimensions.x * numberPrefabWidth) / 2) - (numberPrefabWidth / 2) + GridOffset.x, GridOffset.y);
			wallsInstantiated[0].transform.localScale = new Vector3(wallsInstantiated[0].transform.localScale.x, numberPrefabWidth * GridDimensions.y);
			wallsInstantiated.Add (Instantiate(WallPrefab) as GameObject);
			wallsInstantiated[1].transform.position = new Vector3(((GridDimensions.x * numberPrefabWidth) / 2) + (numberPrefabWidth / 2) + GridOffset.x, GridOffset.y);
			wallsInstantiated[1].transform.localScale = new Vector3(wallsInstantiated[0].transform.localScale.x, numberPrefabWidth * GridDimensions.y);

			Grid.Resize(GridDimensions, (((GridDimensions.y + 1) * numberPrefabWidth) / 2) + GridOffset.y, numberPrefabWidth);

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

	bool MouseInput()
	{
		bool clearSelectedNumbersFlag = false;

		if(Input.GetMouseButton(0))
		{
			Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			mosPos.z = 0;
			
			List<List<GameObject>> numsInstd = Grid.NumbersInstiated;
			
			for(int colIndex = 0; colIndex < numsInstd.Count(); colIndex++)
			{
				for(int rowIndex = 0; rowIndex < numsInstd[colIndex].Count; rowIndex++)
				{
					GameObject go = numsInstd[colIndex][rowIndex];

					if(Vector3.Distance(mosPos, go.transform.position) <= go.GetComponent<CircleCollider2D>().bounds.size.x / 2)
					{
						if(selectedNumbers.Count > 0)
						{
							if(Vector3.Distance(go.transform.position, selectedNumbers.Last().transform.position) <= (Mathf.Sqrt(2) + 0.001))
							{
								NumberScript script = go.GetComponent<NumberScript>();
								
								if(!script.Selected)
								{
									script.Selected = true;
									selectedNumbers.Add (go);
									break;
								}
							}
						}
						else
						{
							NumberScript script = go.GetComponent<NumberScript>();
							
							if(!script.Selected)
							{
								script.Selected = true;
								selectedNumbers.Add (go);
								break;
							}
						}
					}
				}
			}
		}
		else
		{
			if(selectedNumbers.Count > 0)
			{
				clearSelectedNumbersFlag = true;
			}
		}

		return clearSelectedNumbersFlag;
	}

	void Update () 
	{
		try
		{
			bool clearSelectedNumbers = false;
			if(!isTouchDevice)
				clearSelectedNumbers = MouseInput();

			if(clearSelectedNumbers)
			{
				int tally = 0;

				bool allSameColor = true;
				string theColor = string.Empty;

				for(int i = 0; i < selectedNumbers.Count; i++)
				{
					NumberScript selectedScript = selectedNumbers[i].GetComponent<NumberScript>();

					if(selectedScript.Selected)
					{
						if(string.IsNullOrEmpty(theColor))
							theColor = selectedScript.Color_;
						else if(!string.Equals(theColor, selectedScript.Color_, StringComparison.OrdinalIgnoreCase))
							allSameColor = false;

						tally += selectedScript.BaseScore;

						Grid.Remove(selectedNumbers[i].GetInstanceID());
						Destroy(selectedNumbers[i]);
					}
				}

				selectedNumbers.Clear();

				if(allSameColor)
				{
					multiplier++;
					MultiplierScript.MultiplierToDisplay = multiplier;
				}

				score += (tally * multiplier);

				ScoreboardScript.ScoreToDisplay = score;
			}

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
