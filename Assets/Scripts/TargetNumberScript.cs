using UnityEngine;
using System.Collections;

public class TargetNumberScript : MonoBehaviour {
	public Sprite TargetedTexture;
	public Sprite NontargetedTexture;
	public int Number;

	private bool isTarget;
	public bool IsTarget
	{
		get{ return isTarget;}
		set
		{
			if(this.GetComponent<SpriteRenderer>()!=null)
			{
				isTarget = value;

				if(isTarget)
					this.GetComponent<SpriteRenderer>().sprite = TargetedTexture;
				else
					this.GetComponent<SpriteRenderer>().sprite = NontargetedTexture;


				//Debug.Log(Number.ToString() + " ***** " + isTarget.ToString() + " ----- " + sprite.sprite.name);
			}
		}
	}
	
	private static SpriteRenderer sprite;
	
	// Use this for initialization
	void Awake () 
	{
		try
		{
			sprite = GetComponent<SpriteRenderer>();
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
