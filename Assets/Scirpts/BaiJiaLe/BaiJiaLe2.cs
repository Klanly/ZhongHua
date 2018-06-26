using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;


public class BaiJiaLe2 : MonoBehaviour
{
    public int[] playerpos =new int[14];

    private string MeID;                           //玩家自己的ID

    public Text idText;                         //id的文本
    public Text goldText;                       //金币的文本
    public Text zhuangXianText;                 //全台限红
    public Text heXianText;                     //和最高限注
    public Text minScoreText;                   //最低投注

    public GameObject resultPanelGo;              //结果记录1
    public Sprite[] resultPointImage;             //记录1使用到的图片素材

    public Text noticeText;                      //公告（目前为空，未被使用）

    public Sprite[] PokerSprite;                //扑克牌图片集合


    public List<Image> Zhuangpokers;
    public List<Image> Xianpokers;
    public GameObject ZhuangNumberText;
    public GameObject XianNunberText;


    public GameObject resultCenterGo;           //结果记录2
    public Sprite[] resultSpriteCenter;         //记录2使用到的图片素材

    private bool IsGetPoker;
    private bool IsGetPoint;

    public Text juShuText;                      //局数
    public Text roundText;                      //轮数
    public GameObject infoPanelGo;              //本轮投注得分结果提示框
	public GameObject infoPanelGo2;              //本轮投注得分结果提示框
    public GameObject errorPanel;               //错误提示（账号异地登录）
    public GameObject showWining;               //开奖中提示
    public GameObject fengPanelTip;             //封盘中提示

   
    int resultCount;                            //
    public Sprite[] pokerDownSprite;            //庄赢闲赢和赢标志


    public GameObject ChipsState;               //玩家下注信息

    public Text clockText;                      //倒计时


    public List<GameObject> XianYaFen;          //闲压分位置集合
    public List<GameObject> ZhuangYaFen;        //庄压分位置集合
    public List<GameObject> HeYaFen;            //和压分位置集合

    public Text ZhuangYF;
    public Text XianYF;
    public Text HeYF;

    //public List<Text> suitInSelfText;
    //public List<Text> suitInAllText;
    //public List<Text> suitCountText;
    //public List<Text> suitRatetext;


    public List<Button> suitBetBtn;             //玩家庄闲和下注按钮集合
    public GameObject changeBet;                //压分下注按钮
    public Button HideScoreBtn;                 //隐藏压分按钮
    public Button passBtn;                      //取消压分按钮
    public Button MoreBtn;                      //更多选项按钮
    public Button RecordBtn;                    //记录按钮
    public Animator PlayerImage;                //人物角色       

    public GameObject MaskPlace;                //遮挡界面

    public Sprite[] changeSprite;
    int changeNum; //判断当前是切换到多少下注

    public GameObject Handsel_GetType;         //彩金中奖提示
    public GameObject ShowHandsel;             //显示彩金值
    public Sprite[] HandselSprite;              //彩金用的数字图片集合

    bool isFirstBet;
    public GameObject errorGoParent;
    public GameObject errorGo;
    GameObject oldErrorGo;
    List<string> betId;

    public static BaiJiaLe2 instance;
    public NewTcpNet tcpNet;
    private bool isShowMenu;
    public GameObject menuGo;


	private Thread testThread;


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



    //void Update()
    //{
    //    Debug.Log("闲："+X_number+"||庄："+Z_number+"????闲总："+XC_number+"||庄总："+ZC_number);
    //}

    //初始化
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
        MeID = LoginInfo.Instance().mylogindata.user_id;
        betId = new List<string>();
       
        //初始化下注位置【闲】
        for (int i = 0; i < XianYaFen.Count; i++)
        {
            XianYaFen[i].SetActive(false);
        }
        //初始化下注位置【庄】
        for (int i = 0; i < ZhuangYaFen.Count; i++)
        {
            ZhuangYaFen[i].SetActive(false);
        }
        //初始化下注位置【和】
        for (int i = 0; i < HeYaFen.Count; i++)
        {
            HeYaFen[i].SetActive(false);
        }

        Handsel_GetType.SetActive(false);

        //隐藏玩家座位上的东西
        PlayerStateClear();

        for (int i = 0; i < playerpos.Length; i++)
        {
            playerpos[i] = 0;
        }

        XianYF.text = "0";
        ZhuangYF.text = "0";
        HeYF.text = "0";

        if (!IsGetPoker)
        {
            //隐藏扑克牌
            for (int i = 0; i < Zhuangpokers.Count; i++)
            {
                Zhuangpokers[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < Xianpokers.Count; i++)
            {
                Xianpokers[i].gameObject.SetActive(false);
            }

            if (!IsGetPoint)
            {
                ZhuangNumberText.SetActive(false);
                XianNunberText.SetActive(false);
            }
         
        }

       

        //for (int i = 0; i < suitCountText.Count; i++)
        //{
        //    suitCountText[i].text = "0";
        //}

        changeNum = 1;
        LoginInfo.Instance().mylogindata.coindown = 10;
        changeBet.GetComponent<Image>().sprite = changeSprite[changeNum];
        //设置下注额度图片
        //changeCoinImage.sprite = changeSprite[changeNum];
        isFirstBet = true;
    }
    void Addlistener()
    {
        //changeCoinImage.gameObject.GetComponent<Button>().onClick.AddListener(OnChange); //切换下注额度
        changeBet.gameObject.GetComponent<Button>().onClick.AddListener(OnChange);
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


        HideScoreBtn.onClick.AddListener(SetHideScore);
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
					try {
						LoginInfo.Instance().mylogindata.ALLScroce = jd["quick_credit"].ToString();
						goldText.text = LoginInfo.Instance().mylogindata.ALLScroce;
						//压分信息【庄】
						if (jd["BetList"]["drop_content"].ToString().Equals("A"))
						{
							ZhuangYF.text =(int.Parse(ZhuangYF.text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
							for (int i = 0; i < playerpos.Length; i++)
							{
								if (playerpos[i] == int.Parse(MeID))
								{
									ZhuangYaFen[i].gameObject.SetActive(true);
									ZhuangYaFen[i].transform.GetChild(0).GetComponent<Text>().text = ZhuangYF.text;
									break;
								}
							}
						}

						//压分信息【和】
						if (jd["BetList"]["drop_content"].ToString().Equals("B"))
						{
							HeYF.text = (int.Parse(HeYF.text)+int.Parse(jd["BetList"]["num"].ToString())).ToString();
							for (int i = 0; i < playerpos.Length; i++)
							{
								if (playerpos[i] == int.Parse(MeID))
								{
									HeYaFen[i].gameObject.SetActive(true);
									HeYaFen[i].transform.GetChild(0).GetComponent<Text>().text = HeYF.text;
									break;
								}
							}
						}

						//压分信息【闲】
						if (jd["BetList"]["drop_content"].ToString().Equals("C"))
						{
							XianYF.text = (int.Parse(XianYF.text)+int.Parse(jd["BetList"]["num"].ToString())).ToString();
							for (int i = 0; i < playerpos.Length; i++)
							{
								if (playerpos[i] == int.Parse(MeID))
								{
									XianYaFen[i].gameObject.SetActive(true);
									XianYaFen[i].transform.GetChild(0).GetComponent<Text>().text = XianYF.text;
									break;
								}
							}
						}
					} catch (Exception ex) {
						
					}
                    

                    //suitInSelfText[num].text = (int.Parse(suitInSelfText[num].text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                    //suitInAllText[num].text = (int.Parse(suitInAllText[num].text) + int.Parse(jd["BetList"]["num"].ToString())).ToString();
                }
                else
                {
                    //Debug.LogError(jd["code"].ToString());
                    try
                    {
                        ShowMessageToShow(jd["msg"].ToString());
                    }
                    catch (Exception)
                    {

                        //ShowMessageToShow("压分失败");
                    }

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
                if (roundnumber >= jd["ArrList"].Count)
                {
                    ResetResult();
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



    IEnumerator GetWinInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log ("结果"+url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
//				infoPanelGo.SetActive(true);
//                infoPanelGo.transform.GetChild(1).GetComponent<Text>().text = "本局所得分数：" + jd["WinTotal"].ToString();

				infoPanelGo2.SetActive (true);
				infoPanelGo2.transform.GetChild (0).GetComponent<Text> ().text = ((int)float.Parse(jd ["oddWinList"] ["A"].ToString ())).ToString ();
				infoPanelGo2.transform.GetChild (1).GetComponent<Text> ().text = ((int)float.Parse(jd ["oddWinList"] ["B"].ToString ())).ToString ();
				infoPanelGo2.transform.GetChild (2).GetComponent<Text> ().text = ((int)float.Parse(jd ["oddWinList"] ["C"].ToString ())).ToString ();
				infoPanelGo2.transform.GetChild (3).GetComponent<Text> ().text = ((int)float.Parse(jd ["userWinTotal"].ToString ())).ToString ();
//				infoPanelGo2.transform.GetChild (3).GetComponent<Text> ().text = jd["WinTotal"].ToString();


                //彩金公布
                SetHandsel();

                //if (jd["handsel"].ToString()!="0")
                //{
                //    infoPanelGo.transform.GetChild(1).GetComponent<Text>().text += "\n+恭喜你获得彩金："+jd["handsel"].ToString();
                //}

               
                yield return new WaitForSeconds(3);
                infoPanelGo.SetActive(false);
				infoPanelGo2.SetActive (false);
            }
        }
        yield return null;
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

                for (int i = 0; i < playerpos.Length; i++)
                {
                    if (playerpos[i]==int.Parse(MeID))
                    {
                        ZhuangYaFen[i].SetActive(false);
                        HeYaFen[i].SetActive(false);
                        XianYaFen[i].SetActive(false);

                        ZhuangYF.text = "0";
                        HeYF.text = "0";
                        XianYF.text = "0";

                        break;
                    }
                }


                //压分信息
                //for (int i = 0; i < suitInAllText.Count; i++)
                //{

                //    suitInAllText[i].text = (int.Parse(suitInAllText[i].text) - (int.Parse(suitInSelfText[i].text))).ToString();
                //    suitInSelfText[i].text = "0";
                //}
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
            Debug.Log("退出重进"+jd.ToJson().ToString());
            if (jd["code"].ToString() == "200")
            {

                //庄
                if (jd["Oddlist"][0]["user_dnum"].ToString()!="0")
                {
                    ZhuangYF.text =jd["Oddlist"][0]["user_dnum"].ToString();
                    for (int i = 0; i < playerpos.Length; i++)
                    {
                        if (playerpos[i]==int.Parse(MeID))
                        {
                            ZhuangYaFen[i].SetActive(true);
                            ZhuangYaFen[i].transform.GetChild(0).GetComponent<Text>().text = jd["Oddlist"][0]["user_dnum"].ToString();
                        }
                    }
                }

                //和
                if (jd["Oddlist"][1]["user_dnum"].ToString() != "0")
                {
                    ZhuangYF.text = jd["Oddlist"][1]["user_dnum"].ToString();
                    for (int i = 0; i < playerpos.Length; i++)
                    {
                        if (playerpos[i] == int.Parse(MeID))
                        {
                            ZhuangYaFen[i].SetActive(true);
                            ZhuangYaFen[i].transform.GetChild(0).GetComponent<Text>().text = jd["Oddlist"][1]["user_dnum"].ToString();
                        }
                    }
                }

                //闲
                if (jd["Oddlist"][2]["user_dnum"].ToString() != "0")
                {
                    ZhuangYF.text = jd["Oddlist"][2]["user_dnum"].ToString();
                    for (int i = 0; i < playerpos.Length; i++)
                    {
                        if (playerpos[i] == int.Parse(MeID))
                        {
                            ZhuangYaFen[i].SetActive(true);
                            ZhuangYaFen[i].transform.GetChild(0).GetComponent<Text>().text = jd["Oddlist"][2]["user_dnum"].ToString();
                        }
                    }
                }



                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    //压分信息
                    //suitInAllText[i].text = jd["Oddlist"][i]["dnum"].ToString();
                    //suitInSelfText[i].text = jd["Oddlist"][i]["user_dnum"].ToString();
                }
            }
        }
    }


    #endregion




    #region 监听

    


    void OnClickPass()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
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
                changeBet.GetComponent<Image>().sprite = changeSprite[changeNum];
                //下注额度
                //changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 1:
                LoginInfo.Instance().mylogindata.coindown = 10;
                changeBet.GetComponent<Image>().sprite = changeSprite[changeNum];
                //changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 2:
                LoginInfo.Instance().mylogindata.coindown = 100;
                changeBet.GetComponent<Image>().sprite = changeSprite[changeNum];
                //changeCoinImage.sprite = changeSprite[changeNum];
                break;
            case 3:
                LoginInfo.Instance().mylogindata.coindown = 500;
                changeBet.GetComponent<Image>().sprite = changeSprite[changeNum];
                //changeCoinImage.sprite = changeSprite[changeNum];
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
       
        //初始化下注位置【闲】
        for (int i = 0; i < XianYaFen.Count; i++)
        {
            XianYaFen[i].SetActive(false);
        }
        //初始化下注位置【庄】
        for (int i = 0; i < ZhuangYaFen.Count; i++)
        {
            ZhuangYaFen[i].SetActive(false);
        }
        //初始化下注位置【和】
        for (int i = 0; i < HeYaFen.Count; i++)
        {
            HeYaFen[i].SetActive(false);
        }

        if (!IsGetPoker)
        {
            //隐藏发出的牌【庄】
            for (int i = 0; i < Zhuangpokers.Count; i++)
            {
                Zhuangpokers[i].gameObject.SetActive(false);
            }
            //隐藏发出的牌【闲】
            for (int i = 0; i < Xianpokers.Count; i++)
            {
                Xianpokers[i].gameObject.SetActive(false);
            }
            if (!IsGetPoint)
            {
                ZhuangNumberText.SetActive(false);
                XianNunberText.SetActive(false);
            }
        }

        Handsel_GetType.SetActive(false);

        //for (int i = 0; i < suitCountText.Count; i++)
        //{
        //    suitCountText[i].text = "0";
        //}

//		resultCount = 0;
//		for (int i = 0; i < resultCenterGo.transform.childCount; i++)
//		{
//			resultCenterGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
//		}
//        for (int i = 0; i < resultPanelGo.transform.childCount; i++)
//        {
//            resultPanelGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
//        }
//        line = 0;
//        row = 0;
    }

	//重置记录
	public void ResetResult()
	{
		resultCount = 0;
		line = 0;
		row = 0;
		lastPoint = 2;
		for (int i = 0; i < resultCenterGo.transform.childCount; i++)
		{
			resultCenterGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
		}
		for (int i = 0; i < resultPanelGo.transform.childCount; i++)
		{
			resultPanelGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
		}

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
			ResetResult();
        }
        switch (str)
        {
            case "0":
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).GetComponent<Image>().sprite = resultSpriteCenter[0];
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).gameObject.SetActive(true);
                //压分信息
                //suitCountText[0].text = (Convert.ToInt32(suitCountText[0].text.ToString()) + 1).ToString();
                break;
            case "1":
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).GetComponent<Image>().sprite = resultSpriteCenter[2];
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).gameObject.SetActive(true);
                //压分信息
               // suitCountText[2].text = (Convert.ToInt32(suitCountText[2].text.ToString()) + 1).ToString();
                break;
            case "2":
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).GetComponent<Image>().sprite = resultSpriteCenter[1];
                resultCenterGo.transform.GetChild(resultCount).GetChild(0).gameObject.SetActive(true);
                //压分信息
                //suitCountText[1].text = (Convert.ToInt32(suitCountText[1].text.ToString()) + 1).ToString();
                break;
        }
        resultCount++;

        //AddPokerPoint(str);
    }

    int line;  //行    最大值为6
    int row;   //列    最大值为48
    int lastPoint; //上一个点

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
            resultPanelGo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = resultPointImage[pointSever];
            resultPanelGo.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;

        }
        else if (lastPoint == pointSever)
        {

            resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = resultPointImage[pointSever];
            resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;
        }
        else if (lastPoint != pointSever)
        {
            if (pointSever == 2)
            {
                resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = resultPointImage[pointSever];
                resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
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
                            resultPanelGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                        }
                        line = 0;
                        row = 0;

                    }
                }

                resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = resultPointImage[pointSever];
                resultPanelGo.transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
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
                    resultPanelGo.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
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
            resultPanelGo.transform.GetChild(pos).GetChild(0).gameObject.SetActive(true);
            try
            {
                resultPanelGo.transform.GetChild(pos).GetChild(0).GetComponent<Image>().sprite = resultPointImage[int.Parse(nownum)];
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

    #endregion
    #region tcp

    //接收初始化信息
    public void UpdateSuit(JsonData jd)
    {

		for (int i = 0; i < jd ["userData"].Count; i++) {
			playerpos [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1] = int.Parse (jd ["userData"] [i] ["user_id"].ToString ());

			PlayerStateUpdate (jd ["userData"] [i] ["seat_number"].ToString (), jd ["userData"] [i] ["user_id"].ToString (), jd ["userData"] [i] ["userInfo"] ["username"].ToString (), jd ["userData"] [i] ["userInfo"] ["quick_credit"].ToString ());
			// [int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1];


			//庄压分
			if (jd ["userData"] [i] ["A"] ["user_dnum"].ToString () != "0") {
				ZhuangYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].SetActive (true);
				ZhuangYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].transform.GetChild (0).GetComponent<Text> ().text = jd ["userData"] [i] ["A"] ["user_dnum"].ToString ();
			}

			//和压分
			if (jd ["userData"] [i] ["B"] ["user_dnum"].ToString () != "0") {
				HeYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].SetActive (true);
				HeYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].transform.GetChild (0).GetComponent<Text> ().text = jd ["userData"] [i] ["B"] ["user_dnum"].ToString ();
			}


			//闲压分
			if (jd ["userData"] [i] ["C"] ["user_dnum"].ToString () != "0") {
				XianYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].SetActive (true);
				XianYaFen [int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1].transform.GetChild (0).GetComponent<Text> ().text = jd ["userData"] [i] ["C"] ["user_dnum"].ToString ();
			}


            //int pos = int.Parse (jd ["userData"] [i] ["seat_number"].ToString ()) - 1;
            //遮挡判断
            //if (jd ["userData"] [i] ["is_score"].ToString () == "1") {
            //	MaskPlace.transform.GetChild (pos).gameObject.SetActive (true);
            //} else {
            //	MaskPlace.transform.GetChild (pos).gameObject.SetActive (false);
            //}

            //遮挡更新
            for (int j = 0; j < playerpos.Length; j++)
            {
                if (jd["userData"][i]["user_id"].ToString() == playerpos[j].ToString())
                {
                    int pos = int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1;
                    if (jd["userData"][i]["is_score"].ToString() == "1")
                    {
                        if (jd["userData"][i]["user_id"].ToString() == MeID)
                        {
                            Is_score = 1;
                        }
                        MaskPlace.transform.GetChild(pos).gameObject.SetActive(true);
                    }
                    else
                    {
                        if (jd["userData"][i]["user_id"].ToString() == MeID)
                        {
                            Is_score = 0;
                        }
                        MaskPlace.transform.GetChild(pos).gameObject.SetActive(false);
                    }
                    break;
                }

            }

            //自我遍历
            for (int j = 0; j < playerpos.Length; j++) {
				if (jd ["userData"] [i] ["user_id"].ToString () == MeID) {
					ZhuangYF.text = jd ["userData"] [i] ["A"] ["user_dnum"].ToString ();
					HeYF.text = jd ["userData"] [i] ["B"] ["user_dnum"].ToString ();
					XianYF.text = jd ["userData"] [i] ["C"] ["user_dnum"].ToString ();
				}
			}
        }

        //移除不存在于座位上的对象
        for (int i = 0; i < playerpos.Length; i++)
        {
            bool IsExist = false;
            for (int j = 0; j < jd["userData"].Count; j++)
            {

                if (playerpos[i].ToString() == jd["userData"][j]["user_id"].ToString())
                {
                    //该玩家还存在于座位上
                    IsExist = true;
                    break;
                }

            }
            if (!IsExist)
            {
                //该玩家已经不存在了
                playerpos[i] = 0;
                PLayerStateHide(i);
                HideScore(i);
            }
        }


        for (int i = 0; i < jd["oddlist"].Count; i++)
        {
            betId.Add(jd["oddlist"][i]["id"].ToString());

            //压分信息
            //suitRatetext[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
            //suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
            //suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();
            if (i == 1)
            {
                heXianText.text = jd["oddlist"][i]["single_limit"].ToString();
            }

        }
    }

    /*   只是为了在轮询里使用*/     

    bool isFirstJoin;
    int resend;
    bool isPlayOver=true;

    int Z_number, X_number;
    int ZC_number,XC_number;
    int count;
	private int roundnumber;

    public void PollingPeriods(JsonData jd)
    {
		juShuText.text = jd["periods"].ToString();
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

        try
        {
			
            if (jd["countdown"].ToString() == "0"&& jd["zpai_one"]!=null)
            {
                Z_number = 0;
                X_number = 0;
                ZC_number = 0;
                XC_number = 0;

                Z_number += BackVaslue(jd["zpai_one"].ToString());
                Z_number += BackVaslue(jd["zpai_two"].ToString());
                ZC_number = Z_number + BackVaslue(jd["zpai_three"].ToString());

                Z_number = Z_number % 10;
                ZC_number = ZC_number % 10;

                X_number += BackVaslue(jd["xpai_one"].ToString());
                X_number += BackVaslue(jd["xpai_two"].ToString());
                XC_number = X_number + BackVaslue(jd["xpai_three"].ToString());

                X_number = X_number % 10;
                XC_number = XC_number % 10;

                //刷新庄牌
                PokerSpriteUpdate(Zhuangpokers[0], jd["zpai_one"].ToString());
                PokerSpriteUpdate(Zhuangpokers[1], jd["zpai_two"].ToString());
                PokerSpriteUpdate(Zhuangpokers[2], jd["zpai_three"].ToString());

                //刷新闲牌
                PokerSpriteUpdate(Xianpokers[0], jd["xpai_one"].ToString());
                PokerSpriteUpdate(Xianpokers[1], jd["xpai_two"].ToString());
                PokerSpriteUpdate(Xianpokers[2], jd["xpai_three"].ToString());
            }

            if (!IsDeal && jd["zpai_one"] != null&& jd["countdown"].ToString() == "0"&&jd["is_win"].ToString()=="0")
            {

                IsGetPoker = true;
                IsDeal = true;


                count = 0;

                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                XianNunberText.transform.GetChild(0).GetComponent<Text>().color = Color.white;

                if (jd["zpai_three"].ToString() == "kong" && jd["xpai_three"].ToString() == "kong")
                {
                    StartCoroutine("Pai_Deal1");//4张
                    count = 0;
                }
                if (jd["zpai_three"].ToString() != "kong" && jd["xpai_three"].ToString() == "kong")
                {

                    StartCoroutine("Pai_Deal2");//5张  庄
                    count = 1;

                }
                if (jd["zpai_three"].ToString() == "kong" && jd["xpai_three"].ToString() != "kong")
                {
                    StartCoroutine("Pai_Deal3");//5张  闲
                    count = 2;
                }
                if (jd["zpai_three"].ToString() != "kong" && jd["xpai_three"].ToString() != "kong")
                {
                    StartCoroutine("Pai_Deal4");//6张
                    count = 6;
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError("这里报错了");
        }


        if (jd["is_empty"].ToString() == "1")
        {
            ClearInfo();
			ResetResult ();
        }
        if (jd["is_win"].ToString() == "0")
        {
            LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
//            juShuText.text = jd["periods"].ToString();
//            roundText.text = jd["season"].ToString();
//            clockText.text = jd["countdown"].ToString();
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
                HandselUpdate(jd["handsel_total"].ToString());
                if (IsGetPoker)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Zhuangpokers[i].gameObject.SetActive(false);
                        Xianpokers[i].gameObject.SetActive(false);
                    }
                    IsGetPoker = false;
                }

                if (IsGetPoint)
                {
                    ZhuangNumberText.SetActive(false);
                    XianNunberText.SetActive(false);
                    IsGetPoint = false;
                }
                //开始压分
                Audiomanger._instenc.PlayTip(0);
                isPlayOver = true;
                HidePoker();
                ClearWinInfo();
                IsDeal = false;
            }
            else if (jd["countdown"].ToString() == "0" && isPlayOver)
            {
                //停止压分
                Audiomanger._instenc.PlayTip(1);
                fengPanelTip.SetActive(true);
//                clockText.text = jd["countdown"].ToString();
                isPlayOver = false;

                StartCoroutine(GetHideScore
             (
                  LoginInfo.Instance().mylogindata.URL + "is-score?" +
                 "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                 "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                 "&user_id=" + LoginInfo.Instance().mylogindata.user_id +
                 "&is_score=" + 0
             ));

                for (int i = 0; i < MaskPlace.transform.childCount; i++)
                {
                    MaskPlace.transform.GetChild(i).gameObject.SetActive(false);
                }

               
            }


        }
        else if (jd["is_win"].ToString() == "1")
        {
            // fengPanelTip.SetActive(true);
            // clockText.text = jd["countdown"].ToString();
          //  Audiomanger._instenc.playXiannumber(XC_number);


        }
        else if (jd["is_win"].ToString() == "2" && resend != 1)
        {
            //ZhuangNumberText.SetActive(false);
            //XianNunberText.SetActive(false);

            if (jd["winnings"].ToString() == "")
            {
                fengPanelTip.SetActive(true);
//                clockText.text = jd["countdown"].ToString();
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
//                    clockText.text = jd["countdown"].ToString();
//                    juShuText.text = jd["periods"].ToString();
//                    roundText.text = jd["season"].ToString();
                    showWining.SetActive(true);
                    resend = 1;
                }
            }
		}else if(jd ["is_win"].ToString () == "2" && resend == 1){
			//持续获取结果
			StartCoroutine(GetHistory(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.winHistory +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame
			));
		}
    }

    void OnWin(JsonData jd)
    {
//        clockText.text = jd["countdown"].ToString();
        if (jd["winnings"].ToString() != "")
        {
            //pokerDownImage.sprite = pokerDownSprite[Convert.ToInt32(jd["winnings"].ToString())];
            //播放输赢音效
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));

            //停止发牌动画停止牌移动动画
            try
            {
                StopCoroutine("Pai_Deal1");
                StopCoroutine("Pai_Deal2");
                StopCoroutine("Pai_Deal3");
                StopCoroutine("Pai_Deal4");
                StopCoroutine("Pai_Deal");
                StopCoroutine("Pai_Move");
            }
            catch (Exception ex)
            {
                
            }

            ZhuangNumberText.SetActive(true);
            XianNunberText.SetActive(true);
            IsGetPoint = true;

            switch (count)
            {
                case 0:
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);
                    Xianpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    break;
                case 1:
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);
                    Xianpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    Zhuangpokers[2].gameObject.SetActive(true);
                    break;
                case 2:
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);
                    Xianpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    Xianpokers[2].gameObject.SetActive(true);
                    break;
                case 6:
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[2].gameObject.SetActive(true);
                    Xianpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    Xianpokers[2].gameObject.SetActive(true);
                    break;
            }



            if (ZC_number > XC_number)
            {
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().color = Color.red;
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄赢" + ZC_number + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + XC_number + "点";

            }
            else if (ZC_number < XC_number)
            {
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + ZC_number + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().color = Color.red;
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲赢" + XC_number + "点";
            }
            else
            {
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + ZC_number + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + XC_number + "点";
            }


            StartCoroutine(GetHistory(
             LoginInfo.Instance().mylogindata.URL +
             LoginInfo.Instance().mylogindata.winHistory +
             "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            ));

            // AddPoker(jd["winnings"].ToString());
        }
    }

    //接收下注更新
    public void OnOddList(JsonData jd)
    {
        Debug.Log(jd.ToJson().ToString());



        for (int i = 0; i < jd["userData"].Count; i++)
        {
            playerpos[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1] = int.Parse(jd["userData"][i]["user_id"].ToString());

            PlayerStateUpdate(jd["userData"][i]["seat_number"].ToString(), jd["userData"][i]["user_id"].ToString(), jd["userData"][i]["userInfo"]["username"].ToString(), jd["userData"][i]["userInfo"]["quick_credit"].ToString());

            //庄压分
            if (jd["userData"][i]["A"]["user_dnum"].ToString() != "0")
            {
                ZhuangYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].SetActive(true);
                ZhuangYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].transform.GetChild(0).GetComponent<Text>().text = jd["userData"][i]["A"]["user_dnum"].ToString();
            }

            //和压分
            if (jd["userData"][i]["B"]["user_dnum"].ToString() != "0")
            {
                HeYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].SetActive(true);
                HeYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].transform.GetChild(0).GetComponent<Text>().text = jd["userData"][i]["B"]["user_dnum"].ToString();
            }


            //闲压分
            if (jd["userData"][i]["C"]["user_dnum"].ToString() != "0")
            {
                XianYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].SetActive(true);
                XianYaFen[int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1].transform.GetChild(0).GetComponent<Text>().text = jd["userData"][i]["C"]["user_dnum"].ToString();
            }

            //遮挡更新
            for (int j = 0; j < playerpos.Length; j++)
            {
                if (jd["userData"][i]["user_id"].ToString() == playerpos[j].ToString())
                {
                    int pos = int.Parse(jd["userData"][i]["seat_number"].ToString()) - 1;
                    if (jd["userData"][i]["is_score"].ToString() == "1")
                    {
                        if (jd["userData"][i]["user_id"].ToString() == MeID)
                        {
                            Is_score = 1;
                        }
                        MaskPlace.transform.GetChild(pos).gameObject.SetActive(true);
                    }
                    else
                    {
                        if (jd["userData"][i]["user_id"].ToString() == MeID)
                        {
                            Is_score = 0;
                        }
                        MaskPlace.transform.GetChild(pos).gameObject.SetActive(false);
                    }
                    Debug.Log("第"+i+"个玩家"+Is_score);
                    break;
                }
               
            }

            //int pos = int.Parse(jd["userData"][i]["seat_number"].ToString())-1;
            //if (jd["userData"][i]["is_score"].ToString() == "1")
            //{
            //    Is_score = 1;
            //    MaskPlace.transform.GetChild(pos).gameObject.SetActive(true);
            //}
            //else
            //{
            //    Is_score = 0;
            //    MaskPlace.transform.GetChild(pos).gameObject.SetActive(false);
            //}


            //自我遍历
            for (int j = 0; j < playerpos.Length; j++)
            {
                if (jd["userData"][i]["user_id"].ToString() == MeID)
                {
                    ZhuangYF.text = jd["userData"][i]["A"]["user_dnum"].ToString();
                    HeYF.text = jd["userData"][i]["B"]["user_dnum"].ToString();
                    XianYF.text = jd["userData"][i]["C"]["user_dnum"].ToString();
                }
            }
        }

        //移除不存在于座位上的对象
        for (int i = 0; i < playerpos.Length; i++)
        {
            bool IsExist = false;
            for (int j = 0; j < jd["userData"].Count; j++)
            {

                if (playerpos[i].ToString() == jd["userData"][j]["user_id"].ToString())
                {
                    //该玩家还存在于座位上
                    IsExist = true;
                    break;
                }
                
            }
            if (!IsExist)
            {
                //该玩家已经不存在了
                playerpos[i] = 0;
                PLayerStateHide(i);
                HideScore(i);
            }
        }

    }


	public void GetOnPing()
	{
		StartCoroutine (OnWebGet(LoginInfo.Instance().mylogindata.URL+"update-seat?"
			+"game_id="+LoginInfo.Instance().mylogindata.choosegame
			+"&room_id="+LoginInfo.Instance().mylogindata.room_id
			+"&user_id="+LoginInfo.Instance().mylogindata.user_id));
	}

	IEnumerator OnWebGet(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send ();

		JsonData jd = JsonMapper.ToObject (www.downloadHandler.text);

		Debug.Log (jd.ToJson().ToString());

		if(jd["code"].ToString()=="200")
		{
			if (bool.Parse(jd ["bool"].ToString())) {
				//已超时需要离开房间
				UnityWebRequest www2 = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
					"room-end?"
					+ "user_id=" + LoginInfo.Instance().mylogindata.user_id
					+ "&game_id=" + LoginInfo.Instance().mylogindata.choosegame);
				yield return www2.Send ();
				NewTcpNet.IsKick = true;
				tcpNet.SocketQuit();
				DisconnectPanel.GetInstance ().Show ();
				DisconnectPanel.GetInstance ().Modification ("","长时间未操作，你已被移除房间");
			} else {
				//未超时 可以考虑刷新下房间座位

			}
		}
	}



    private void OnApplicationQuit()
    {
        tcpNet.SocketQuit();
    }

	//获得焦点
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
//            StartCoroutine(OnReGet
//                (
//                LoginInfo.Instance().mylogindata.URL +
//                LoginInfo.Instance().mylogindata.newInit +
//                "user_id=" + LoginInfo.Instance().mylogindata.user_id +
//                "&unionuid=" + LoginInfo.Instance().mylogindata.token +
//                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
//                "&game_id=" + LoginInfo.Instance().mylogindata.choosegame
//                ));
        }
    }

    //扑克牌动画协程--播放（待修改）
    IEnumerator PokerAni(int num)
    {
      
		Audiomanger._instenc.playXiannumber (XC_number);
		yield return new WaitForSeconds(1.5f);
		Audiomanger._instenc.playZhuangnumber (ZC_number);
		yield return new WaitForSeconds(1.5f);
		Audiomanger._instenc.PlayZhuangXianWin(num);
        StartCoroutine(GetWinInfo
            (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.winInfo +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                "&drop_date=" + LoginInfo.Instance().mylogindata.dropContent +
                "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                "&user_id=" + LoginInfo.Instance().mylogindata.user_id
            ));
       // StartCoroutine(PokerAniBack());
       // StartCoroutine(SuitLight(num));

    }


    //扑克牌动画协程--回收（待修改）
    IEnumerator PokerAniBack()
    {
        yield return new WaitForSeconds(4f);
        //Tween tw = pokerUp.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
       
        yield return new WaitForSeconds(3.1f);

        //pokerBack.transform.GetComponent<RectTransform>().anchoredPosition = backPokerVec;
      
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
            //中奖区域闪烁
            //lightImage[value].gameObject.SetActive(!acticveNow);
            //acticveNow = !acticveNow;
            //b++;
            //yield return new WaitForSeconds(0.4f);
            ////Debug.LogError("这里调用了" + b + "次");
            //if (b == 8)
            //{
            //    //Debug.LogError("已经超过了八次");
            //    lightImage[value].gameObject.SetActive(false);
            //    break;
            //}

        }
    }

    /// <summary>
    /// 结束后清空信息
    /// </summary>
    void ClearWinInfo()
    {
        //数据清空
        ZhuangYF.text = "0";
        XianYF.text = "0";
        HeYF.text = "0";

        


        //对象清空
        for (int i = 0; i < XianYaFen.Count; i++)
        {
            XianYaFen[i].transform.GetChild(0).GetComponent<Text>().text = "0";
            XianYaFen[i].SetActive(false);
        }
        for (int i = 0; i < ZhuangYaFen.Count; i++)
        {
            ZhuangYaFen[i].transform.GetChild(0).GetComponent<Text>().text = "0";
            ZhuangYaFen[i].SetActive(false);
        }
        for (int i = 0; i < HeYaFen.Count; i++)
        {
            HeYaFen[i].transform.GetChild(0).GetComponent<Text>().text = "0";
            HeYaFen[i].SetActive(false);
        }

        ZhuangNumberText.SetActive(false);
        XianNunberText.SetActive(false);
        Handsel_GetType.SetActive(false);
        //for (int i = 0; i < suitInAllText.Count; i++)
        //{
        //    suitInAllText[i].text = "0";
        //    suitInSelfText[i].text = "0";
        //}
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



    //发牌动画区

    private GameObject target;
    public GameObject origin;
    public GameObject MoveCard;
    public void FaPai_01()
    {
        //一般情况
        for (int i = 0; i < 4; i++)
        {

        }
    }


    
   


    private bool IsDeal;
    IEnumerator Pai_Deal1()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            PlayerImage.Play("FaPai");
            yield return new WaitForSeconds(0.3f);
            PlayerImage.Play("ZhaYan");

            MoveCard.SetActive(true);
            _timer = 0;

            switch (i)
            {
                case 0: //闲
                    target = Xianpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 1://庄
                    Xianpokers[0].gameObject.SetActive(true);

                    target = Zhuangpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 2://闲
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);

                    target = Xianpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 3://庄
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    target = Zhuangpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
            }
            StartCoroutine("Pai_Move");
            yield return new WaitForSeconds(1f);
        }

        ZhuangNumberText.SetActive(true);
        XianNunberText.SetActive(true);
        ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄"+Z_number.ToString()+"点";
        XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + X_number.ToString()+"点";


        yield return null;
    }

    IEnumerator Pai_Deal2()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            PlayerImage.Play("FaPai");
            yield return new WaitForSeconds(0.3f);
            PlayerImage.Play("ZhaYan");

            MoveCard.SetActive(true);
            _timer = 0;

            switch (i)
            {
                case 0: //闲
                    target = Xianpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 1://庄
                    Xianpokers[0].gameObject.SetActive(true);

                    target = Zhuangpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 2://闲
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);

                    target = Xianpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 3://庄
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    target = Zhuangpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);

                    break;
                case 4:
                    ZhuangNumberText.SetActive(false);
                    XianNunberText.SetActive(false);
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);

                    target = Zhuangpokers[2].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangZeng);
                    break;
            }
            StartCoroutine("Pai_Move");
            if (i==3)
            {
                //显示庄闲点数
                yield return new WaitForSeconds(1f);
                ZhuangNumberText.SetActive(true);
                XianNunberText.SetActive(true);
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + Z_number.ToString() + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + X_number.ToString() + "点";
            }
            yield return new WaitForSeconds(1f);
        }

        //显示庄闲最终数值
        ZhuangNumberText.SetActive(true);
        XianNunberText.SetActive(true);
        ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + ZC_number.ToString() + "点";
        XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + XC_number.ToString() + "点";

        yield return null;
    }

    IEnumerator Pai_Deal3()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
           PlayerImage.Play("FaPai");
            yield return new WaitForSeconds(0.3f);
            PlayerImage.Play("ZhaYan");

            MoveCard.SetActive(true);
            _timer = 0;

            switch (i)
            {
                case 0: //闲
                    target = Xianpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 1://庄
                    Xianpokers[0].gameObject.SetActive(true);

                    target = Zhuangpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 2://闲
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);

                    target = Xianpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 3://庄
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    target = Zhuangpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 4:
                    ZhuangNumberText.SetActive(false);
                    XianNunberText.SetActive(false);
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);

                    target = Xianpokers[2].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianZeng);
                    break;
            }
            StartCoroutine("Pai_Move");
            if (i == 3)
            {
                //显示庄闲点数
                yield return new WaitForSeconds(1f);
                ZhuangNumberText.SetActive(true);
                XianNunberText.SetActive(true);
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + Z_number.ToString() + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + X_number.ToString() + "点";
            }
            yield return new WaitForSeconds(1f);
        }

        //显示庄闲最终数值
        ZhuangNumberText.SetActive(true);
        XianNunberText.SetActive(true);
        ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + ZC_number.ToString() + "点";
        XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + XC_number.ToString() + "点";

        yield return null;
    }

    IEnumerator Pai_Deal4()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 6; i++)
        {
            PlayerImage.Play("FaPai");
            yield return new WaitForSeconds(0.3f);
            PlayerImage.Play("ZhaYan");

            MoveCard.SetActive(true);
            _timer = 0;

            switch (i)
            {
                case 0: //闲
                    target = Xianpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 1://庄
                    Xianpokers[0].gameObject.SetActive(true);

                    target = Zhuangpokers[0].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 2://闲
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);

                    target = Xianpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianP);
                    break;
                case 3://庄
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);

                    target = Zhuangpokers[1].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangP);
                    break;
                case 4:
                    ZhuangNumberText.SetActive(false);
                    XianNunberText.SetActive(false);

                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);

                    target = Xianpokers[2].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.XianZeng);
                    break;
                case 5:
                    Xianpokers[0].gameObject.SetActive(true);
                    Zhuangpokers[0].gameObject.SetActive(true);
                    Xianpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[1].gameObject.SetActive(true);
                    Zhuangpokers[2].gameObject.SetActive(true);
                    target = Zhuangpokers[2].gameObject;
                    Audiomanger._instenc.PlaySound(Audiomanger._instenc.ZhuangZeng);
                    break;
            }
            StartCoroutine("Pai_Move");
            if (i == 3)
            {
                //显示庄闲点数
                yield return new WaitForSeconds(1f);
                ZhuangNumberText.SetActive(true);
                XianNunberText.SetActive(true);
                ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄" + Z_number.ToString() + "点";
                XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲" + X_number.ToString() + "点";
            }
            yield return new WaitForSeconds(1f);
        }


        //显示庄闲最终数值
        ZhuangNumberText.SetActive(true);
        XianNunberText.SetActive(true);
        ZhuangNumberText.transform.GetChild(0).GetComponent<Text>().text = "庄"+ZC_number.ToString()+"点";
        XianNunberText.transform.GetChild(0).GetComponent<Text>().text = "闲"+XC_number.ToString()+"点";

        yield return null;
    }

    float _timer = 0;
    IEnumerator Pai_Move()
    {
        
        while (true)
        {
            MoveCard.transform.position = Vector3.Lerp(origin.transform.position,target.transform.position,_timer);
            _timer += 0.1f;
            if ((MoveCard.transform.position- target.transform.position).magnitude<=20f)
            {
                MoveCard.SetActive(false);
                target.SetActive(true);
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    public void SetHandsel()
    {
        StartCoroutine(GetHandsel(
            LoginInfo.Instance().mylogindata.URL + "handsel-notice?drop_date=" + LoginInfo.Instance().mylogindata.dropContent
            ));
            
    }



    //接收彩金
    IEnumerator GetHandsel(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log ("彩金"+url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);



            if (jd["code"].ToString()!="404")
            {
                Handsel_GetType.SetActive(true);
                Handsel_GetType.transform.GetChild(0).GetComponent<Text>().text = jd["msg"].ToString();
            }
           

        }
            yield return null;
    }

    //是否点击隐分
    int  Is_score;
    int send_score;
    public void SetHideScore()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (clockText.text != "0")
        {
            if (Is_score == 0)
            {
                send_score = 1;
            }
            else
            {
                send_score = 0;
            }

            StartCoroutine(GetHideScore
               (
                    LoginInfo.Instance().mylogindata.URL + "is-score?" +
                   "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                   "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
                   "&user_id=" + LoginInfo.Instance().mylogindata.user_id +
                   "&is_score=" + send_score
               ));
        }

        //if (Is_score == 0)
        //{
        //    send_score = 1;
        //}
        //else
        //{
        //    send_score = 0;
        //}
        //Debug.Log("发送的隐分设置:"+send_score);
        //StartCoroutine(GetHideScore
        //   (
        //        LoginInfo.Instance().mylogindata.URL + "is-score?" +
        //       "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
        //       "&room_id=" + LoginInfo.Instance().mylogindata.room_id +
        //       "&user_id=" + LoginInfo.Instance().mylogindata.user_id +
        //       "&is_score=" + send_score
        //   ));
    }

    //接收隐分
    IEnumerator GetHideScore(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        if (www.error == null)
        {
            //JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            //if (jd["is_score"].ToString() == "1")
            //{
            //    for (int i = 0; i < playerpos.Length; i++)
            //    {
            //        if (playerpos[i] == int.Parse(MeID))
            //        {
            //            MaskPlace.transform.GetChild(i).gameObject.SetActive(false);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < playerpos.Length; i++)
            //    {
            //        if (playerpos[i] == int.Parse(MeID))
            //        {
            //            MaskPlace.transform.GetChild(i).gameObject.SetActive(true);
            //            break;
            //        }
            //    }
            //}

        }
        yield return null;
    }


    //更新对应扑克牌图片的显示
    public void PokerSpriteUpdate(Image _image,string str)
    {
        int num=0;
        if (str!="kong")
        {
            num = int.Parse(str.Substring(1,1));

            switch (str.Substring(2, 1))
            {
                case "1":
                    _image.sprite = PokerSprite[num*13];
                    break;
                case "2":
                    _image.sprite = PokerSprite[num * 13+1];
                    break;
                case "3":
                    _image.sprite = PokerSprite[num * 13 + 2];
                    break;
                case "4":
                    _image.sprite = PokerSprite[num * 13 + 3];
                    break;
                case "5":
                    _image.sprite = PokerSprite[num * 13 + 4];
                    break;
                case "6":
                    _image.sprite = PokerSprite[num * 13 + 5];
                    break;
                case "7":
                    _image.sprite = PokerSprite[num * 13 + 6];
                    break;
                case "8":
                    _image.sprite = PokerSprite[num * 13 + 7];
                    break;
                case "9":
                    _image.sprite = PokerSprite[num * 13 + 8];
                    break;
                case "0":
                    _image.sprite = PokerSprite[num * 13 + 9];
                    break;
                case "J":
                    _image.sprite = PokerSprite[num * 13 + 10];
                    break;
                case "Q":
                    _image.sprite = PokerSprite[num * 13 + 11];
                    break;
                case "K":
                    _image.sprite = PokerSprite[num * 13 + 12];
                    break;
            }
        }
    }

    public int BackVaslue(string str)
    {
        int num=0;
        if (str != "kong")
        {
            switch (str.Substring(2, 1))
            {
                case "1":
                    num= 1;
                    break;
                case "2":
                    num = 2;
                    break;
                case "3":
                    num = 3;
                    break;
                case "4":
                    num = 4;
                    break;
                case "5":
                    num = 5;
                    break;
                case "6":
                    num = 6;
                    break;
                case "7":
                    num = 7;
                    break;
                case "8":
                    num = 8;
                    break;
                case "9":
                    num = 9;
                    break;
                case "0":
                    num = 0;
                    break;
                case "J":
                    num = 0;
                    break;
                case "Q":
                    num = 0;
                    break;
                case "K":
                    num = 0;
                    break;
            }
        }
        else
        {
            num = 0;
        }

        return num;
    }

    public void HidePoker()
    {
        if (!IsGetPoker)
        {
            for (int i = 0; i < 3; i++)
            {
                Zhuangpokers[i].gameObject.SetActive(false);
                Xianpokers[i].gameObject.SetActive(false);
            }
            if (!IsGetPoint)
            {
                ZhuangNumberText.SetActive(false);
                XianNunberText.SetActive(false);
            }
        }
        
        StopCoroutine("Pai_Move");
    }


    private void HandselUpdate(string Handselstr)
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < Handselstr.Length)
            {
                ShowHandsel.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                ShowHandsel.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = HandselSprite[int.Parse(Handselstr.Substring(i, 1))];
            }
            else
            {
                ShowHandsel.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
          
        }
    }


    //更新玩家信息
    public void PlayerStateUpdate(string id,string Uid,string Nimename, string countBet)
    {
        int pos = int.Parse(id)-1;

        ChipsState.transform.GetChild(2 * pos).gameObject.SetActive(true);
        ChipsState.transform.GetChild(2 * pos+1).gameObject.SetActive(true);

        ChipsState.transform.GetChild(2 * pos).GetChild(0).GetComponent<Text>().text = ((int)float.Parse(countBet)).ToString();
        ChipsState.transform.GetChild(2 * pos+1).GetChild(0).GetComponent<Text>().text =  Uid;
        ChipsState.transform.GetChild(2 * pos+1).GetChild(1).GetComponent<Text>().text = Nimename;
        

    }

    public void PLayerStateHide(int pos)
    {
        ChipsState.transform.GetChild(2 * pos).gameObject.SetActive(false);
        ChipsState.transform.GetChild(2 * pos + 1).gameObject.SetActive(false);


    }

    //清除全部
    public void PlayerStateClear()
    {
        for (int i = 0; i < ChipsState.transform.childCount; i++)
        {
            ChipsState.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //隐藏
    public void HideScore(int pos)
    {
        MaskPlace.transform.GetChild(pos).gameObject.SetActive(false);
    }

    //打开
    public void SHowScore(int pos)
    {
        MaskPlace.transform.GetChild(pos).gameObject.SetActive(true);
    }

    }