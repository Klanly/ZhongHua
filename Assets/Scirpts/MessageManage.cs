//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using LitJson;
//using UnityEngine.Events;

//public class MessageManage : MonoBehaviour
//{

//    public static string countDown;  //剩余时间
//    public static string periods;   //回合数
//    public static string season; // 轮数


//    #region 总下注
//    public static List<string> dnum = new List<string>();


//    public static bool isUpdateAllDnum = false;
//    #endregion

//    public static bool isUpdateRate;

//    public static MessageManage instance;

//    public static bool isUpdate = false;
//    public static MessageManage GetInstance()
//    {
//        if (instance == null)
//        {
//            instance = new MessageManage();
//        }
//        return instance;
//    }

//    int resend;
//    public static bool isFirst = false;

//    void test()
//    {

//    }
//    public void Message(string str)
//    {
//        //Debug.LogError(str);
//        JsonData jd = JsonMapper.ToObject(str);
//        //Loom.QueueOnMainThread(
//        //           () =>
//        //           {
//        //               DanTiao.instance.OnShowTest();
//        //           }
//        //           );
//        //DanTiao.instance.MessageManage(newStr[i]);
//        switch (jd["type"].ToString())
//        {
//            case "Periods":





//                if (jd["is_empty"].ToString() == "1")
//                {
//                    DanTiao.instance.isOnClear = true;

//                }
//                if (jd["is_win"].ToString() == "0")
//                {
//                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
//                    countDown = jd["countdown"].ToString();
//                    periods = jd["periods"].ToString();
//                    season = jd["season"].ToString();
//                    if (!isUpdate)
//                    {
//                        isUpdate = true;
//                    }
//                    if (DanTiao.winInfo != "")
//                    {
//                        DanTiao.winInfo = "";
//                    }
//                    resend = 0;
//                    if (!isFirst)
//                    {
//                        isFirst = true;
//                    }
//                }
//                else if (jd["is_win"].ToString() == "2" && resend != 1 && jd["winnings"].ToString() != "")
//                {
//                    if (isFirst)
//                    {
//                        DanTiao.is_WinTwo = true;
//                        DanTiao.winInfo = str;
//                        resend = 1;
//                    }
//                    else
//                    {
//                        LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
//                        countDown = jd["countdown"].ToString();
//                        periods = jd["periods"].ToString();
//                        season = jd["season"].ToString();
//                        if (!isUpdate)
//                        {
//                            isUpdate = true;
//                        }
//                    }

//                }


//                break;

//            case "odd-list":
//                //if (jd["0"]["id"].ToString() == LoginInfo.Instance().mylogindata.user_id)
//                //{
//                //    return;
//                //}
//                //Debug.LogError(str);
//                dnum.Clear();
//                for (int i = 0; i < jd["Oddlist"].Count; i++)
//                {
//                    //Debug.LogError(jd["Oddlist"][i].ToString() + "*****");
//                    dnum.Add(jd["Oddlist"][i].ToString());
//                }

//                //dnum.Add(jd["Oddlist"][0].ToString());
//                //dnum.Add(jd["Oddlist"][1].ToString());
//                //dnum.Add(jd["Oddlist"][2].ToString());
//                //dnum.Add(jd["Oddlist"][3].ToString());
//                //dnum.Add(jd["Oddlist"][4].ToString());
//                ////Debug.LogError(jd["Oddlist"][0].ToString());
//                isUpdateAllDnum = true;
//                break;
//            case "start":
//                for (int i = 0; i < DanTiao.betId.Length; i++)
//                {
//                    DanTiao.betId[i] = jd["oddlist"][i]["id"].ToString();
//                    DanTiao.rateInfo[i] = jd["oddlist"][i]["rate"].ToString();
//                    DanTiao.dnumInfo[i] = jd["oddlist"][i]["dnum"].ToString();
//                }
//                isUpdateRate = true;
//                break;


//        }


//        //JsonData jd = JsonMapper.ToObject(str);

//    }
//}
