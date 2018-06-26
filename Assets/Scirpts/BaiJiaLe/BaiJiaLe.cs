using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaiJiaLe : MonoBehaviour
{
    #region  UP
    public Text idText;
    public Text goldText;
    public Text zhuangXianText;
    public Text heXianText;
    public Text minScoreText;
    public GameObject resultPanelGo;
    public Text noticeText;


    /* 需要用到的一些东西*/


    public Sprite[] resultPointImage;


    #endregion

    /*-------------------------------------------------------------*/


    #region  Center
    public Image pokerUp;
    public Image pokerDownImage;
    public GameObject resultCenterGo;
    public Text juShuText;
    public Text roundText;
    public GameObject infoPanelGo;
    public GameObject errorPanel;
    public GameObject showWining;
    public GameObject fengPanelTip;



    /*需要用到的一些东西  */

    public Sprite[] resultSpriteCenter;
    int resultCount;
    public Sprite[] pokerDownSprite;


    #endregion




    /*-------------------------------------------------------------*/


    #region left
    public Text clockText;
    #endregion


    /*--------------------------------------------------------------*/


    #region right
    #endregion

    /*---------------------------------------------------------------*/


    #region  Down
    public List<Text> suitInSelfText;
    public List<Text> suitInAllText;
    public List<Text> suitCountText;
    public List<Text> suitRatetext;
    public List<Button> suitBetBtn;
    public Image changeCoinImage;
    public Button passBtn;
    public GameObject[] lightImage;


    /*Down 需要用到的一些东西 */
    public Sprite[] changeSprite;
    int changeNum; //判断当前是切换到多少下注
    #endregion


    #region Other
    bool isFirstBet;
    public GameObject errorGoParent;
    public GameObject errorGo;
    GameObject oldErrorGo;
    List<string> betId;

    public static BaiJiaLe instance;
    public NewTcpNet tcpNet;
    private bool isShowMenu;
    public GameObject menuGo;

    #endregion
    void Awake()
    {
        instance = this;

    }

    // Use this for initialization
    void Start()
    {
        tcpNet = NewTcpNet.GetInstance();
        StartCoroutine(ShowLoading());
        Init();
        Addlistener();
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
        //tcpNet = NewTcpNet.GetInstance();


        Application.targetFrameRate = 30;
        isPlayOver = false;
        line = 0;
        row = 0;
        lastPoint = 2;
        zhuangXianText.text = LoginInfo.Instance().mylogindata.roomlitmit;
        minScoreText.text = LoginInfo.Instance().mylogindata.roomcount;
        idText.text = "id:" + LoginInfo.Instance().mylogindata.user_id;
        betId = new List<string>();
        for (int i = 0; i < suitCountText.Count; i++)
        {
            suitCountText[i].text = "0";
        }
        changeNum = 1;
        LoginInfo.Instance().mylogindata.coindown = 10;
        changeCoinImage.sprite = changeSprite[changeNum];
        isFirstBet = true;
    }
    void Addlistener()
    {
        changeCoinImage.gameObject.GetComponent<Button>().onClick.AddListener(OnChange); //切换下注额度
        for (int i = 0; i < suitBetBtn.Count; i++)
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



    #region UnityWebRequest
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
           LoginInfo.Instance().mylogindata.betDown_bl +
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
                    suitInAllText[num].text = (int.Parse(suitInAllText[num].text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
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
                for (int i = 0; i < suitInAllText.Count; i++)
                {

                    suitInAllText[i].text = (int.Parse(suitInAllText[i].text) - (int.Parse(suitInSelfText[i].text))).ToString();
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


    #endregion




    #region 监听






    void OnClickPass()
    {
        StartCoroutine(OnSendPass
            (
            LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.betCancel_bl +
            "room_id=" +
            LoginInfo.Instance().mylogindata.room_id +
            "&user_id=" +
            LoginInfo.Instance().mylogindata.user_id +
            "&drop_date=" +
            LoginInfo.Instance().mylogindata.dropContent
            ));
    }




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
                changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 1:
                LoginInfo.Instance().mylogindata.coindown = 10;
                changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 2:
                LoginInfo.Instance().mylogindata.coindown = 100;
                changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 3:
                LoginInfo.Instance().mylogindata.coindown = 500;
                changeCoinImage.sprite = changeSprite[changeNum];
                break;
        }

    }

    void BetDown(int num)
    {
        StartCoroutine(OnBetDown(num));
        Audiomanger._instenc.clickvoice();
    }

    #endregion









    #region Other

    void ShowOtherMess(string str)
    {
        errorPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
        errorPanel.SetActive(true);
    }

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


    /// <summary>
    /// 清除牌
    /// </summary>
    void ClearInfo()
    {
        resultCount = 0;
        for (int i = 0; i < resultCenterGo.transform.childCount; i++)
        {
            resultCenterGo.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < suitCountText.Count; i++)
        {
            suitCountText[i].text = "0";
        }
        for (int i = 0; i < resultPanelGo.transform.childCount; i++)
        {
            resultPanelGo.transform.GetChild(i).gameObject.SetActive(false);
        }
        line = 0;
        row = 0;
    }


    /// <summary>
    /// 添加牌    
    /// 
    /// 后台传过来的是  庄0  闲1  和2
    /// 前端是    庄0   闲2   和1
    /// </summary>
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
                resultCenterGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSpriteCenter[0];
                resultCenterGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCountText[0].text = (Convert.ToInt32(suitCountText[0].text.ToString()) + 1).ToString();
                break;
            case "1":
                resultCenterGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSpriteCenter[2];
                resultCenterGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCountText[2].text = (Convert.ToInt32(suitCountText[2].text.ToString()) + 1).ToString();
                break;
            case "2":
                resultCenterGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = resultSpriteCenter[1];
                resultCenterGo.transform.GetChild(resultCount).gameObject.SetActive(true);
                suitCountText[1].text = (Convert.ToInt32(suitCountText[1].text.ToString()) + 1).ToString();
                break;
        }
        resultCount++;

        AddPokerPoint(str);
    }

    int line;  //行    最大值为6
    int row;   //列    最大值为48
    int lastPoint; //上一个点

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
            resultPanelGo.transform.GetChild(0).GetComponent<Image>().sprite = resultPointImage[pointSever];
            resultPanelGo.transform.GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;

        }
        else if (lastPoint == pointSever)
        {

            resultPanelGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = resultPointImage[pointSever];
            resultPanelGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
            lastPoint = pointSever;
        }
        else if (lastPoint != pointSever)
        {
            if (pointSever == 2)
            {
                resultPanelGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = resultPointImage[pointSever];
                resultPanelGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
            }
            else
            {
                if (line != 0)
                {
                    line = 0;
                    row++;
                    if (row > 48)
                    {

                        for (int i = 0; i < resultPanelGo.transform.childCount; i++)
                        {
                            resultPanelGo.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        line = 0;
                        row = 0;

                    }
                }

                resultPanelGo.transform.GetChild((line + (row * 6))).GetComponent<Image>().sprite = resultPointImage[pointSever];
                resultPanelGo.transform.GetChild((line + (row * 6))).gameObject.SetActive(true);
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

                for (int i = 0; i < resultPanelGo.transform.childCount; i++)
                {
                    resultPanelGo.transform.GetChild(i).gameObject.SetActive(false);
                }

                row = 0;

            }
        }
    }



    #endregion
    #region tcp

    public void UpdateSuit(JsonData jd)
    {
        for (int i = 0; i < jd["oddlist"].Count; i++)
        {
            betId.Add(jd["oddlist"][i]["id"].ToString());
            suitRatetext[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
            suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
            suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();
            if (i == 1)
            {
                heXianText.text = jd["oddlist"][i]["single_limit"].ToString();
            }

        }
    }

    /*   只是为了在轮询里使用*/

    bool isFirstJoin;
    int resend;
    bool isPlayOver;

    public void PollingPeriods(JsonData jd)
    {
        if (jd["is_empty"].ToString() == "1")
        {
            ClearInfo();
        }
        if (jd["is_win"].ToString() == "0")
        {
            LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
            juShuText.text = jd["periods"].ToString();
            roundText.text = jd["season"].ToString();
            clockText.text = jd["countdown"].ToString();
            if (resend != 0)
            {
                resend = 0;
            }
            if (!isFirstJoin)
            {
                showWining.SetActive(false);
                isFirstJoin = true;
            }
            if (jd["countdown"].ToString() != "0" && !isPlayOver)
            {
                Audiomanger._instenc.PlayTip(0);
                isPlayOver = true;
            }
            else if (jd["countdown"].ToString() == "0" && isPlayOver)
            {
                Audiomanger._instenc.PlayTip(1);
                fengPanelTip.SetActive(true);
                isPlayOver = false;

            }
        }
        else if (jd["is_win"].ToString() == "1")
        {
            fengPanelTip.SetActive(true);
            clockText.text = jd["countdown"].ToString();
        }
        else if (jd["is_win"].ToString() == "2" && resend != 1)
        {
            if (jd["winnings"].ToString() == "")
            {
                fengPanelTip.SetActive(true);
                clockText.text = jd["countdown"].ToString();
            }
            else
            {
                if (isFirstJoin)
                {
                    fengPanelTip.SetActive(false);
                    OnWin(jd);
                    resend = 1;
                }
                else
                {
                    fengPanelTip.SetActive(false);
                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                    clockText.text = jd["countdown"].ToString();
                    juShuText.text = jd["periods"].ToString();
                    roundText.text = jd["season"].ToString();
                    showWining.SetActive(true);
                    resend = 1;
                }
            }


        }
    }

    void OnWin(JsonData jd)
    {
        clockText.text = jd["countdown"].ToString();
        if (jd["winnings"].ToString() != "")
        {
            pokerDownImage.sprite = pokerDownSprite[Convert.ToInt32(jd["winnings"].ToString())];
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));
            Audiomanger._instenc.PlayZhuangXianWin(Convert.ToInt32(jd["winnings"].ToString()));
            AddPoker(jd["winnings"].ToString());
        }
    }

    public void OnOddList(JsonData jd)
    {
        for (int i = 0; i < jd["Oddlist"].Count; i++)
        {
            suitInAllText[i].text = jd["Oddlist"][i].ToString();
        }
    }

    private void OnApplicationQuit()
    {
        tcpNet.SocketQuit();
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


    IEnumerator PokerAni(int num)
    {
        Tween tw = pokerUp.GetComponent<RectTransform>().DOAnchorPosY(-315f, 3f);
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
        Tween tw = pokerUp.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
        yield return new WaitForSeconds(3.1f);
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

    /// <summary>
    /// 结束后清空信息
    /// </summary>
    void ClearWinInfo()
    {
        for (int i = 0; i < suitInAllText.Count; i++)
        {
            suitInAllText[i].text = "0";
            suitInSelfText[i].text = "0";


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