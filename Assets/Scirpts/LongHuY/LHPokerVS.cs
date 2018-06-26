using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 龙虎牌比较效果
/// </summary>
public class LHPokerVS : MonoBehaviour {

	public Sprite[] LHType;
	public Sprite[] LH_value;

	public Sprite[] PokerSprites;

	public string pokersvalue_L;
	public string pokersvalue_H;

	public GameObject RightBG;
	public GameObject LiftBG;
	public GameObject VsBG;

	public GameObject poker_L;
	public GameObject poker_H;

	public GameObject draw;

	public Image L_Value;
	public Image H_Value;

	public Image L_Type;
	public Image H_Type;


	private Vector2 r_pos;
	private Vector2 l_pos;

	private bool IsPanelMove;
	private bool IsdealCard_1,IsdealCard_2;
	private bool IsDrawCard_1,IsDrawCard_2;

	float MoveValue;
	float dealValue1,dealValue2;
	float Drawvalue;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Reset ();

		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			IsPanelMove = true;

		}

		if(IsPanelMove)
		{
			MoveValue += Time.deltaTime/2;
			RightBG.GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (r_pos,Vector2.zero,MoveValue);
			LiftBG.GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (l_pos,Vector2.zero,MoveValue);
			if(MoveValue>=1f)
			{
				MoveValue=0;
				RightBG.GetComponent<RectTransform> ().anchoredPosition=Vector2.zero;
				IsPanelMove=false;
				IsdealCard_1 = true;
				poker_H.SetActive (true);

				VsBG.SetActive (true);
				L_Type.sprite = LHType [0];
				H_Type.sprite = LHType [1];
				L_Type.gameObject.SetActive (true);
				H_Type.gameObject.SetActive (true);
			}
		}
		if(IsdealCard_1)
		{
			dealValue1 += Time.deltaTime*2;
			poker_H.transform.position = Vector3.Lerp (draw.transform.position,poker_H.transform.parent.position,dealValue1);
			if(dealValue1>=1f)
			{
				dealValue1 = 0;
				poker_H.transform.position = poker_H.transform.parent.position;
				poker_L.SetActive (true);
				IsdealCard_1 = false;
				IsdealCard_2 = true;
			}
		}
		if(IsdealCard_2)
		{
			dealValue2 += Time.deltaTime*2;
			poker_L.transform.position = Vector3.Lerp (draw.transform.position,poker_L.transform.parent.position,dealValue2);
			if(dealValue2>=1f)
			{
				dealValue2 = 0;
				poker_L.transform.position = poker_L.transform.parent.position;
				IsdealCard_2 = false;
				IsDrawCard_1 = true;
			}
		}

		if(IsDrawCard_1)
		{
			Drawvalue += Time.deltaTime * 2;
			poker_H.transform.localScale = Vector3.Lerp (Vector3.one,Vector3.up+Vector3.forward,Drawvalue);
			poker_L.transform.localScale = Vector3.Lerp (Vector3.one,Vector3.up+Vector3.forward,Drawvalue);
			if(Drawvalue>=1)
			{
				poker_H.transform.localScale = Vector3.up + Vector3.forward;
				poker_L.transform.localScale = Vector3.up + Vector3.forward;
				IsDrawCard_1 = false;
				IsDrawCard_2 = true;
			}
		}
		if(IsDrawCard_2)
		{
			Drawvalue -= Time.deltaTime * 2;
			poker_H.transform.localScale = Vector3.Lerp (Vector3.one,Vector3.up+Vector3.forward,Drawvalue);
			poker_L.transform.localScale = Vector3.Lerp (Vector3.one,Vector3.up+Vector3.forward,Drawvalue);
			if(Drawvalue<=0)
			{
				poker_H.transform.localScale = Vector3.one;
				poker_L.transform.localScale =  Vector3.one;
				IsDrawCard_2 = false;
				Analysis_1 ();
				Analysis_2 (H_Value.GetComponent<Image>(),pokersvalue_H);
				Analysis_2 (L_Value.GetComponent<Image>(),pokersvalue_L);
			}
		}


	}


	public void Reset()
	{
		r_pos = Vector2.right * RightBG.GetComponent<RectTransform> ().rect.width;
		l_pos  = Vector2.left * LiftBG.GetComponent<RectTransform> ().rect.width;

		RightBG.GetComponent<RectTransform> ().anchoredPosition = r_pos;
		LiftBG.GetComponent<RectTransform> ().anchoredPosition = l_pos;

		poker_L.SetActive (false);
		poker_H.SetActive (false);

		L_Type.gameObject.SetActive (false);
		H_Type.gameObject.SetActive (false);

		L_Value.gameObject.SetActive (false);
		H_Value.gameObject.SetActive (false);

		IsPanelMove=false;
		IsdealCard_1 =false;
		IsdealCard_2 =false;
		IsDrawCard_1 = false;
		IsDrawCard_2=false;

		VsBG.SetActive (false);
	}

	public void SetValues(string str_L,string str_H)
	{
		pokersvalue_L = str_L;
		pokersvalue_H = str_H;
	}

	public void Analysis_1()
	{
		if(pokersvalue_L!="")
		{
			Debug.Log ("虎牌"+pokersvalue_L);
			switch (pokersvalue_L.Substring(0,1)) {
			case "a":
				poker_L.GetComponent<Image> ().sprite = PokerSprites [big (pokersvalue_L.Substring (1, 1))];
				break;
			case "b":
				poker_L.GetComponent<Image> ().sprite = PokerSprites [13+big (pokersvalue_L.Substring (1, 1))];
				break;
			case "c":
				poker_L.GetComponent<Image> ().sprite = PokerSprites [13*2+big (pokersvalue_L.Substring (1, 1))];
				break;
			case "d":
				poker_L.GetComponent<Image> ().sprite = PokerSprites [13*3+big(pokersvalue_L.Substring (1, 1))];
				break;
			}
		}
		Debug.Log ("龙牌"+pokersvalue_H);
		if(pokersvalue_H!="")
		{
			switch (pokersvalue_H.Substring(0,1)) {
			case "a":
				poker_H.GetComponent<Image> ().sprite = PokerSprites [big(pokersvalue_H.Substring (1, 1))];
				break;
			case "b":
				poker_H.GetComponent<Image> ().sprite = PokerSprites [13+big (pokersvalue_H.Substring (1, 1))];
				break;
			case "c":
				poker_H.GetComponent<Image> ().sprite = PokerSprites [13*2+big(pokersvalue_H.Substring (1, 1))];
				break;
			case "d":
				poker_H.GetComponent<Image> ().sprite = PokerSprites [13*3+big (pokersvalue_H.Substring (1, 1))];
				break;
			}
		}
	}

	public void Analysis_2(Image image,string str)
	{
		switch (str.Substring (1, 1)) {
		case "0":
			image.sprite = LH_value [10];
			break;
		case "1":
			image.sprite = LH_value [1];
			break;
		case "2":
			image.sprite = LH_value [2];
			break;
		case "3":
			image.sprite = LH_value [3];
			break;
		case "4":
			image.sprite = LH_value [4];
			break;
		case "5":
			image.sprite = LH_value [5];
			break;
		case "6":
			image.sprite = LH_value [6];
			break;
		case "7":
			image.sprite = LH_value [7];
			break;
		case "8":
			image.sprite = LH_value [8];
			break;
		case "9":
			image.sprite = LH_value [9];
			break;
		case "j":
			image.sprite = LH_value [11];
			break;
		case "q":
			image.sprite = LH_value [12];
			break;
		case "k":
			image.sprite = LH_value [13];
			break;

		}
		image.gameObject.SetActive (true);
	}

	public int big(string nub)
	{
		if (nub == "0")
		{
			return 10;
		}
		if (nub == "1")
		{
			return 1;
		}
		if (nub == "2")
		{
			return 2;
		}
		if (nub == "3")
		{
			return 3;
		}
		if (nub == "4")
		{
			return 4;
		}
		if (nub == "5")
		{
			return 5;
		}
		if (nub == "6")
		{
			return 6;
		}
		if (nub == "7")
		{
			return 7;
		}
		if (nub == "8")
		{
			return 8;
		}
		if (nub == "9")
		{
			return 9;
		}
		if (nub == "j")
		{
			return 11;
		}
		if (nub == "q")
		{
			return 12;
		}
		if (nub == "k")
		{
			return 13;
		}
		return 0;
	}
}
