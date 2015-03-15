using UnityEngine;
using System.Collections;

public class MultiplierScript : MonoBehaviour {
	
	private static int multiplierToDisplay;
	public static int MultiplierToDisplay
	{
		get{ return multiplierToDisplay;}
		set
		{
			multiplierToDisplay = value;
			text.text = "x" + multiplierToDisplay.ToString();
		}
	}
	
	private static TextMesh text;
	
	// Use this for initialization
	void Start () 
	{
		try
		{
			text = GetComponent<TextMesh>();
			MultiplierToDisplay = 0;
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
