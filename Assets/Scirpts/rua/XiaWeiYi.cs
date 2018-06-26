using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XiaWeiYi : MonoBehaviour
{

    #region UP
    public Text idText;
    public Text goldText;
    public Text LongHuXian;
    //public Text heXian;
    public Text minScore;

    #endregion

    #region Center
    public GameObject pointGo;
    public Text inningText;
    public Text roundText;
    public Image winPoker;
    public Image pokerBack;
    public GameObject fengPan;
    public GameObject kaiJiang;
    public GameObject resultGo;
    public GameObject infoPanelGo;
    public GameObject errorPanel;

    /*  用到的一些  */
    public Sprite[] resultSprite;
    public Sprite[] pointSprite;
    public Sprite[] winPokerSpriteRed;
    public Sprite[] winPokerSpriteGreen;
    int resultCount;


    #endregion

    #region right
    public Text clockText;

    #endregion

    #region Down
    public Text[] suitInAllText;
    public Text[] suitInSelfText;
    public Text[] suitCount;
    public Text[] suitRateText;
    public Button[] suitBetBtn;
    public GameObject[] lightImage;
    public Button changeBetBtn;
    public Button passBtn;

    /*  用到的一些   */

    public Sprite[] betCoinSprite;
    List<string> betId;
    int changeNum;
    bool isFirstBet;
    #endregion

    #region Right
    public Button plusBtn;
    public NewTcpNet tcpNet;
    #endregion



    #region Other
    public static XiaWeiYi instance;
    int line;
    int row;
    int lastPoint;
    GameObject oldErrorGo;
    public GameObject errorGo;
    public GameObject errorGoParent;
    #endregion


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
    }

    void Init()
    {
        Application.targetFrameRate = 30;
        winPoker.sprite = winPokerSpriteGreen[0];
        resultCount = 0;
        line = 0;
        row = 0;
        lastPoint = 2;
        LongHuXian.text = LoginInfo.Instance().mylogindata.roomlitmit;
        minScore.text = LoginInfo.Instance().mylogindata.roomcount;
        idText.text = "id:" + LoginInfo.Instance().mylogindata.user_id;
        betId = new List<string>();
        changeNum = 1;
        LoginInfo.Instance().mylogindata.coindown = 10;
        changeBetBtn.gameObject.GetComponent<Image>().sprite = betCoinSprite[changeNum];
        isFirstBet = true;

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
    }

    void OnClickPass()
    {
        StartCoroutine(OnSendPass
            (
            LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.betCancel_xwy +
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
           LoginInfo.Instance().mylogindata.betDown_xwy +
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
                ShowMessageToShow("下注失败！");
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
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < jd["ArrList"].Count; i++)
                {
                    AddPoker(jd["ArrList"][i]["winnings"].ToString());
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
            pointGo.transform.GetChild(i).gameObject.SetActive(false);
        }
        line = 0;
        row = 0;
    }


    void OnWin(JsonData jd)
    {
        clockText.text = jd["countdown"].ToString();
        if (jd["winnings"].ToString() != "")
        {
            //winPoker.sprite = winPokerSprite[Convert.ToInt32(jd["winnings"].ToString())];

            //ChangeWinCoin(jd["winnings"].ToString());
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));
            AddPoker(jd["winnings"].ToString());
        }
    }


    ///// <summary>
    ///// 数组  280
    ///// </summary>
    ///// <param name="str"></param>
    //void ChangeWinCoin(string str)
    //{
    //    switch (str)
    //    {
    //        case "0":
    //            winPoker.sprite = winPokerSprite[0];
    //            break;
    //        case "1":
    //            winPoker.sprite = winPokerSprite[1];
    //            break;

    //    }
    //}














    IEnumerator PokerAni(int num)
    {
        //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(-315f, 3f);
        //yield return new WaitForSeconds(3.2f);   
        if (num == 0)
        {
            for (int i = 0; i < winPokerSpriteRed.Length; i++)
            {
                winPoker.sprite = winPokerSpriteRed[i];
                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            for (int i = 0; i < winPokerSpriteGreen.Length; i++)
            {
                winPoker.sprite = winPokerSpriteRed[i];
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(GetWinInfo
            (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winInfo +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                "&drop_date=" + LoginInfo.Instance().mylogindata.dropContent +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&user_id=" + LoginInfo.Instance().mylogindata.user_id
            ));
        StartCoroutine(PokerAniBack(num));
        StartCoroutine(SuitLight(num));

    }
    IEnumerator PokerAniBack(int num)
    {
        yield return new WaitForSeconds(4f);
        //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
        //yield return new WaitForSeconds(3.1f);
        //pokerBack.transform.GetComponent<RectTransform>().anchoredPosition = backPokerVec;
        if (num == 0)
        {
            for (int i = winPokerSpriteRed.Length - 1; i <= 0; i--)
            {
                winPoker.sprite = winPokerSpriteRed[i];
                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            for (int i = winPokerSpriteGreen.Length - 1; i <= 0 ; i--)
            {
                winPoker.sprite = winPokerSpriteRed[i];
                yield return new WaitForSeconds(0.2f);
            }
        }
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
    /// <param name="str"></param>
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
                suitCount[2].text = (Convert.ToInt32(suitCount[2].text.ToString()) + 1).ToString();
                break;
            case "1":
                resultGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSprite[1];
                resultGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCount[0].text = (Convert.ToInt32(suitCount[0].text.ToString()) + 1).ToString();
                break;
            case "2":
                resultGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSprite[2];
                resultGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCount[1].text = (Convert.ToInt32(suitCount[1].text.ToString()) + 1).ToString();
                break;
        }
        resultCount++;

        AddPokerPoint(str);
    }



    void AddPokerPoint(string str)
    {



        AddPointRule(Convert.ToInt32(str));


    }


    /// <summary>
    /// 添加点的规则
    /// </summary>
    void AddPointRule(int pointSever)
    {

        if (lastPoint == 2) //则代表为第一次
        {
            pointGo.transform.GetChild(0).GetComponent<Image>().sprite = pointSprite[pointSever];
            pointGo.transform.GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;

        }
        else if (lastPoint == pointSever)
        {

            pointGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = pointSprite[pointSever];
            pointGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
            lastPoint = pointSever;
        }
        else if (lastPoint != pointSever)
        {
            if (pointSever == 2)
            {
                pointGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = pointSprite[pointSever];
                pointGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
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
                            pointGo.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        line = 0;
                        row = 0;

                    }
                }

                pointGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = pointSprite[pointSever];
                pointGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
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
                    pointGo.transform.GetChild(i).gameObject.SetActive(false);
                }

                row = 0;

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

    public void PollingPeriods(JsonData jd)
    {

        if (jd["is_empty"].ToString() == "1")
        {
            ClearInfo();
        }

        if (jd["is_win"].ToString() == "0")
        {
            LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
            inningText.text = jd["periods"].ToString();
            roundText.text = jd["season"].ToString();
            clockText.text = jd["countdown"].ToString();
            if (resend != 0)
            {
                resend = 0;
            }
            if (!isFirstJoin)
            {
                kaiJiang.SetActive(false);
                isFirstJoin = true;
            }
        }
        else if (jd["is_win"].ToString() == "1")
        {
            fengPan.SetActive(true);
            clockText.text = jd["countdown"].ToString();
        }
        else if (jd["is_win"].ToString() == "2" && resend != 1)
        {
            if (jd["winnings"].ToString() == "")
            {
                fengPan.SetActive(true);
                clockText.text = jd["countdown"].ToString();
            }
            else
            {
                fengPan.SetActive(false);
                if (isFirstJoin)
                {

                    OnWin(jd);
                    resend = 1;

                }
                else
                {
                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                    clockText.text = jd["countdown"].ToString();
                    inningText.text = jd["periods"].ToString();
                    roundText.text = jd["season"].ToString();
                    kaiJiang.SetActive(true);
                    resend = 1;
                }
            }
        }

    }

    public void UpdateSuit(JsonData jd)
    {
        for (int i = 0; i < jd["oddlist"].Count; i++)
        {
            betId.Add(jd["oddlist"][i]["id"].ToString());
            suitRateText[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
            suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
            suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();
            //if (i == 1)
            //{
            //    heXian.text = jd["oddlist"][i]["single_limit"].ToString();
            //}

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
}
