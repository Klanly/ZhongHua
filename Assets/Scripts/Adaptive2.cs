using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：屏幕适配相关
/// </summary>
public class Adaptive2 : MonoBehaviour {

	//数量
	public float X_Count, Y_Count;

	private Vector2 ObjectSize;

	private float X_Ratio=0.5f,Y_Ratio=7f/15f;
	//间隔比例
	private float X_Interval,Y_Interval; 


	void Start () {
		Invoke ("SetCellSize",0.1f);
	}
	

	//设置当期尺寸
	public void SetCellSize()
	{
		ObjectSize = Vector2.right*this.transform.GetComponent<RectTransform> ().rect.width+Vector2.up*this.transform.GetComponent<RectTransform> ().rect.height;

		X_Interval = ObjectSize.x / X_Count * X_Ratio;
		Y_Interval= ObjectSize.y / Y_Count * Y_Ratio;

		this.GetComponent<GridLayoutGroup> ().cellSize = Vector2.right*((ObjectSize.x-X_Interval*(X_Count-1)) / X_Count) + Vector2.up*((ObjectSize.y-Y_Interval*(Y_Count-1)) / Y_Count);


//		this.GetComponent<GridLayoutGroup> ().spacing=Vector2.right*(ObjectSize.x / X_Count*X_Interval) + Vector2.up*(ObjectSize.y / Y_Count*Y_Interval);
		this.GetComponent<GridLayoutGroup> ().spacing=Vector2.right*X_Interval+Vector2.up*Y_Interval;
//		this.GetComponent<GridLayoutGroup> ().spacing = Vector2.right * 6.5f + Vector2.up * 7f;
	}
}
