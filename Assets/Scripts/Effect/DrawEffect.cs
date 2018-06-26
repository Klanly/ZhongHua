using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 功能：翻牌效果2.0调整版
/// </summary>
public class DrawEffect : MonoBehaviour {

	public GameObject BackCard;

	public GameObject Card_Down;
	public GameObject Card_Up;
	public GameObject Card_Up2;

	private Vector2 Origin;

	public bool IsMove;
	public bool IsRotare;
	public bool IsReturn;
	public bool IsOpen;
	public bool IsOPen2;

	private float MoveValue;
	private float RotareValue;
	private float Value;

	public Sprite[] backcardSprites1;			//半明牌
	public Sprite[] backcardSprites2;			//全明牌
	public Sprite[] EffectObject;				

	

	void Update () {
//		if(Input.GetKeyDown(KeyCode.A))
//		{
//			Reset ();
//		}
//		if(Input.GetKeyDown(KeyCode.S))
//		{
//			SetUpPos ();
//			IsMove = true;
//		}
		if(IsMove)
		{

			Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (Origin,Card_Down.GetComponent<RectTransform> ().anchoredPosition,MoveValue*2);

			Card_Up.GetComponent<Image> ().fillAmount = MoveValue;
			Card_Down.GetComponent<Image> ().fillAmount = 1 - MoveValue;
			MoveValue += Time.deltaTime/5;
			if(MoveValue>=0.2f)
			{
				IsMove = false;
				Card_Up.GetComponent<Image> ().fillAmount = 0.2f;
				Card_Down.GetComponent<Image> ().fillAmount = 0.8f;
				MoveValue = 0.2f;
			}
		}
		if(IsRotare)
		{
			BackCard.transform.eulerAngles = Vector3.Lerp (BackCard.transform.eulerAngles,Vector3.forward*45,RotareValue);
			RotareValue += Time.deltaTime/5;
			if(RotareValue>=1f)
			{
				IsRotare = false;
				BackCard.transform.eulerAngles = Vector3.forward * 45;
				RotareValue = 0;
			}
		}
		if(IsReturn)
		{
			Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (Origin,Card_Down.GetComponent<RectTransform> ().anchoredPosition,MoveValue*2);

			Card_Up.GetComponent<Image> ().fillAmount = MoveValue;
			Card_Down.GetComponent<Image> ().fillAmount = 1 - MoveValue;
			MoveValue-= Time.deltaTime/5;
			if(MoveValue<=0f)
			{
				IsReturn = false;
				IsOpen = true;
				Card_Up.GetComponent<Image> ().fillAmount = 0;
				Card_Down.GetComponent<Image> ().fillAmount = 1;
				MoveValue = 0;
			}
		}
		if(IsOpen)
		{
			Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.Lerp (Origin,Card_Down.GetComponent<RectTransform> ().anchoredPosition,Value*2);

			Card_Up.GetComponent<Image> ().fillAmount = Value;
			Card_Down.GetComponent<Image> ().fillAmount = 1 - Value;
			Value += Time.deltaTime;
			if(Value>=0.5f)
			{
				IsOpen = false;
				IsOPen2 = true;
				Card_Up.GetComponent<Image> ().fillAmount = 0.5f;
				Card_Down.GetComponent<Image> ().fillAmount = 0.5f;
			}
		}
		if(IsOPen2)
		{
			Card_Up2.transform.localScale = Vector3.right * Value + Vector3.up + Vector3.forward;
			Value += Time.deltaTime*2;
			if(Value>=1f)
			{
				Value = 1;
				Card_Up.GetComponent<Image> ().fillAmount = 1;
				Card_Up2.transform.localScale = Vector3.one;
				IsOPen2 = false;
			}
		}
	}


	//充值牌
	public void Reset()
	{
		BackCard.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		Card_Down.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		Card_Up2.transform.localScale = Vector3.up + Vector3.forward;

		BackCard.transform.eulerAngles = Vector3.zero;

		Card_Up.GetComponent<Image> ().fillAmount = 0;
		Card_Down.GetComponent<Image> ().fillAmount = 1;

		BackCard.transform.GetChild (0).gameObject.SetActive (false);
		Card_Up.transform.GetChild (0).gameObject.SetActive (false);

		Value = 0;
		MoveValue = 0;
		RotareValue = 0;
		IsMove = false;
		IsRotare = false;
		IsReturn = false;
		IsOpen = false;
		IsOPen2 = false;
	}

	//初始进入【设置前面带翻的牌的应有位置】
	public void SetUpPos()
	{
		Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.left*Card_Down.GetComponent<RectTransform> ().rect.width;
		Origin = Card_Up.GetComponent<RectTransform> ().anchoredPosition;
	}

	//开奖后加入【设置背后的牌应该位于的位置】(只有一张牌)
	public void GetWin()
	{
		Card_Up.GetComponent<Image> ().fillAmount = 0f;
		Card_Down.GetComponent<Image> ().fillAmount = 1f;
		Card_Up2.transform.localScale = Vector3.up + Vector3.forward;
	}

	public void GetWin0()
	{
		Card_Up.GetComponent<Image> ().fillAmount = 1f;
		Card_Up2.transform.localScale = Vector3.one;
	}


	//开奖后加入【设置背后的牌应该位于的位置】(两张牌)
	public void GetWin1()
	{
		BackCard.transform.eulerAngles = Vector3.forward*45;
		Card_Up.GetComponent<Image> ().fillAmount = 0.5f;
		Card_Down.GetComponent<Image> ().fillAmount = 0.5f;
		Card_Up2.transform.localScale = Vector3.right * 0.5f + Vector3.up + Vector3.forward;
	}

	//(两张牌)
	public void GetWin2()
	{
		BackCard.transform.eulerAngles = Vector3.forward*45;
		Card_Up.GetComponent<Image> ().fillAmount = 1f;
		Card_Up.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		Card_Up2.transform.localScale = Vector3.one;
	}

	//设置下面那张牌的值
	public void SetBackCard1(int Color, int CardValue)//半明牌
	{
		if(CardValue != -1)	
		{
			if(Color==1)
			{//黑色
				if (CardValue >= 0 && CardValue <= 12) {
					BackCard.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
				} else {
					BackCard.GetComponent<Image> ().sprite = backcardSprites1 [CardValue-26];
				}
			}else if(Color==2)
			{//红色
				if (CardValue >= 13 && CardValue <= 25) {
					BackCard.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
				} else {
					BackCard.GetComponent<Image> ().sprite = backcardSprites1 [CardValue-26];
				}

			}
		}


		//BackCard.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
	}

	//设置下面那张牌的值
	public void SetBackCard2(int Color,int CardValue)//全明牌
	{
		
		if (CardValue != -1) {
			BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			if (Color == 3) {
//				//黑桃
//				BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 4) {
//				//红心
//				BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 5) {
//				//梅花
//				BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 6) {
//				//方片
//				BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			}
		}
	}

	//设置上面那张牌的值
	public void SetUpCard(int Color,int CardValue)
	{
		if(CardValue!=-1)
		{
			if (Color == 1) {//黑色
				if (CardValue >= 0 && CardValue <= 12) {
					Card_Up.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
					Card_Up2.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
				} else {
					Card_Up.GetComponent<Image> ().sprite = backcardSprites1 [CardValue - 26];
					Card_Up2.GetComponent<Image> ().sprite = backcardSprites1 [CardValue - 26];
				}

			} else if (Color == 2) {//红色
				if (CardValue >= 13 && CardValue <= 25) {
					Card_Up.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
					Card_Up2.GetComponent<Image> ().sprite = backcardSprites1 [CardValue];
				} else {
					Card_Up.GetComponent<Image> ().sprite = backcardSprites1 [CardValue - 26];
					Card_Up2.GetComponent<Image> ().sprite = backcardSprites1 [CardValue - 26];
				}
			} else {
				Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
				Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
			}
//			else if (Color == 3) {
//				//黑桃
//				Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//				Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 4) {
//				//红心
//				Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//				Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 5) {
//				//梅花
//				Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//				Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			} else if (Color == 6) {
//				//方片
//				Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//				Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
//			}
		}

		//Card_Up.GetComponent<Image> ().sprite = EffectObject [CardValue];
	}

	public void SetBackCard(int CardValue)
	{
		if(CardValue!=-1)
		{
			BackCard.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
		}

	}

	public void SetUpCard(int CardValue)
	{
		if(CardValue!=-1)
		{
			Card_Up.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
			Card_Up2.GetComponent<Image> ().sprite = backcardSprites2 [CardValue];
		}

	}
}
