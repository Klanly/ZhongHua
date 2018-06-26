using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：表格1管理
/// </summary>
public class Table1 : MonoBehaviour {

	public static  string tablenumber;



	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			tablenumber+="1";
			RefreshTable ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			tablenumber+="2";
			RefreshTable ();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			tablenumber+="0";
			RefreshTable ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			ClearTable ();
		}
	}


	//清空表格
	public void ClearTable()
	{
		tablenumber = "";
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).GetChild (0).gameObject.SetActive (false);
		}
	}


	//刷新
	public void RefreshTable()
	{
		int x=0,y=0;
		int pos;
		string lastnum="",nownum="";
		string type = "";

		for (int i = 0; i < tablenumber.Length; i++) {
			nownum=tablenumber.Substring (i,1);
			if (lastnum != "") {
				if (type == "")
				{
					x++;
				} else 
				{
					if (nownum == type || nownum == "2")
					{
						x++;
					} else 
					{
						x = 0;
						y++;
					}

				}

			} 
			if(x>=6)
			{
				x = 0;
				y++;
			}
			pos = x + y * 6;
			this.transform.GetChild (pos).GetChild (0).gameObject.SetActive (true);
//			this.transform.GetChild(pos).GetChild(0).GetComponent<Image>().sprite=SpriteData.Instance.Num[int.Parse(nownum)];
			this.transform.GetChild(pos).GetChild(0).GetComponent<Image>().sprite=SpriteData.Instance.Dot[int.Parse(nownum)];
			lastnum = nownum;
			if(nownum!="2")
			{
				type = nownum;
			}
		}
	}
}
