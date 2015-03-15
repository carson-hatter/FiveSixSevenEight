using UnityEngine;

using System.Collections.Generic;
using System;
using System.Linq;

public class NumberScript : MonoBehaviour {

	public int Weight;

	public int Number;

	public string Color_;

	public int BaseScore;

	public Sprite UnselectedTexture;
	public Sprite SelectedTexture;

	private bool selected;
	public bool Selected
	{ 
		get
		{
			return selected;
		}
		set
		{
			if(value == true)
			{
				this.GetComponent<SpriteRenderer>().sprite = SelectedTexture;
			}
			else
			{
				this.GetComponent<SpriteRenderer>().sprite = UnselectedTexture;
			}

			selected = value;
		} 
	}

	void Start () 
	{
		Selected = false;
	}
}
