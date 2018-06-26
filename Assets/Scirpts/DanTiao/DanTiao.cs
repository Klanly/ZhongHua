using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class DanTiao : MonoBehaviour
{
    #region UP
    public Text xianHongText;
    public Text yaFenText;
    public Text juShuText;
    public Text roundText;
    public Text idText;
    public Text GoldText;
    public Text ClockText;
    #endregion


    #region Center

    public Image pokerCard;
    public Image pokerBack; //背面这张
    public GameObject picGo; //图片显示结果父物体
    public GameObject numGo; //数字显示结果
    public GameObject wordGo; //文字显示结果
    public GameObject winInfoPanel;
    public GameObject winState; //显示正在开奖中
    public GameObject fengPanGo;




    /*
     * 这里是一些需要用到的文件 和变量
     * 
     * */
    public Sprite[] pokerCardSprite;  //开奖牌   黑 红 花 片 王  下同
    public Sprite[] picSprite; //
    public Sprite[] blackNumSprite;
    public Sprite[] redNumSprite;
    public Sprite[] wordSprite;


    int resultCount; //多少期进行清空

    Vector2 backPokerVec;
    public static bool isFengPan;

    #endregion

    #region Down

    /* 0为黑桃  1为红桃  2为梅花  3为方块  4为王*/
    public Button[] suitBtn;
    public Text[] suitRateText; //赔率表
    public Text[] suitInSelfText; //自己下注
    public Text[] suitCountText; //出现次数
    public Text[] suitInAllText; //总下注
    public List<Image> suitBackLight;

    public Button changeBtn; //切换下注信息
    public Button passBtn; //取消下注信息

    public GameObject betMask; //防止还没有获取到下注id就开始下注


    /*               
     * 这里所需要的一些文件和变量
     * 
     * */

    public Sprite[] changeSprite;//切换需要的Sprite
    int changeKind;//用来记录当前切换到了哪一个下注额度
    string[] buttonIDCode = new string[5];  //用来保存当前下注按钮的ID

    bool isFirstBet; //判断是否为第一次下注  true为第一次

    #endregion


    #region Right
    public Button plusBtn;
    public static string[] betId = new string[5];
    public static string[] rateInfo = new string[5];
    public static string[] allDnumInfo = new string[5];
    public static string[] selfDnumInfo = new string[5];
    //待定
    #endregion


    #region Other
    public NewTcpNet tcpNet;

    public GameObject errorPanel;

    public static DanTiao instance;

    bool isShowMenu; //是否显示菜单
    public GameObject menuGo;

    public static bool is_WinTwo;
    public static string winInfo;

    
    public GameObject errorGoParent;
    public GameObject errorGo;
    GameObject oldErrorGo;

    public bool isOnClear; //判断后台是否已经清空并重开

    //public string url;
    //public uniweblist uniwebtoopen;


    #endregion


    private void Awake()
    {
        instance = this;

        //betMask.SetActive(true);
        //StartCoroutine(GetBetId(LoginInfo.Instance().mylogindata.URL +
        //   LoginInfo.Instance().mylogindata.newInit +
        //   "&user_id=" + LoginInfo.Instance().mylogindata.user_id +
        //   "&unionuid=" + LoginInfo.Instance().mylogindata.token +
        //   "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
        //   "&game_id=" + LoginInfo.Instance().mylogindata.choosegame));
    }
    void Start()
    {
        
        tcpNet = NewTcpNet.GetInstance();

        StartCoroutine(ShowLoading());
        Application.targetFrameRate = 30;
        Init();
        AddListener();


        //初始化信息
        StartCoroutine(Polling
            (
             LoginInfo.Instance().mylogindata.URL +
             LoginInfo.Instance().mylogindata.hallaliveAPI +
            "user_id=" + LoginInfo.Instance().mylogindata.user_id +
            "&unionuid=" + LoginInfo.Instance().mylogindata.token/* +
            "&room_id" + LoginInfo.Instance().mylogindata.room_id +
            "&game_id" + LoginInfo.Instance().mylogindata.game_id*/
            ));
     
    }

  

    /// <summary>
    /// 在这里进行一些初始化操作
    /// </summary>
    void Init()
    {
        
        backPokerVec = pokerBack.transform.GetComponent<RectTransform>().anchoredPosition;
        ClearInfo();
        ClockText.text = "0";
        isFirstBet = true;
        isPause = false;
        isFocus = false;
        numGo.SetActive(true);
        if (PlayerPrefs.GetString("isShowWord") == "true")  //开奖结果显示方式
        {
            wordGo.SetActive(true);
            picGo.SetActive(false);
        }
        else
        {
            wordGo.SetActive(false);
            picGo.SetActive(true);
        }
        for (int i = 0; i < suitInSelfText.Length; i++)  //初始下注信息
        {
            suitInSelfText[i].text = "0";
            suitInAllText[i].text = "0";
            suitCountText[i].text = "0";
        }
        resultCount = 0; //开奖总数
        xianHongText.text = LoginInfo.Instance().mylogindata.roomlitmit; //限红
        yaFenText.text = LoginInfo.Instance().mylogindata.roomcount; //最小压分
        idText.text = "id：" + LoginInfo.Instance().mylogindata.user_id; //用户id
        GoldText.text = LoginInfo.Instance().mylogindata.ALLScroce;  //金币
        changeBtn.transform.GetComponent<Image>().sprite = changeSprite[1]; //初始默认下注额度为10;
        LoginInfo.Instance().mylogindata.coindown = 10;
        changeKind = 1; //并且记录当前切换到了哪一个图片 这里是记录切换下注额度

        StartCoroutine(GetHistory(LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.winHistory +
            "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));



    }
    /// <summary>
    /// 事件监听
    /// </summary>
    void AddListener()
    {
        //下注事件
        //for (int i = 0; i < suitBtn.Length; i++)
        //{
        //    int num = i;
        //    OnClickSuit(num);
        //}
        changeBtn.onClick.AddListener(OnChangeCoinDown);  //切换下注额度信息
        passBtn.onClick.AddListener(OnClickPass);  //切换取消消息

        for (int i = 0; i < suitBtn.Length; i++)
        {
            //Debug.Log("点击下注");
            int num = i;
            suitBtn[i].onClick.AddListener(
                delegate ()
                {
                    StartCoroutine(OnBetDown(num));
                    
                }
                );

        }

        //suitBtn[0].onClick.AddListener(delegate () { /*StartCoroutine(OnBetDown(0));*/BetDown(0);  });
        //suitBtn[1].onClick.AddListener(delegate () { /*StartCoroutine(OnBetDown(1)); */BetDown(1); });
        //suitBtn[2].onClick.AddListener(delegate () { /*StartCoroutine(OnBetDown(2)); */BetDown(2); });
        //suitBtn[3].onClick.AddListener(delegate () { /*StartCoroutine(OnBetDown(3));*/ BetDown(3);  });
        //suitBtn[4].onClick.AddListener(delegate () {/* StartCoroutine(OnBetDown(4));*/ BetDown(4);  });

    }

    private void Update()
    {
        if (NewTcpNet.isUpdate)
        {
            fengPanGo.SetActive(false);
            ClockText.text = NewTcpNet.countDown;
            roundText.text = NewTcpNet.season;
            juShuText.text = NewTcpNet.periods;
            NewTcpNet.isUpdate = false;
        }
        if (NewTcpNet.isUpdateAllDnum)
        {
            for (int i = 0; i < suitInAllText.Length; i++)
            {
                suitInAllText[i].text = NewTcpNet.dnum[i];

            }
            NewTcpNet.isUpdateAllDnum = false;
        }
        if (is_WinTwo)
        {
            
            fengPanGo.SetActive(false);
            OnWin(winInfo);
            is_WinTwo = false;
        }
        if (NewTcpNet.isUpdateRate)
        {
            for (int i = 0; i < suitRateText.Length; i++)
            {
                suitRateText[i].text = "X" + rateInfo[i];
                suitInAllText[i].text = allDnumInfo[i];
                suitInSelfText[i].text = selfDnumInfo[i];
            }
            NewTcpNet.isUpdateRate = false;
        }
        if (!NewTcpNet.isFirst)
        {
            winState.SetActive(true);
        }
        else if (winState.activeSelf)
        {
            winState.SetActive(false);
        }
        if (isOnClear)
        {
            ClearInfo();
            isOnClear = false;
        }
        if (isFengPan)
        {
            ClockText.text = NewTcpNet.countDown;
            fengPanGo.SetActive(true);
        }
        

    }

    // Update is called once per frame
#region  UnityWebRequest


    IEnumerator GetHistory(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < jd["ArrList"].Count; i++)
                {
                    ColorAndNum(jd["ArrList"][i]["winnings"].ToString());
                }
            }
        }
      
    }

    /// <summary>
    /// 轮询信息
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator Polling(string url)
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.Send();
            if (www.error == null)
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["Userinfo"]["quick_credit"].ToString();
                    GoldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
                    if (jd["Userinfo"]["status"].ToString() == "2")
                    {
                        ShowOtherMess(jd["msg"].ToString());
                        yield return new WaitForSeconds(2f);
                        tcpNet.SocketQuit();
                        SceneManager.LoadScene(0);
                    }
                }
                else
                {
                    ShowOtherMess(jd["msg"].ToString());
                    yield return new WaitForSeconds(2f);
                    tcpNet = NewTcpNet.GetInstance();
                    SceneManager.LoadScene(0);
                }

            }

            yield return new WaitForSeconds(4f);
        }


    }

    /// <summary>
    /// 点击取消事件
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator OnSendPass(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                isFirstBet = true;
                for (int i = 0; i < suitInAllText.Length; i++)
                {

                    suitInAllText[i].text = (float.Parse(suitInAllText[i].text) - (float.Parse(suitInSelfText[i].text))).ToString();
                    suitInSelfText[i].text = "0";
                }
                ShowMessageToShow("取消下注成功!");
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
    /*IEnumerator OnReceiveResult(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                //进行更新结果
                //进行更新时间
                //进行更新数据
                //
            }

        }
    }*/

    /// <summary>
    /// 下注事件
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    /// 

    void BetDown(int num)
    {
        StartCoroutine(OnBetDown(num));
    }
    IEnumerator OnBetDown(int num)
    {
        Audiomanger._instenc.clickvoice();
        //Debug.Log("开始下注");
        if (LoginInfo.Instance().mylogindata.ALLScroce != "0")
        {
            string coin = "";
            if (isFirstBet && LoginInfo.Instance().mylogindata.coindown < int.Parse(LoginInfo.Instance().mylogindata.roomcount))
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
            else if (isFirstBet && LoginInfo.Instance().mylogindata.coindown >= int.Parse(LoginInfo.Instance().mylogindata.roomcount))
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


            string url = LoginInfo.Instance().mylogindata.URL +
           LoginInfo.Instance().mylogindata.BetDown_mpzzs +
           "user_id=" + LoginInfo.Instance().mylogindata.user_id +
           "&num=" + coin +
           "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
           "&id=" + betId[num];/* +
               "&drop_content=" + betIdWord[num];*/
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.Send();
            //Debug.LogError("已经发送");
            if (www.error == null)
            {
                //Debug.LogError("发送成功");
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["quick_credit"].ToString();
                    GoldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
                    suitInSelfText[num].text = (float.Parse(suitInSelfText[num].text) + float.Parse(jd["BetList"]["num"].ToString())).ToString();
                    suitInAllText[num].text = (float.Parse(suitInAllText[num].text) + float.Parse(jd["BetList"]["num"].ToString())).ToString();
                }
                else
                {
                    //Debug.LogError(jd["code"].ToString());
                    ShowMessageToShow(jd["msg"].ToString());
                }
            }
            else
            {
                ShowMessageToShow("下注失败！");
            }


        }

        //Debug.Log("点击下注");


    }


    IEnumerator GetBetId(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    betId[i] = jd["Oddlist"][i]["id"].ToString();
                    //betIdWord[i] = jd["Oddlist"][i]["num"].ToString();
                    suitRateText[i].text = "X" + jd["Oddlist"][i]["rate"].ToString();
                }
                betMask.SetActive(false);
            }
        }
    }

    IEnumerator GetWinInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                winInfoPanel.transform.GetChild(1).GetComponent<Text>().text = "本局所得分数：" + jd["WinTotal"].ToString();
                winInfoPanel.SetActive(true);
                yield return new WaitForSeconds(3);
                winInfoPanel.SetActive(false);

            }
        }
    }



#endregion


#region Socket

    ///// <summary>
    ///// 点击下注事件
    ///// </summary>
    //void OnClickSuit(int num)
    //{
    //    //向服务器发送消息发送消息

    //}

    /// <summary>
    /// 切换下注额度事件
    /// </summary>
    /// <param name="num"></param>
    void OnChangeCoinDown()
    {
        changeKind++;
        if (changeKind > 3)
        {
            changeKind = 0;
        }
        switch (changeKind)
        {
            case 0:
                LoginInfo.Instance().mylogindata.coindown = 1;
                changeBtn.transform.GetComponent<Image>().sprite = changeSprite[changeKind];
                break;
            case 1:
                LoginInfo.Instance().mylogindata.coindown = 10;
                changeBtn.transform.GetComponent<Image>().sprite = changeSprite[changeKind];
                break;
            case 2:
                LoginInfo.Instance().mylogindata.coindown = 100;
                changeBtn.transform.GetComponent<Image>().sprite = changeSprite[changeKind];
                break;
            case 3:
                LoginInfo.Instance().mylogindata.coindown = 500;
                changeBtn.transform.GetComponent<Image>().sprite = changeSprite[changeKind];
                break;
        }
    }


    /// <summary>
    /// 当前点击取消事件时
    /// </summary>
    void OnClickPass()
    {
        StartCoroutine(OnSendPass
            (
            LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.betCancel_mpzzs +
            "room_id=" +
            LoginInfo.Instance().mylogindata.room_id +
            "&user_id=" +
            LoginInfo.Instance().mylogindata.user_id +
            "&drop_date=" +
            LoginInfo.Instance().mylogindata.dropContent
            ));
    }
#endregion


#region UI面板的一些更新信息

    /// <summary>
    /// 更新左上角金币信息
    /// </summary>
    /// <param name="num"></param>
    void ShowGold(int num)
    {
        LoginInfo.Instance().mylogindata.ALLScroce = (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) - num).ToString();
        GoldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
    }

    void ShowRound(string juShu, string round)
    {
        juShuText.text = juShu;
        roundText.text = round;
    }

#endregion


#region 其他一些逻辑

    void ColorAndNum(string poker)
    {
        int pokerNum = Convert.ToInt32(poker);

        int color = pokerNum / 13;
        int num = pokerNum % 13;
        OnAddPoker(color, num);
    }

    /// <summary>
    /// 添加牌的方法
    /// </summary>
    /// <param name="color"></param>
    /// <param name="num"></param>
    void OnAddPoker(int color, int num)
    {
        if (resultCount > 99)
        {
            ClearInfo();
            //清空历史
            //resultCount = 0;
        }
        
        if (color == 4)
        {
            if (num == 0)
            {
                wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = wordSprite[4];
                wordGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[4];
                picGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            }
            else
            {
                wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = wordSprite[5];
                wordGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[5];
                picGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            }
        }
        else
        {
            picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[color];
            picGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = wordSprite[color];
            wordGo.transform.GetChild(resultCount).gameObject.SetActive(true);
        }

        if (color != 4)
        {
            //numGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = blackNumSprite[num];
            //numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            if (color == 0 || color == 2)
            {
                numGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = blackNumSprite[num];
                numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            }
            else
            {
                numGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = redNumSprite[num];
                numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            }
        }


        resultCount++;

        switch (color)
        {
            case 0:
                suitCountText[0].text = (int.Parse(suitCountText[0].text) + 1).ToString();
                break;
            case 1:
                suitCountText[1].text = (int.Parse(suitCountText[1].text) + 1).ToString();
                break;
            case 2:
                suitCountText[2].text = (int.Parse(suitCountText[2].text) + 1).ToString();
                break;
            case 3:
                suitCountText[3].text = (int.Parse(suitCountText[3].text) + 1).ToString();
                break;
            case 4:
                suitCountText[4].text = (int.Parse(suitCountText[4].text) + 1).ToString();
                break;

        }
    }

    /// <summary>
    /// 清空和下注开奖有关信息
    /// </summary>
    void ClearInfo()
    {
        for (int i = 0; i < numGo.transform.childCount; i++)
        {
            numGo.transform.GetChild(i).gameObject.SetActive(false);
            picGo.transform.GetChild(i).gameObject.SetActive(false);
            wordGo.transform.GetChild(i).gameObject.SetActive(false);

        }
        resultCount = 0;
        for (int i = 0; i < suitCountText.Length; i++)
        {
            suitCountText[i].text = "0";
        }

    }


#endregion


#region Other
    void ShowOtherMess(string str)
    {
        errorPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
        errorPanel.SetActive(true);
    }



    /// <summary>
    /// 监听服务器发来的消息
    /// </summary>


    public void ShowMenu()
    {
        if (isShowMenu)
        {
            menuGo.SetActive(false);
            isShowMenu = false;
        }
        else
        {
            menuGo.SetActive(true);
            isShowMenu = true;
        }
    }


    public void OnWin(string str)
    {

        JsonData jd = JsonMapper.ToObject(str);
        ClockText.text = jd["countdown"].ToString();
        //Debug.LogWarning()
        if (jd["winnings"].ToString() != "")
        {
            pokerCard.sprite = pokerCardSprite[Convert.ToInt32(float.Parse(jd["winnings"].ToString()))];
            
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));
            Audiomanger._instenc.PlayDanTiaoWin(Convert.ToInt32(jd["winnings"].ToString()));
            ColorAndNum(jd["winnings"].ToString());
        }
       

    }

    IEnumerator PokerAni(int num)
    {
        Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(-368f, 3f);
        yield return new WaitForSeconds(3.2f);
        
        StartCoroutine(GetWinInfo
            (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winInfo +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                "&drop_date=" + LoginInfo.Instance().mylogindata.dropContent +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&user_id=" + LoginInfo.Instance().mylogindata.user_id
            ));
        StartCoroutine(PokerAniBack());
        StartCoroutine(SuitLight(num));

    }

    IEnumerator PokerAniBack()
    {
        yield return new WaitForSeconds(4f);
        Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
        yield return new WaitForSeconds(3.1f);
        pokerBack.transform.GetComponent<RectTransform>().anchoredPosition = backPokerVec;
        ClearWinInfo();
    }

    /// <summary>
    /// 显示中奖区域
    /// </summary>
    /// <returns></returns>
    IEnumerator SuitLight(int value)
    {
        int a = value / 13;
        int b = 0;
        bool acticveNow = false;

        while (true)
        {
            suitBackLight[a].gameObject.SetActive(!acticveNow);
            acticveNow = !acticveNow;
            b++;
            yield return new WaitForSeconds(0.4f);
            //Debug.LogError("这里调用了" + b + "次");
            if (b == 8)
            {
                //Debug.LogError("已经超过了八次");
                suitBackLight[a].gameObject.SetActive(false);
                break;
            }

        }
    }

    /// <summary>
    /// 结束后清空信息
    /// </summary>
    void ClearWinInfo()
    {
        for (int i = 0; i < suitInAllText.Length; i++)
        {
            suitInAllText[i].text = "0";
            suitInSelfText[i].text = "0";
            winInfo = null;

        }
    }

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
        GameObject go = GameObject.Instantiate(errorGo, errorGoParent.transform, true);
        go.transform.localPosition = Vector3.zero;
        oldErrorGo = go;
        oldErrorGo.transform.GetChild(0).GetComponent<Text>().text = str;

        oldErrorGo.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(oldErrorGo.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);
    }

    bool isPause = false;
    bool isFocus = false;
    private void OnApplicationFocus(bool focus)
    {

        if (focus)
        {
            //uniWebView.OnClose();
            //StartCoroutine(ShowLoading());
            //Debug.LogWarning("145*******");
            NewTcpNet.isFirst = false;
            OnLogin onLo = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
            string str = JsonMapper.ToJson(onLo);

            tcpNet.SendMessage(str);
            ClearInfo();
            StartCoroutine(GetHistory(
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winHistory +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame
               ));
            StartCoroutine(OnReGet
                (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.newInit +
                "user_id=" + LoginInfo.Instance().mylogindata.user_id +
                "&unionuid=" + LoginInfo.Instance().mylogindata.token +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&game_id=" + LoginInfo.Instance().mylogindata.choosegame
                ));
        }

        
    }

    /// <summary>
    /// 退出重进
    /// </summary>
    /// <returns></returns>
    IEnumerator OnReGet(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    suitInAllText[i].text = jd["Oddlist"][i]["dnum"].ToString();
                    suitInSelfText[i].text = jd["Oddlist"][i]["user_dnum"].ToString();
                }
            }
        }
    }






    /*   void OnApplicationPause()
       {

#if UNITY_IPHONE || UNITY_ANDROID

           //Debug.Log(“OnApplicationPause  “+isPause +”  “+isFocus);

           if (!isPause)

           {

               // 强制暂停时，事件

               //pauseTime();
               isFocus = true;

           }

           else

           {

               isFocus = true;

           }

           isPause = true;

#endif

       }

       void OnApplicationFocus()
       {

#if UNITY_IPHONE || UNITY_ANDROID

           //Debug.Log(“OnApplicationFocus  “+isPause +”  “+isFocus);

           if (isFocus)

           {

               // “启动”手机时，事件





               Debug.LogWarning("145*******");
               OnLogin onLo = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
               string str = JsonMapper.ToJson(onLo);

               tcpNet.SendMessage(str);
               ClearInfo();
               StartCoroutine(GetHistory(LoginInfo.Instance().mylogindata.URL +
                  LoginInfo.Instance().mylogindata.winHistory
                  ));
               StartCoroutine(OnReGet
                   (
                   LoginInfo.Instance().mylogindata.URL +
                   LoginInfo.Instance().mylogindata.newInit +
                   "user_id=" + LoginInfo.Instance().mylogindata.user_id +
                   "&unionuid=" + LoginInfo.Instance().mylogindata.token +
                   "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                   "&game_id=" + LoginInfo.Instance().mylogindata.choosegame
                   ));













               isPause = false;

               isFocus = false;

           }

           if (isPause)

           {

               isFocus = true;

           }

#endif

       }*/

    public GameObject loadingBack;
    public Slider loadingSlider;
    //public UniWebView uniWebView;

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

#endregion
}
