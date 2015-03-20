using UnityEngine;
using System.Collections;

public class MultiplierScript : MonoBehaviour {
	
	private static int multiplierToDisplay = -1;
	public static int MultiplierToDisplay
	{
		get{ return multiplierToDisplay;}
		set
		{
			multiplierToDisplay = value;

			text.text = string.Format(MultiplierText, multiplierToDisplay.ToString());
		}
	}

	public static string MultiplierText = string.Empty;
	
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
