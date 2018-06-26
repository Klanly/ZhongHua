using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：屏幕适配相关
/// </summary>
public class Adaptive1 : MonoBehaviour {

	//数量
	public float X_Count, Y_Count;

	private Vector2 ObjectSize;


	void Start () {
		Invoke ("SetCellSize",0.1f);
	}




	//设置当期尺寸
	public void SetCellSize()
	{
		ObjectSize = Vector2.right*this.transform.GetComponent<RectTransform> ().rect.width+Vector2.up*this.transform.GetComponent<RectTransform> ().rect.height;
	
		this.GetComponent<GridLayoutGroup> ().cellSize = Vector2.right*ObjectSize.x / X_Count + Vector2.up*ObjectSize.y / Y_Count;
	}



}
