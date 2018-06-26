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

public class DanTiao2 : MonoBehaviour
{
    #region UP
    public Text xianHongText;
    public Text yaFenText;
	public Text inningText;
    public Text roundText;
    public Text idText;
    public Text GoldText;
    public Text ClockText;
    #endregion


    #region Center

	public DrawEffect draweffect;		//牌效果脚本
	public GameObject Handsel;				//彩金显示区域
	public Sprite []handselsprite;			//彩金数字图集
    public Image pokerCard;
    public Image pokerBack; //背面这张
	public GameObject gridItem;
    public GameObject picGo; //图片显示结果父物体
    public GameObject numGo; //数字显示结果
    public GameObject wordGo; //文字显示结果
    public GameObject winInfoPanel;
    public GameObject winState; //显示正在开奖中
    public GameObject fengPanGo;

	public Sprite[] Picture;	//照片
	public GameObject Glisten;	//闪亮效果

    /*
     * 这里是一些需要用到的文件 和变量
     * 
     * */
    public Sprite[] pokerCardSprite;  //开奖牌   黑 红 花 片 王  下同
    



	public Sprite[] picSprite; //

    public Sprite[] blackNumSprite;
	public Sprite[] redNumSprite;
    public Sprite[] wordSprite;

	public Sprite[] Handsel_picSprite;			//带彩金的图像

	public Sprite[] Handsel_NumSprite;			//带彩金的图像1
	public Sprite[] Handsel_wordSprite;			//带彩金的图像2

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
	public Animator Goldanime;

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
//    public static string[] betId = new string[5];
	List<string> betId =new List<string>();
    public static string[] rateInfo = new string[5];
    public static string[] allDnumInfo = new string[5];
    public static string[] selfDnumInfo = new string[5];
    //待定
    #endregion


    #region Other
    public NewTcpNet tcpNet;

    public GameObject errorPanel;

    public static DanTiao2 instance;

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

	//初始化信息
    void Start()
    {
		tcpNet = NewTcpNet.GetInstance();
		StartCoroutine(ShowLoading());
        Init();
        AddListener();

        //踢出检测（通用...会自动延长10触发）
        LoginInfo.Instance().GetOnPing();

        StartCoroutine(Polling
            (
             LoginInfo.Instance().mylogindata.URL +
             LoginInfo.Instance().mylogindata.hallaliveAPI +
            "user_id=" + LoginInfo.Instance().mylogindata.user_id +
            "&unionuid=" + LoginInfo.Instance().mylogindata.token/* +
            "&room_id" + LoginInfo.Instance().mylogindata.room_id +
            "&game_id" + LoginInfo.Instance().mylogindata.game_id*/
            ));
		StartCoroutine(GetHistory(LoginInfo.Instance().mylogindata.URL +
			LoginInfo.Instance().mylogindata.winHistory +
			"game_id=" + LoginInfo.Instance().mylogindata.choosegame
		));

        StartCoroutine("GetBetNum");
    }

  

    /// <summary>
    /// 在这里进行一些初始化操作
    /// </summary>
    void Init()
    {
		Application.targetFrameRate = 30;
        backPokerVec = pokerBack.transform.GetComponent<RectTransform>().anchoredPosition;
        ClearInfo();
        ClockText.text = "0";

		

        isFirstBet = true;
        isPause = false;
        isFocus = false;

		isPlayOver = false;
		IsGetMove = false;
        //numGo.SetActive(true);

		Goldanime.gameObject.SetActive (false);
		for (int i = 0; i < suitBackLight.Count; i++) {

			suitBackLight [i].gameObject.SetActive (false);
		}

		draweffect.BackCard.transform.GetChild (0).gameObject.SetActive (false);
		draweffect.Card_Up.transform.GetChild (0).gameObject.SetActive (false);

        if (PlayerPrefs.GetString("isShowWord") == "true")  //开奖结果显示方式
        {
//            wordGo.SetActive(true);
//			picGo.SetActive(false);
			for (int p = 0; p < gridItem.transform.childCount; p++) {
				gridItem.transform.GetChild(p).GetChild (2).gameObject.SetActive (true);
				gridItem.transform.GetChild(p).GetChild (1).gameObject.SetActive (false);
			}

          
        }
        else
        {
//            wordGo.SetActive(false);
//            picGo.SetActive(true);
			for (int p = 0; p < gridItem.transform.childCount; p++) {
				gridItem.transform.GetChild(p).GetChild (2).gameObject.SetActive (false);
				gridItem.transform.GetChild(p).GetChild (1).gameObject.SetActive (true);
			}
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

       
    }
    
	#region 监听事件
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

	//切换下注筹码
	void OnChangeCoinDown()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
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


	//取消下注
	void OnClickPass()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
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

	//刷新彩金
	public void UpdateHandsel(string Hvalue)
	{
		for (int i = 0; i < Handsel.transform.childCount; i++) {
			if (i < Hvalue.Length) {
				Handsel.transform.GetChild (i).gameObject.SetActive (true);
				Handsel.transform.GetChild (i).GetComponent<Image> ().sprite = handselsprite [int.Parse(Hvalue.Substring(i,1))];
			} else {
				Handsel.transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}



	#endregion


//	void Update()
//	{
//		if(Input.GetKeyDown(KeyCode.A))
//		{
//			Debug.Log (PictureNum);
//		}
//
//		if(Input.GetKeyDown(KeyCode.S))
//		{
//			draweffect.Card_Down.GetComponent<Image>().sprite =Picture[PictureNum];	
//		}
//	}

	/*
    private void Update()
    {
        if (NewTcpNet.isUpdate)
        {
            fengPanGo.SetActive(false);
            ClockText.text = NewTcpNet.countDown;
            roundText.text = NewTcpNet.season;
			inningText.text = NewTcpNet.periods;
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
	*/

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
				if (roundnumber >= jd ["ArrList"].Count) 
				{
					ClearInfo();
					for (int i = 0; i < jd["ArrList"].Count; i++)
					{
						ColorAndNum(jd["ArrList"][i]["winnings"].ToString(),jd["ArrList"][i]["handsel"].ToString(),jd["ArrList"][i]["open_deal"].ToString());
					}
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
                        if (tcpNet!=null)
                        {
                            tcpNet.SocketQuit();
                        }
                        yield return new WaitForSeconds(2f);
                        
                        SceneManager.LoadScene(0);
                    }
                }
                else
                {
                    ShowOtherMess(jd["msg"].ToString());
                    if (tcpNet != null)
                    {
                        tcpNet.SocketQuit();
                    }
                    yield return new WaitForSeconds(2f);
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
			Debug.Log ("下注："+url);
            yield return www.Send();
            //Debug.LogError("已经发送");
            if (www.error == null)
            {
                //Debug.LogError("发送成功");
				if(www.responseCode==200)
				{
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
		Debug.Log (url);

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

	int PictureNum;
	int NowPictureNum;

#region Socket
	private bool IsGetResult;
	private bool isPlayOver;
	private bool isFirstJoin;

	private bool IsGetMove;
	private bool IsFirstResult;

	private bool IsShowGlisten;
	private float GlistenTime;
	private int roundnumber;

	public void PollingPeriods(JsonData jd)
	{
		inningText.text = jd["periods"].ToString();
		roundText.text = jd["season"].ToString();
		ClockText.text = jd["countdown"].ToString();
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


		//豹炸图片更新
		PictureNum = ((int.Parse (jd ["periods"].ToString ()) * int.Parse (jd ["season"].ToString ())) % 183);
		if(NowPictureNum!=PictureNum)
		{
			draweffect.Card_Down.GetComponent<Image>().sprite =Picture[PictureNum];
			NowPictureNum = PictureNum;
		}


        try
        {
            //彩金显示状态
            if (jd["handsel_status"].ToString() == "1")
            {
                Handsel.gameObject.SetActive(false);
            }
            else if (jd["handsel_status"].ToString() == "2")
            {
                Handsel.gameObject.SetActive(true);
                Handsel.transform.GetChild(0).GetComponent<Text>().text = "彩金：" + jd["handsel"].ToString();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }


		if (jd["is_empty"].ToString() == "1")
		{
			ClearInfo();
		}

		if (jd ["is_win"].ToString () == "0") {
			//获取记录基本参数
			LoginInfo.Instance ().mylogindata.dropContent = jd ["drop_date"].ToString ();
//			inningText.text = jd["periods"].ToString();
//			roundText.text = jd["season"].ToString();
//			ClockText.text = jd["countdown"].ToString();
//			Handsel.text = "彩金：" + jd ["handsel"].ToString ();
			UpdateHandsel(jd ["handsel"].ToString ());

			if (IsGetResult) {
				IsGetResult = false;
			}
			if (!isFirstJoin) {
				winState.SetActive (false);
				isFirstJoin = true;
			}
			if (jd ["countdown"].ToString () != "0") {
				if (jd ["open_deal"].ToString () == "1") {//半明牌
					draweffect.BackCard.SetActive (true);
					draweffect.SetUpCard (int.Parse (jd ["open_deal_type_one"].ToString ()), int.Parse (jd ["pai_one"].ToString ()));
					draweffect.SetBackCard1 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
					draweffect.IsRotare = true;
					draweffect.IsMove = true;

					if (!IsShowGlisten) {
						IsShowGlisten = true;
						Glisten.SetActive (true);					
					} else {
						if (GlistenTime <= 5f) {
							GlistenTime += 1;
							if (GlistenTime >= 5f) {
								Glisten.SetActive (false);
							}
						}
					}

//					if(Glisten.activeSelf)
//					{
//						GlistenTime += 1;
//						if(GlistenTime>=6)
//						{
//							Glisten.SetActive (false);
//						}
//					}
//
//					if(jd["video_sign"].ToString()!="0"&&GlistenTime==0)
//					{
//						Glisten.SetActive (true);
//					}





				} else if (jd ["open_deal"].ToString () == "2") {//全明牌
					draweffect.BackCard.SetActive (true);
					draweffect.SetUpCard (int.Parse (jd ["open_deal_type_one"].ToString ()), int.Parse (jd ["pai_one"].ToString ()));
					draweffect.SetBackCard2 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
					draweffect.IsRotare = true;
					draweffect.IsMove = true;

					if (!IsShowGlisten) {
						IsShowGlisten = true;
						Glisten.SetActive (true);					
					} else {
						if (GlistenTime <= 5f) {
							GlistenTime += 1;
							if (GlistenTime >= 5f) {
								Glisten.SetActive (false);
							}
						}
					}



				} else {
					draweffect.BackCard.SetActive (false);

				}

				if (!isPlayOver) {
					ClearWinInfo ();
					//开始压分
					Audiomanger._instenc.PlayTip (0);
					fengPanGo.SetActive (false);
					IsFirstResult = false;
					draweffect.Reset ();
					draweffect.SetUpPos ();
					isPlayOver = true;
					Goldanime.gameObject.SetActive (false);
					for (int i = 0; i < suitBackLight.Count; i++) {

						suitBackLight [i].gameObject.SetActive (false);
					}
				}
			} else if (jd ["countdown"].ToString () == "0" && isPlayOver) {
				//停止压分
				Audiomanger._instenc.PlayTip (1);
				fengPanGo.SetActive (true);
				isPlayOver = false;
				IsGetMove = false;
				IsShowGlisten = false;
				GlistenTime = 0;
				Glisten.SetActive (false);

				if (jd ["open_deal"].ToString () == "1") {
					draweffect.SetBackCard1 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
				} else if (jd ["open_deal"].ToString () == "2") {
					draweffect.SetBackCard2 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
				}
				draweffect.SetUpCard (int.Parse (jd ["pai_one"].ToString ()));

				if (jd ["open_deal"].ToString () != "0") {
					if (!IsFirstResult) {
						//关闭之前的牌效果
						IsFirstResult = true;
						draweffect.IsMove = false;
						draweffect.IsRotare = false;

						if (jd ["open_deal"].ToString () == "0") {
							draweffect.IsOpen = true;
							draweffect.GetWin ();
						} else {
							draweffect.IsReturn = true;
							draweffect.GetWin1 ();
						}
					}
				}
			} else {
				
			}

		} else if (jd ["is_win"].ToString () == "1") {
			fengPanGo.SetActive (true);
			ClockText.text = jd ["countdown"].ToString ();
//			Handsel.text = "彩金：" + jd ["handsel"].ToString ();
			UpdateHandsel(jd ["handsel"].ToString ());

			if (jd ["open_deal"].ToString () == "1") {
				draweffect.SetBackCard1 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
			} else if (jd ["open_deal"].ToString () == "2") {
				draweffect.SetBackCard2 (int.Parse (jd ["open_deal_type_two"].ToString ()), int.Parse (jd ["pai_two"].ToString ()));
			}
			draweffect.SetUpCard (int.Parse (jd ["winnings"].ToString ()));

			if (!IsFirstResult) {
				//关闭之前的牌效果
				IsFirstResult = true;
				draweffect.IsMove = false;
				draweffect.IsRotare = false;

				if (jd ["open_deal"].ToString () == "0") {
					draweffect.IsOpen = true;
					draweffect.GetWin ();
				} else {
					draweffect.IsReturn = true;
					draweffect.GetWin1 ();
				}

			}


		} else if (jd ["is_win"].ToString () == "2" && !IsGetResult) {

//			Handsel.text = "彩金：" + jd ["handsel"].ToString ();
			UpdateHandsel(jd ["handsel"].ToString ());

			if (jd ["handsel"].ToString () != "0") {
				draweffect.BackCard.transform.GetChild (0).gameObject.SetActive (true);
				draweffect.Card_Up.transform.GetChild (0).gameObject.SetActive (true);
			} 

			if (jd ["open_deal"].ToString () != "0") {
				draweffect.SetBackCard (int.Parse (jd ["pai_two"].ToString ()));
			}

			draweffect.SetUpCard (int.Parse (jd ["winnings"].ToString ()));

			//重置下牌应该在的位置
			if (!IsFirstResult) {
				IsFirstResult = true;
				if (jd ["open_deal"].ToString () == "0") {
					draweffect.IsOpen = true;
					draweffect.GetWin ();
				} else {
					draweffect.IsReturn = true;
					draweffect.GetWin1 ();
				}
			} 



			if (jd ["winnings"].ToString () == "") {
				//没有开奖结果
				winState.SetActive (true);
				ClockText.text = jd ["countdown"].ToString ();
			} else {
				fengPanGo.SetActive (false);
				if (isFirstJoin) {
//					if (jd ["open_deal"].ToString () == "0") {
//						draweffect.GetWin0 ();
//					} else {
//						draweffect.GetWin2 ();
//					}

					OnWin (jd);
					IsGetResult = true;
					//灯光闪烁  第一个
					int winnum = int.Parse (jd ["winnings"].ToString ());
					if (winnum != -1) {
						if (winnum > 52) {
							suitBackLight [4].gameObject.SetActive (true);


						} else {
							if (winnum < 13) {
								suitBackLight [0].gameObject.SetActive (true);
							} else if (winnum < 26) {
								suitBackLight [1].gameObject.SetActive (true);
							} else if (winnum < 39) {
								suitBackLight [2].gameObject.SetActive (true);
							} else if (winnum < 52) {
								suitBackLight [3].gameObject.SetActive (true);
							}
						}
					}

					//灯光闪烁   第二个
					winnum = int.Parse (jd ["winnings_two"].ToString ());
					if (winnum != -1) {
						if (winnum > 52) {
							suitBackLight [4].gameObject.SetActive (true);
						} else {
							if (winnum < 13) {
								suitBackLight [0].gameObject.SetActive (true);
							} else if (winnum < 26) {
								suitBackLight [1].gameObject.SetActive (true);
							} else if (winnum < 39) {
								suitBackLight [2].gameObject.SetActive (true);
							} else if (winnum < 52) {
								suitBackLight [3].gameObject.SetActive (true);
							}
						}
					}
					if (jd ["handsel"].ToString () != "0") {
						//礼花效果
						Goldanime.gameObject.SetActive (true);
					}
				} else {
					LoginInfo.Instance ().mylogindata.dropContent = jd ["drop_date"].ToString ();
//					ClockText.text = jd["countdown"].ToString();
//					inningText.text = jd["periods"].ToString();
//					roundText.text = jd["season"].ToString();
					winState.SetActive (true);
					IsGetResult = true;
				}
			}
		} else if (jd ["is_win"].ToString () == "2" && IsGetResult) {
			//持续获取结果
			StartCoroutine(GetHistory(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.winHistory +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame
			));
		}

	}

	public void UpdateSuit(JsonData jd)
	{
        //StartCoroutine("GetBetNum");
        try {
            //betId.Clear();
            for (int i = 0; i < jd["oddlist"].Count; i++)
			{
				//betId.Add(jd["oddlist"][i]["id"].ToString());
				suitRateText[i].text = "X" + jd["oddlist"][i]["rate"].ToString();
				suitInAllText[i].text = jd["oddlist"][i]["dnum"].ToString();
				suitInSelfText[i].text = jd["oddlist"][i]["user_dnum"].ToString();


			}
		} catch (Exception ex) {
          
		}
	}

	public void OddList(JsonData jd)
	{
		for (int i = 0; i < jd["Oddlist"].Count; i++)
		{
			suitInAllText[i].text = jd["Oddlist"][i].ToString();
		}

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
		inningText.text = juShu;
        roundText.text = round;
    }

#endregion


#region 其他一些逻辑

	void ColorAndNum(string poker,string handsel,string opendeal)
    {
		try {
			int pokerNum = Convert.ToInt32(poker);

			int color = pokerNum / 13;
			int num = pokerNum % 13;
			OnAddPoker(color, num,handsel,opendeal);
		} catch (Exception ex) {
			
		}
       
    }

    /// <summary>
    /// 添加牌的方法
    /// </summary>
    /// <param name="color"></param>
    /// <param name="num"></param>
	void OnAddPoker(int color, int num,string handsel,string opendeal)
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
				
				if (handsel != "0") {
//					wordGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = Handsel_wordSprite [4];
//					picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = Handsel_picSprite[4];
					gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite=Handsel_wordSprite [4];
					gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= Handsel_picSprite[4];

				} else {
//					wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = wordSprite[4];
//					picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[4];
					gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite= wordSprite[4];
					gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= picSprite[4];
				}
				gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).gameObject.SetActive(true);
				gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
				if (handsel != "0") {
//					wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = Handsel_wordSprite[5];
//					picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = Handsel_picSprite[5];
					gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite=Handsel_wordSprite [5];
					gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= Handsel_picSprite[5];
				} else {
//					wordGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = wordSprite[5];
//					picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[5];
					gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite= wordSprite[5];
					gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= picSprite[5];
				}
//                wordGo.transform.GetChild(resultCount).gameObject.SetActive(true);
//                picGo.transform.GetChild(resultCount).gameObject.SetActive(true);
				gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).gameObject.SetActive(true);
				gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
			if (handsel != "0") {
//				wordGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = Handsel_wordSprite [color];
//				picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = Handsel_picSprite[color];
				gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite= Handsel_wordSprite [color];
				gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= Handsel_picSprite[color];
			} else {
//				wordGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = wordSprite [color];
//				picGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = picSprite[color];
				gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).GetComponent<Image> ().sprite= wordSprite[color];
				gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).GetComponent<Image> ().sprite= picSprite[color];
			}
//            wordGo.transform.GetChild(resultCount).gameObject.SetActive(true);
//			picGo.transform.GetChild(resultCount).gameObject.SetActive(true);
			gridItem.transform.GetChild (resultCount).GetChild(1).GetChild(0).gameObject.SetActive(true);
			gridItem.transform.GetChild (resultCount).GetChild(2).GetChild(0).gameObject.SetActive(true);
        }

        if (color != 4)
        {
            //numGo.transform.GetChild(resultCount).GetComponent<Image>().sprite = blackNumSprite[num];
            //numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
            if (color == 0 || color == 2)
            {
				if (handsel != "0") {
//					numGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = Handsel_NumSprite [num];
					gridItem.transform.GetChild (resultCount).GetChild(0).GetComponent<Image>().sprite=Handsel_NumSprite [num];
				} else {
//					numGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = blackNumSprite [num];
					gridItem.transform.GetChild (resultCount).GetChild(0).GetComponent<Image>().sprite=blackNumSprite [num];
				}
//                numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
				gridItem.transform.GetChild (resultCount).GetChild(0).gameObject.SetActive(true);
            }
            else
			{
				if (handsel != "0") {
//					numGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = Handsel_NumSprite [num];
					gridItem.transform.GetChild (resultCount).GetChild(0).GetComponent<Image>().sprite=Handsel_NumSprite [num];
				} else {
//					numGo.transform.GetChild (resultCount).GetComponent<Image> ().sprite = redNumSprite [num];
					gridItem.transform.GetChild (resultCount).GetChild(0).GetComponent<Image>().sprite=redNumSprite [num];
				}
//                numGo.transform.GetChild(resultCount).gameObject.SetActive(true);
				gridItem.transform.GetChild (resultCount).GetChild(0).gameObject.SetActive(true);
            }
        }

		if (opendeal != "0") {
			gridItem.transform.GetChild (resultCount).GetChild (3).gameObject.SetActive (true);
		} else {
			gridItem.transform.GetChild (resultCount).GetChild (3).gameObject.SetActive (false);
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
		for (int i = 0; i < gridItem.transform.childCount; i++)
		{


			gridItem.transform.GetChild(i).GetChild (0).gameObject.SetActive (false);
			gridItem.transform.GetChild(i).GetChild (1).GetChild(0).gameObject.SetActive (false);
			gridItem.transform.GetChild(i).GetChild (2).GetChild(0).gameObject.SetActive (false);
			gridItem.transform.GetChild(i).GetChild (3).gameObject.SetActive (false);

		}
		resultCount = 0;
		for (int i = 0; i < suitCountText.Length; i++)
		{
			suitCountText[i].text = "0";
		}

    }

	public void ResetResult()
	{
		//			numGo.transform.GetChild(i).gameObject.SetActive(false);
		//			picGo.transform.GetChild(i).gameObject.SetActive(false);
		//			wordGo.transform.GetChild(i).gameObject.SetActive(false);


	}

	public void Show_1()
	{
		for (int p = 0; p < gridItem.transform.childCount; p++) {
			gridItem.transform.GetChild(p).GetChild (2).gameObject.SetActive (true);
			gridItem.transform.GetChild(p).GetChild (1).gameObject.SetActive (false);
		}
	}

	public void Show_2()
	{
		for (int p = 0; p < gridItem.transform.childCount; p++) {
			gridItem.transform.GetChild(p).GetChild (2).gameObject.SetActive (false);
			gridItem.transform.GetChild(p).GetChild (1).gameObject.SetActive (true);
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


	public void OnWin(JsonData jd)
    {
		isPlayOver = false;
		IsGetMove = false;

        ClockText.text = jd["countdown"].ToString();
        //Debug.LogWarning()
        if (jd["winnings"].ToString() != "")
        {
            //pokerCard.sprite = pokerCardSprite[Convert.ToInt32(float.Parse(jd["winnings"].ToString()))];
            
            StartCoroutine(PokerAni(Convert.ToInt32(jd["winnings"].ToString())));
            //Audiomanger._instenc.PlayDanTiaoWin(Convert.ToInt32(jd["winnings"].ToString()));
            //ColorAndNum(jd["winnings"].ToString());

			StartCoroutine(GetHistory(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.winHistory +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame
			));
        }
       

    }

    IEnumerator PokerAni(int num)
    {
		

        //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(-368f, 3f);
        yield return new WaitForSeconds(3.2f); 
		if (num != -1) {
			if (num > 52) {
				if (num == 52) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [4]);
				} else if (num == 53) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [5]);
				}

			} else {
				if (num < 13) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [0]);
				} else if (num < 26) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [1]);
				} else if (num < 39) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [2]);
				} else if (num < 52) {
					Audiomanger._instenc.PlaySound (Audiomanger._instenc.pokerColor [3]);
				}
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
		
        //StartCoroutine(PokerAniBack());


    }

    IEnumerator PokerAniBack()
    {
        yield return new WaitForSeconds(4f);
        //Tween tw = pokerBack.GetComponent<RectTransform>().DOAnchorPosY(0, 3f);
        yield return new WaitForSeconds(3.1f);
        pokerBack.transform.GetComponent<RectTransform>().anchoredPosition = backPokerVec;
        ClearWinInfo();
    }

    /// <summary>
    /// 显示中奖区域
    /// </summary>
    /// <returns></returns>
	bool IsLight;
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
			if (IsLight)
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
			
            NewTcpNet.isFirst = false;
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

    IEnumerator GetBetNum()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL + "room-odds-limit"+"?room_id="+ LoginInfo.Instance().mylogindata .room_id+ "&game_id="+ LoginInfo.Instance().mylogindata.choosegame);
        yield return www.Send();
        Debug.Log(www.url);
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

            if (jd["code"].ToString() == "200")
            {
                betId.Clear();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < jd["oddsData"].Count; j++)
                    {
                        switch (i)
                        {
                            case 0: //黑
                                if (jd["oddsData"][j]["tp"].ToString() == "黑")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 1: //红
                                if (jd["oddsData"][j]["tp"].ToString() == "红")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 2: //梅
                                if (jd["oddsData"][j]["tp"].ToString() == "梅")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 3: //方
                                if (jd["oddsData"][j]["tp"].ToString() == "方")
                                {
                                    betId.Add(jd["oddsData"][j]["id"].ToString());
                                }
                                break;
                            case 4: //王
                                if (jd["oddsData"][j]["tp"].ToString() == "王")
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
