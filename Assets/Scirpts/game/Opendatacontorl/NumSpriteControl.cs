using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumSpriteControl : MonoBehaviour
{

    // Use this for initialization
    public List<Sprite> num_Sprite;
    public static NumSpriteControl Instances;
    int childNum = 0;
    public Transform imageParent;
    void Awake()
    {
        Instances = this;
    }

    /// <summary>
    /// 暂停一个
    /// </summary>
    /// <param name="num"></param>
    public void StopImage(int num)
    {
        if (childNum < 10 && imageParent.GetChild(childNum).GetComponent<NumSprite>().isChoose)
        {
            imageParent.GetChild(childNum).GetComponent<NumSprite>().isChoose = false;
            imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num-1];
            childNum++;
        }

    }

    /// <summary>
    /// 暂停多个
    /// </summary>
    /// <param name="num"></param>
    public void StopImage(int[] num)
    {
        if (childNum == 0 && imageParent.GetChild(childNum).GetComponent<NumSprite>().isChoose)
        {

            //foreach (var item in num)
            //{
            for (int i = 0; i < num.Length; i++)
            {
                imageParent.GetChild(childNum).GetComponent<NumSprite>().isChoose = false;
                imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num[i]];
                childNum++;
            }
                
            //}
        }


    }

    public void SetImage(int[] num)
    {
        childNum = 0;
        //foreach (var item in num)
        //{
        for (int i = 0; i < num.Length; i++)
        {
            imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num[i] - 1];
            childNum++;
        }
            //imageParent.GetChild(childNum).GetComponent<NumSprite>().isChoose = false;
           
        //}
    }



    public void StartImage()
    {
        NumSprite[] numSprite = imageParent.GetComponentsInChildren<NumSprite>();
        //foreach (var item in imageParent.GetComponentsInChildren<NumSprite>())
        //{
        for (int i = 0; i < numSprite.Length; i++)
        {
            if (!numSprite[i].isChoose)
            {
                numSprite[i].isChoose = true;
                numSprite[i].StartCoroutine("ChooseSprite", 0.1f);
            }
        }
           
               
            
        //}
        childNum = 0;
    }
}
