using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
/// <summary>
/// 龙虎管理脚本
/// </summary>
public class NewLongHu : MonoBehaviour {

    
    public NewTcpNet tcpNet;
    Text nubText;
    public bool isok;
    bool isend;
    bool isoo=false;//是否隐分;
    //动画图片数组
    public Sprite[] pk;
    //开奖图片数组
    public Sprite[] tr;//右链表
    public Sprite[] left;//左链表
    //切换图片的图集数组
    public Sprite[] yyc;

	public Sprite[] QH_sprite;	//切换按钮使用的图集

	private GameObject longbetList;
	private GameObject hubetList;
	private GameObject hebetList;

	private GameObject Userslist;
	private GameObject Goldslist;

	private Text XianHong;
	private Text ZuiDiXianZhu;
	private Text HeXianZhu;
//
//	private Text Long_Rate;
//	private Text Hu_Rate;
//	private Text He_Rate;

	public LHPokerVS lhpokervs;

    public bool isOpt = false;
    public bool isOpen = false;//是否开牌
    public bool isOK = false;//是否开始传参
    public bool isook = false;
    int a = 0;//龙的初始下注
    int b = 0;//和的初始下注
    int c = 0;//虎的初始下注
    //龙牌虎牌
    string t;
    string d;

    string Myid;
    //单例
	public static NewLongHu Instance;
	public static NewLongHu insatnce {
        get {
			if (Instance == null) Instance = new NewLongHu();
            return Instance;
        }
    }
    //发牌的链表
    //public List<GameObject> cardList;

    private void Awake()
    {
        Instance = this;
    }

    void Start() {
        line = 0;
        row = 0;
        lastPoint = 2;
        AllNo();
        Myid = LoginInfo.Instance().mylogindata.user_id;
        //isoo = false;
        //游戏开始开启pk特效
        StartCoroutine(ShowPK());
        //cardList = new List<GameObject>();//初始化卡牌链表
        tcpNet = NewTcpNet.GetInstance();
        //获取一下当前筹码面板
        nubText = transform.Find("BG/NubText").GetComponent<Text>();




        //【设置按钮监听事件】
		transform.Find("Buttons/SetBtn").GetComponent<Button>().onClick.AddListener(delegate {
            if (isok == false)
            {
				transform.Find("Buttons/SetBtn/Image").gameObject.SetActive(true);
                isok = true;
            }
            else
            {
				transform.Find("Buttons/SetBtn/Image").gameObject.SetActive(false);
                isok = false;
            }
        });
		transform.Find("Buttons/SetBtn/Image/Button1").GetComponent<Button>().onClick.AddListener(delegate {
            transform.Find("Information").gameObject.SetActive(true);
        });
        transform.Find("Information/BackBtn").GetComponent<Button>().onClick.AddListener(delegate {
            transform.Find("Information").gameObject.SetActive(false);
        });
        //筹码事件绑定
        transform.Find("BG/1").GetComponent<Button>().onClick.AddListener(delegate { transform.Find("BG/NubText").GetComponent<Text>().text = "1"; });
        transform.Find("BG/10").GetComponent<Button>().onClick.AddListener(delegate { transform.Find("BG/NubText").GetComponent<Text>().text = "10"; });
        transform.Find("BG/100").GetComponent<Button>().onClick.AddListener(delegate { transform.Find("BG/NubText").GetComponent<Text>().text = "100"; });
        transform.Find("BG/1000").GetComponent<Button>().onClick.AddListener(delegate { transform.Find("BG/NubText").GetComponent<Text>().text = "1000"; });
        
		//切换按钮事件绑定
		transform.Find("Buttons/QH").GetComponent<Button>().onClick.AddListener(ChipChange);

		longbetList = transform.Find ("LongBetList").gameObject;
		hubetList = transform.Find ("HuBetList").gameObject;
		hebetList = transform.Find ("HeBetList").gameObject;

		//龙和虎按钮事件绑定
		transform.Find("Buttons/Dragon").GetComponent<Button>().onClick.AddListener(delegate {
            if (isOpt == true)
            {
                //int dragon = a += Convert.ToInt32(nubText.text);
                //transform.Find("BG/Image5/DragonText").GetComponent<Text>().text = dragon.ToString();
                //下注事件传参
                //transform.Find("BG/NubText").GetComponent<Text>().text
                StartCoroutine(UpData(LoginInfo.Instance().mylogindata.URL +
                  LoginInfo.Instance().mylogindata.betDown_lh +
                  "user_id=" + Myid +
					"&num=" + chip +
                  "&room_id=" + roomid +
                  "&id=" + id)); 
            }
        });
		transform.Find("Buttons/And").GetComponent<Button>().onClick.AddListener(delegate {
            if (isOpt == true)
            {
                //int and = b += Convert.ToInt32(nubText.text);
                //transform.Find("BG/Image5/AndText").GetComponent<Text>().text = and.ToString();
                //下注事件传参
                StartCoroutine(UpData(LoginInfo.Instance().mylogindata.URL +
                 LoginInfo.Instance().mylogindata.betDown_lh +
                 "user_id=" + Myid +
					"&num=" + chip +
                 "&room_id=" + roomid +
                 "&id=" + id1));
            }
        });
		transform.Find("Buttons/Tiger").GetComponent<Button>().onClick.AddListener(delegate {
            if (isOpt == true)
            {
                //int tiger = c += Convert.ToInt32(nubText.text);
                //transform.Find("BG/Image5/TigerText").GetComponent<Text>().text = tiger.ToString();
                //下注事件传参
                StartCoroutine(UpData(LoginInfo.Instance().mylogindata.URL +
                 LoginInfo.Instance().mylogindata.betDown_lh +
                 "user_id=" + Myid +
					"&num=" + chip +
                 "&room_id=" + roomid +
                 "&id=" + id2));
            }
        });
        //结算面板返回按钮
        //transform.Find("EndImage/Back").GetComponent<Button>().onClick.AddListener(delegate {transform.Find("EndImage").gameObject.SetActive(false); });
        //隐分按钮点击事件
		transform.Find("Buttons/YButton").GetComponent<Button>().onClick.AddListener(delegate {
            if (Myid != null && gameid != null && roomid != null)
            {
                if (isoo == false)//隐分
                {
                    isoo = true;
                    StartCoroutine(NoShow(
                        LoginInfo.Instance().mylogindata.URL +
                        "is-score?" +
                        "user_id=" + Myid +
                        "&game_id=" + gameid +
                        "&room_id=" + roomid +
                        "&is_score=" + "1" 
                        ));
                    return;
                }
                
               if (isoo == true)//取消隐分
                {
                    isoo = false;
                    StartCoroutine(NoShow(
                        LoginInfo.Instance().mylogindata.URL +
                        "is-score?"+
                        "user_id=" + Myid +
                        "&game_id=" + gameid +
                        "&room_id=" + roomid +
                        "&is_score=" + "0"
                        ));
                    return;
                }
            }
        });

        //取消续押点击事件
		transform.Find("Buttons/Q").GetComponent<Button>().onClick.AddListener(delegate {
            StartCoroutine(BC(
                        LoginInfo.Instance().mylogindata.URL +
                        "lh-cancel-all?" +
                        "room_id=" + roomid  +
                        "&user_id=" + Myid));
        });



        //图集信息获取
        yyc = Resources.LoadAll<Sprite>("card");



		StartCoroutine(ShowLoading());

		//更新状态
		StartCoroutine(
			OnPolling
			(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.hallaliveAPI +
				"user_id=" + LoginInfo.Instance().mylogindata.user_id +
				"&unionuid=" + LoginInfo.Instance().mylogindata.token

			));

		//更新历史记录
		StartCoroutine(EndOpen(
			LoginInfo.Instance().mylogindata.URL +
			LoginInfo.Instance().mylogindata.winHistory +
			"game_id="+gameid 
		));
    }

	public int chip=1;

	//切换当前筹码值
	public void ChipChange()
	{
		switch (chip) {
		case 1:
			transform.Find ("Buttons/QH").GetComponent<Image> ().sprite = QH_sprite [1];
			chip = 10;
			break;
		case 10:
			transform.Find ("Buttons/QH").GetComponent<Image> ().sprite = QH_sprite [2];
			chip = 100;
			break;
		case 100:
			transform.Find ("Buttons/QH").GetComponent<Image> ().sprite = QH_sprite [3];
			chip = 500;
			break;
		case 500:
			transform.Find ("Buttons/QH").GetComponent<Image> ().sprite = QH_sprite [0];
			chip = 10;
			break;
		}
	}





	//检测当前在线状态
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
					transform.Find("BG/UserMessage/MenmyText").GetComponent<Text>().text ="金币:"+ LoginInfo.Instance().mylogindata.ALLScroce;
					if (jd["Userinfo"]["status"].ToString() == "2")
					{
						transform.Find ("ErrorPanel").gameObject.SetActive(true);
						transform.Find ("ErrorPanel/ErrorPanel/Text").GetComponent<Text> ().text = jd ["msg"].ToString ();
						yield return new WaitForSeconds(2f);
						SceneManager.LoadScene(0);
					}
				}
				else
				{
					
					transform.Find ("ErrorPanel").gameObject.SetActive(true);
					transform.Find ("ErrorPanel/ErrorPanel/Text").GetComponent<Text> ().text = jd ["msg"].ToString ();
					yield return new WaitForSeconds(2f);
					SceneManager.LoadScene(0);
				}

			}

			yield return new WaitForSeconds(4f);
		}
	}

	//显示加载内容
	IEnumerator ShowLoading()
	{
		transform.Find ("Panel").gameObject.SetActive(true);
		transform.Find ("Panel/Slider").GetComponent<Slider>().value = 0;
		while (true)
		{
			float a = UnityEngine.Random.Range(0.1f, 0.4f);
			transform.Find ("Panel/Slider").GetComponent<Slider>().value += a;
			if (transform.Find ("Panel/Slider").GetComponent<Slider>().value >= 1)
			{
				transform.Find ("Panel").gameObject.SetActive(false);
				//uniWebView.Hide();
				break;
			}
			yield return new WaitForSeconds(a);        
		}
	}

    //请求退出游戏
    IEnumerator Back(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
            }
        }
    }

    //取消押注
    IEnumerator BC(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
			
			if (www.responseCode == 200) {
				JsonData jd = JsonMapper.ToObject (www.downloadHandler.text);
				ShowMessageToShow (jd ["msg"].ToString ());

//				if (jd ["code"].ToString () != "200") {
//					
//				} else {
//				
//				}
			} 
        }
    }

    //隐分
    IEnumerator NoShow(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            }
            else
            {
                print("暂不能隐分");
            }
        }
    }

    //请求押分记录
    IEnumerator JShow(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200) {

            }
        }
    }




	public GameObject errorGo;
	public GameObject errorGoParent;

	private GameObject oldErrorGo;
	//错误提示
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

    //清空左链表
    public void AllNo()
    {
		line = 0;
		row = 0;
		resultCount = 0;

        for (int i = 0; i < transform.Find("BG/Scroll View/Viewport/Content").childCount ; i++)
        {
            transform.Find("BG/Scroll View/Viewport/Content").GetChild(i).GetChild(0).gameObject.SetActive(false);
        } 

    }

	void AddPoker(int num)
	{
		if (resultCount >= 66)
		{
			AllNo();
		}
		switch (num)
		{
		case 0:
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).GetComponent<Image>().sprite = tr[0];
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).gameObject.SetActive(true);

			break;
		case 1:
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).GetComponent<Image>().sprite = tr[1];
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).gameObject.SetActive(true);

			break;
		case 2:
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).GetComponent<Image>().sprite = tr[2];
			GameObject.Find("BG/ListImage").transform.GetChild(resultCount).gameObject.SetActive(true);
			break;
		}
		resultCount++;

		AddPokerPoint(num);
	}



	void AddPokerPoint(int num)
	{
		AddPointRule(num);

	}

    /// <summary>
    /// 计时器上的提示框
    /// </summary>
    public void Show_Tips(string data) {
        if (isOpt == true)
        {
            transform.Find("TimeImage/Tips").GetComponent<Text>().text = data;
            transform.Find("TimeImage/Tips").gameObject.SetActive(true);
        }
        //transform.Find("BG/RedImage").gameObject.SetActive(true);
        //transform.Find("BG/RedImage/Text").transform.Translate(Vector3.left * 30 * Time.deltaTime);
        //if (Vector3.Distance(transform.Find("BG/RedImage/Text").transform.position, new Vector3(0, 245, 0)) == 0)
        //{
        if (isOpt == false)
        {
			transform.Find("TimeImage/Tips").gameObject.SetActive(false);
        }
    }

    //请求结果
    IEnumerator End(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log (url);
        yield return www.Send();
        if (www.error == null && www.isDone) {
            if (www.responseCode == 200)
            {
				JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
				if(jd["code"].ToString()=="200")
				{
					
					if (jd["oddWinList"]["A"]!=null )
					{
						transform.Find("EndImage/LText").GetComponent<Text>().text = ((int)float.Parse(jd["oddWinList"]["A"].ToString())).ToString();
						transform.Find("EndImage/AText").GetComponent<Text>().text = ((int)float.Parse(jd["oddWinList"]["B"].ToString())).ToString();
						transform.Find("EndImage/HText").GetComponent<Text>().text = ((int)float.Parse(jd["oddWinList"]["C"].ToString())).ToString();
						transform.Find("EndImage/EText").GetComponent<Text>().text = ((int)float.Parse(jd["userWinTotal"].ToString())).ToString();
						isend = false;
					}
					transform.Find("pk").gameObject.SetActive(false);
					transform.Find("LC").localPosition = transform.Find("lcpos").localPosition;
					transform.Find("HC").localPosition = transform.Find("hcpos").localPosition;
					transform.Find("1").localPosition = transform.Find("1pos").localPosition;
					transform.Find("2").localPosition = transform.Find("2pos").localPosition;
					transform.Find("DI").gameObject.SetActive(false);
					transform.Find("TI").gameObject.SetActive(false);
					transform.Find("DW").gameObject.SetActive(false);
					transform.Find("TW").gameObject.SetActive(false);
					transform.Find("AI").gameObject.SetActive(false);
					transform.Find("EndImage").gameObject.SetActive(true);
				}
            }
            else
            {
                yield return null;
            }
        }
    }

    //请求开奖记录List(右)
    IEnumerator EndOpen(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
				AllNo ();
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
				if (jd ["code"].ToString () == "200") {
					for (int i = 0; i < jd ["ArrList"].Count; i++) {
						AddPoker (int.Parse (jd ["ArrList"] [i] ["winnings"].ToString ()));
//                    if (jd["ArrList"][i]["winnings"].ToString() == "0") {
//                        transform.Find("BG/ListImage").GetChild(i).GetComponent<Image>().sprite = tr[0];
//                        transform.Find("BG/ListImage").GetChild(i).gameObject.SetActive(true);
//                    }
//                    if (jd["ArrList"][i]["winnings"].ToString() == "1")
//                    {
//                        transform.Find("BG/ListImage").GetChild(i).GetComponent<Image>().sprite = tr[1];
//                        transform.Find("BG/ListImage").GetChild(i).gameObject.SetActive(true);
//                    }
//                    if (jd["ArrList"][i]["winnings"].ToString() == "2")
//                    {
//                        transform.Find("BG/ListImage").GetChild(i).GetComponent<Image>().sprite = tr[2];
//                        transform.Find("BG/ListImage").GetChild(i).gameObject.SetActive(true);
						//                  }

					}
				}
                
            }
        }
    }

    //接收到玩家初始化信息
    IEnumerator getversioningame(string uni)
    {
		
        UnityWebRequest www = UnityWebRequest.Get(uni);
		Debug.Log(uni);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
                print(www.downloadHandler.text);
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                transform.Find("BG/UserMessage/UserName").GetComponent<Text>().text = ("昵称:" + jd["Userinfo"]["username"].ToString());
                transform.Find("BG/UserMessage/IDText").GetComponent<Text>().text = ("ID:" + jd["Userinfo"]["user_id"].ToString());
                transform.Find("BG/UserMessage/MenmyText").GetComponent<Text>().text = ("金币:" + jd["Userinfo"]["quick_credit"].ToString());
               
            }
        }
        else
        {

        }
    }

	//更新下注信息
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
					
				}
			}
		}
	}

    //www连接所需参数
    string u_id;
    string uni;
    string id,id1,id2;//龙和虎ID
    string num;
    string roomid;
    string seat;//座位号
    string DT;
    string TT;
    string AT;
    string gameid;
    bool isshow = false;
    string dropdata;
    string HP, LP;//获取龙牌虎牌的权值

    //游戏一开始的数据获取
    public void GameStart(string data)
    {
        print("yyc收到的数据为:" + data);
        JsonData jd = JsonMapper.ToObject(data);
        if (jd["code"].ToString () == "200") {
            uni = jd["userData"][0]["unionuid"].ToString();
            u_id = jd["userData"][0]["user_id"].ToString();
        }
    }//无效方法(弃用)

    //拿到用户初始化信息
    public void UserData(UnityWebRequest data)
    {
        JsonData jd = JsonMapper.ToObject(data.downloadHandler .text);
        transform.Find("BG/UserMessage/UserName").GetComponent<Text>().text = ("昵称:" + jd["username"].ToString());
        transform.Find("BG/UserMessage/IdText").GetComponent<Text>().text = ("ID:" + jd["id"].ToString());
        transform.Find("BG/UserMessage/MenmyText").GetComponent<Text>().text = ("金币:" + jd["quick_credit"].ToString());
    }//无效方法(弃用)

    
	//点击押注,信息上传
     IEnumerator UpData(string data) {
        UnityWebRequest www = UnityWebRequest.Get(data);
        yield return www.Send();
        if (www.error == null && www.isDone)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
			if (jd ["code"].ToString () == "200") {
				Debug.Log ("押注成功"+jd.ToJson().ToString());
				transform.Find ("BG/UserMessage/MenmyText").GetComponent<Text> ().text = jd ["quick_credit"].ToString ();
			} else {
				try {
					ShowMessageToShow (jd["msg"].ToString());
				} catch (Exception ex) {
					
				}

			}
               
        }
    }
	private bool isPlayOver;
	private bool isGetResult;
	private bool isFirstJoin;

	private string Lpai="";
	private string Hpai="";

    string tips;
    //拿到后台的数据--期数信息,倒计时
    public void PollingPeriods(JsonData data)
    {
        //更新  时间 局数 轮数
        transform.Find("TimeImage/ShowTime").GetComponent<Text>().text = data["countdown"].ToString();
		transform.Find("BG/JuText").GetComponent<Text>().text = data["periods"].ToString();
		transform.Find("BG/ChangText").GetComponent<Text>().text = data["season"].ToString();


        //刷新中奖信息
        if (data["is_win"].ToString() != "0" || (data["is_win"].ToString() == "0" && data["countdown"].ToString() == "0"))
        {
            Show_Tips("正在开奖");
        }
        else
        {
            Show_Tips("下注中");
        }

        //记录龙牌
        if (data["lpai"].ToString()!="")
        {
            Lpai = data["lpai"].ToString();
        }

        //记录虎牌
        if (data["hpai"].ToString()!="")
        {
            Hpai = data["hpai"].ToString();
        }


        /*
		if (data["is_win"].ToString() == "0" && data["countdown"].ToString() == "0")
        {
            string aaa;
            isOpt = true;
            aaa  = "正在开奖";
            Show_Tips(aaa);
        }

		//龙虎牌获取
		try {
			if (data["countdown"].ToString() == "0"&& data["lpai"]!=null)
			{
				Lpai=data["lpai"].ToString();
				Hpai=data["hpai"].ToString();
				lhpokervs.SetValues(Lpai,Hpai);
			}
		} catch (Exception ex) {
			
		}


		if (data ["is_win"].ToString () == "0") {
			if(!isFirstJoin)
			{
				isFirstJoin = true;
			}
			if(isGetResult)
			{
				isGetResult	=false;
			}
			if (data ["countdown"].ToString () != "0" && !isPlayOver) {
				//唯一次运行
				isPlayOver = true;
				//开始压分
				Audiomanger._instenc.PlayTip(0);
				ClearTable ();

			} else if (data ["countdown"].ToString () == "0" && isPlayOver){
				//停止压分
				Audiomanger._instenc.PlayTip(1);
				isPlayOver = false;
			}


		} else if(data ["is_win"].ToString () == "1"){
		

		}else if(data ["is_win"].ToString () == "2"&&!isGetResult)
		{
			if (data ["winnings"].ToString () == "") {
				//没拿到结果
			} else {
				if (isFirstJoin) {



					isGetResult = true;
				} else {
					//中途加入的
					isGetResult=true;
				}
			}
		}



		/*

        if (data["is_win"].ToString() == "0" && data["countdown"].ToString ()=="30")
        {
           
        }
        if (data["is_win"].ToString() == "0"&&int.Parse (data["countdown"].ToString())>5)
        {
            transform.Find("Win_A").gameObject.SetActive(false);
            transform.Find("Win_T").gameObject.SetActive(false);
            transform.Find("Win_D").gameObject.SetActive(false);
            //transform.Find("BG/t/CardPfb").gameObject.SetActive(false);
            //transform.Find("BG/d/CardPfb").gameObject.SetActive(false);
			transform.Find("VS/LC").localPosition = new Vector3(690, 175, 0);
			transform.Find("VS/HC").localPosition = new Vector3(-690, 175, 0);
            for (int i = 0; i < yyc.Length ; i++)
            {
                if (yyc[i].name == "card_52") {
					transform.Find("VS/LC").GetComponent<Image>().sprite = yyc[i];
					transform.Find("VS/HC").GetComponent<Image>().sprite = yyc[i];
                }
            }
            
            isend = false;
            LP = null;
            HP = null;
			transform.Find("VS/pk").gameObject.SetActive(false);
			transform.Find("VS/1").localPosition = transform.Find("1pos").localPosition;
			transform.Find("VS/2").localPosition = transform.Find("2pos").localPosition;
			transform.Find("VS/LC").localPosition = transform.Find("lcpos").localPosition;
			transform.Find("VS/HC").localPosition = transform.Find("hcpos").localPosition;
			transform.Find("VS/DI").gameObject.SetActive(false);
			transform.Find("VS/TI").gameObject.SetActive(false);
			transform.Find("VS/DW").gameObject.SetActive(false);
			transform.Find("VS/TW").gameObject.SetActive(false);
			transform.Find("VS/AI").gameObject.SetActive(false);
            transform.Find("EndImage").gameObject.SetActive(false);
            isOpt = true;
            tips = "可以下注";
            Show_Tips(tips);
            
            //第几局第几场显示
            transform.Find("BG/JuText").GetComponent<Text>().text = data["periods"].ToString();
            transform.Find("BG/ChangText").GetComponent<Text>().text = data["season"].ToString();
            dropdata = data["drop_date"].ToString();
        }
        if(int.Parse(data["countdown"].ToString()) < 5)
        {
            string stop;
            isOpt = true;
            stop = "暂停下注";
            Show_Tips(stop);
        }
        if (data["is_win"].ToString() == "1" || data["is_win"].ToString() == "2")
        {
            isOpen = true;
			HP = data["hpai"].ToString();
			LP = data["lpai"].ToString();
        }
        //if (data["is_win"].ToString() == "0" && data["countdown"].ToString() == "5")
        //{
           // Debug.Log("开始发牌");
            //transform.Find("BG/t/CardPfb").gameObject.SetActive(true);
            //transform.Find("BG/d/CardPfb").gameObject.SetActive(true);

        //}
        if (data["is_win"].ToString() == "0" && data["countdown"].ToString() == "0")
        {
            HP = data["hpai"].ToString();
            LP = data["lpai"].ToString();
            isend = true;
            
        }
        if (data["is_win"].ToString() == "2" && data["winnings"] != null) {
            //开启请求结果的协程
            StartCoroutine(End(
                 LoginInfo.Instance().mylogindata.URL +
                 LoginInfo.Instance().mylogindata.winInfo +
                 "game_id="+gameid+
                 "&room_id="+ roomid+
                 "&user_id="+Myid 
                ));
            //开启请求开奖记录协程
            StartCoroutine(EndOpen(
                 LoginInfo.Instance().mylogindata.URL +
                 LoginInfo.Instance().mylogindata.winHistory +
                 "game_id="+gameid 
                ));

            if (data["winnings"].ToString() == "0") {
                transform.Find("Win_D").gameObject.SetActive(true);
            }
            if (data["winnings"].ToString() == "1")
            {
                transform.Find("Win_T").gameObject.SetActive(true);
            }
            if (data["winnings"].ToString() == "2")
            {
                transform.Find("Win_A").gameObject.SetActive(true);
            }
        }
        */
    }

   //拿到后台的数据--用户当前下注信息,初始化信息
   public void UpdateSuit(JsonData data)
       {
        //获取人物初始化信息
        if (data["code"].ToString()== "200")
        {
            Myid = LoginInfo.Instance().mylogindata.user_id;
            for (int i = 0; i < data["userData"].Count ; i++)
            {
                if (data["userData"][i]["user_id"].ToString ()==Myid )
                {
                    uni = data["userData"][i]["unionuid"].ToString();
                    gameid = data["userData"][i]["game_id"].ToString();
                    roomid = data["userData"][i]["room_id"].ToString();
                    seat = data["userData"][i]["seat_number"].ToString();
                }
            }

			//
            id = data["oddlist"][0]["id"].ToString();
            id1 = data["oddlist"][1]["id"].ToString();
            id2 = data["oddlist"][2]["id"].ToString();
            num = data["oddlist"][0]["num"].ToString();

			if (XianHong == null) {
				XianHong = transform.Find ("BG/GradeImage/XianHong").GetComponent<Text> ();
				XianHong.text= data["oddlist"][0]["single_limit"].ToString();
			} else {
				XianHong.text= data["oddlist"][0]["single_limit"].ToString();
			}
			if (ZuiDiXianZhu == null) {
				ZuiDiXianZhu = transform.Find ("BG/GradeImage/ZuiDiXianZhu").GetComponent<Text>();
				ZuiDiXianZhu.text=LoginInfo.Instance().mylogindata.roomcount;
			} else {
				ZuiDiXianZhu.text=LoginInfo.Instance().mylogindata.roomcount;
			}
			if (HeXianZhu == null) {
				HeXianZhu = transform.Find ("BG/GradeImage/HeXianZhu").GetComponent<Text>();
				HeXianZhu.text=data["oddlist"][1]["single_limit"].ToString();
			} else {
				HeXianZhu.text=data["oddlist"][1]["single_limit"].ToString();
			}







			//下注赔率显示
            DT = data["oddlist"][0]["rate"].ToString();
            TT = data["oddlist"][2]["rate"].ToString();
            AT = data["oddlist"][1]["rate"].ToString();

			//所有玩家基础信息面板
			if(Userslist==null)
			{
				Userslist = transform.Find ("BG/Users").gameObject;
			}
			//玩家资金
			if(Goldslist==null)
			{
				Goldslist=transform.Find ("BG/Golds").gameObject;
			}

			for (int i = 0; i < data["userData"].Count; i++) {
				Userslist.transform.GetChild(int.Parse(data["userData"][i]["seat_number"].ToString())-1).GetChild(0).GetComponent<Text>().text= data["userData"][i]["userInfo"]["id"].ToString();
				Userslist.transform.GetChild(int.Parse(data["userData"][i]["seat_number"].ToString())-1).GetChild(1).GetComponent<Text>().text= data["userData"][i]["userInfo"]["username"].ToString();;
				Userslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);

				if (float.Parse (data ["userData"] [i] ["userInfo"] ["quick_credit"].ToString ()) > 0) {
					Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = ((int)float.Parse (data ["userData"] [i] ["userInfo"] ["quick_credit"].ToString ())).ToString ();
					Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
				} else {
					Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
				}

				if (data ["userData"] [i] ["is_score"].ToString () == "1") {
					//隐分
					transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (true);
				} else {
					//显分
					transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (false);
				}
			}

//            //初始化房间隐分显示
//            for (int i = 0; i < data["userData"].Count; i++)
//            {
//                //用户隐分显示
//				if (data ["userData"] [i] ["is_score"].ToString () == "1") {
//					//隐分
//					transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (true);
//				} else {
//					//显分
//					transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (false);
//				}
//                if (data["userData"][i]["is_score"].ToString() == "0")//取消隐分
//                {
//                    for (int q = 0; q < transform.Find("YF").childCount; q++)
//                    {
//                        if (transform.Find("YF").GetChild(q).name == "Image" + data["userData"][i]["seat_number"].ToString())
//                        {
//                            transform.Find("YF").GetChild(q).gameObject.SetActive(false);
//                        }
//                    }
//                }
//            }

            //初始化看到玩家押注信息
//            for (int i = 0; i < transform.Find("LhList").childCount; i++)
//            {
//                for (int j = 0; j < data["userData"].Count; j++)
//                {
//                    if (data["userData"][j]["A"]["user_dnum"].ToString() != "0")
//                    {
//                        if (transform.Find("LhList").GetChild(i).name == "User" + data["userData"][j]["seat_number"].ToString() + "_" + "1")
//                        {
//                            transform.Find("LhList").GetChild(i).transform.Find("Text").GetComponent<Text>().text = data["userData"][j]["A"]["user_dnum"].ToString();
//                            transform.Find("LhList").GetChild(i).gameObject.SetActive(true);
//                            transform.Find("BG/Image5/DragonText").GetComponent<Text>().text = data["userData"][j]["A"]["user_dnum"].ToString();
//                        }
//                    }
//                    if (data["userData"][j]["B"]["user_dnum"].ToString() != "0")
//                    {
//                        if (transform.Find("LhList").GetChild(i).name == "User" + data["userData"][j]["seat_number"].ToString() + "_" + "3")
//                        {
//                            transform.Find("LhList").GetChild(i).transform.Find("Text").GetComponent<Text>().text = data["userData"][j]["B"]["user_dnum"].ToString();
//                            transform.Find("LhList").GetChild(i).gameObject.SetActive(true);
//                            transform.Find("BG/Image5/AndText").GetComponent<Text>().text = data["userData"][j]["C"]["user_dnum"].ToString();
//                        }
//                        if (data["userData"][j]["C"]["user_dnum"].ToString() != "0")
//                        {
//                            if (transform.Find("LhList").GetChild(i).name == "User" + data["userData"][j]["seat_number"].ToString() + "_" + "2")
//                            {
//                                transform.Find("LhList").GetChild(i).transform.Find("Text").GetComponent<Text>().text = data["userData"][j]["C"]["user_dnum"].ToString();
//                                transform.Find("LhList").GetChild(i).gameObject.SetActive(true);
//                                transform.Find("BG/Image5/TigerText").GetComponent<Text>().text = data["userData"][j]["C"]["user_dnum"].ToString();
//                            }
//
//                        }
//                    }
//                }
//            }
			//遍历  列表
			for (int i = 0; i < data["userData"].Count; i++) {
				if(data["userData"][i]["userInfo"]["id"].ToString()==Myid)
				{
					//是我
					transform.Find("BG/Image5/DragonText").GetComponent<Text>().text= data ["userData"] [i] ["A"] ["user_dnum"].ToString ();
					transform.Find("BG/Image5/AndText").GetComponent<Text>().text= data ["userData"] [i] ["B"] ["user_dnum"].ToString ();
					transform.Find("BG/Image5/TigerText").GetComponent<Text>().text= data ["userData"] [i] ["C"] ["user_dnum"].ToString ();

				}
					

				//龙
				if (data ["userData"] [i] ["A"] ["user_dnum"].ToString () != "0") {
					longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["A"] ["user_dnum"].ToString ();
					longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
				} else {
					longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
				}
				//虎
				if (data ["userData"] [i] ["C"] ["user_dnum"].ToString () != "0") {
					hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["C"] ["user_dnum"].ToString ();
					hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
				} else {
					hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
				}
				//和
				if (data ["userData"] [i] ["B"] ["user_dnum"].ToString () != "0") {
					hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["B"] ["user_dnum"].ToString ();
					hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
				} else {
					hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
				}
			}




//            for (int i = 0; i < data["userData"].Count ; i++)
//            {
//                //用户信息
//                for (int j = 0; j < transform.Find("BG/Image").childCount; j++)
//                {
//                    if (transform.Find("BG/Image").GetChild(j).name == "User" + data["userData"][i]["seat_number"])
//                    {
//                        transform.Find("BG/Image").GetChild(j).transform.Find("Text").GetComponent<Text>().text = data["userData"][i]["userInfo"]["id"].ToString();
//                        transform.Find("BG/Image").GetChild(j).transform.Find("Text (1)").GetComponent<Text>().text = data["userData"][i]["userInfo"]["quick_credit"].ToString();
//                        transform.Find("BG/Image").GetChild(j).gameObject.SetActive(true);
//                    }
//                }
//                //用户金币显示
//                for (int q = 0; q < transform.Find("BG/Image").childCount; q++)
//                {
//                    if (transform.Find("BG/Image").GetChild(q).name == "Image" + data["userData"][i]["seat_number"])
//                    {
//                        transform.Find("BG/Image").GetChild(q).transform.Find("Text").GetComponent<Text>().text = data["userData"][i]["userInfo"]["quick_credit"].ToString();
//                        transform.Find("BG/Image").GetChild(q).gameObject.SetActive(true);
//                    }
//                }
//            }
        }  
       }

    //拿到后台的数据--用户当前座位，用户余额，当前下注，其他用户的下注
    public void OddList(JsonData data)
    {
		//遍历  列表
		for (int i = 0; i < data["userData"].Count; i++) {
			if(data["userData"][i]["userInfo"]["id"].ToString()==Myid)
			{
				//是我
				transform.Find("BG/Image5/DragonText").GetComponent<Text>().text= data ["userData"] [i] ["A"] ["user_dnum"].ToString ();
				transform.Find("BG/Image5/AndText").GetComponent<Text>().text= data ["userData"] [i] ["B"] ["user_dnum"].ToString ();
				transform.Find("BG/Image5/TigerText").GetComponent<Text>().text= data ["userData"] [i] ["C"] ["user_dnum"].ToString ();

			}

			//龙
			if (data ["userData"] [i] ["A"] ["user_dnum"].ToString () != "0") {
				longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["A"] ["user_dnum"].ToString ();
				longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
			} else {
				longbetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
			}
			//虎
			if (data ["userData"] [i] ["C"] ["user_dnum"].ToString () != "0") {
				hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["C"] ["user_dnum"].ToString ();
				hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
			} else {
				hubetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
			}
			//和
			if (data ["userData"] [i] ["B"] ["user_dnum"].ToString () != "0") {
				hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = data ["userData"] [i] ["B"] ["user_dnum"].ToString ();
				hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
			} else {
				hebetList.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
			}
		}
        
        //所有玩家基础信息面板
		if(Userslist==null)
		{
			Userslist = transform.Find ("BG/Users").gameObject;
		}
		//玩家资金
		if(Goldslist==null)
		{
			Goldslist=transform.Find ("BG/Golds").gameObject;
		}

		for (int i = 0; i < data["userData"].Count; i++) {
			Userslist.transform.GetChild(int.Parse(data["userData"][i]["seat_number"].ToString())-1).GetChild(0).GetComponent<Text>().text= data["userData"][i]["userInfo"]["id"].ToString();
			Userslist.transform.GetChild(int.Parse(data["userData"][i]["seat_number"].ToString())-1).GetChild(1).GetComponent<Text>().text= data["userData"][i]["userInfo"]["username"].ToString();;
			Userslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);

			if (float.Parse (data ["userData"] [i] ["userInfo"] ["quick_credit"].ToString ()) > 0) {
				Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).GetChild (0).GetComponent<Text> ().text = ((int)float.Parse (data ["userData"] [i] ["userInfo"] ["quick_credit"].ToString ())).ToString ();
				Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (true);
			} else {
				Goldslist.transform.GetChild (int.Parse (data ["userData"] [i] ["seat_number"].ToString ()) - 1).gameObject.SetActive (false);
			}

			if (data ["userData"] [i] ["is_score"].ToString () == "1") {
				//隐分
				transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (true);
			} else {
				//显分
				transform.Find ("YF").GetChild (int.Parse(data ["userData"] [i] ["seat_number"].ToString ())-1).gameObject.SetActive (false);
			}
		}
    }

    int line, row, lastPoint;
	int resultCount;
    /// <summary>
    /// 添加点的规则
    /// </summary>
    void AddPointRule(int pointSever)
    {

        if (lastPoint == 2) //则代表为第一次
        {
            transform .Find ("BG/Scroll View/Viewport/Content").transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = left[pointSever];
            transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;

        }
        else if (lastPoint == pointSever)
        {

            transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = left[pointSever];
            transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
            lastPoint = pointSever;
        }
        else if (lastPoint != pointSever)
        {
            if (pointSever == 2)
            {
                transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = left[pointSever];
                transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (line != 0)
                {
                    line = 0;
                    row++;
                    if (row > 42)
                    {

                        for (int i = 0; i < transform.Find("BG/Scroll View/Viewport/Content").transform.childCount; i++)
                        {
                            transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                        }
                        line = 0;
                        row = 0;

                    }
                }
                transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).GetComponent<Image>().sprite = left[pointSever];
                transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild((line + (row * 6))).GetChild(0).gameObject.SetActive(true);
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

                for (int i = 0; i < transform.Find("BG/Scroll View/Viewport/Content").transform.childCount; i++)
                {
                    transform.Find("BG/Scroll View/Viewport/Content").transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }

                row = 0;

            }
        }
    }

    //比大小
    public int big(string nub)
    {
        if (nub == "0")
        {
            return 10;
        }
        if (nub == "1")
        {
            return 1;
        }
        if (nub == "2")
        {
            return 2;
        }
        if (nub == "3")
        {
            return 3;
        }
        if (nub == "4")
        {
            return 4;
        }
        if (nub == "5")
        {
            return 5;
        }
        if (nub == "6")
        {
            return 6;
        }
        if (nub == "7")
        {
            return 7;
        }
        if (nub == "8")
        {
            return 8;
        }
        if (nub == "9")
        {
            return 9;
        }
        if (nub == "j")
        {
            return 11;
        }
        if (nub == "q")
        {
            return 12;
        }
        if (nub == "k")
        {
            return 13;
        }
        return 0;
    }

	/// <summary>
	/// 清理桌面（主要进行筹码清理，玩家下注信息清空）
	/// </summary>
	public void ClearTable()
	{
		//清空玩家押分
		for (int i = 0; i < longbetList.transform.childCount; i++)
		{
			longbetList.transform.GetChild(i).gameObject.SetActive(false);
			hubetList.transform.GetChild(i).gameObject.SetActive(false);
			hebetList.transform.GetChild(i).gameObject.SetActive(false);
		}

		transform.Find ("BG/Image5/DragonText").GetComponent<Text> ().text = "0";
		transform.Find("BG/Image5/AndText").GetComponent<Text>().text = "0";
		transform.Find("BG/Image5/TigerText").GetComponent<Text>().text = "0";

		transform.Find("Win_D").gameObject.SetActive(false);
		transform.Find("Win_T").gameObject.SetActive(false);
		transform.Find("Win_A").gameObject.SetActive(false);
	}


    //开奖动画
    public void ShowOpen()
    {
        if (isend == true)
        {
			transform.Find("VS/1").localPosition = Vector3.MoveTowards(transform.Find("VS/1").localPosition, transform .Find ("1to").localPosition, speed);
			transform.Find("VS/2").localPosition = Vector3.MoveTowards(transform.Find("VS/2").localPosition, transform .Find ("2to").localPosition, speed);
			transform.Find("VS/TI").gameObject.SetActive(true);
			transform.Find("VS/DI").gameObject.SetActive(true);
            

			if (transform.Find("VS/1").localPosition == transform .Find ("1to").localPosition )
            {
				transform.Find("VS/LC").localPosition = Vector3.MoveTowards(transform.Find("VS/LC").localPosition, transform .Find ("lcto").localPosition, speed);
				transform.Find("VS/HC").localPosition = Vector3.MoveTowards(transform.Find("VS/HC").localPosition, transform .Find ("hcto").localPosition, speed);
            }
			if (transform.Find("VS/LC").localPosition==transform .Find ("lcto").localPosition)
            {
                //开牌
                for (int i = 0; i < yyc.Length; i++)
                {
                    //Debug.Log("开牌");

                    if (yyc[i].name == ("Card" + HP))
                    {
						transform.Find("VS/HC").GetComponent<Image>().sprite = yyc[i];
						transform.Find("VS/HC").gameObject.SetActive(true);
                    }
                    if (yyc[i].name == ("Card" + LP))
                    {
						transform.Find("VS/LC").GetComponent<Image>().sprite = yyc[i];
						transform.Find("VS/LC").gameObject.SetActive(true);
                    }

                    //预出结果
					if(LP.Length>1&&HP.Length>1)
					{
						string a = LP.Substring(1, 1);
						string b = HP.Substring(1, 1);
						if (a != null && b != null)
						{
							if (big(a) > big(b))//龙赢
							{
								//isend = true;
								transform.Find("VS/DW").gameObject.SetActive(true);
								transform.Find("VS/DI").gameObject.SetActive(false);
								transform.Find("VS/TI").gameObject.SetActive(true);
								transform.Find("VS/pk").gameObject.SetActive(true);

							}
							if (big(a) < big(b))//虎赢
							{
								//isend = true;
								transform.Find("VS/TW").gameObject.SetActive(true);
								transform.Find("VS/TI").gameObject.SetActive(false);
								transform.Find("VS/DI").gameObject.SetActive(true);
								transform.Find("VS/pk").gameObject.SetActive(true);
							}
							if (big(a) == big(b))//和赢
							{
								//isend = true;
								transform.Find("VS/DI").gameObject.SetActive(true);
								transform.Find("VS/TI").gameObject.SetActive(true);
								transform.Find("VS/AI").gameObject.SetActive(true);
								transform.Find("VS/pk").gameObject.SetActive(true);
							}
						}
					}
                }
            }
        }
    }

    //PK特效
    IEnumerator ShowPK()
    {
//        while (true) {
//            for (int i = 0; i < pk.Length ; i++)
//            {
//			transform.Find("VS/pk").GetComponent<Image>().sprite = pk[i];
//                yield return new WaitForSeconds(0.1f);
//            }
//        }

		yield return null;
    }

    float showtime = 0;
    float yy = 0;
    float cc = 0;
    float dd = 0;
    float speed = 10;
        void Update()
        {
        //提示框逻辑
        //  Show_Tips();

        //总押分

        if (uni != null&&isOK ==false)
        {
            StartCoroutine(getversioningame(
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.hallaliveAPI +
                "user_id="+Myid+
                "&unionuid="+uni 
                ));
            isOK = true;
        }

        //开局拿到开奖信息
        if (gameid !=null&&isook ==false)
        {
            StartCoroutine(EndOpen(
                 LoginInfo.Instance().mylogindata.URL +
                 LoginInfo.Instance().mylogindata.winHistory +
                 "game_id=" + gameid 
                ));
            isook = true;
        }

        //开奖特效
        ShowOpen();

        //下注赔率显示
        transform.Find("TText").GetComponent<Text>().text = "1:" + TT;
        transform.Find("DText").GetComponent<Text>().text = "1:" + DT;
        transform.Find("AText").GetComponent<Text>().text = "1:" + AT;

        if (isshow == true) {
            showtime += Time.deltaTime;
            if (showtime >= 3.0f) {
                transform.Find("BG/t/CardPfb").gameObject.SetActive(false);
                transform.Find("BG/d/CardPfb").gameObject.SetActive(false);
                isshow = false;
                showtime = 0;
            }
        }
    }







	//切换到外面在切回来
	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			OnLogin onLo = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
			string str = JsonMapper.ToJson(onLo);

			tcpNet.SendMessage(str);
			StartCoroutine(EndOpen(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.winHistory +
				"game_id="+gameid 
			));
				
		}
	}
} 
