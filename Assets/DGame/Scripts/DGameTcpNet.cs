using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DGameTcpNet
{
    private static volatile DGameTcpNet instance;

    // private static readonly object obj = new object();

    private DGameTcpNet()
    {

    }

    public static DGameTcpNet Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new DGameTcpNet();
            }
            return instance;
        }
    }

    public IEnumerator SendVoid(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
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
                    DGameData.Instance.IsNetworkError = true;
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
                break;
            case "代理房间列表":
                for (int i = 0; i < 3; i++)
                {
                    DGameData.Instance.RoomList_ID[i] = data["roomList"][i]["id"].ToString();
                    DGameData.Instance.RoomList_Title[i] = data["roomList"][i]["title"].ToString();
                    DGameData.Instance.RoomList_Ratio[i] = data["roomList"][i]["conversion_ratio"].ToString();
                }
                DGameData.Instance.EnterTheScene("DGame_ChooseRoom");
                Debug.Log("进入 房间列表");
                break;
            case "加入房间成功":
                if (data["trigger"].Count != 0)
                {
                    DGameData.Instance.PlayerNum = data["trigger"].Count;
                    for (int i = 0; i < data["trigger"].Count; i++)
                    {
                        DGameData.Instance.Trigger_Username[i] = data["trigger"][i]["userInfo"]["username"].ToString();
                        DGameData.Instance.Trigger_SeatNum[i] = int.Parse(data["trigger"][i]["seat_number"].ToString());
                        DGameData.Instance.Trigger_Is_Machine[i] = int.Parse(data["trigger"][i]["is_machine"].ToString());
                        DGameData.Instance.Trigger_Time[i] = int.Parse(data["trigger"][i]["time"].ToString());
                    }
                }
                else
                {
                    Debug.Log("当前并没有玩家游玩");
                    //初始化 座位信息
                    DGameData.Instance.Trigger_Username = new string[30];
                    DGameData.Instance.Trigger_SeatNum = new int[30];
                    DGameData.Instance.Trigger_Is_Machine = new int[30];
                    DGameData.Instance.Trigger_Time = new int[30];
                }
                Debug.Log("进入房间成功，跳转选择座位场景");
                DGameData.Instance.EnterTheScene("DGame_PTCChooseJiQi");
                break;

            case "坐下成功":

                DGameData.Instance.TriggerID = data["trigger_id"].ToString();
                for (int i = 0; i < 10; i++)
                {
                    switch (data["oddList"][i]["num"].ToString())
                    {
                        case "A":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "B":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "C":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "D":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "E":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "F":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "H":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "I":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "J":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        case "K":
                            DGameData.Instance.OddList[i] = int.Parse(data["oddList"][i]["rate"].ToString());
                            break;
                        default:
                            Debug.Log("倍率错误" + i);
                            break;
                    }
                }
                Debug.Log(" 进入到游戏场景");
                DGameData.Instance.EnterTheScene("DGame");
                break;
            case "座位已有人":
                Debug.Log("座位已有人");
                break;
            case "退出成功":
                DGameData.Instance.EnterTheScene("DGame_PTCChooseJiQi");
                break;
            case "第一次开牌":
                for (int i = 0; i < 5; i++)
                {
                    DGameData.Instance.CardType[i] = data["result"]["data"][i].ToString();
                }
                DGameData.Instance.CardTitle = data["result"]["title"].ToString();
                DGameData.Instance.HoldData = AutoLeaveCard(DGameData.Instance.CardType, DGameData.Instance.CardTitle);
                DGameData.Instance.IsWinEffect = true;
                DGameData.Instance.Periods_ID = data["result"]["periods_id"].ToString();
                LoginInfo.Instance().mylogindata.ALLScroce = data["userInfo"]["quick_credit"].ToString();
                DGameData.Instance.WinMoney = int.Parse(data["result"]["winMoney"].ToString());
                DGameData.Instance.Fraction = int.Parse(data["userInfo"]["fraction"].ToString());
                DGameData.Instance.IsFlopCard = true;
                DGameData.Instance.IsStart = false;
                break;
            case "第二次开牌":

                DGameData.Instance.Periods_ID = data["result"]["periods_id"].ToString();

                for (int i = 0; i < 5; i++)
                {
                    DGameData.Instance.CardType[i] = data["result"]["data"][i].ToString();
                    Debug.Log("第" + (i + 1) + "张牌是：" + DGameData.Instance.CardType[i]);
                }
                DGameData.Instance.CardTitle = data["result"]["title"].ToString();
                DGameData.Instance.IsWinEffect = true;
                DGameData.Instance.Periods_ID = data["result"]["periods_id"].ToString();
                LoginInfo.Instance().mylogindata.ALLScroce = data["userInfo"]["quick_credit"].ToString();
                DGameData.Instance.IsFlopCard = true;
                DGameData.Instance.WinMoney = int.Parse(data["result"]["winMoney"].ToString());
                if (DGameData.Instance.WinMoney != 0)
                {
                    DGameData.Instance.IsWin = true;         //显示得分按钮
                }
                else
                {
                    //没有奖励
                    Debug.Log("没有奖励");
                    //准备下一轮游戏
                }
                break;
            case "比倍成功":
                if (data["code"].ToString() == "200")
                {
                    //连接成功 
                    DGameData.Instance.IsFlopColumnBoard = true;
                }
                DGameData.Instance.Result_result = data["result"]["result"].ToString();
                DGameData.Instance.Result_Data = data["result"]["data"].ToString();
                DGameData.Instance.WinMoney = int.Parse(data["result"]["winTotal"].ToString());
                if (DGameData.Instance.WinMoney != 0)
                {
                    DGameData.Instance.IsBiBei = true;
                }
                DGameData.Instance.Periods_ID = data["result"]["periods_id"].ToString();
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
