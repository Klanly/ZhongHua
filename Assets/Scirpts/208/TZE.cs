using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TZE : MonoBehaviour
{
    //自身单例
    public static TZE instance;

    public ItemRotate1 ItemRotate1;       //转盘
    public Text idText;                 //id
    public Text goldText;               //金币
    public Text danShuangXian;          
    public Text heXian;                 
    public Text minScore;

    public Text inningText;             //局数
    public Text roundText;              //轮数

    public GameObject baoji;            //爆机
    private bool IsBaoji;               //是否爆机了

    public GameObject pointGo;          //表格1
   
    public Image winPoker;              //已经不需要的【原结果判定显示】
    public Image pokerBack;             //已经不需要的【原结果判定显示】

    public GameObject fengPan;          //封盘提示
    public GameObject kaiJiang;         //开奖中提示

    public GameObject resultGo;         //表格2
    public GameObject infoPanelGo;
    public GameObject errorPanel;

    public bool IsTurn;                //是否启动旋转

    public Sprite[] resultSprite;
    public Sprite[] pointSprite;
    public Sprite[] winPokerSprite;
    int resultCount;


    public Text clockText;


    public Text[] suitInAllText;
    public Text[] suitInSelfText;
    public Text[] suitCount;
    public Text[] suitRateText;
    public Button[] suitBetBtn;
    public GameObject[] lightImage;
    public Button changeBetBtn;
    public Button passBtn;


    public Sprite[] betCoinSprite;
    public List<string> betId;
    int changeNum;
    bool isFirstBet;

    public Button plusBtn;
    public NewTcpNet tcpNet;


  
    int line;
    int row;
    int lastPoint;
    GameObject oldErrorGo;
    public GameObject errorGo;
    public GameObject errorGoParent;


    private void Awake()
    {
        instance = this;


    }
    void Start()
    {
        tcpNet = NewTcpNet.GetInstance();
        StartCoroutine(ShowLoading());
        Init();
        AddListener();

        //LoginInfo.

        StartCoroutine(
           OnPolling
           (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.hallaliveAPI +
               "user_id=" + LoginInfo.Instance().mylogindata.user_id +
               "&unionuid=" + LoginInfo.Instance().mylogindata.token

            ));
        StartCoroutine(GetHistory(LoginInfo.Instance().mylogindata.URL +
           LoginInfo.Instance().mylogindata.winHistory +
           "game_id=" + LoginInfo.Instance().mylogindata.choosegame
           ));

        StartCoroutine("GetBetNum");
    }

    void Init()
    {
        Application.targetFrameRate = 30;
        resultCount = 0;
        line = 0;
        row = 0;
        lastPoint = 2;
        danShuangXian.text = LoginInfo.Instance().mylogindata.roomlitmit;
        minScore.text = LoginInfo.Instance().mylogindata.roomcount;
        idText.text = "id:" + LoginInfo.Instance().mylogindata.user_id;
        betId = new List<string>();
        changeNum = 1;
        LoginInfo.Instance().mylogindata.coindown = 10;
        changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
        isFirstBet = true;
        isPlayOver = false;

    }

    void AddListener()
    {
        changeBetBtn.onClick.AddListener(OnChange); //切换下注额度
        for (int i = 0; i < suitBetBtn.Length; i++)
        {
            int num = i;
            suitBetBtn[i].onClick.AddListener(
                delegate ()
                {
                    BetDown(num);
                }
                );

        }
        passBtn.onClick.AddListener(OnClickPass);
    }




    #region 监听事件

    void OnChange()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        changeNum++;
        if (changeNum > 3)
        {
            changeNum = 0;
        }
        switch (changeNum)
        {
            case 0:
                LoginInfo.Instance().mylogindata.coindown = 1;
                changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
                break;
            case 1:
                LoginInfo.Instance().mylogindata.coindown = 10;
                changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
                break;
            case 2:
                LoginInfo.Instance().mylogindata.coindown = 100;
                changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
                break;
            case 3:
                LoginInfo.Instance().mylogindata.coindown = 500;
                changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
                break;
        }
    }


    void BetDown(int num)
    {
        StartCoroutine(OnBetDown(num));
        Audiomanger._instenc.clickvoice();
    }

    void OnClickPass()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        StartCoroutine(OnSendPass
            (
            LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.betCancel_elb +
            "room_id=" +
            LoginInfo.Instance().mylogindata.room_id +
            "&user_id=" +
            LoginInfo.Instance().mylogindata.user_id +
            "&drop_date=" +
            LoginInfo.Instance().mylogindata.dropContent
            ));
    }
    #endregion


    #region UnityWebRequest

    /// <summary>
    /// 下注
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    IEnumerator OnBetDown(int num)
    {
        Debug.Log("开始下注");
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
           LoginInfo.Instance().mylogindata.betDown_elb +
           "user_id=" + LoginInfo.Instance().mylogindata.user_id +
           "&num=" + coin +
           "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
           "&id=" + betId[num];/* +
               "&drop_content=" + betIdWord[num];*/
            
            //get发送
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.Send();
            Debug.Log(www.url);
            if (www.error == null)
            {
                //Debug.LogError("发送成功");
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["quick_credit"].ToString();
                    goldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
                    suitInSelfText[num].text = (int.Parse(suitInSelfText[num].text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                    //suitInAllText[num].text = (int.Parse(suitInAllText[num].text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                }
                else
                {
                    //Debug.LogError(jd["code"].ToString());
                    ShowMessageToShow(jd["msg"].ToString());
                }
            }
            else
            {
                //ShowMessageToShow("下注失败！");
            }


        }

        //Debug.Log("点击下注");


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

                    //suitInAllText[i].text = (int.Parse(suitInAllText[i].text) - (int.Parse(suitInSelfText[i].text))).ToString();
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




    IEnumerator GetWinInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                infoPanelGo.transform.GetChild(1).GetComponent<Text>().text = "本局所得分数：" + jd["WinTotal"].ToString();
                infoPanelGo.SetActive(true);
                yield return new WaitForSeconds(3);
                infoPanelGo.SetActive(false);

            }
        }
    }


    IEnumerator OnPolling(string url)
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
                    goldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
                    if (jd["Userinfo"]["status"].ToString() == "2")
                    {
                        ShowOtherMess(jd["msg"].ToString());
                        yield return new WaitForSeconds(2f);
                        SceneManager.LoadScene(0);
                    }
                }
                else
                {
                    ShowOtherMess(jd["msg"].ToString());
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene(0);
                }

            }

            yield return new WaitForSeconds(4f);
        }
    }


    IEnumerator GetHistory(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        Debug.Log("获取结果"+www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString().Equals("200"))
            {
                if (IsBaoji)
                {
                    ClearInfo();

                    List<string> _list = new List<string>();

                    for (int i = 0; i < jd["ArrList"].Count; i++)
                    {
                        AddPoker(jd["ArrList"][i]["winnings"].ToString());
                        _list.Add(jd["ArrList"][i]["winnings"].ToString());

                    }
                    AddPointTable(_list.ToArray());
                }
                else
                {
                    if (roundnumber >= jd["ArrList"].Count)
                    {
                        ClearInfo();

                        List<string> _list = new List<string>();

                        for (int i = 0; i < jd["ArrList"].Count; i++)
                        {
                            AddPoker(jd["ArrList"][i]["winnings"].ToString());
                            _list.Add(jd["ArrList"][i]["winnings"].ToString());

                        }
                        AddPointTable(_list.ToArray());
                    }
                }

				
            }
        }
    }




    #endregion






    #region Other

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



    void ClearInfo()
    {
        resultCount = 0;
		lastPoint = 2;
		line = 0;
		row = 0;
        for (int i = 0; i < resultGo.transform.childCount; i++)
        {
            resultGo.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < suitCount.Length; i++)
        {
            suitCount[i].text = "0";
        }
        for (int i = 0; i < pointGo.transform.childCount; i++)
        {
            pointGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
		

    void OnWin(JsonData jd)
    {
        clockText.text = jd["countdown"].ToString();
        if (jd["winnings"].ToString() != "")
        {
            //winPoker.sprite = winPokerSprite[Convert.ToInt32(jd["winnings"].ToString())];

            ChangeWinCoin(jd["winnings"].ToString());
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));
//            Audiomanger._instenc.PlayElbWin(Convert.ToInt32(jd["winnings"].ToString()));


            StartCoroutine(GetHistory(
             LoginInfo.Instance().mylogindata.URL +
             LoginInfo.Instance().mylogindata.winHistory +
             "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));

            //AddPoker(jd["winnings"].ToString());
        }
    }


    /// <summary>
    /// 数组 0为字 1 为花
    /// </summary>
    /// <param name="str"></param>
    void ChangeWinCoin(string str)
    {
        switch (str)
        {
            case "0":

                winPoker.sprite = winPokerSprite[0];
   
                break;
            case "1":
             
                winPoker.sprite = winPokerSprite[1];
               
                break;
            case "2":
                
                winPoker.sprite = winPokerSprite[2];
                break;

        }
    }














    IEnumerator PokerAni(int num)
    {
        if (!isMing)
        {
           // Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(-315f, 3f);
        }
       
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
        ItemRotate1.ResetItem();
        StartCoroutine(PokerAniBack());
        StartCoroutine(SuitLight(num));

    }
    IEnumerator PokerAniBack()
    {
        yield return new WaitForSeconds(4f);
        //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
        yield return new WaitForSeconds(3.1f);
        isMing = false;
        //pokerBack.transform.GetComponent<RectTransform>().anchoredPosition = backPokerVec;
        ClearWinInfo();
    }

    /// <summary>
    /// 显示中奖区域
    /// </summary>
    /// <returns></returns>
    IEnumerator SuitLight(int value)
    {
        //int a = value / 13;
        int b = 0;
        bool acticveNow = false;

        while (true)
        {
            lightImage[value].gameObject.SetActive(!acticveNow);
            acticveNow = !acticveNow;
            b++;
            yield return new WaitForSeconds(0.4f);
            //Debug.LogError("这里调用了" + b + "次");
            if (b == 8)
            {
                //Debug.LogError("已经超过了八次");
                lightImage[value].gameObject.SetActive(false);
                break;
            }

        }
    }

    void ClearWinInfo()
    {
        for (int i = 0; i < suitInAllText.Length; i++)
        {
            suitInAllText[i].text = "0";
            suitInSelfText[i].text = "0";


        }
    }













    /// <summary>
    /// 添加牌    
    /// 0 双
    /// 1 单
    /// 2 和
    /// </su mmary>
    void AddPoker(string str)
    {
        if (resultCount >= 66)
        {
            ClearInfo();
        }
        switch (str)
        {
            case "0":
                resultGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSprite[0];
                resultGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCount[0].text = (Convert.ToInt32(suitCount[0].text.ToString()) + 1).ToString();
                break;
            case "1":
                resultGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSprite[1];
                resultGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCount[2].text = (Convert.ToInt32(suitCount[2].text.ToString()) + 1).ToString();
                break;
            case "2":
                resultGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSprite[2];
                resultGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCount[1].text = (Convert.ToInt32(suitCount[1].text.ToString()) + 1).ToString();
                break;
        }
        resultCount++;

        //AddPokerPoint(str);
    }



    void AddPokerPoint(string str)
    {
		try {
			AddPointRule(Convert.ToInt32(str));
		} catch (Exception ex) {
			
		}
    }



    /// <summary>
    /// 添加点的规则
    /// </summary>
    void AddPointRule(int pointSever)
    {

        if (lastPoint == 2) //则代表为第一次
        {
            pointGo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = pointSprite[pointSever];
            pointGo.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;

        }
        else if (lastPoint == pointSever)
        {

            pointGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = pointSprite[pointSever];
            pointGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;
        }
        else if (lastPoint != pointSever)
        {
            if (pointSever == 2)
            {
                pointGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = pointSprite[pointSever];
                pointGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (line != 0)
                {
                    line = 0;
                    row++;
                    if (row > 48)
                    {

                        for (int i = 0; i < pointGo.transform.childCount; i++)
                        {
                            pointGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                        }
                        line = 0;
                        row = 0;

                    }
                }

                pointGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = pointSprite[pointSever];
                pointGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
                lastPoint = pointSever;
            }

        }

        line++;
        if (line > 5)
        {
            line = 0;
            row++;
            if (row > 48)
            {

                for (int i = 0; i < pointGo.transform.childCount; i++)
                {
                    pointGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }

                row = 0;

            }
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        resultCount = 0;
    //        line = 0;
    //        row = 0;
    //        lastPoint = 2;
    //        string[] sss = new string[] { "2", "2", "1", "1", "1", "0", "0", "0", "0", "1", "1", "1", "1", "1", };
    //        for (int i = 0; i < sss.Length; i++)
    //        {
    //            AddPoker(sss[i]);
    //        }
    //        AddPointTable(sss);

    //    }
    //}

    public void AddPointTable(string[] strs)
    {
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
                    if (nownum == type || nownum == "2")
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
            pointGo.transform.GetChild(pos).GetChild(0).gameObject.SetActive(true);
            try
            {
                pointGo.transform.GetChild(pos).GetChild(0).GetComponent<Image>().sprite = pointSprite[int.Parse(nownum)];
            }
            catch (Exception)
            {
                //Debug.LogError(nownum);
            }

            lastnum = nownum;
            if (nownum != "2")
            {
                type = nownum;
            }
        }
    }

    void ShowOtherMess(string str)
    {
        errorPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
        errorPanel.SetActive(true);
    }

    bool isShowMenu;
    public GameObject menuGo;
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







    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //uniWebView.OnClose();
            //StartCoroutine(ShowLoading());
            isFirstJoin = false;
            OnLogin onLo = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
            string str = JsonMapper.ToJson(onLo);

			//判断是否断开连接
			//Debug.Log("连接状态："+tcpNet.GetConnectionStatus());
			if (tcpNet.GetConnectionStatus()) {
				tcpNet.SendMessage(str);
			} else {
				tcpNet=NewTcpNet.GetInstance();
			}
           
            //ClearInfo();

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




    #endregion



    #region Socket

    bool isMing;
    bool isPlayOver;
	private int roundnumber;

    public void PollingPeriods(JsonData jd)
    {
		
		inningText.text = jd["periods"].ToString();
		roundText.text = jd["season"].ToString();
		clockText.text = jd["countdown"].ToString();
		if (roundnumber == 0) {
			roundnumber = int.Parse (jd ["season"].ToString ());
			StartCoroutine(GetHistory(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.winHistory +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame
			));
		} else {
			roundnumber = int.Parse (jd ["season"].ToString ());
		}


		try {
			//ItemRotate.winnings = jd ["winnings"].ToString ();
		} catch (Exception ex) {
			
		}

        //if (jd["is_empty"].ToString() == "1")
        //{
        //    ClearInfo();
        //    ItemRotate.ResetItem();
        //}

        try
        {
            if (jd["violent"].ToString() == "1")
            {
                IsBaoji = true;
                baoji.SetActive(true);
            }
            else
            {
                IsBaoji = false;
                baoji.SetActive(false);
            }
        }
        catch (Exception)
        {
           
        }

        


        if (jd["is_win"].ToString() == "0")
        {
            LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
            if (resend != 0)
            {
                resend = 0;
            }
            if (!isFirstJoin)
            {
                kaiJiang.SetActive(false);
                isFirstJoin = true;
            }
            if (jd["countdown"].ToString() != "0" && !isPlayOver)
            {
                ClearWinInfo();
                if (!IsBaoji)
                {
                    //Debug.Log("更新路单");
                    StartCoroutine(GetHistory(
               LoginInfo.Instance().mylogindata.URL +
               LoginInfo.Instance().mylogindata.winHistory +
               "game_id=" + LoginInfo.Instance().mylogindata.choosegame
           ));
                }
                Audiomanger._instenc.PlayTip(0);
                isPlayOver = true;
                ItemRotate1.IsWin = false;
                ItemRotate1.ResetItem();
            }
            else if (jd["countdown"].ToString() == "0" && isPlayOver)
            {
                Audiomanger._instenc.PlayTip(1);
                fengPan.SetActive(true);
                isPlayOver = false;
                //跑灯在这开始
                //ItemRotate1.MoveStart();

                //ItemRotate.SetOrigin();
                //ItemRotate.SetInterval = 0.01f;
                //ItemRotate.IsTurn = true;
                //ItemRotate.IsNotFirst = true;


            }
            if (jd["winnings"].ToString() != "" && !isMing)
            {
                //得到结果【原扑克牌显示可能要废弃掉】
                //switch (jd["winnings"].ToString() )
                //{
                //    case "0":

                //        winPoker.sprite = winPokerSprite[0];

                //        break;
                //    case "1":

                //        winPoker.sprite = winPokerSprite[1];

                //        break;
                //    case "2":

                //        winPoker.sprite = winPokerSprite[2];
                //        break;

                //}

                //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(-315f, 3f);

                //跑灯减速的时机点  //确定得到结果
                fengPan.SetActive(true);
                ItemRotate1.SetPos(int.Parse(jd["x"].ToString()),int.Parse(jd["y"].ToString()),int.Parse(jd["winnings"].ToString()));
                //ItemRotate.Ludan = jd["winnings"].ToString();
                isMing = true;
            }
        }
        else if(jd["is_win"].ToString() == "1")
        {
            fengPan.SetActive(true);
            clockText.text = jd["countdown"].ToString();
            if (!isMing)
            {
                //在跑灯很久后中途进入
                if (jd["winnings"]!=null&& jd["y"]!=null)
                {
                    if (jd["y"].ToString() != "")
                    {
                        try
                        {
                            ItemRotate1.GetEnd(int.Parse(jd["y"].ToString()));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    //ItemRotate.Ludan = jd["winnings"].ToString();
                   
                }
            }
        }
        else if (jd["is_win"].ToString() == "2" && resend != 1)
        {
            //开奖
            if (jd["winnings"].ToString() == "")
            {
                fengPan.SetActive(true);
                clockText.text = jd["countdown"].ToString();
                //未得到开奖结果
            }
            else
            {
                //得到开奖结果
                fengPan.SetActive(false);
                if (isFirstJoin)
                {
                    //第一次接受显示中奖得分并更新当前游戏数据
                    OnWin(jd);
                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                    clockText.text = jd["countdown"].ToString();
//                    inningText.text = jd["periods"].ToString();
//                    roundText.text = jd["season"].ToString();
//					roundnumber = int.Parse (jd["season"].ToString());
                    resend = 1;

                    if (jd["y"].ToString() != "")
                    {
                        try
                        {
                            ItemRotate1.GetEnd(int.Parse(jd["y"].ToString()));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    //ItemRotate.winnings = jd ["winnings"].ToString ();
                    //ItemRotate.RandomPos(jd["winnings"].ToString());
                    //ItemRotate.IsLightning = true;
                }
                else
                {
                    //非第一次显示出开奖状态并更新当前游戏数据
                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                    clockText.text = jd["countdown"].ToString();
                    if (jd["y"].ToString() != "")
                    {
                        try
                        {
                            ItemRotate1.GetEnd(int.Parse(jd["y"].ToString()));
                        }
                        catch (Exception)
                        {

                        }
                    }
                    //inningText.text = jd["periods"].ToString();
                    //roundText.text = jd["season"].ToString();
                    //roundnumber = int.Parse (jd["season"].ToString());
                    kaiJiang.SetActive(true);
                    resend = 1;
                }
            }
		}else if(jd["is_win"].ToString() == "2" && resend==1)
		{
            //读取结果
            if (ItemRotate1.IsWin)
            {
                StartCoroutine(GetHistory(
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winHistory +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));
            } else if (IsBaoji)
            {
                StartCoroutine(GetHistory(
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winHistory +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));
            }
		}

    }

    public void GetHis()
    {
        StartCoroutine(GetHistory(
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winHistory +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));
    }


    //得到
    public void UpdateSuit(JsonData jd)
    {
        if (betId.Count == 3)
        {
            for (int i = 0; i < jd["oddlist"].Count; i++)
            {
                betId[i]=jd["oddlist"][i]["id"].ToString();
                suitRateText[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
                suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
                suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();
                if (i == 1)
                {
                    heXian.text = jd["oddlist"][i]["single_limit"].ToString();
                }
            }
        }
        else
        {
            betId.Clear();
            for (int i = 0; i < jd["oddlist"].Count; i++)
            {
                betId.Add(jd["oddlist"][i]["id"].ToString());
                suitRateText[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
                suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
                suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();
                if (i == 1)
                {
                    heXian.text = jd["oddlist"][i]["single_limit"].ToString();
                }
            }
        }
    }
    /*只是为了在轮询里使用*/
    bool isFirstJoin;
    int resend;

    public void OddList(JsonData jd)
    {
        for (int i = 0; i < jd["Oddlist"].Count; i++)
        {
            suitInAllText[i].text = jd["Oddlist"][i].ToString();
        }
    }


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

    IEnumerator GetBetNum()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL + "room-odds-limit" + "?room_id=" + LoginInfo.Instance().mylogindata.room_id + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame);
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

            if (jd["code"].ToString() == "200")
            {
                betId.Clear();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < jd["oddsData"].Count; j++)
                    {
                        switch (i)
                        {
                            case 0: //2
                                if (jd["oddsData"][j]["tp"].ToString() == "2")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 1: //0
                                if (jd["oddsData"][j]["tp"].ToString() == "0")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 2: //8
                                if (jd["oddsData"][j]["tp"].ToString() == "8")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine("GetBetNum");
            }
        }
    }

    IEnumerator TurntaleStart()
    {

        yield return null;
    }





}
