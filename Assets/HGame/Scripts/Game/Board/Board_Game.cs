using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board_Game : MonoBehaviour
{
    /// <summary>
    /// 纸牌组
    /// </summary>
    public Sprite[] boardGroup;

    /// <summary>
    /// 反面纸牌
    /// </summary>
    public Sprite[] reverseBoard;

    /// <summary>
    /// 特殊纸牌
    /// </summary>
    public Sprite[] specialBoard;

    /// <summary>
    /// 纸牌
    /// </summary>
    public Image[] boards;

    /// <summary>
    /// 翻牌序号 0.第一张 1.第二张 2.第三张 3.第四张 4.第五张
    /// </summary>
    private int FlopNum = 0;
    /// <summary>
    /// 翻牌帧数动画帧数
    /// </summary>
    private int FlopFrame = 0;
    /// <summary>
    /// 翻牌速度计时
    /// </summary>
    private float FlopTimes;
    /// <summary>
    /// 翻牌速度
    /// </summary>
    public float FlopSpeed;

    public GameObject[] HOLD;

    public AudioSource Audio;

    private void Start()
    {
        Audio.volume = PlayerPrefs.GetFloat("sliderTwo");
    }

    private void Update()
    {
        if (HGameData.Instance.IsFlopCard)
        {
            OnFlop(ref FlopNum);
        }

        if (HGameData.Instance.IsCardStart)
        {
            Debug.Log("牌初始化背面");
            HGameData.Instance.IsCardStart = false;
            for (int i = 0; i < 5; i++)
            {
                boards[i].sprite = reverseBoard[0];
                HOLD[i].SetActive(false);
            }
            HGameData.Instance.FlopNum = 1;
            if (HGameData.Instance.IsAutomatic == false)
                HGameData.Instance.BiType = 0;
            Debug.Log("执行1");
            HGameData.Instance.IsStart = false;
        }

        if (HGameData.Instance.IsFlopCard == false)
            for (int i = 0; i < 5; i++)
            {
                if (HGameData.Instance.HoldData[i] == (i + 1).ToString())
                {
                    HOLD[i].SetActive(true);
                }
                else
                {
                    HOLD[i].SetActive(false);
                }
            }
    }


    /// <summary>
    /// 播放翻牌动画
    /// </summary>
    /// <param name="num"></param>
    void OnFlop(ref int num)
    {
        if (HGameData.Instance.HoldData[num] == (num + 1).ToString() && HGameData.Instance.FlopNum == 3)
        {
            FlopFrame = 0;
            int a = Card_Num(num);
            boards[num].GetComponent<Image>().sprite = boardGroup[a];
            num++;
            Debug.Log("跳过一次翻牌");
            if (num > 4)
            {
                num = 0;
                HGameData.Instance.IsFlopCard = false;
                HGameData.Instance.IsStart = false;
                if (HGameData.Instance.FlopNum == 3 && HGameData.Instance.WinMoney == 0)
                {
                    Invoke("OnCardStart", 0.5f);
                }
            }
        }
        else
        {
            if (FlopFrame < reverseBoard.Length)
            {
                FlopTimes += Time.deltaTime;
                if (FlopTimes >= FlopSpeed)
                {
                    FlopTimes = 0;
                    boards[num].GetComponent<Image>().sprite = reverseBoard[FlopFrame];
                    FlopFrame++;
                }
            }
            else
            {
                FlopFrame = 0;
                Audio.Play();
                int a = Card_Num(num);
                boards[num].GetComponent<Image>().sprite = boardGroup[a];

                //成功翻牌，开始下一张牌 翻牌
                num++;
                if (num > 4)
                {
                    num = 0;
                    HGameData.Instance.IsFlopCard = false;
                    if (HGameData.Instance.FlopNum == 3 && HGameData.Instance.WinMoney == 0)
                    {
                        Invoke("OnCardStart", 0.5f);
                    }
                }

            }
        }


    }

    void OnCardStart()
    {
        HGameData.Instance.IsCardStart = true;
        HGameData.Instance.FlopNum = 1;
    }


    int Card_Num(int num)
    {
        int a = 0;
        string[] b = HGameData.Instance.CardType[num].Split('-');
        //正常类型的牌
        switch (HGameData.Instance.CardType[num][0])
        {
            case 'A':
                //黑桃              
                a = int.Parse(b[1]) - 1;
                break;
            case 'B':
                //红桃
                a = int.Parse(b[1]) + 12;
                break;
            case 'C':
                a = int.Parse(b[1]) + 25;
                //梅花
                break;
            case 'D':
                //方块
                a = int.Parse(b[1]) + 38;
                break;
            case 'E':
                //鬼牌
                a = 52;
                break;
            default:
                Debug.Log("解析牌类错误，不属于ABCDE类牌");
                a = 52;
                break;
        }
        return a;
    }


}
