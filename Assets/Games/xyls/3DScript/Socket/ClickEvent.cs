using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;


public class dongwu
{
    public int id;
    public string tp;
    public int num;
    public void Tuzi(int ID, string Tp, int Num)
    {
        this.id = ID;
        this.tp = Tp;
        this.num = Num;

    }
}

public class ClickEvent : MonoBehaviour
{
    public bool IsStart = false;
    public float z = 0;
    public bool Game = true;
    private string NumName = "Num_Y_";
    private Vector3 m_vFistPos = new Vector3();
    public static ClickEvent ins;
    //public  List<string> AllZoomName = new List<string>();
    private string m_info = string.Empty;
    private Dictionary<string, int> Dic = new Dictionary<string, int>();
    int k = -1;
    int j = -1;
    float ClickTime;
    public bool isqyfen = false;
    //  private GameObject LoginInfo;
    //游戏ID

    //获取用户信息
    public string UserInfoUrl = "http://paiji-web.weiec4.cn/api/userinfo?user_id=5&unionuid=201805151735232044226";

    //赔率接口
    public string EnterRoomUrl = "http://47.106.66.89:81/sd-api/room-start?game_id=13&user_id=3&unionuid=201806081406562518896";
    //上分
    public string AddScore = "http://47.106.66.89:81/sd-api/top-score?user_id=93&room_id=6&game_id=13";
    //登录
    public string LoginUrl = "http://47.106.66.89:81/sd-api/login?username=aa&password=123456";
    //下分
    public string xiaScore = "http://47.106.66.89:81/sd-api/lower-score?room_id=6&game_id=13&user_id=93";
    //下注
    public string XiaZhu = "http://47.106.66.89:81/sd-api/sd-bets?room_id=6&user_id=93&num=100&sign_num=C&id=33";
    //取消押分
    public string CancleYaFen = "http://47.106.66.89:81/sd-api/sd-cancel-all?room_id=6&user_id=93";
    //续压
    public string Continue = "http://47.106.66.89:81/sd-api/continuous-pressure?room_id=6&user_id=3&game_id=1&drop_date=20180608221";
    //上一次记录
    public string LastRecord = "http://47.106.66.89:81/sd-api/laet-time-record?room_id=7&user_id=6&game_id=1&drop_date=20180515736";
    //房间记录
    public string roomRecord = "http://47.106.66.89:81/sd-api/now-room-date?game_id=1&room_id=6";
    // Use this for initialization
    //下机
    public string xiaji = "http://47.106.66.89:81/sd-api/trigger-ent?game_id=11&user_id=93&room_id=3";
    public string Winning = "http://47.106.66.89:81/sd-api/user-win-and-lose?room_id=6&user_id=209&game_id=13&drop_date=201806221264";
    void Awake()
    {
        ins = this;
    }

    void Start()
    {
        PlayerData.ins.userid = LoginInfo.Instance().mylogindata.user_id;
        PlayerData.ins.username = LoginInfo.Instance().mylogindata.username;
        PlayerData.ins.quick_credit = LoginInfo.Instance().mylogindata.ALLScroce;
        PlayerData.ins.unionuid = LoginInfo.Instance().mylogindata.token;
        PlayerData.ins.room_id = LoginInfo.Instance().mylogindata.room_id.ToString();
        PlayerData.ins.game_id = LoginInfo.Instance().mylogindata.choosegame.ToString();
        ChipScript.ins.Credit.GetComponent<UILabel>().text = PlayerData.ins.quick_credit;
        ChipScript.ins.CunQu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<UILabel>().text = PlayerData.ins.quick_credit;
        ChipScript.ins.CunQu.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<UILabel>().text = "0";
        LoginInfo.Instance().mylogindata.room_id = 6;
        ChipScript.net = NewTcpNet.GetInstance();
    }
    //得到数据
    public IEnumerator RecevedURL(string url, int number)
    {

        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        //  ChipScript.ins.uilabel.text = url;
        www.timeout = 3;
        yield return www.Send();


        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {

                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

                ReceiveData(jd, url, number);
                print(jd.ToJson().ToString());
                //        ChipScript.ins.uilabel.text = jd.ToJson().ToString();
            }
            else
            {

            }
        }
        else
        {
            ChipScript.ins.Tip("服务器繁忙");
        }
    }

    public void ReceiveData(JsonData jd, string str, int number)
    {
        //    Debug.Log(jd[0]["id"].ToString() + ".......");
        if (jd["code"].ToString() == "200")
        {
            if (number == 0)
            {
                InsInterf(jd);
            }
            else if (number == 1)
            {
                EnterRoomInterf(jd);
            }
            else if (number == 2)
            {
                UserInfo(jd);
            }
            else if (number == 3)
            {
                AddScoreInterf(jd);
            }
            else if (number == 4)
            {
                AddScoreInterf(jd);
            }
            else if (number == 5)
            {
                CanCleInterf(jd);
            }
            else if (number == 6)
            {

                ContinueScore(jd);
            }
            else if (number == 7)
            {

                XiaZhuInterf(jd);
            }
            else if (number == 8)
            {

                GameID(jd);
            }
            else if (number == 9)
            {

                UserInfo(jd);
            }
            else if (number == 10)
            {

                RoomRecord(jd);
            }
            else if (number == 11)
            {

                lastRecord(jd);
            }
            else if (number == 12)
            {
                WinningWuPin(jd);
            }
            else if (number == 13)
            {
                Quit(jd);
            }

        }
        else
        {
            ChipScript.ins.Tip(jd["msg"].ToString());
        }

    }
    //登录
    public void UserInfo(JsonData jd)
    {

        //    Debug.Log(jd[0]["id"].ToString() + ".......");
        if (jd["code"].ToString() == "200")
        {

            PlayerData.ins.userid = jd["UserInfo"]["user_id"].ToString();
            PlayerData.ins.username = jd["UserInfo"]["username"].ToString();
            PlayerData.ins.quick_credit = jd["UserInfo"]["quick_credit"].ToString();
            PlayerData.ins.unionuid = jd["UserInfo"]["unionuid"].ToString();
            //   LoginInfo.Instance().mylogindata.user_id = jd["UserInfo"]["user_id"].ToString();

            Debug.Log(jd["UserInfo"]["quick_credit"].ToString());
            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["UserInfo"]["quick_credit"].ToString();

            //  LoginInfo.Instance().mylogindata.room_id = 6;
            //   LoginInfo.Instance().mylogindata.choosegame = 1;
            //   PlayerData.ins.room_id = LoginInfo.Instance().mylogindata.room_id.ToString();
            //    PlayerData.ins.game_id = LoginInfo.Instance().mylogindata.choosegame.ToString();


        }
        else
        {


        }

    }

    //下机
    public void Quit(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            Application.Quit();
        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }

    }

    //进入房间
    public void InsInterf(JsonData jd)
    {



        string strRate = string.Empty;
        string StrDnum = string.Empty;
        string strNum = string.Empty;
        PlayerData.ins.game_id = jd["game_id"].ToString();
        PlayerData.ins.room_id = jd["room"].ToString();
        PlayerData.ins.drop_date = jd["drop_date"].ToString();

        for (int i = 0; i < jd["oddlist"].Count; i++)
        {


            strRate = jd["oddlist"][i]["rate"].ToString();

            StrDnum = jd["oddlist"][i]["dnum"].ToString();


            switch (jd["oddlist"][i]["tp"].ToString())
            {

                case "黄兔子":
                    k = 2;
                    j = 3;

                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿兔子":
                    j = 3;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红兔子":
                    j = 3;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄猴子":
                    j = 2;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿猴子":
                    j = 2;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红猴子":
                    j = 2;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄熊猫":
                    j = 1;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿熊猫":
                    j = 1;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红熊猫":
                    j = 1;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄狮":
                    j = 0;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿狮":
                    j = 0;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红狮":
                    j = 0;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "庄":
                    k = 0;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "闲":
                    k = 2;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "和":
                    k = 1;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                default:
                    break;
            }
        }

        //  string[] Str = new string[] {"A","B" };

        //for (int j = 0; j <ChipScript.ins.list.Count; j++)
        //{
        //    Debug.Log(ChipScript.ins.list[j].GetComponent<YAbei>().num);
        //    Dic.Add(jd["userData"]["bet"][ChipScript.ins.list[j].GetComponent<YAbei>().num].ToString(), int.Parse(jd["userData"]["bet"][ChipScript.ins.list[j].GetComponent<YAbei>().num]["total_dnum"].ToString()));

        //    //if (ChipScript.ins.list[j].GetComponent<YAbei>().num == jd["userData"]["bet"][strNum].ToString())
        //    //{
        //    //    ChipScript.ins.list[j].transform.GetChild(2).GetComponent<UILabel>().text = jd["userData"]["bet"][strNum]["total_dnum"].ToString();

        //    //}

        //}
        //for (int j = 0; j <ChipScript.ins.list.Count; j++)
        //{
        //    foreach (var item in Dic)
        //    {
        //        if (item.Key == ChipScript.ins.list[j].GetComponent<YAbei>().num)
        //        {
        //            ChipScript.ins.list[j].transform.GetChild(2).GetComponent<UILabel>().text = item.Value.ToString(); 

        //        }
        //    }



        //}
        //Dic.Clear();



    }
    public void EnterRoomInterf(JsonData jd)
    {

        string strRate = string.Empty;
        string StrDnum = string.Empty;
        string strNum = string.Empty;
        PlayerData.ins.win_type = jd["win_type"].ToString();
        PlayerData.ins.drop_date = jd["drop_date"].ToString();
        PlayerData.ins.handsel_total = jd["handsel_total"].ToString();
        if (jd["winnings"].ToString() != "0" )
        {

            Debug.Log("1231");
            Debug.Log(jd["winnings"].ToString());
            PlayerData.ins.InterfZoomName = jd["winnings"].ToString();
            Debug.Log(jd["winnings"].ToString());
            GetZoomName(PlayerData.ins.InterfZoomName);

        }
        if (jd["winnings_one"].ToString() != "" )
        {
            PlayerData.ins.InterZHX = jd["winnings_one"].ToString();
            ZHX(PlayerData.ins.InterZHX);


        }
        PlayerData.ins.handsel_total = jd["handsel_total"].ToString();
        bonuUI.ins.SetNum(PlayerData.ins.handsel_total);
        // Debug.Log(PlayerData.ins.handsel_total.Length);
        PlayerData.ins.DataTime = jd["countdown"].ToString();

        //有奖励的
        if (int.Parse(jd["countdown"].ToString()) <= 60 && int.Parse(jd["countdown"].ToString()) >= 40 && IsStart==false)
        {

            
            bonuUI.ins.time = int.Parse(jd["countdown"].ToString()) - 40;
           
        }
        if (int.Parse(jd["countdown"].ToString()) >= 58)
        {

            Game = false;

            ChipScript.ins.SetSprite(false);
            if (ChipScript.ins.TipPanel.gameObject.activeSelf)
            {
                ChipScript.ins.TipPanel.gameObject.SetActive(false);
            }

        }


        //else
        //{
        //    //没奖励  优缺点
        //    if (int.Parse(jd["countdown"].ToString()) < 60 && int.Parse(jd["countdown"].ToString()) >= 40)
        //    {

        //        bonuUI.ins.time = int.Parse(jd["countdown"].ToString()) - 40;


        //    }

        //    if (int.Parse(jd["countdown"].ToString()) >= 58 && int.Parse(jd["countdown"].ToString()) <= 60)
        //    {
        //        Game = false;
        //        ChipScript.ins.SetSprite(false);
        //        if (ChipScript.ins.TipPanel.gameObject.activeSelf)
        //        {
        //            ChipScript.ins.TipPanel.gameObject.SetActive(false);
        //        }

        //    }

        //}

        if (Game == true)
        {
            if (!ChipScript.ins.TipPanel.gameObject.activeSelf)
            {
                ChipScript.ins.Tip("请等待下一局.");
            }

            z += 0.1f;
            if (z <= 0.2f)
            {

                ChipScript.ins.TipPanel.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = "请等待下一局.";


            }
            else if (z > 0.2f && z <= 0.4)
            {

                ChipScript.ins.TipPanel.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = "请等待下一局..";
            }
            else if (z >= 0.6f)
            {
                ChipScript.ins.TipPanel.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = "请等待下一局...";
                z = 0;
            }


        }
        else
        {
            //持续时间
            //   Debug.Log(ZPan.ins.time);
         
            if (ChipScript.ins.CunQu.transform.GetChild(1).gameObject.activeSelf)
            {
                ChipScript.ins.CunQu.transform.GetChild(1).gameObject.SetActive(false);

                //ChipScript.ins.Btn_OmClick(ChipScript.ins.BetPanel);
            }

            if (PlayerData.ins.win_type == "1" || PlayerData.ins.win_type == "2")
            {


                ZPan.ins.time = int.Parse(jd["countdown"].ToString()) - 15;
                //if (ZPan.ins.time == ClickTime)
                //{
                //    ZPan.ins.shiJian(ZPan.ins.time--);
                //}
                //else
                //{
                //    ZPan.ins.shiJian(ZPan.ins.time);
                //}


                //ClickTime = ZPan.ins.time;
                ZPan.ins.shiJian(ZPan.ins.time);
            }

            if (PlayerData.ins.win_type == "3" || PlayerData.ins.win_type == "4")
            {


                ZPan.ins.time = int.Parse(jd["countdown"].ToString()) -12;
                //   ZPan.ins.shiJian(ZPan.ins.time);

                  
                ZPan.ins.shiJian(ZPan.ins.time);
             

            }

            // PlayerData.ins.iswin = int.Parse(jd["is_win"].ToString());
            //倒计时为0；

            //      Debug.Log(bonuUI.ins.time);
            if (bonuUI.ins.time == 0 && int.Parse(jd["countdown"].ToString()) == 40)
            {

                Debug.Log(jd["countdown"].ToString());
                ChipScript.ins.yazhu.transform.GetChild(0).gameObject.SetActive(false);
                ChipScript.ins.BetPanel.transform.localPosition = new Vector3(0, -660, 0);
                ZPan.ins.Received(true);
                ZPan.ins.GunRotate();
                ChipScript.ins.SetSprite(true);
                for (int i = 0; i < ZPan.ins.Zoom.Length; i++)
                {
                    if (ZPan.ins.Zoom[i].GetComponent<Animator>().enabled = true)
                    {
                        ZPan.ins.Zoom[i].GetComponent<Animator>().enabled = false;
                    }

                }
                isqyfen = true;
            }





        }



        //   PlayerData.ins.room_id = jd["room_id"].ToString();


        for (int i = 0; i < jd["oddlist"].Count; i++)
        {


            strRate = jd["oddlist"][i]["rate"].ToString();

            StrDnum = jd["oddlist"][i]["dnum"].ToString();


            switch (jd["oddlist"][i]["tp"].ToString())
            {

                case "黄兔子":
                    k = 2;
                    j = 3;

                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿兔子":
                    j = 3;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红兔子":
                    j = 3;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄猴子":
                    j = 2;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿猴子":
                    j = 2;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红猴子":
                    j = 2;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄熊猫":
                    j = 1;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿熊猫":
                    j = 1;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红熊猫":
                    j = 1;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "黄狮":
                    j = 0;
                    k = 2;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "绿狮":
                    j = 0;
                    k = 1;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "红狮":
                    j = 0;
                    k = 0;
                    ChipScript.ins.DefalutNumber(strRate, k, j, StrDnum);
                    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "庄":
                    k = 0;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.HZX.transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "闲":
                    k = 2;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.HZX.transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                case "和":
                    k = 1;
                    ChipScript.ins.DeFalutHZX(strRate, k, StrDnum);
                    ChipScript.ins.HZX.transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["oddlist"][i]["id"].ToString()), jd["oddlist"][i]["tp"].ToString(), jd["oddlist"][i]["num"].ToString(), jd["oddlist"][i]["rate"].ToString(), jd["oddlist"][i]["dnum"].ToString());
                    break;
                default:
                    break;
            }
        }
        //for (int j = 0; j <ChipScript.ins.list.Count; j++)
        //{
        //    Debug.Log(ChipScript.ins.list[j].GetComponent<YAbei>().num);
        //    Dic.Add(jd["userData"]["bet"][ChipScript.ins.list[j].GetComponent<YAbei>().num].ToString(), int.Parse(jd["userData"]["bet"][ChipScript.ins.list[j].GetComponent<YAbei>().num]["total_dnum"].ToString()));

        //    //if (ChipScript.ins.list[j].GetComponent<YAbei>().num == jd["userData"]["bet"][strNum].ToString())
        //    //{
        //    //    ChipScript.ins.list[j].transform.GetChild(2).GetComponent<UILabel>().text = jd["userData"]["bet"][strNum]["total_dnum"].ToString();

        //    //}

        //}
        //for (int j = 0; j <ChipScript.ins.list.Count; j++)
        //{
        //    foreach (var item in Dic)
        //    {
        //        if (item.Key == ChipScript.ins.list[j].GetComponent<YAbei>().num)
        //        {
        //            ChipScript.ins.list[j].transform.GetChild(2).GetComponent<UILabel>().text = item.Value.ToString(); 

        //        }
        //    }



        //}
        //Dic.Clear();




    }
    //增加分
    public void AddScoreInterf(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.Fraction.GetComponent<UILabel>().text = jd["fraction"].ToString();
            PlayerData.ins.quick_credit = jd["quick_credit"].ToString();
            PlayerData.ins.fraction = jd["fraction"].ToString();

            ChipScript.ins.CunQu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.CunQu.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<UILabel>().text = jd["fraction"].ToString();

        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }


    }
    //下注
    public void XiaZhuInterf(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            PlayerData.ins.drop_date = jd["drop_date"].ToString();
            //jd["drop_date"].ToString();
            //jd["BetTotal"].ToString();

            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.Fraction.GetComponent<UILabel>().text = jd["fraction"].ToString();
            for (int i = 0; i < ChipScript.ins.objs.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (ChipScript.ins.objs[i].transform.GetChild(j).GetComponent<YAbei>().tp == jd["BetList"]["tp"].ToString())
                    {
                        ChipScript.ins.objs[i].transform.GetChild(j).GetChild(2).GetComponent<UILabel>().text = (int.Parse(ChipScript.ins.objs[i].transform.GetChild(j).GetChild(2).GetComponent<UILabel>().text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                    }
                }

            }
            for (int i = 0; i < ChipScript.ins.HZX.transform.childCount; i++)
            {

                if (ChipScript.ins.HZX.transform.GetChild(i).GetComponent<YAbei>().tp == jd["BetList"]["tp"].ToString())
                {
                    ChipScript.ins.HZX.transform.GetChild(i).GetChild(2).GetComponent<UILabel>().text = (int.Parse(ChipScript.ins.HZX.transform.GetChild(i).GetChild(2).GetComponent<UILabel>().text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                }
            }

        }
        else
        {
            ChipScript.ins.Tip(jd["msg"].ToString());
        }


    }
    //取消押分
    public void CanCleInterf(JsonData jd)
    {

        if (jd["code"].ToString() == "405" || jd["code"].ToString() == "404")
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }

        else
        {
            ChipScript.ins.Defalut();
            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.Fraction.GetComponent<UILabel>().text = jd["fraction"].ToString();
        }


    }
    //获取GameID
    public void GameID(JsonData jd)
    {

        if (jd["code"].ToString() == "200")
        {
            //    PlayerData.ins.game_id = jd["GameList"][0]["id"].ToString();
            //   LoginInfo.Instance().mylogindata.choosegame = int.Parse(jd["GameList"][0]["id"].ToString());

        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }



    }

    //续压
    public void ContinueScore(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.Fraction.GetComponent<UILabel>().text = jd["fraction"].ToString();
            //ChipScript.ins.GetWinning();
            // PlayerData.ins.drop_date = jd["NowDate"].ToString();
            for (int i = 0; i < jd["BetList"]["betList"].Count; i++)
            {
                for (int j = 0; j < ChipScript.ins.ChipNumberBtn.Length; j++)
                {
                    if (jd["BetList"]["betList"][i]["drop_content"].ToString() == ChipScript.ins.ChipNumberBtn[j].GetComponent<YAbei>().num)
                    {
                        ChipScript.ins.ChipNumberBtn[j].transform.GetChild(2).GetComponent<UILabel>().text = Mathf.Floor(float.Parse(jd["BetList"]["betList"][i]["drop_money"].ToString())).ToString();
                    }
                }
            }

        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }


    }
    //上一次记录=
    public void lastRecord(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            string strRate = string.Empty;
            string StrDnum = string.Empty;
            string SumStrDnum = string.Empty;
            ChipScript.ins.Credit.GetComponent<UILabel>().text = jd["quick_credit"].ToString();
            ChipScript.ins.Fraction.GetComponent<UILabel>().text = jd["fraction"].ToString();

            //  PlayerData.ins.room_id = jd["room_id"].ToString();
            for (int i = 0; i < jd["batLast"].Count; i++)
            {
                strRate = jd["batLast"][i]["rate"].ToString();
                StrDnum = Mathf.Floor(float.Parse(jd["batLast"][i]["user_dnum"].ToString())).ToString();
                SumStrDnum = jd["batLast"][i]["all_dnum"].ToString();
                //      dic.Add(jd["odds"][i]["rate"].ToString(),int.Parse(jd["odds"][i]["id"].ToString()));
                switch (jd["batLast"][i]["tp"].ToString())
                {

                    case "黄兔子":
                        k = 2;
                        j = 3;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //  ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "绿兔子":
                        j = 3;
                        k = 1;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //  ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "红兔子":
                        j = 3;
                        k = 0;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //  ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "黄猴子":
                        j = 2;
                        k = 2;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "绿猴子":
                        j = 2;
                        k = 1;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //     ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "红猴子":
                        j = 2;
                        k = 0;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "黄熊猫":
                        j = 1;
                        k = 2;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "绿熊猫":
                        j = 1;
                        k = 1;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //   ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "红熊猫":
                        j = 1;
                        k = 0;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //   ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "黄狮":
                        j = 0;
                        k = 2;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "绿狮":
                        j = 0;
                        k = 1;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //     ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "红狮":
                        j = 0;
                        k = 0;
                        ChipScript.ins.LastDefalutNumber(strRate, k, j, StrDnum, SumStrDnum);
                        //    ChipScript.ins.objs[j].transform.GetChild(k).transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "庄":
                        k = 0;
                        ChipScript.ins.LastDeFalutHZX(strRate, k, StrDnum, SumStrDnum);
                        //    ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "闲":
                        k = 2;
                        ChipScript.ins.LastDeFalutHZX(strRate, k, StrDnum, SumStrDnum);
                        //   ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    case "和":
                        k = 1;
                        ChipScript.ins.LastDeFalutHZX(strRate, k, StrDnum, SumStrDnum);
                        //    ChipScript.ins.ChipNumberBtn[k].transform.GetComponent<YAbei>().Yabei(int.Parse(jd["odds"][i]["id"].ToString()), jd["odds"][i]["tp"].ToString(), jd["odds"][i]["num"].ToString(), jd["odds"][i]["rate"].ToString(), jd["odds"][i]["dnum"].ToString());
                        break;
                    default:
                        break;
                }



            }
        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }

    }
    public void RoomRecord(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            string str = "0_0";
            string ZHX = string.Empty;
            Debug.Log("319");
            for (int i = 0; i < jd["data"].Count; i++)
            {

                Debug.Log(jd["data"][i]["winnings_one"]);
                if (jd["data"][i]["winnings_one"].ToString() == "闲")
                {
                    ZHX = "1";
                }
                else if (jd["data"][i]["winnings_one"].ToString() == "和")
                {
                    ZHX = "2";
                }
                else
                {
                    ZHX = "0";
                }
                switch (jd["data"][i]["winnings"].ToString())
                {
                    case "黄兔子":
                        str = "3_2";
                        break;
                    case "绿兔子":
                        str = "3_1";
                        break;
                    case "红兔子":
                        str = "3_0";
                        break;
                    case "黄猴子":
                        str = "2_2";
                        break;
                    case "绿猴子":
                        str = "2_1";
                        break;
                    case "红猴子":
                        str = "2_0";
                        break;
                    case "红熊猫":
                        str = "1_0";
                        break;
                    case "黄熊猫":
                        str = "1_2";
                        break;
                    case "绿熊猫":
                        str = "1_1";
                        break;
                    case "黄狮":
                        str = "0_2";
                        break;
                    case "红狮":
                        str = "0_0";
                        break;
                    case "绿狮":
                        str = "0_1";
                        break;
                    default:
                        break;

                }
                Debug.Log(UIManager.ins.ZoomName + str);
                Debug.Log(ChipScript.ins.roomRecord.transform.GetChild(i).GetChild(0).name);
                ChipScript.ins.roomRecord.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = UIManager.ins.ZoomName + str;
                ChipScript.ins.roomRecord.transform.GetChild(i).GetChild(2).GetComponent<UISprite>().spriteName = UIManager.ins.ZXHName + ZHX;


            }
        }
        else
        {

            ChipScript.ins.Tip(jd["msg"].ToString());
        }

    }

    public TweenPosition ResetToBeginning { get; set; }

    public void GetZoomName(string str)
    {

        string string1 = string.Empty;
        string string2 = string.Empty;
        if (str.Substring(0, 1) == "红")
        {
            string2 = "_red";
        }
        else if (str.Substring(0, 1) == "黄")
        {
            string2 = "_yellow";

        }
        else if (str.Substring(0, 1) == "绿")
        {
            string2 = "_green";
        }
        else
        {

        }
        if (str.Substring(1, 1) == "兔")
        {
            string1 = "R";

        }
        else if (str.Substring(1, 1) == "狮")
        {
            string1 = "L";
        }
        else if (str.Substring(1, 1) == "熊")
        {
            string1 = "P";
        }
        else if (str.Substring(1, 1) == "猴")
        {
            string1 = "M";
        }
        else
        {

        }
        PlayerData.ins.ZoomName = string1 + string2;
        Debug.Log(PlayerData.ins.ZoomName);

    }

    public string GetAllZoomName(string str)
    {

        string string1 = string.Empty;
        string string2 = string.Empty;

        if (str.Substring(0, 1) == "红")
        {
            string2 = "_red";
        }
        else if (str.Substring(0, 1) == "黄")
        {
            string2 = "_yellow";

        }
        else
        {
            string2 = "_green";
        }

        if (str.Substring(1, 1) == "兔")
        {
            string1 = "R";

        }
        else if (str.Substring(1, 1) == "狮")
        {
            string1 = "L";
        }
        else if (str.Substring(1, 1) == "熊")
        {
            string1 = "P";
        }
        else
        {
            string1 = "M";
        }
        return string1 + string2;



    }
    public void ZHX(string str)
    {

        if (str == "和")
        {
            PlayerData.ins.InterZHX = "H";
        }
        else if (str == "闲")
        {
            PlayerData.ins.InterZHX = "X";
        }
        else if (str == "庄")
        {
            PlayerData.ins.InterZHX = "Z";
        }
        else
        {

        }
    }

    public void WinningWuPin(JsonData jd)
    {
        if (jd["code"].ToString() == "200")
        {
            ChipScript.ins.Score.GetComponent<UILabel>().text = jd["info"]["win_fraction"].ToString();
            //  ChipScript.ins.dongwuScore.GetComponent<UILabel>().text = "X" + jd["info"]["winnings_odds"].ToString();
            //   ChipScript.ins.ZHXScore.GetComponent<UILabel>().text = "X" + jd["info"]["winnings_one_odds"].ToString();
            //   SetNum(jd["info"]["winnings_odds"].ToString(), UIManager.ins.obj1.gameObject,0);
            //     SetNum(jd["info"]["winnings_one_odds"].ToString(), UIManager.ins.obj1.gameObject,1);

            PlayerData.ins.winning_one = jd["info"]["winnings_odds"].ToString();
            PlayerData.ins.winning_ZHX = jd["info"]["winnings_one_odds"].ToString();

            PlayerData.ins.winnings_two_odds = jd["info"]["winnings_two_odds"].ToString();
            PlayerData.ins.winnings_three_odds = jd["info"]["winnings_three_odds"].ToString();
            PlayerData.ins.winnings_four_odds = jd["info"]["winnings_four_odds"].ToString();
            PlayerData.ins.winnings_two = jd["info"]["winnings_two"].ToString();
            PlayerData.ins.winnings_three = jd["info"]["winnings_three"].ToString();
            PlayerData.ins.winnings_four = jd["info"]["winnings_four"].ToString();
            PlayerData.ins.JX = jd["info"]["handsel_odds"].ToString();
            ChipScript.ins.Caijing.text = PlayerData.ins.JX;

            //if (jd["info"]["winnings_two"].ToString() != "0")
            //{
            //    GetAllZoomName(jd["info"]["winnings_two"].ToString());
            //}
            //if (jd["info"]["winnings_three"].ToString() != "0")
            //{
            //    GetAllZoomName(jd["info"]["winnings_three"].ToString());

            //}
            //if (jd["info"]["winnings_four"].ToString() != "0")
            //{
            //    GetAllZoomName(jd["info"]["winnings_four"].ToString());
            //    SetNum(jd["info"]["winnings_four_odds"].ToString(), UIManager.ins.obj3.gameObject, 1);
            //}

            /*  if ( PlayerData.ins.win_type == "4"){
                   GetAllZoomName(jd["info"]["winnings_two"].ToString());
                   GetAllZoomName(jd["info"]["winnings_three"].ToString());
                   GetAllZoomName(jd["info"]["winnings_four"].ToString());
                   SetNum(jd["info"]["winnings_two_odds"].ToString(), UIManager.ins.obj3.gameObject, 0);
                   SetNum(jd["info"]["winnings_three_odds"].ToString(), UIManager.ins.obj3.gameObject, 1);
                   SetNum(jd["info"]["winnings_four_odds"].ToString(), UIManager.ins.obj3.gameObject, 2);
              }
              else if( PlayerData.ins.win_type == "3"){
                   GetAllZoomName(jd["info"]["winnings_two"].ToString());
                   GetAllZoomName(jd["info"]["winnings_three"].ToString());
                   SetNum(jd["info"]["winnings_three_odds"].ToString(), UIManager.ins.obj2.gameObject, 0);
                   SetNum(jd["info"]["winnings_four_odds"].ToString(), UIManager.ins.obj2.gameObject, 1);
              }
              else{
            
              }*/

        }
        else
        {
            ChipScript.ins.Tip(jd["msg"].ToString());
        }
    }
    public void SetNum(string str, GameObject go, int i)
    {

        go.transform.GetChild(i).transform.gameObject.SetActive(true);
        /*     go.transform.GetChild(i).transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
             go.transform.GetChild(i).transform.GetChild(2).GetChild(1).transform.GetComponent<UISprite>().spriteName = NumName + str;*/
       
        if (str.Length == 1)
        {
            go.transform.GetChild(i).transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            go.transform.GetChild(i).transform.GetChild(2).GetChild(1).transform.GetComponent<UISprite>().spriteName = NumName + str;
            go.transform.GetChild(i).transform.GetChild(2).transform.localPosition = new Vector3(30, 0, 0);
            go.transform.GetChild(i).transform.GetChild(2).GetChild(2).gameObject.SetActive(false);


        }
        else
        {
            go.transform.GetChild(i).transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            go.transform.GetChild(i).transform.GetChild(2).GetChild(1).transform.GetComponent<UISprite>().spriteName = NumName + str.Substring(0, 1);
            go.transform.GetChild(i).transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            go.transform.GetChild(i).transform.GetChild(2).GetChild(2).transform.GetComponent<UISprite>().spriteName = NumName + str.Substring(1, 1);
        }

    }
    public void ReSetPosition()
    {
        for (int i = 0; i < UIManager.ins.obj1.transform.childCount; i++)
        {
            UIManager.ins.obj1.transform.GetChild(i).GetChild(2).transform.localPosition = new Vector3(0, 0, 0);
            //	UIManager.ins.obj1.transform.GetChild (i).gameObject.SetActive (false);

        }

        for (int i = 0; i < UIManager.ins.obj2.transform.childCount; i++)
        {
            UIManager.ins.obj2.transform.GetChild(i).GetChild(2).transform.localPosition = new Vector3(0, 0, 0);
            UIManager.ins.obj2.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < UIManager.ins.obj3.transform.childCount; i++)
        {
            UIManager.ins.obj3.transform.GetChild(i).GetChild(2).transform.localPosition = new Vector3(0, 0, 0);
            UIManager.ins.obj3.transform.GetChild(i).gameObject.SetActive(false);
        }

    }
}
