//using testJson.Json;
using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HGameTcpNet
{
    private static volatile HGameTcpNet instance;
    private static readonly object obj = new object();

    private HGameTcpNet() { }

    public static HGameTcpNet Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new HGameTcpNet();
            }
            return instance;
        }
    }

    public IEnumerator SendVoid(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();

        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
                try
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    ReceiveData(jd);
                }
                catch
                {
                    HGameData.Instance.IsNetworkError = true;
                    Debug.Log("JSON解析错误");
                }
            }
        }
    }

    void ReceiveData(JsonData data)
    {
        Debug.Log("msg:" + data["msg"].ToString());
        switch (data["msg"].ToString())
        {
            case "登陆成功":
                //用户登录成功 
                //Debug.Log("连接成功,登录成功");
                //HGameData.Instance.RegisterErrorType = 4;
                //Debug.Log("code:" + data["code"].ToString());
                //if (data["code"].ToString() == "200")
                //{
                //    //连接成功 

                //}
                //HGameData.Instance.PlayerID = data["UserInfo"]["user_id"].ToString();
                //Debug.Log("用户ID：" + HGameData.Instance.PlayerID);
                //HGameData.Instance.PlayerName = data["UserInfo"]["username"].ToString();
                //Debug.Log("用户名字：" + HGameData.Instance.PlayerName);
                //HGameData.Instance.Gold = data["UserInfo"]["quick_credit"].ToString();
                //Debug.Log("用户余额：" + HGameData.Instance.Gold);
                //HGameData.Instance.Unionuid = data["UserInfo"]["unionuid"].ToString();
                //Debug.Log("临时会话密令：" + HGameData.Instance.Unionuid);
                //HGameData.Instance.Status = data["UserInfo"]["status"].ToString();
                //Debug.Log("账号状态：" + HGameData.Instance.Status);
                break;
            case "代理房间列表":
                ///获取当前用户的代理所属房间

                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                Debug.Log("代理所属房间ID:" + data["roomList"][0]["id"].ToString());
                for (int i = 0; i < 3; i++)
                {
                    HGameData.Instance.RoomID[i] = data["roomList"][i]["id"].ToString();
                    Debug.Log("房间ID：" + HGameData.Instance.RoomID[i]);
                    HGameData.Instance.RoomName[i] = data["roomList"][i]["title"].ToString();
                    Debug.Log("房间名字：" + HGameData.Instance.RoomName[i]);
                    HGameData.Instance.RoomRatio[i] = data["roomList"][i]["conversion_ratio"].ToString();
                    Debug.Log("房间倍率：" + HGameData.Instance.RoomRatio[i]);
                }
                HGameData.Instance.EnterTheScene("ChooseRoom");
                Debug.Log("进入 房间列表");
                break;
            case "加入房间成功":
                ///进入房间(选择座位房间)    
                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                HGameData.Instance.RoomTitle = data["room_title"].ToString();
                Debug.Log("房间类型：" + HGameData.Instance.RoomTitle);
                HGameData.Instance.TriggerTotal = data["triggerTotal"].ToString();
                Debug.Log("有多少个座位：" + HGameData.Instance.TriggerTotal);
                HGameData.Instance.TriggerNum = data["trigger"].Count;
                Debug.Log("有多少人在玩：" + HGameData.Instance.TriggerNum);
                if (HGameData.Instance.TriggerNum != 0)
                {
                    for (int i = 0; i < data["trigger"].Count; i++)
                    {
                        HGameData.Instance.TriggerGold[i] = data["trigger"][i]["userInfo"]["quick_credit"].ToString();
                        Debug.Log("坐下来玩家余额：" + HGameData.Instance.TriggerGold[i]);
                        HGameData.Instance.TriggerID[i] = data["trigger"][i]["id"].ToString();
                        Debug.Log("坐下来玩家ID：" + HGameData.Instance.TriggerID[i]);
                        HGameData.Instance.TriggerName[i] = data["trigger"][i]["userInfo"]["username"].ToString();
                        Debug.Log("坐下来的玩家名字：" + HGameData.Instance.TriggerName[i]);
                        HGameData.Instance.TriggerSeatNum[i] = data["trigger"][i]["seat_number"].ToString();
                        Debug.Log("坐下来的玩家座位编号：" + HGameData.Instance.TriggerSeatNum[i]);
                        HGameData.Instance.Is_machine[i] = data["trigger"][i]["is_machine"].ToString();
                        Debug.Log("坐下来的玩家是否留机：" + HGameData.Instance.Is_machine[i]);
                        HGameData.Instance.Time[i] = int.Parse(data["trigger"][i]["time"].ToString());
                        Debug.Log("坐下来的玩家留机时间：" + HGameData.Instance.Time[i]);
                    }

                }
                else
                {
                    Debug.Log("当前并没有玩家游玩");
                    //初始化 座位信息
                    HGameData.Instance.TriggerGold = new string[int.Parse(HGameData.Instance.TriggerTotal)];
                    HGameData.Instance.TriggerID = new string[int.Parse(HGameData.Instance.TriggerTotal)];
                    HGameData.Instance.TriggerName = new string[int.Parse(HGameData.Instance.TriggerTotal)];
                    HGameData.Instance.TriggerSeatNum = new string[int.Parse(HGameData.Instance.TriggerTotal)];
                    HGameData.Instance.Time = new int[int.Parse(HGameData.Instance.TriggerTotal)];
                    HGameData.Instance.Is_machine = new string[int.Parse(HGameData.Instance.TriggerTotal)];
                }

                Debug.Log("进入房间成功，跳转选择座位场景");
                HGameData.Instance.EnterTheScene("PTCChooseJiQi");
                break;
            case "获取成功":
                //Debug.Log("获取用户信息");
                /////获取用户信息  轮询接口  可以登录成功后就一直轮询
                //Debug.Log("code:" + data["code"].ToString());
                //if (data["code"].ToString() == "200")
                //{
                //    //连接成功 

                //}
                //HGameData.Instance.PlayerID = data["Userinfo"]["user_id"].ToString();
                //Debug.Log("用户ID：" + HGameData.Instance.PlayerID);
                //HGameData.Instance.Unionuid = data["Userinfo"]["unionuid"].ToString();
                //Debug.Log("临时会话密令：" + HGameData.Instance.Unionuid);
                //HGameData.Instance.PlayerName = data["Userinfo"]["username"].ToString();
                //Debug.Log("用户名字：" + HGameData.Instance.PlayerName);
                //HGameData.Instance.Gold = data["Userinfo"]["quick_credit"].ToString();
                //Debug.Log("用户余额：" + HGameData.Instance.Gold);
                //HGameData.Instance.Status = data["Userinfo"]["status"].ToString();
                //Debug.Log("账号状态：" + HGameData.Instance.Status);
                break;
            case "坐下成功":
                //坐下成功
                ///选择机子  跳转到下注页面的

                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                HGameData.Instance.Trigger_id = data["trigger_id"].ToString();
                Debug.Log("机子ID：" + HGameData.Instance.Trigger_id);
                HGameData.Instance.RoomTitleID = data["room_id"].ToString();
                Debug.Log("自己进入的房间编号：" + HGameData.Instance.RoomTitleID);
                for (int i = 0; i < 12; i++)
                {
                    Debug.Log("赔率表：" + data["oddList"][i]["desc"].ToString() + " : " + data["oddList"][i]["rate"].ToString());
                    switch (data["oddList"][i]["desc"].ToString())
                    {
                        case "对子":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "两对":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "三条":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "顺子":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "同花":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "葫芦":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "小四梅":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "大四梅":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "同花小顺":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "五梅":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "同花大顺":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        case "五鬼":
                            HGameData.Instance.Multiplying[i] = data["oddList"][i]["rate"].ToString();
                            break;
                        default:
                            Debug.Log("倍率错误" + i);
                            break;
                    }
                }
                Debug.Log(" 坐下成功");
                //进入到游戏场景（游玩界面）
                HGameData.Instance.EnterTheScene("HGame");
                break;
            case "座位已有人":
                //弹出提示 座位已有人

                break;

            case "退出成功":
                //下机，退出房间，返回到选择座位场景
                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                HGameData.Instance.EnterTheScene("PTCChooseJiQi");
                break;

            case "第一次开牌":

                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                for (int i = 0; i < 5; i++)
                {
                    HGameData.Instance.CardType[i] = data["result"]["data"][i].ToString();
                    Debug.Log("第" + (i + 1) + "张牌是：" + HGameData.Instance.CardType[i]);
                }
                HGameData.Instance.FlopNum = 2;
                HGameData.Instance.Odds = data["result"]["odds"].ToString();
                Debug.Log("倍率：" + HGameData.Instance.Odds);
                HGameData.Instance.CardTitle = data["result"]["title"].ToString();
                Debug.Log(HGameData.Instance.CardTitle);
                HGameData.Instance.HoldData = AutoLeaveCard(HGameData.Instance.CardType, HGameData.Instance.CardTitle);
                HGameData.Instance.IsWinEffect = true;
                HGameData.Instance.PeriodsID = data["result"]["periods_id"].ToString();
                Debug.Log("这次下注的会话ID：" + HGameData.Instance.PeriodsID);
                LoginInfo.Instance().mylogindata.ALLScroce = data["userInfo"]["quick_credit"].ToString();
                Debug.Log("用户余额：" + LoginInfo.Instance().mylogindata.ALLScroce);
                HGameData.Instance.WinMoney = int.Parse(data["result"]["winMoney"].ToString());
                HGameData.Instance.Fraction = int.Parse(data["userInfo"]["fraction"].ToString());
                HGameData.Instance.IsFlopCard = true;
                HGameData.Instance.IsStart = false;
                break;
            case "第二次开牌":
                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 

                }
                HGameData.Instance.PeriodsID = data["result"]["periods_id"].ToString();
                Debug.Log("这次下注的会话ID：" + HGameData.Instance.PeriodsID);
                for (int i = 0; i < 5; i++)
                {
                    HGameData.Instance.CardType[i] = data["result"]["data"][i].ToString();
                    Debug.Log("第" + (i + 1) + "张牌是：" + HGameData.Instance.CardType[i]);
                }

                HGameData.Instance.FlopNum = 3;
                HGameData.Instance.Odds = data["result"]["odds"].ToString();
                Debug.Log("倍率：" + HGameData.Instance.Odds);
                HGameData.Instance.CardTitle = data["result"]["title"].ToString();
                Debug.Log(HGameData.Instance.Odds);
                HGameData.Instance.IsWinEffect = true;
                HGameData.Instance.PeriodsID = data["result"]["periods_id"].ToString();
                Debug.Log("这次下注的会话ID：" + HGameData.Instance.PeriodsID);
                LoginInfo.Instance().mylogindata.ALLScroce = data["userInfo"]["quick_credit"].ToString();
                Debug.Log("用户余额：" + LoginInfo.Instance().mylogindata.ALLScroce);
                HGameData.Instance.IsFlopCard = true;
                HGameData.Instance.WinMoney = int.Parse(data["result"]["winMoney"].ToString());
                Debug.Log("赢：" + HGameData.Instance.WinMoney);
                if (HGameData.Instance.WinMoney != 0)
                {
                    Debug.Log("奖励");
                    HGameData.Instance.IsWin = true;         //显示得分按钮
                }
                else
                {
                    //没有奖励
                    Debug.Log("没有奖励");
                    //GameData.Instance.IsCardStart = true;
                    //准备下一轮游戏
                }
                break;
            case "比倍成功":
                Debug.Log("code:" + data["code"].ToString());
                if (data["code"].ToString() == "200")
                {
                    //连接成功 
                    HGameData.Instance.IsFlopColumnBoard = true;
                }
                HGameData.Instance.Result_result = data["result"]["result"].ToString();
                HGameData.Instance.Result_Data = data["result"]["data"].ToString();
                Debug.Log("中奖分数：" + data["result"]["winTotal"].ToString());
                HGameData.Instance.WinMoney = int.Parse(data["result"]["winTotal"].ToString());
                if (HGameData.Instance.WinMoney != 0)
                {
                    HGameData.Instance.IsBiBei = true;
                    Debug.Log("是否一直继续比倍");
                }
                HGameData.Instance.PeriodsID = data["result"]["periods_id"].ToString();
                break;

            default:

                break;
        }
    }


    /// <summary>
    /// 判断那张牌保留
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    string[] AutoLeaveCard(string[] card, string type)
    {
        string[] data = new string[5];
        string[] A = card[0].Split('-');
        string[] B = card[1].Split('-');
        string[] C = card[2].Split('-');
        string[] D = card[3].Split('-');
        string[] E = card[4].Split('-');
        for (int i = 0; i < 5; i++)
        {
            if (card[i][0] == 'E')
            {
                data[i] = (i + 1).ToString();
            }
        }
        switch (type)
        {
            case "散牌":
                break;
            case "对子":
                for (int i = 0; i < 5; i++)
                {
                    if (card[i][0] == 'E')
                    {
                        data[i] = (i + 1).ToString();
                        return data;
                    }
                }
                if (int.Parse(A[1]) >= 7)
                {
                    if (A[1] == B[1])
                    {
                        data[0] = "1";
                        data[1] = "2";
                    }
                    if (A[1] == C[1])
                    {
                        data[0] = "1";
                        data[2] = "3";
                    }
                    if (A[1] == D[1])
                    {
                        data[0] = "1";
                        data[3] = "4";
                    }
                    if (A[1] == E[1])
                    {
                        data[0] = "1";
                        data[4] = "5";
                    }
                }

                if (int.Parse(B[1]) >= 7)
                {
                    if (B[1] == C[1])
                    {
                        data[1] = "2";
                        data[2] = "3";
                    }
                    if (B[1] == D[1])
                    {
                        data[1] = "2";
                        data[3] = "4";
                    }
                    if (B[1] == E[1])
                    {
                        data[1] = "2";
                        data[4] = "5";
                    }
                }

                if (int.Parse(C[1]) >= 7)
                {
                    if (C[1] == D[1])
                    {
                        data[2] = "3";
                        data[3] = "4";
                    }
                    if (C[1] == E[1])
                    {
                        data[2] = "3";
                        data[4] = "5";
                    }
                }
                if (int.Parse(D[1]) >= 7)
                {
                    if (D[1] == E[1])
                    {
                        data[3] = "4";
                        data[4] = "5";
                    }
                }
                break;
            case "两对":
                for (int i = 0; i < 5; i++)
                {
                    if (card[i][0] == 'E')
                    {
                        data[i] = (i + 1).ToString();
                    }
                }
                if (A[1] == B[1])
                {
                    data[0] = "1";
                    data[1] = "2";
                }
                if (A[1] == C[1])
                {
                    data[0] = "1";
                    data[2] = "3";
                }
                if (A[1] == D[1])
                {
                    data[0] = "1";
                    data[3] = "4";
                }
                if (A[1] == E[1])
                {
                    data[0] = "1";
                    data[4] = "5";
                }

                if (B[1] == C[1])
                {
                    data[1] = "2";
                    data[2] = "3";
                }
                if (B[1] == D[1])
                {
                    data[1] = "2";
                    data[3] = "4";
                }
                if (B[1] == E[1])
                {
                    data[1] = "2";
                    data[4] = "5";
                }

                if (C[1] == D[1])
                {
                    data[2] = "3";
                    data[3] = "4";
                }
                if (C[1] == E[1])
                {
                    data[2] = "3";
                    data[4] = "5";
                }

                if (D[1] == E[1])
                {
                    data[3] = "4";
                    data[4] = "5";
                }
                break;
            case "三条":
                for (int i = 0; i < 5; i++)
                {
                    if (card[i][0] == 'E')
                    {
                        data[i] = (i + 1).ToString();
                    }
                }
                if (A[1] == B[1])
                {
                    data[0] = "1";
                    data[1] = "2";
                    if (A[1] == C[1])
                    {
                        data[0] = "1";
                        data[2] = "3";
                    }
                    if (A[1] == D[1])
                    {
                        data[0] = "1";
                        data[3] = "4";
                    }
                    if (A[1] == E[1])
                    {
                        data[0] = "1";
                        data[4] = "5";
                    }
                }
                if (A[1] == C[1])
                {
                    data[0] = "1";
                    data[2] = "3";
                    if (A[1] == D[1])
                    {
                        data[0] = "1";
                        data[3] = "4";
                    }
                    if (A[1] == E[1])
                    {
                        data[0] = "1";
                        data[4] = "5";
                    }
                }
                if (A[1] == D[1])
                {
                    data[0] = "1";
                    data[3] = "4";
                    if (A[1] == E[1])
                    {
                        data[0] = "1";
                        data[4] = "5";
                    }
                }
                if (A[1] == E[1])
                {
                    data[0] = "1";
                    data[4] = "5";
                }

                if (B[1] == C[1])
                {
                    data[1] = "2";
                    data[2] = "3";
                    if (C[1] == D[1])
                    {
                        data[2] = "3";
                        data[3] = "4";
                    }
                    if (C[1] == E[1])
                    {
                        data[2] = "3";
                        data[4] = "5";
                    }
                }
                if (B[1] == D[1])
                {
                    data[1] = "2";
                    data[3] = "4";
                }
                if (B[1] == E[1])
                {
                    data[1] = "2";
                    data[4] = "5";
                }

                if (C[1] == D[1])
                {
                    data[2] = "3";
                    data[3] = "4";
                    if (C[1] == E[1])
                    {
                        data[2] = "3";
                        data[4] = "5";
                    }
                }
                if (C[1] == E[1])
                {
                    data[2] = "3";
                    data[4] = "5";
                }

                if (D[1] == E[1])
                {
                    data[3] = "4";
                    data[4] = "5";
                }
                break;
            default:
                for (int i = 0; i < 5; i++)
                {
                    data[i] = (i + 1).ToString();
                }
                break;
        }
        return data;
    }
}
