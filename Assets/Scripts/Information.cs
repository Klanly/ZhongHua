using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：获得信息更新界面
/// </summary>
public class Information : MonoBehaviour {

	private Text TimeText;
	private Text InningText;

	private Text Text1;
	private Text Text2;
	private Text Text3;

	void Start () {
		TimeText = this.transform.GetChild (5).GetChild (0).GetComponent<Text> ();
		InningText = this.transform.GetChild (6).GetChild (0).GetComponent<Text> ();
		Text1 = this.transform.GetChild (7).GetComponent<Text> ();
		Text2 = this.transform.GetChild (8).GetComponent<Text> ();
		Text3 = this.transform.GetChild (9).GetComponent<Text> ();
	}
	


	public void SetTime(string t)
	{
		if (t == "-1") {
			TimeText.text = "0";
		} else {
			TimeText.text = t;
		}

	}

	public void SetInning(string s)
	{
		InningText.text = "第"+s+"局";
	}


	public void SetText1(string string1)
	{
		Text1.text = "全台限红\n"+string1;
	}

	public void SetText2(string string2)
	{
//		Text2.text = " 0 限注\n"+string2;
		Text2.text = " 和限注\n"+string2;
	}

	public void SetText3(string string3)
	{
		Text3.text = "最低限注\n"+string3;
	}

}
