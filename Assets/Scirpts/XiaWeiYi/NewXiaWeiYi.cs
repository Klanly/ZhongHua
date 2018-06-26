using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// 
/// </summary>
public class NewXiaWeiYi : MonoBehaviour
{
    public static NewXiaWeiYi instance;

    //长连接
    public NewTcpNet tcp;

    //表格
    public GameObject Table1;
    public GameObject Table2;

    public GameObject Conch;            //贝壳

    public GameObject baoji;            //爆机

    public GameObject Handsel;          //彩金


    public Text Inning;                 //局数
    public Text Rounds;                 //轮数


    public Text Red_Count;              //出现次数
    public Text Red_AllScore;           //总压
    public Text Red_UserScore;          //下注
    public Text Red_Rate;               //倍率
    public GameObject Red_Win;          

    public Text Green_Count;
    public Text Green_AllScore;
    public Text Green_UserScore;
    public Text Green_Rate;
    public GameObject Green_Win;

    public Text Clock;             //计时器
    public Text XianHong;         //限红

    public GameObject PlayerPanel;      //玩家属性  账号id  金钱数
    public Text playerid;
    public Text gold;

    //按钮
    public Button Red_Button;       //红按钮
    public Button Green_Button;     //绿按钮 
    public Button Switch_Button;    //切换按钮
    public Button Cancel_Button;    //取消按钮
    public Button Other_Button;     //其他按钮

    //界面
    public GameObject ErrorPanel;   //异地登录界面
    public GameObject MenuPanel;    //功能界面
    public GameObject OtherPanel;   //其他界面
    public GameObject InfoPanel;    //结算界面
    public GameObject FengPanPanel; //封盘
    public GameObject KaiJiangPanel;//开奖

    //错误提示
    public GameObject oldErrorGo;   //旧的提示
    public GameObject ErrorGo;      //提示
    public GameObject ErrorGoParent; //提示放置的父对象

    //加载界面
    public GameObject loadingBack;      //加载背景
    public Slider loadingSlider;        //加载进度条

    //图集
    public Sprite[] Switch_Sprite;  //切换按钮图集
    public Sprite[] Conch_Sprite;   //珍珠类型

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        tcp = NewTcpNet.GetInstance();

        StartCoroutine(ShowLoading());

        TableClear();
        Init();
        AddListener();

        StartCoroutine(OnPolling());
        StartCoroutine(GetHistory());
    }

    #region 其他方法
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        Application.targetFrameRate = 30;
        XianHong.text = "限红：" + LoginInfo.Instance().mylogindata.roomlitmit;
        playerid.text = "ID:" + LoginInfo.Instance().mylogindata.user_id;
       

        changeNum = 0;
        isFirstBet = true;
        LoginInfo.Instance().mylogindata.coindown = 10;
        Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[0];


        StartCoroutine(betpolling());

    }

    /// <summary>
    /// 添加按钮监听
    /// </summary>
    public void AddListener()
    {
        GameObject obj1;
        obj1 = Red_Button.gameObject;
        Red_Button.onClick.AddListener(delegate ()
        {
            BetMethod(obj1);
        });

        GameObject obj2;
        obj2 = Green_Button.gameObject;
        Green_Button.onClick.AddListener(delegate ()
        {
            BetMethod(obj2);
        });

        Switch_Button.onClick.AddListener(SwitchMethod);

        Cancel_Button.onClick.AddListener(CancelMethod);

        Other_Button.onClick.AddListener(delegate ()
        {
            if (OtherPanel.activeSelf)
            {
                OtherPanel.SetActive(false);
            }
            else
            {
                OtherPanel.SetActive(true);
            }
        });
    }

    /// <summary>
    /// 表格清理
    /// </summary>
    public void TableClear()
    {
        Red_Count.text = "0";
        Green_Count.text = "0";
        for (int i = 0; i < Table1.transform.childCount; i++)
        {
            Table1.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < Table2.transform.childCount; i++)
        {
            Table2.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 清除下注信息
    /// </summary>
    public void ClearBet()
    {
        Red_AllScore.text = "0";
        Green_AllScore.text = "0";
        Green_UserScore.text = "0";
        Red_UserScore.text = "0";
    }

    /// <summary>
    /// 表格内容刷新
    /// </summary>
    public void TableUpdate(string [] strs)
    {
        //表1
        for (int i = 0; i < strs.Length; i++)
        {
            Table1.transform.GetChild(i).gameObject.SetActive(true);
            Table1.transform.GetChild(i).GetComponent<Image>().sprite = Conch_Sprite[int.Parse(strs[i])];
        }


        //表2
        int x = 0, y = 0;
        int pos;
        string lastnum = "", nownum = "";
        string type = "";

        for (int i = 0; i < strs.Length; i++)
        {
            nownum = strs[i];
            if (lastnum != "")
            {
                if (type == "")
                {
                    x++;
                }
                else
                {
                    if (nownum == type)
                    {
                        x++;
                    }
                    else
                    {
                        x = 0;
                        y++;
                    }

                }

            }
            if (x >= 6)
            {
                x = 0;
                y++;
            }
            pos = x + y * 6;
            Table2.transform.GetChild(pos).GetChild(0).gameObject.SetActive(true);
            if (nownum == "0")
            {
                Red_Count.text= (Convert.ToInt32(Red_Count.text.ToString()) + 1).ToString();
                Table2.transform.GetChild(pos).GetChild(0).GetComponent<Image>().color = Color.red;
            } else if (nownum == "1")
            {
                Green_Count.text = (Convert.ToInt32(Green_Count.text.ToString()) + 1).ToString();
                Table2.transform.GetChild(pos).GetChild(0).GetComponent<Image>().color = Color.green;
            }

            //try
            //{
            //    Table2.transform.GetChild(pos).GetComponent<Image>().sprite = pointSprite[int.Parse(nownum)];
            //}
            //catch (Exception)
            //{

            //}

            lastnum = nownum;
            type = nownum;
            //if (nownum != "2")
            //{
            //    type = nownum;
            //}
        }
    }
    #endregion

    #region 按钮方法

    bool isFirstBet;
    string coin;
    /// <summary>
    /// 下注
    /// </summary>
    public void BetMethod(GameObject obj)
    {
        string num = obj.name;
        coin = "";
        if (LoginInfo.Instance().mylogindata.ALLScroce != "0")
        {

            if (isFirstBet && LoginInfo.Instance().mylogindata.coindown < float.Parse(LoginInfo.Instance().mylogindata.roomcount))
            {
                if (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) < float.Parse(LoginInfo.Instance().mylogindata.roomcount))
                {

                    //提示下注失败
                    coin = LoginInfo.Instance().mylogindata.ALLScroce;

                }
                else if (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) >= float.Parse(LoginInfo.Instance().mylogindata.roomcount))
                {
                    coin = LoginInfo.Instance().mylogindata.roomcount.ToString();
                }
                isFirstBet = false;
            }
            else if (isFirstBet && LoginInfo.Instance().mylogindata.coindown >= float.Parse(LoginInfo.Instance().mylogindata.roomcount))
            {
                if (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) < float.Parse(LoginInfo.Instance().mylogindata.coindown.ToString()))
                {
                    coin = LoginInfo.Instance().mylogindata.ALLScroce;
                }
                else if (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) >= float.Parse(LoginInfo.Instance().mylogindata.coindown.ToString()))
                {
                    coin = LoginInfo.Instance().mylogindata.coindown.ToString();
                }
            }
            else if (!isFirstBet && LoginInfo.Instance().mylogindata.coindown <= Convert.ToInt32(float.Parse(LoginInfo.Instance().mylogindata.ALLScroce)))
            {
                coin = LoginInfo.Instance().mylogindata.coindown.ToString();
            }
            else if (!isFirstBet && LoginInfo.Instance().mylogindata.coindown > Convert.ToInt32(float.Parse(LoginInfo.Instance().mylogindata.ALLScroce)))
            {
                coin = LoginInfo.Instance().mylogindata.ALLScroce;
            }

            StartCoroutine(OnBetMethod(num));
        }
        else
        {
            //没有分可以下了
        }
    }

    int changeNum = 0;
    /// <summary>
    /// 切换方法
    /// </summary>
    public void SwitchMethod()
    {
        changeNum++;
        if (changeNum > 5)
        {
            changeNum = 0;
        }
        switch (changeNum)
        {
            case 0:
                LoginInfo.Instance().mylogindata.coindown = 10;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[0];
                break;
            case 1:
                LoginInfo.Instance().mylogindata.coindown = 50;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[1];
                break;
            case 2:
                LoginInfo.Instance().mylogindata.coindown = 100;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[2];
                break;
            case 3:
                LoginInfo.Instance().mylogindata.coindown = 200;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[3];
                break;
            case 4:
                LoginInfo.Instance().mylogindata.coindown = 500;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[4];
                break;
            case 5:
                LoginInfo.Instance().mylogindata.coindown = 1000;
                Switch_Button.transform.GetChild(0).GetComponent<Image>().sprite = Switch_Sprite[5];
                break;
        }
    }

    /// <summary>
    /// 取消下注
    /// </summary>
    public void CancelMethod()
    {
        StartCoroutine(OnSendPass());
    }

    /// <summary>
    /// 显示错误提示
    /// </summary>
    public void ShowMessageToShow(string str)
    {

        if (!LoginInfo.Instance().mylogindata.isOpenError)
        {
            return;
        }
        if (oldErrorGo != null)
        {
            Destroy(oldErrorGo);
        }
        //此重载可先定义父级后生成
        GameObject go = GameObject.Instantiate(ErrorGo, ErrorGoParent.transform, true);
        go.transform.localPosition = Vector3.zero;
        oldErrorGo = go;
        oldErrorGo.transform.GetChild(0).GetComponent<Text>().text = str;

        oldErrorGo.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(oldErrorGo.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);
    }


    private bool IsPlay;
    private bool IsPlay_MP;
    public void PlayConchAnime(string win)
    {
        if (!IsPlay)
        {
            IsPlay = true;
            if (win == "0")
            {
                Conch.GetComponent<Animator>().Play("conch_red");
            } else if (win=="1")
            {
                Conch.GetComponent<Animator>().Play("conch_green");
            }
        }
    }

    public void PlayConchAnime_MP(string win)
    {
        if (!IsPlay_MP)
        {
            IsPlay_MP = true;
            if (win == "0")
            {
                Conch.GetComponent<Animator>().Play("conch_red");
            }
            else if (win == "1")
            {
                Conch.GetComponent<Animator>().Play("conch_green");
            }
        }
    }

    #endregion

    #region 协程
    //下注
    IEnumerator OnBetMethod(string num)
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
           LoginInfo.Instance().mylogindata.betDown_xwy +
           "user_id=" + LoginInfo.Instance().mylogindata.user_id +
           "&num=" + coin +
           "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
           "&id=" + num);
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                gold.text = jd["quick_credit"].ToString();
               // if (jd["BetList"]["drop_content"].ToString()=="A")
               // {
                    //红
                    //Red_UserScore.text = (float.Parse(Red_UserScore.text) + float.Parse(jd["BetList"]["num"].ToString())).ToString();
               // }
                //if (jd["BetList"]["drop_content"].ToString() == "B")
               // {
                    //绿
                    //Green_UserScore.text = (float.Parse(Green_UserScore.text) + float.Parse(jd["BetList"]["num"].ToString())).ToString();
               // }
            }
            else
            {
                ShowMessageToShow(jd["msg"].ToString());
            }
        }
    }

    //取消下注
    IEnumerator OnSendPass()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL+ 
            LoginInfo.Instance().mylogindata.betCancel_xwy+
            "room_id=" +
                LoginInfo.Instance().mylogindata.room_id +
                "&user_id=" +
                LoginInfo.Instance().mylogindata.user_id +
                "&drop_date=" +
                LoginInfo.Instance().mylogindata.dropContent
            );
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                isFirstBet = true;
                ShowMessageToShow("取消下注成功!");
                Green_UserScore.text = "0";
                Red_UserScore.text = "0";
            }
            else
            {
                ShowMessageToShow(jd["msg"].ToString());
            }
        }
        else
        {
            ShowMessageToShow("取消下注失败！");
        }

    }

    IEnumerator GetHistory()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winHistory +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame);
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                TableClear();
                List<string> _list = new List<string>();

                for (int i = 0; i < jd["ArrList"].Count; i++)
                {
                    _list.Add(jd["ArrList"][i]["winnings"].ToString());
                }
                TableUpdate(_list.ToArray());
            }
        }

    }

    //登录状态轮询
    IEnumerator OnPolling()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
             LoginInfo.Instance().mylogindata.hallaliveAPI +
            "user_id=" + LoginInfo.Instance().mylogindata.user_id +
            "&unionuid=" + LoginInfo.Instance().mylogindata.token);
            yield return www.Send();
            Debug.Log(www.url);
            if (www.error == null)
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["Userinfo"]["quick_credit"].ToString();
                    gold.text = LoginInfo.Instance().mylogindata.ALLScroce;
                    if (jd["Userinfo"]["status"].ToString() == "2")
                    {
                        ShowOtherMess(jd["msg"].ToString());
                        yield return new WaitForSeconds(2f);
                        tcp.SocketQuit();
                        SceneManager.LoadScene(0);
                    }
                }
                else
                {
                    ShowOtherMess(jd["msg"].ToString());
                    yield return new WaitForSeconds(2f);
                    tcp.SocketQuit();
                    SceneManager.LoadScene(0);
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }

    //获得输赢结果
    IEnumerator GetWinInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winInfo +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                "&drop_date=" + LoginInfo.Instance().mylogindata.dropContent +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&user_id=" + LoginInfo.Instance().mylogindata.user_id);

        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                //显示得分
                InfoPanel.transform.GetChild(1).GetComponent<Text>().text = "本局所得分数：" + jd["WinTotal"].ToString();
                InfoPanel.SetActive(true);
                StartCoroutine(ShowLight(jd["winnings"].ToString()));
                yield return new WaitForSeconds(2);
                InfoPanel.SetActive(false);
                //ClearBet();
            }
        }
    }

    //重新获取数据
    IEnumerator OnReGet()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.newInit +
                "user_id=" + LoginInfo.Instance().mylogindata.user_id +
                "&unionuid=" + LoginInfo.Instance().mylogindata.token +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&game_id=" + LoginInfo.Instance().mylogindata.choosegame);
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString()=="200")
            {
                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    if (jd["Oddlist"][i]["num"].ToString()=="A")
                    {
                        //红
                        Red_Button.name = jd["Oddlist"][i]["id"].ToString();

                        Red_Rate.text= "X" + jd["Oddlist"][i]["rate"].ToString();
                        //Red_AllScore.text = jd["Oddlist"][i]["dnum"].ToString();
                        //Red_UserScore.text = jd["Oddlist"][i]["user_dnum"].ToString();
                    }

                    if (jd["Oddlist"][i]["num"].ToString() == "B")
                    {
                        //绿
                        Green_Button.name = jd["Oddlist"][i]["id"].ToString();

                        Green_Rate.text = "X"+jd["Oddlist"][i]["rate"].ToString();
                        //Green_AllScore.text = jd["Oddlist"][i]["dnum"].ToString();
                        //Green_UserScore.text = jd["Oddlist"][i]["user_dnum"].ToString();
                    }
                }
            }
        }

    }

    //加载进度条
    IEnumerator ShowLoading()
    {
        loadingBack.SetActive(true);
        loadingSlider.value = 0;
        while (true)
        {
            float a = UnityEngine.Random.Range(0.1f, 0.4f);
            loadingSlider.value += a;
            if (loadingSlider.value >= 1)
            {
                loadingBack.SetActive(false);
                //uniWebView.Hide();
                break;
            }
            yield return new WaitForSeconds(a);
        }
    }

    //显示灯闪烁
    IEnumerator ShowLight(string str)
    {
        float time = 0;
        while (time>2)
        {
            time += 0.2f;
            yield return new WaitForSeconds(0.2f);
            if (str == "0")
            {
                if (Red_Win.activeSelf)
                {
                    Red_Win.SetActive(false);
                }
                else
                {
                    Red_Win.SetActive(true);
                }
            } else if (str=="1")
            {
                if (Green_Win.activeSelf)
                {
                    Green_Win.SetActive(false);
                }
                else
                {
                    Green_Win.SetActive(true);
                }
            }
        }
        yield return null;
    }

    /// <summary>
    /// 下注用轮询
    /// </summary>
    /// <returns></returns>
    IEnumerator betpolling()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
          "user-bets-room-data?" +
          "user_id=" + LoginInfo.Instance().mylogindata.user_id +
          "&room_id="+ LoginInfo.Instance().mylogindata.room_id+
          "&game_id=" + LoginInfo.Instance().mylogindata.choosegame);

            www.timeout = 1;
            yield return www.Send();
            Debug.Log(www.url);
            if (www.error == null)
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    Red_UserScore.text = jd["data"]["userA"].ToString();
                    Green_UserScore.text = jd["data"]["userB"].ToString();
                    Red_AllScore.text = jd["data"]["totalA"].ToString();
                    Green_AllScore.text = jd["data"]["totalB"].ToString();
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }


    #endregion

    #region Socket接收相关

    bool isPlayOver;
    bool isGetWin;
    int state = 0;
    //长连接轮询
    public void PollingPeriods(JsonData jd)
    {
        //倒计时
        Clock.text = jd["countdown"].ToString();
        //局数
        Inning.text = jd["periods"].ToString();
        //轮数
        Rounds.text = jd["season"].ToString();
        
        //彩金相关
        if (jd["handsel_status"].ToString() == "1")
        {
            Handsel.gameObject.SetActive(false);
        }
        else if (jd["handsel_status"].ToString() == "2")
        {
            Handsel.gameObject.SetActive(true);
            Handsel.transform.GetChild(0).GetComponent<Text>().text = "彩金：" + jd["handsel"].ToString();
        }
      


        //爆机
        if (jd["violent"].ToString() == "1")
        {
            baoji.SetActive(true);
        }
        else
        {
            baoji.SetActive(false);
        }



        if (jd["countdown"].ToString()=="0")
        {
            if (isPlayOver)
            {
                //播放停止压分
                Audiomanger._instenc.PlayTip(1);
                FengPanPanel.SetActive(true);
                isPlayOver = false;
            }
        }

        //记录期数
        LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();

        if (jd["is_win"].ToString() == "0")
        {
            if (jd["countdown"].ToString() != "0")
            {
                if (!isPlayOver)
                {
                    //刷新结果一次
                    StartCoroutine(GetHistory());

                    //播放开始压分
                    Audiomanger._instenc.PlayTip(0);
                    FengPanPanel.SetActive(false);
                    KaiJiangPanel.SetActive(false);
                    Red_Win.SetActive(false);
                    Green_Win.SetActive(false);
                    isPlayOver = true;
                    isGetWin = false;

                    IsPlay = false;
                    IsPlay_MP = false;

                    state = 0;
                }
                //明牌相关
                if(jd["open_deal"]!=null)
                {
                    if (jd["open_deal"].ToString() == "10")
                    {
                        Conch.GetComponent<Animator>().Play("Conch");
                    }
                    else
                    {
                        //明牌
                        if (jd["open_deal"].ToString() == "0" || jd["open_deal"].ToString() == "1")
                        {
                            if (!isGetWin)
                            {
                                isGetWin = true;
                                PlayConchAnime(jd["open_deal"].ToString());
                                //PlayConchAnime_MP(jd["open_deal"].ToString());
                            }
                        }
                    }
                }
               
              

            }
            else if (jd["countdown"].ToString() == "0")
            {

                if (jd["winnings"].ToString() != "")
                {
                    if (!isGetWin)
                    {
                        isGetWin = true;
                        FengPanPanel.SetActive(false);
                        PlayConchAnime(jd["winnings"].ToString());
                    }
                }
            }
        }
        else if (jd["is_win"].ToString() == "1")
        {
            if (jd["winnings"].ToString() != "")
            {
                if (!isGetWin)
                {
                    Debug.Log("开奖");
                    isGetWin = true;
                    FengPanPanel.SetActive(false);
                    PlayConchAnime(jd["winnings"].ToString());
                }
            }
        }
        else if (jd["is_win"].ToString() == "2")
        {
            if (!isGetWin)
            {
                if (jd["winnings"].ToString() != "")
                {
                    isGetWin = true;
                    FengPanPanel.SetActive(false);
                    PlayConchAnime(jd["winnings"].ToString());
                }
            }

            if (IsPlay)
            {//动画播放中
                FengPanPanel.SetActive(false);
                state++;
                if (state == 3)
                {
                    if (jd["winnings"].ToString() == "0")
                    {
                        //播放语音红
                        Audiomanger._instenc.PlaySound(Audiomanger._instenc.Xwy_Win[0]);
                    }
                    else if (jd["winnings"].ToString() == "1")
                    {
                        //播放语音绿
                        Audiomanger._instenc.PlaySound(Audiomanger._instenc.Xwy_Win[1]);
                    }

                    KaiJiangPanel.SetActive(false);

                    //报结果
                    StartCoroutine(GetWinInfo());

                    //获取历史记录
                    StartCoroutine(GetHistory());

                } else if (state > 3)
                {
                    //持续获取历史记录
                    StartCoroutine(GetHistory());
                }

            }
            else
            {//没有播放动画   直接显示
                KaiJiangPanel.SetActive(true);
                //if (jd["winnings"].ToString() == "0")
                //{
                //    Conch.GetComponent<Animator>().Play("Conch_redEnd");
                //} else if (jd["winnings"].ToString()=="1")
                //{
                //    Conch.GetComponent<Animator>().Play("Conch_greedEnd");
                //}
                //持续获取历史记录
                StartCoroutine(GetHistory());
            }
        }





    }

    //长连接初始化
    public void UpdateSuit(JsonData jd)
    {
        for (int i = 0; i < jd["oddlist"].Count; i++)
        {
            if (i==0)//红
            {
                Red_Button.name = jd["oddlist"][0]["id"].ToString();
                Red_Rate.text = "X" + jd["oddlist"][0]["rate"].ToString();
                //Red_AllScore.text = jd["oddlist"][0]["dnum"].ToString();
                //Red_UserScore.text = jd["oddlist"][0]["user_dnum"].ToString();
            }
            if (i==1)//绿
            {
                Green_Button.name = jd["oddlist"][1]["id"].ToString();
                Green_Rate.text = "X" + jd["oddlist"][1]["rate"].ToString();
                Green_AllScore.text = jd["oddlist"][1]["dnum"].ToString();
                Green_UserScore.text = jd["oddlist"][1]["user_dnum"].ToString();
            }
        }

    }

    //长连接下注更新
    public void OddList(JsonData jd)
    {
        for (int i = 0; i < jd["Oddlist"].Count; i++)
        {
            if (i == 0)
            {
                //红
                //Red_AllScore.text= jd["Oddlist"][i].ToString();

            } else if (i==1)
            {
                //绿
                //Green_AllScore.text = jd["Oddlist"][i].ToString();
            }

        }
    }

    #endregion


    void ShowOtherMess(string str)
    {
        ErrorPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
        ErrorPanel.SetActive(true);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            OnLogin onLo = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
            string str = JsonMapper.ToJson(onLo);

            tcp.SendMessage(str);
            StartCoroutine(GetHistory());
            StartCoroutine(OnReGet());

        }
    }
}