using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class SpriteData : MonoBehaviour {

	public static SpriteData Instance;

	public Sprite [] Num;
	public Sprite[] Dot;

	void Awake()
	{
		Instance = this;
		Application.runInBackground = true;
	}
}
