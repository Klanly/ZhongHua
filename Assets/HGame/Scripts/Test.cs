using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    // Use this for initialization
    public Sprite[] yyc;


    //计时器
    private float times;

    private int num = -1;


    void Update()
    {
        //游戏还没有开始押分
        times += Time.deltaTime;
        if (times >= 0.07f)
        {
            num++;
            times = 0;
            if (num >= 30)
            {
                num = 0;
            }
            if (num < yyc.Length)
            {
                this.GetComponent<Image>().sprite = yyc[num];
            }
            else
            {
                this.GetComponent<Image>().sprite = yyc[8];
            }

        }
    }
}