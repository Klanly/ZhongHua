using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：设置按钮
/// </summary>
public class SetPanel : MonoBehaviour {

	public void SetIP()
	{
		if(this.transform.GetChild(0).GetComponent<InputField>().text!=null)
		{
			PlayerPrefs.SetString ("ip", this.transform.GetChild (0).GetComponent<InputField> ().text);
		}

		this.gameObject.SetActive (false);
	}

}
