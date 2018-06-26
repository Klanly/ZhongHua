using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：表格2管理
/// </summary>
public class Table2 : MonoBehaviour {

	public static  string tablenumber;




	//清空表格
	public void ClearTable()
	{
		tablenumber = "";
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).gameObject.SetActive (false);
		}
	}


	//刷新
	public void RefreshTable()
	{
		string nownum;
		for (int i = 0; i < tablenumber.Length; i++) {
			nownum=tablenumber.Substring (i,1);
			this.transform.GetChild (i).gameObject.SetActive (true);
			this.transform.GetChild(i).GetComponent<Image>().sprite=SpriteData.Instance.Num[int.Parse(nownum)];
		}


	}
}
