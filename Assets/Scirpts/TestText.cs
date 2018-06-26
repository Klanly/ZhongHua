using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour {

	public static string Debugtext;


	void Start () {
		
	}
	

	void Update () {
		if(Debugtext==null||Debugtext=="")
		{
			this.transform.GetChild (0).GetComponent<Text> ().text = Debugtext;
		}
	}
}
