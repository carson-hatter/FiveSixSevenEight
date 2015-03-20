using UnityEngine;
using System.Collections;

public class TargetNumberScript : MonoBehaviour {
	
	private static int targetToDisplay = -1;
	public static int TargetToDisplay
	{
		get{ return targetToDisplay;}
		set
		{
			targetToDisplay = value;

			text.text = string.Format(TargetText, targetToDisplay.ToString());
		}
	}
	
	public static string TargetText = string.Empty;
	
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
