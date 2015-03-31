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

	public GameObject NumberFivePrefab;
	private GameObject numFiveInst;
	public GameObject NumberSixPrefab;
	private GameObject numSixInst;
	public GameObject NumberSevenPrefab;
	private GameObject numSevenInst;
	public GameObject NumberEightPrefab;
	private GameObject numEightInst;

	public GameObject NextNumberDisplayPrefab;
	private GameObject nextNumDisplayInst;
	private NextNumberDto nextNum;

	public string MultiplierText;
	public string ScoreboardText;

	public GameObject WallPrefab;
	private List<GameObject> wallsInstantiated = null;

	public GameObject [] NumberPrefabs;

	public Vector2 GridOffset;
	public Vector2 GridDimensions;

	public float DropPauseInSeconds;

	private float lastDrop;

	private System.Random random;

	private List<GameObject> selectedNumbers = new List<GameObject>();

	private int score;
	private int targetNumber;
	private int multiplier;

	void Start () 
	{
		try
		{
			lastDrop = Time.realtimeSinceStartup;

			random = new System.Random();

			if(NumberPrefabs.Length <= 0)
				throw new NullReferenceException("No number prefabs found");

			scoreboardInstantiated = Instantiate(ScoreboardPrefab) as GameObject;
			multiplierBoardInstantiated = Instantiate(MultiplierPrefab) as GameObject;
			numFiveInst = Instantiate(NumberFivePrefab) as GameObject;
			numSixInst = Instantiate(NumberSixPrefab) as GameObject;
			numSevenInst = Instantiate(NumberSevenPrefab) as GameObject;
			numEightInst = Instantiate(NumberEightPrefab) as GameObject;
			score = 0;
			multiplier = 1;
			targetNumber = 5;
			Update5678();
			ScoreboardScript.ScoreText = ScoreboardText;
			MultiplierScript.MultiplierText = MultiplierText;

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

			Grid.Resize(GridDimensions, (((GridDimensions.y + 1) * numberPrefabWidth) / 2) + GridOffset.y, numberPrefabWidth, GridOffset.x);

			nextNumDisplayInst = Instantiate(NextNumberDisplayPrefab) as GameObject;
		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	NextNumberDto GetNumberPrefab()
	{
		int totalWeight = NumberPrefabs.Sum (np => np.GetComponent<NumberScript> ().Weight);
		int randomNumber = random.Next (0, totalWeight);

		NextNumberDto toReturn = null;
		foreach(GameObject go in NumberPrefabs)
		{
			if(randomNumber < go.GetComponent<NumberScript>().Weight)
			{
				toReturn = new NextNumberDto(go.GetComponent<NumberScript>().Color_, go.GetComponent<NumberScript>().Shape, go.GetComponent<NumberScript>().UnselectedTexture, go.GetComponent<NumberScript>().Number);
				break;
			}
			randomNumber -= go.GetComponent<NumberScript>().Weight;
		}

		return toReturn;
	}

	void Update5678()
	{
		if(targetNumber == 5)
		{
			numFiveInst.GetComponent<TargetNumberScript>().IsTarget = true;
			numSixInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSevenInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numEightInst.GetComponent<TargetNumberScript>().IsTarget = false;
			return;
		}
		if(targetNumber == 6)
		{
			numFiveInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSixInst.GetComponent<TargetNumberScript>().IsTarget = true;
			numSevenInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numEightInst.GetComponent<TargetNumberScript>().IsTarget = false;
			return;
		}
		if(targetNumber == 7)
		{
			numFiveInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSixInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSevenInst.GetComponent<TargetNumberScript>().IsTarget = true;
			numEightInst.GetComponent<TargetNumberScript>().IsTarget = false;
			return;
		}
		if(targetNumber == 8)
		{
			numFiveInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSixInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numSevenInst.GetComponent<TargetNumberScript>().IsTarget = false;
			numEightInst.GetComponent<TargetNumberScript>().IsTarget = true;
			return;
		}
	}

	bool UserInput()
	{
		bool clearSelectedNumbersFlag = false;

		if(Input.GetMouseButton(0) || Input.touches.Count() > 0)
		{
			Vector3 inPos;
#if UNITY_ANDROID || UNITY_IOS
			inPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
#else
			inPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
			inPos.z = 0;
			
			List<List<GameObject>> numsInstd = Grid.NumbersInstiated;
			
			for(int colIndex = 0; colIndex < numsInstd.Count(); colIndex++)
			{
				for(int rowIndex = 0; rowIndex < numsInstd[colIndex].Count; rowIndex++)
				{
					GameObject go = numsInstd[colIndex][rowIndex];
					if(Vector3.Distance(inPos, go.transform.position) <= go.GetComponent<CircleCollider2D>().bounds.size.x / 2)
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
			if(ScoreboardScript.ScoreToDisplay != score)
				ScoreboardScript.ScoreToDisplay = score;
			if(MultiplierScript.MultiplierToDisplay != multiplier)
				MultiplierScript.MultiplierToDisplay = multiplier;

			bool clearSelectedNumbers = false;

			clearSelectedNumbers = UserInput();

			if(clearSelectedNumbers)
			{
				int tally = 0;

				bool allSameColor = true;
				string theColor = string.Empty;

				bool allSameShape = true;
				string theShape = string.Empty;

				for(int i = 0; i < selectedNumbers.Count; i++)
				{
					NumberScript selectedScript = selectedNumbers[i].GetComponent<NumberScript>();

					if(selectedScript.Selected)
					{
						if(string.IsNullOrEmpty(theColor))
							theColor = selectedScript.Color_;
						else if(!string.Equals(theColor, selectedScript.Color_, StringComparison.OrdinalIgnoreCase))
							allSameColor = false;

						if(string.IsNullOrEmpty(theShape))
							theShape = selectedScript.Shape;
						else if(!string.Equals(theShape, selectedScript.Shape, StringComparison.OrdinalIgnoreCase))
							allSameShape = false;

						tally += selectedScript.BaseScore;
					}
				}

				if(tally == targetNumber)
				{
					for(int i = 0; i < selectedNumbers.Count; i++)
					{
						Grid.Remove(selectedNumbers[i].GetInstanceID());
						Destroy(selectedNumbers[i]);
					}

					selectedNumbers.Clear();

					if(allSameColor && allSameShape)
					{
						if(multiplier == 1)
							multiplier += 3;
						else
							multiplier*=multiplier;
					}
					else if(allSameColor)
					{
						multiplier++;
					}
					else if(allSameShape)
					{
						multiplier++;
					}

					score += (tally * multiplier);

					targetNumber++;
					if(targetNumber > 8)
						targetNumber = 5;

					Update5678();
				}
				else
				{
					foreach(GameObject targetNumPF in selectedNumbers)
					{
						targetNumPF.GetComponent<NumberScript>().Selected = false;
					}

					selectedNumbers.Clear();
				}
			}

			if(Time.realtimeSinceStartup > lastDrop + DropPauseInSeconds)
			{
				if(Grid.AnyOpenSpaces)
				{
					if(nextNum == null)
						nextNum = GetNumberPrefab();

					Grid.Add (Instantiate(NumberPrefabs.First(n => n.GetComponent<NumberScript>().Color_ == nextNum.Color_ && n.GetComponent<NumberScript>().Shape == nextNum.Shape && n.GetComponent<NumberScript>().Number == nextNum.Number)) as GameObject);
					lastDrop = Time.realtimeSinceStartup;

					nextNum = GetNumberPrefab();
					nextNumDisplayInst.GetComponent<NextNumberDisplayScript>().Sprite_ = nextNum.Sprite_;
				}
			}
		}
		catch(Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
