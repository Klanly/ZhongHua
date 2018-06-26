using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Board_Contrast : MonoBehaviour
{
    public Sprite[] boardGroup;  //纸牌组

    public Sprite[] reverseBoard;//反面纸牌

    public Image guessingBoard;  //竞猜纸牌

    public Image[] columnBoard;    //例外显示纸牌


    private float FlopTime;//翻牌时间
    private int FlopNum;   //翻牌序列帧

    public GameObject[] SmallOrBig;

    private bool IsTwinkle = true;
    private float twinkleTime = 1;

    public AudioSource Audio;

    public AudioClip[] clip;

    private void Start()
    {
        Audio.volume = PlayerPrefs.GetFloat("sliderTwo");
    }

    private void Update()
    {
        if (HGameData.Instance.IsColumnBoard)
        {
            IsTwinkle = true;
            HGameData.Instance.IsColumnBoard = false;
            int a = 0;
            for (int i = 0; i < columnBoard.Length; i++)
            {
                a = ColumnBoard_Num(i);
                columnBoard[i].sprite = boardGroup[a];
            }
        }

        if (HGameData.Instance.IsFlopColumnBoard)
        {
            FlopTime += Time.deltaTime;
            if (FlopTime > 0.06)
            {
                FlopTime = 0;
                guessingBoard.sprite = reverseBoard[FlopNum];
                FlopNum++;
            }
            if (FlopNum >= reverseBoard.Length)
            {
                Debug.Log("得出比倍结果");
                Audio.clip = clip[2];
                Audio.Play();
                IsTwinkle = false;
                switch (HGameData.Instance.Result_result)
                {
                    case "1":
                        SmallOrBig[0].SetActive(true);
                        SmallOrBig[1].SetActive(false);
                        break;
                    case "2":
                        SmallOrBig[1].SetActive(true);
                        SmallOrBig[0].SetActive(false);
                        break;
                }
                FlopNum = 0;
                HGameData.Instance.IsFlopColumnBoard = false;

                guessingBoard.sprite = boardGroup[GuessingBoard_Num()];
                Audio.clip = clip[0];
                if (HGameData.Instance.WinMoney == 0)
                {
                    Audio.clip = clip[1];
                    Invoke("OnStart", 0.5f);
                }
                Audio.Play();
            }
        }

        if (IsTwinkle)
        {
            twinkleTime += Time.deltaTime;
            if (twinkleTime >= 1)
            {
                twinkleTime = 0;
                if (SmallOrBig[0].activeSelf)
                {
                    SmallOrBig[0].SetActive(false);
                    SmallOrBig[1].SetActive(true);
                }
                else
                {
                    SmallOrBig[0].SetActive(true);
                    SmallOrBig[1].SetActive(false);
                }
            }
        }
    }


    void OnStart()
    {
        guessingBoard.sprite = reverseBoard[0];
        HGameData.Instance.BiType = 0;
        Debug.Log("执行2");
        HGameData.Instance.IsWin = false;
    }

    int ColumnBoard_Num(int num)
    {
        int a = 0;
        string[] b = HGameData.Instance.str_result[num].Split('-');
        switch (HGameData.Instance.str_result[num][0])
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

    int GuessingBoard_Num()
    {
        int a = 0;
        string[] b = HGameData.Instance.Result_Data.Split('-');
        switch (HGameData.Instance.Result_Data[0])
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
