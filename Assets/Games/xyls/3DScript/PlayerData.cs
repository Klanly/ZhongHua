using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData ins;
    public string userid = string.Empty;
    public string username = string.Empty;
    public string game_id = string.Empty;
    public string room_id = string.Empty;
    public string unionuid = string.Empty;
    public string quick_credit = string.Empty;
    public string fraction = string.Empty;
    public string score = string.Empty;
    public string drop_date = string.Empty;
    //服务端得到数据
    public string InterfZoomName = string.Empty;
    public string InterZHX = string.Empty;
    //当前的时间
    public string DataTime = string.Empty;
    //彩金
    public string handsel_total = string.Empty;
    //客户端真实数据
    public string ZoomName = string.Empty;

    //中奖类型
    public string win_type = string.Empty;
    //持续时间
    public int iswin = -1;
    public string winning_one = string.Empty;
    public string winning_ZHX = string.Empty;
    //赔率2
    public string winnings_two_odds = string.Empty;
    public string winnings_two = string.Empty;
    //赔率3
    public string winnings_three_odds = string.Empty;
    public string winnings_three = string.Empty;
    //赔率4
    public string winnings_four_odds = string.Empty;
    public string winnings_four = string.Empty;
    //JX奖
    public string JX = string.Empty;
    ////大三元
    //public string winnings_two = string.Empty;

    //public string winnings_three = string.Empty;
    void Awake()
    {
        ins = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
