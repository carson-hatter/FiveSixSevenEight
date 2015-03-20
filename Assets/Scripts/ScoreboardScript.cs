using UnityEngine;
using System.Collections;

public class ScoreboardScript : MonoBehaviour {

	private static int scoreToDisplay = -1;
	public static int ScoreToDisplay
	{
		get{ return scoreToDisplay;}
		set
		{
			scoreToDisplay = value;
		
			text.text = string.Format(ScoreText,scoreToDisplay.ToString());
		}
	}

	public static string ScoreText = string.Empty;

	private static TextMesh text;

	// Use this for initialization
	void Start () 
	{
		try
		{
			text = GetComponent<TextMesh>();
			text.text = string.Empty;
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
