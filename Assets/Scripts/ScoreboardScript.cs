using UnityEngine;
using System.Collections;

public class ScoreboardScript : MonoBehaviour {

	private static int scoreToDisplay;
	public static int ScoreToDisplay
	{
		get{ return scoreToDisplay;}
		set
		{
			scoreToDisplay = value;
			text.text = "Score: " + scoreToDisplay.ToString();
		}
	}

	private static TextMesh text;

	// Use this for initialization
	void Start () 
	{
		try
		{
			text = GetComponent<TextMesh>();
			ScoreToDisplay = 0;
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
