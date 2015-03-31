using UnityEngine;
using System.Collections;

public class NextNumberDisplayScript : MonoBehaviour {

	public Sprite Sprite_
	{
		private get
		{
			return GetComponent<SpriteRenderer>().sprite;
		}
		set
		{
			if(sr != null)
				sr.sprite = value;
		}
	}

	private SpriteRenderer sr;
	void Awake()
	{
		sr = GetComponent<SpriteRenderer> ();
	}
}
