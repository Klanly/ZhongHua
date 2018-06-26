using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DGame_PuKe_Record : MonoBehaviour
{
    private GameObject[,] one_Card = new GameObject[15, 5];
    private GameObject[,] two_Card = new GameObject[15, 5];
    private GameObject[,] retain_data = new GameObject[15, 5];

    public Text CREDIT;
    public Text BET;
    public Text WIN;

    string Name = "";


    public Sprite[] CardSprite;


    private int num = 15;//页数
    public Text PageNum;

    public RectTransform posx;
    private float x = 0;
    float time;

    private AudioSource Audio;

    private void Awake()
    {
        posx = this.GetComponent<RectTransform>();
        for (int i = 0; i < 15; i++)
        {
            for (int b = 0; b < 5; b++)
            {
                Name = (i + 1) + "/One/" + (b + 1);
                one_Card[i, b] = GameObject.Find(Name);
                Name = (i + 1) + "/Two/" + (b + 1);
                two_Card[i, b] = GameObject.Find(Name);
                Name = (i + 1) + "/One/" + (b + 1) + "/Hold";
                retain_data[i, b] = GameObject.Find(Name);
            }
        }


    }

    private void Update()
    {
        time += Time.deltaTime;
        x = Mathf.Lerp(x, (num - 1) * -752, time);
        posx.anchoredPosition = new Vector2(x, 0);

        OnUI();

        for (int i = 0; i < 15; i++)
        {
            for (int e = 0; e < 5; e++)
            {
                one_Card[i, e].GetComponent<Image>().sprite = CardSprite[CardNum(DGameData.Instance.one_data[i], e)];
                two_Card[i, e].GetComponent<Image>().sprite = CardSprite[CardNum(DGameData.Instance.two_data[i], e)];
                if (DGameData.Instance.retain_data[i] != "")
                {
                    if (retain(DGameData.Instance.retain_data[i], e) == true)
                        retain_data[i, e].SetActive(true);
                    else
                        retain_data[i, e].SetActive(false);
                }
                else
                    retain_data[i, e].SetActive(false);
            }
        }
    }

    public void PuKe_LetfButton()
    {
        Audio.Play();
        if (num <= 1)
            return;
        time = 0;
        num--;
    }

    public void PuKe_RightButton()
    {
        Audio.Play();
        if (num >= 15)
            return;
        time = 0;
        num++;
    }

    void OnUI()
    {
        PageNum.text = (16 - num).ToString();

        try { CREDIT.text = ((int)(float.Parse(DGameData.Instance.user_balance[num - 1]))).ToString(); }
        catch { CREDIT.text = "0"; }
        try { BET.text = ((int)(float.Parse(DGameData.Instance.bet_total[num - 1]))).ToString(); }
        catch { BET.text = "0"; }
        try { WIN.text = ((int)(float.Parse(DGameData.Instance.win_total[num - 1]))).ToString(); }
        catch { WIN.text = "0"; }

    }


    int CardNum(string card, int num)
    {
        int a = 0;

        try
        {
            if (card == "")
                return a = num + 9;
            string[] data = card.Split(',');

            char[] s = { 'A', 'B', 'C', 'D', 'E' };
            string[] b = data[num].Split(s);
            //正常类型的牌
            switch (data[num][0])
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
                    a = int.Parse(b[1]) + 51;
                    break;
                default:
                    Debug.Log("解析牌类错误，不属于ABCDE类牌");
                    a = 52;
                    break;
            }
            if (a >= 55)
                a = 52;

            return a;
        }
        catch
        {
            return a = num + 9;
        }

    }

    bool retain(string data, int num)
    {
        try
        {
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                if ((num + 1).ToString() == data[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
