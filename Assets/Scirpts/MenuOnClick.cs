using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 查找在线玩家
/// </summary>
[System.Serializable]
public class PlayerInfo
{
	public string seat; //座位
	public string playerId; //id
}

//单挑
[Serializable]
public class History_mpzzs
{
	public string num;  //第几条战绩
	public string playerId; //id
	public string round; //是哪一轮
	public string spade; //黑桃
	public string heart; //红桃
	public string club; //草花
	public string diamond; //方块
	public string trump; //王
	public string winPoker; //开奖
	public string allBet; //总压分
	public string winScore; //总赢分
	public string allScore; //总积分
}

//缺一门
[Serializable]
public class History_qym
{
	public string num;  //第几条战绩
	public string playerId; //id
	public string round; //是哪一轮
	public string spade; //黑桃
	public string heart; //红桃
	public string club; //草花
	public string trump; //王
	public string winPoker; //开奖
	public string allBet; //总压分
	public string winScore; //总赢分
	public string allScore; //总积分
}

//百乐
[Serializable]
public class History_bl
{
	public string num;
	public string playerId;
	public string round;
	public string zhuang;
	public string he;
	public string xian;
	public string kaiJiang;
	public string allScore;
	public string allWinScore;
	public string allGold;
}

//天地
[Serializable]
public class History_td
{
	public string num;
	public string playerId;
	public string round;
	public string tian;
	public string di;
	public string kaiJiang;
	public string allScore;
	public string allWinScore;
	public string allGold;
}

//夏威夷
[Serializable]
public class History_xwy
{
    public string num;
    public string playerId;
    public string round;
    public string red;
    public string grren;
    public string kaiJiang;
    public string allScore;
    public string allWinScore;
    public string allGold;
}


public class MenuOnClick : MonoBehaviour
{
	public GameObject MenuPanel;

	public List<PlayerInfo> playerInfo;
	public List<History_mpzzs> history_mpzzs;
	public List<History_bl> history_bl;
	public List<History_td> history_td;
	public List<History_qym> history_qym;
    public List<History_xwy> history_xwy;

	public Button cancelBtn;  //返回
	public Button changeBtn;  //切换路单
	public Button playerBtn;  //在线玩家
	public Button methodBtn;  //玩法说明
	public Button noticeBtn;  //公告声明
	public Button historyBtn; //历史记录
	public Button quiteBtn; //返回面板上的退出按钮

	public GameObject cancelPanel;
	public GameObject playerPanel;
	public GameObject methodPanel;
	public GameObject noticePanel;
	public GameObject historyPanel;

	public List<GameObject> historyGo;
	public List<GameObject> playerInfoGrid;

	int onLineCount;
	int historyCount;
	GameObject webSocket;
	// Use this for initialization




	private void Awake()
	{
		//LoadWebSocket();
		GameObject obj= Instantiate(Resources.Load("Notice") as GameObject, MenuPanel.transform, false);
		obj.transform.SetAsFirstSibling();
	}

    //已经弃用
	void LoadWebSocket()
	{
		#if UNITY_IOS
		if (Screen.width == 2436)
		{

		webSocket = Instantiate(Resources.Load("WebSocketForX") as GameObject,GameObject.Find("Canvas").transform,false);   //为iphoneX适配
		return;
		}
		#endif
		webSocket = Instantiate(Resources.Load("WebSocket") as GameObject,GameObject.Find("Canvas").transform,false);
	}

    //初始化添加按钮
	void Start()
	{
		if (cancelBtn!=null)//返回
		{
			cancelBtn.onClick.AddListener(OnCancel);
		}
		if (methodBtn != null)//玩法
		{
			methodBtn.onClick.AddListener(OnMethod);
		}
		if (noticeBtn != null)//公告
		{
			noticeBtn.onClick.AddListener(OnNotice);
		}
		if (quiteBtn != null)//退出
		{
			quiteBtn.onClick.AddListener(OnQuite);
		}
		if (playerBtn != null)//在线玩家
		{
			playerBtn.onClick.AddListener(OnPalyer);
		}
		if (historyBtn != null)//历史记录
		{
			historyBtn.onClick.AddListener(OnHistory);
		}
		if (playerPanel != null)
		{
			playerPanel.transform.GetChild(0).Find("upBtn").GetComponent<Button>().onClick.AddListener(BackBtnOnLine);
			playerPanel.transform.GetChild(0).Find("downBtn").GetComponent<Button>().onClick.AddListener(NextBtnOnLine);
		}
		switch (LoginInfo.Instance().mylogindata.choosegame)
		{
		case 1:  //单挑
			changeBtn.onClick.AddListener(OnChange);
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_mpzzs);
			break;
		case 2: //百家乐
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);

			break;
		case 3: //单双
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);
			break;
		case 4: //208
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);
			break;
		case 5:  //天地
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_td);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_td);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_td);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_td);
			break;
		case 10:  //夏威夷
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_xwy);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_xwy);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_xwy);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_xwy);
			break;
		case 7:  // 此为龙虎
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);
			break;
		case 8:  // 百乐单张
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);
			break;
		case 111: // 暂为大小豹   //TODO
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_bl);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_bl);
			break;
		case 222:  //暂为缺一门  //TODO
			changeBtn.onClick.AddListener(OnChange);
			historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory_mpzzs);
			historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory_mpzzs);
			break;
        default:
            historyPanel.transform.GetChild(1).Find("First").GetComponent<Button>().onClick.AddListener(FirstBtnHistory);
            historyPanel.transform.GetChild(1).Find("Back").GetComponent<Button>().onClick.AddListener(BackBtnHistory);
            historyPanel.transform.GetChild(1).Find("Next").GetComponent<Button>().onClick.AddListener(NextBtnHistory);
            historyPanel.transform.GetChild(1).Find("Last").GetComponent<Button>().onClick.AddListener(LastBtnHistory);
          break;
		}


	}



	/// <summary>
	/// 返回
	/// </summary>
	void OnCancel()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        cancelPanel.SetActive(true);
	}

	/// <summary>
	/// 切换路单
	/// </summary>
	void OnChange()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        switch (LoginInfo.Instance().mylogindata.choosegame)
		{
		case 1:
			if (PlayerPrefs.GetString("isShowWord") == "true")
			{
//				DanTiao2.instance.picGo.SetActive(true);
//				DanTiao2.instance.wordGo.SetActive(false);
				DanTiao2.instance.Show_2 ();
				PlayerPrefs.SetString("isShowWord", "false");
			}
			else
			{
//				DanTiao2.instance.picGo.SetActive(false);
//				DanTiao2.instance.wordGo.SetActive(true);
				DanTiao2.instance.Show_1 ();
				PlayerPrefs.SetString("isShowWord", "true");
			}
			break;
		case 222:
			if (PlayerPrefs.GetString("isShowWord") == "true")
			{
				QueYiMen.instance.picGo.SetActive(true);
				QueYiMen.instance.wordGo.SetActive(false);
				PlayerPrefs.SetString("isShowWord", "false");
			}
			else
			{
				QueYiMen.instance.picGo.SetActive(false);
				QueYiMen.instance.wordGo.SetActive(true);
				PlayerPrefs.SetString("isShowWord", "true");
			}
			break;


		}


	}

	/// <summary>
	/// 在线玩家
	/// </summary>
	void OnPalyer()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        StartCoroutine(Player
			(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.playerOnLine +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame +
				"&room_id=" + LoginInfo.Instance().mylogindata.room_id
			));
	}

	/// <summary>
	/// 玩法说明
	/// </summary>
	void OnMethod()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        methodPanel.SetActive(true);
	}

	/// <summary>
	/// 公告说明
	/// </summary>
	void OnNotice()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        noticePanel.SetActive(true);
	}

	void OnHistory()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        StartCoroutine(History
			(
				LoginInfo.Instance().mylogindata.URL +
				LoginInfo.Instance().mylogindata.playerHistory +
				"game_id=" + LoginInfo.Instance().mylogindata.choosegame +
				"&room_id=" + LoginInfo.Instance().mylogindata.room_id +
				"&user_id=" + LoginInfo.Instance().mylogindata.user_id
			)
		);
	}

	IEnumerator Leave_Ienumerator(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

			if (jd["code"].ToString() == "200")
			{
				NewTcpNet.IsQuit = true;
				//Debug.Log(jd["msg"].ToString());
				switch (LoginInfo.Instance().mylogindata.choosegame)
				{
				case 1://单挑
					DanTiao2.instance.tcpNet.SocketQuit();
					break;
				case 2://百家乐
					BaiJiaLe2.instance.tcpNet.SocketQuit();
					break;
				case 3://单双
					DanShuang.instance.tcpNet.SocketQuit();
					break;
				case 4://208
					TZE.instance.tcpNet.SocketQuit();
					break;
				case 5://天地
					TianDi.instance.tcpNet.SocketQuit();
					break;
				case 10://夏威夷
					NewXiaWeiYi.instance.tcp.SocketQuit();
					break;
				case 7://龙虎
					NewLongHu.Instance.tcpNet.SocketQuit();
					break;
				case 8://单张百乐
					BaiJiaLe3.instance.tcpNet.SocketQuit();
					break;
				case 111: 
					XiaWeiYi.instance.tcpNet.SocketQuit();
					break;
				case 222:  //todo  //暂为缺一门
					QueYiMen.instance.tcpNet.SocketQuit();
					break;

				}
				Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.LobbyBG;
				Audiomanger._instenc.GetComponent<AudioSource>().Play();
				SceneManager.LoadScene(1);
			}
			else
			{
				try
				{
					switch (LoginInfo.Instance().mylogindata.choosegame)
					{
					case 2:
						BaiJiaLe2.instance.ShowMessageToShow(jd["msg"].ToString());
						break;
					}
				}
				catch (Exception)
				{
					switch (LoginInfo.Instance().mylogindata.choosegame)
					{
					case 1://单挑
						DanTiao2.instance.tcpNet.SocketQuit();
						break;
					case 2://百家乐
						BaiJiaLe2.instance.tcpNet.SocketQuit();
						break;
					case 3://单双
						DanShuang.instance.tcpNet.SocketQuit();
						break;
					case 4://208
						TZE.instance.tcpNet.SocketQuit();
						break;
					case 5://天地
						TianDi.instance.tcpNet.SocketQuit();
						break;
					case 10://夏威夷
						XiaWeiYi.instance.tcpNet.SocketQuit();
						break;
					case 7://龙虎
						LongHu.instance.tcpNet.SocketQuit();
						break;
					case 8://百家乐
						BaiJiaLe3.instance.tcpNet.SocketQuit();
						break;
					case 111:  //TODO   
						XiaWeiYi.instance.tcpNet.SocketQuit();
						break;
					case 222:  //todo  //暂为缺一门
						QueYiMen.instance.tcpNet.SocketQuit();
						break;

					}
					Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.LobbyBG;
					Audiomanger._instenc.GetComponent<AudioSource>().Play();
					SceneManager.LoadScene(1);
				}
			}
		}

		yield return null;
	}



	void OnQuite()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        StartCoroutine(Leave_Ienumerator(
			LoginInfo.Instance().mylogindata.URL +
			"room-end?"
			+ "user_id=" + LoginInfo.Instance().mylogindata.user_id
			+ "&game_id=" + LoginInfo.Instance().mylogindata.choosegame
		));

	}


	#region unitywebrequest

	IEnumerator Player(string url)
	{

		UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
			playerInfo.Clear();

			Debug.Log(jd.ToJson().ToString());

			if (jd["code"].ToString() == "200")
			{
				for (int i = 0; i < jd["userData"].Count; i++)
				{
					PlayerInfo player = new PlayerInfo();

					player.playerId = "id:" + jd["userData"][i]["user_id"].ToString()/*+"  昵称:"+ jd["userData"][i]["username"]*/;

					player.seat = "座位:" + jd["userData"][i]["seat_number"];

					playerInfo.Add(player);
				}
			}
		}
        for (int i = 0; i < playerInfoGrid.Count; i++)
        {
            playerInfoGrid[i].transform.GetChild(0).gameObject.SetActive(false);
            playerInfoGrid[i].transform.GetChild(1).gameObject.SetActive(false);
            playerInfoGrid[i].transform.GetChild(2).gameObject.SetActive(false);
        }
		for (int i = 0; i < playerInfo.Count; i++)
		{
			if (i == 8)
			{
				break;
			}
			else
			{
				if (playerInfo.Count < (i + 1)) 
				{
					playerInfoGrid[i].transform.GetChild(0).gameObject.SetActive(false);
					playerInfoGrid[i].transform.GetChild(1).gameObject.SetActive(false);
					playerInfoGrid[i].transform.GetChild(2).gameObject.SetActive(false);
				}
				else
				{
					playerInfoGrid[i].transform.GetChild(0).GetComponent<Text>().text = playerInfo[i].seat;
					playerInfoGrid[i].transform.GetChild(0).gameObject.SetActive(true);
					playerInfoGrid[i].transform.GetChild(1).GetComponent<Text>().text = playerInfo[i].playerId;
					playerInfoGrid[i].transform.GetChild(1).gameObject.SetActive(true);
					playerInfoGrid[i].transform.GetChild(2).gameObject.SetActive(true);
				}

			}
		}
		onLineCount = playerInfo.Count / 8;
		if (playerInfo.Count % 8 > 0)
		{
			onLineCount += 1;
		}
		playerPanel.transform.GetChild(0).Find("PageText").GetComponent<Text>().text = "1/" + onLineCount;
		playerPanel.SetActive(true);


	}

	IEnumerator History(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
			Debug.Log(jd.ToJson().ToString());
			switch (LoginInfo.Instance().mylogindata.choosegame)
			{
			#region  1


			case 1:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_mpzzs.Clear();

					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_mpzzs history = new History_mpzzs();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						if (jd["betData"][i]["season"] != null && jd["betData"][i]["periods"] != null)
						{
							history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						}
						else
						{
							history.round = "获取失败";
						}
						history.spade = jd["betData"][i]["A"].ToString();
						history.heart = jd["betData"][i]["B"].ToString();
						history.club = jd["betData"][i]["C"].ToString();
						history.diamond = jd["betData"][i]["D"].ToString();
						history.trump = jd["betData"][i]["E"].ToString();
						history.winPoker = GetPokerName(jd["betData"][i]["winnings"].ToString());
						history.allBet = jd["betData"][i]["totalCredit"].ToString();
						history.winScore = jd["betData"][i]["totalWin"].ToString();
						history.allScore = jd["betData"][i]["userBalance"].ToString();

						this.history_mpzzs.Add(history);

					}
				}

				for (int i = 0; i < history_mpzzs.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_mpzzs.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(10).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(11).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
							historyGo[i].transform.GetChild(10).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
							historyGo[i].transform.GetChild(11).gameObject.SetActive(true);
						}


					}
				}
				historyCount = history_mpzzs.Count / 8;
				if (history_mpzzs.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 2
			case 2:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();

						if (jd["betData"][i]["season"] != null && jd["betData"][i]["periods"] != null)
						{
							history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						}
						else
						{
							history.round = "获取失败";
						}

						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = jd["betData"][i]["B"].ToString();
						history.xian = jd["betData"][i]["C"].ToString();
						if (jd["betData"][i]["winnings"] != null)
						{
							history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						}
						else
						{
							history.kaiJiang = "获取失败";
						}
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);

				break;
				#endregion

				#region 3
			case 3:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						if (jd["betData"][i]["season"] != null && jd["betData"][i]["periods"] != null)
						{
							history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						}
						else
						{
							history.round = "获取失败";
						}
						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = jd["betData"][i]["B"].ToString();
						history.xian = jd["betData"][i]["C"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 4
			case 4:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = jd["betData"][i]["B"].ToString();
						history.xian = jd["betData"][i]["C"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 5

			case 5:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_td.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_td history = new History_td();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.tian = jd["betData"][i]["A"].ToString();
						history.di = jd["betData"][i]["B"].ToString();                           
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_td.Add(history);
					}
				}
				for (int i = 0; i < history_td.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_td.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);

						}
					}
				}
				historyCount = history_td.Count / 8;
				if (history_td.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 10
			case 10:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_xwy.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_xwy history = new History_xwy();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.red = jd["betData"][i]["A"].ToString();
						history.grren = jd["betData"][i]["B"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_xwy.Add(history);
					}
				}
				for (int i = 0; i < history_xwy.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_xwy.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_xwy[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_xwy[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_xwy[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_xwy[i].red;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_xwy[i].grren;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_xwy[i].kaiJiang == "0" ? "红":"绿";
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_xwy[i].allScore;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_xwy[i].allWinScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_xwy[i].allGold;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);

						}
					}
				}
				historyCount = history_xwy.Count / 8;
				if (history_xwy.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 7
			case 7:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = jd["betData"][i]["B"].ToString();
						history.xian = jd["betData"][i]["C"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 8
			case 8:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						if (jd["betData"][i]["season"] != null && jd["betData"][i]["periods"] != null)
						{
							history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						}
						else
						{
							history.round = "获取失败";
						}
						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = "X";
						history.xian = jd["betData"][i]["B"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion

				#region 111  暂为大小豹
			case 111:  //大小豹  TODO
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_bl.Clear();
					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_bl history = new History_bl();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.zhuang = jd["betData"][i]["A"].ToString();
						history.he = jd["betData"][i]["B"].ToString();
						history.xian = jd["betData"][i]["C"].ToString();
						history.kaiJiang = jd["betData"][i]["winnings"].ToString();
						history.allScore = jd["betData"][i]["totalCredit"].ToString();
						history.allWinScore = jd["betData"][i]["totalWin"].ToString();
						history.allGold = jd["betData"][i]["userBalance"].ToString();
						this.history_bl.Add(history);
					}
				}
				for (int i = 0; i < history_bl.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_bl.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
						}
					}
				}
				historyCount = history_bl.Count / 8;
				if (history_bl.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
				#endregion
				#region  222   暂为缺一门
			case 222:
				if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
				{
					this.history_qym.Clear();

					for (int i = 0; i < jd["betData"].Count; i++)
					{
						History_qym history = new History_qym();
						history.num = (i + 1).ToString();
						history.playerId = jd["betData"][i]["user_id"].ToString();
						history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
						history.spade = jd["betData"][i]["A"].ToString();
						history.heart = jd["betData"][i]["B"].ToString();
						history.club = jd["betData"][i]["C"].ToString();
						history.trump = jd["betData"][i]["D"].ToString();
						history.winPoker = GetPokerName(jd["betData"][i]["winnings"].ToString());
						history.allBet = jd["betData"][i]["totalCredit"].ToString();
						history.winScore = jd["betData"][i]["totalWin"].ToString();
						history.allScore = jd["betData"][i]["userBalance"].ToString();

						this.history_qym.Add(history);

					}
				}

				for (int i = 0; i < history_qym.Count; i++)
				{
					if (i == 8)
					{
						break;
					}
					else
					{
						if (history_qym.Count < i + 1)
						{
							historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(9).gameObject.SetActive(false);
							historyGo[i].transform.GetChild(10).gameObject.SetActive(false);
						}
						else
						{
							historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_qym[i].num;
							historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_qym[i].playerId;
							historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_qym[i].round;
							historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_qym[i].spade;
							historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_qym[i].heart;
							historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_qym[i].club;
							historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_qym[i].trump;
							historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_qym[i].winPoker;
							historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_qym[i].allBet;
							historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_qym[i].winScore;
							historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
							historyGo[i].transform.GetChild(10).GetComponent<Text>().text = history_qym[i].allScore;
							historyGo[i].transform.GetChild(10).gameObject.SetActive(true);
						}


					}
				}
				historyCount = history_qym.Count / 8;
				if (history_qym.Count % 8 > 0)
				{
					historyCount += 1;
				}
				historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
				historyPanel.SetActive(true);
				break;
                #endregion
                #region  default全通用
                default:
               
               
                    if (jd["code"].ToString() == "200" && jd["betData"].Count > 0)
                    {
                        this.history_td.Clear();
                        for (int i = 0; i < jd["betData"].Count; i++)
                        {
                            History_td history = new History_td();
                            history.num = (i + 1).ToString();
                            history.playerId = jd["betData"][i]["user_id"].ToString();
                            history.round = jd["betData"][i]["season"].ToString() + "/" + jd["betData"][i]["periods"].ToString();
                            history.tian = jd["betData"][i]["A"].ToString();
                            history.di = jd["betData"][i]["B"].ToString();
                            history.kaiJiang = jd["betData"][i]["winnings"].ToString();
                            history.allScore = jd["betData"][i]["totalCredit"].ToString();
                            history.allWinScore = jd["betData"][i]["totalWin"].ToString();
                            history.allGold = jd["betData"][i]["userBalance"].ToString();
                            this.history_td.Add(history);
                        }
                    }
                    for (int i = 0; i < history_td.Count; i++)
                    {
                        if (i == 8)
                        {
                            break;
                        }
                        else
                        {
                            if (history_td.Count < i + 1)
                            {
                                historyGo[i].transform.GetChild(0).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(1).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(2).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(3).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(4).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(5).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(6).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(7).gameObject.SetActive(false);
                                historyGo[i].transform.GetChild(8).gameObject.SetActive(false);
                            }
                            else
                            {
                                historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
                                historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
                                historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
                                historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
                                historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
                                historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
                                historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
                                historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
                                historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
                                historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
                                historyGo[i].transform.GetChild(8).gameObject.SetActive(true);

                            }
                        }
                    }
                    historyCount = history_td.Count / 8;
                    if (history_td.Count % 8 > 0)
                    {
                        historyCount += 1;
                    }
                    historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
                    historyPanel.SetActive(true);
                    break;
                    #endregion

			}

		}

	}


	string GetPokerName(string str)
	{
		string pokerName = "";
		string pokerNum = "";
		int num = Convert.ToInt32(str);

		int a = num / 13;
		int b = num % 13;
		switch (b)
		{
		case 0:
			if (a != 4)
			{
				pokerNum = "A";
			}
			break;
		case 10:
			if (a != 4)
			{
				pokerNum = "J";
			}
			break;
		case 11:
			if (a != 4) 
			{
				pokerNum = "Q";
			}
			break;
		case 12:
			if (a != 4)
			{
				pokerNum = "K";
			}
			break;           
		}
		switch (a)
		{
		case 0:
			pokerName = "黑桃";
			break;
		case 1:
			pokerName = "红桃";
			break;
		case 2:
			pokerName = "梅花";
			break;
		case 3:
			pokerName = "方块";
			break;
		case 4:
			if (a == 0)
			{
				pokerName = "小王";
			}
			else
			{
				pokerName = "大王";
			}
			break;           
		}
		return (pokerName + pokerNum);
	}
	#endregion



	#region 在线玩家里的 上一页 下一页 按钮

	int numOnLine = 1;

	/// <summary>
	/// 上一页
	/// </summary>
	void BackBtnOnLine()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (onLineCount == 1 || numOnLine == 1)
		{
			return;
		}
		int num = 0;
        //怎么算的我也懵了  不过大概是对了
		for (int i = (numOnLine - 2) * 8; i < ((numOnLine - 2) * 8 + 8); i++)
		{
			playerInfoGrid[num].transform.GetChild(0).GetComponent<Text>().text = playerInfo[i].seat;
			playerInfoGrid[num].transform.GetChild(0).gameObject.SetActive(true);
			playerInfoGrid[num].transform.GetChild(1).GetComponent<Text>().text = playerInfo[i].playerId;
			playerInfoGrid[num].transform.GetChild(1).gameObject.SetActive(true);
			playerInfoGrid[num].transform.GetChild(2).gameObject.SetActive(true);

			num++;
		}
		numOnLine -= 1;
		playerPanel.transform.GetChild(0).Find("PageText").GetComponent<Text>().text = numOnLine + "/" + onLineCount;

	}


	void NextBtnOnLine()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (onLineCount == 1 || numOnLine == onLineCount)
		{
			return;
		}
		int num = 0;
		for (int i = (numOnLine) * 8; i < (numOnLine) * 8 + 8; i++)
		{
			if (i + 1 > playerInfo.Count)
			{

				playerInfoGrid[num].transform.GetChild(0).gameObject.SetActive(false);
				playerInfoGrid[num].transform.GetChild(1).gameObject.SetActive(false);
				playerInfoGrid[num].transform.GetChild(2).gameObject.SetActive(false);
			}
			else
			{
				playerInfoGrid[num].transform.GetChild(0).GetComponent<Text>().text = playerInfo[i].seat;
				playerInfoGrid[num].transform.GetChild(0).gameObject.SetActive(true);
				playerInfoGrid[num].transform.GetChild(1).GetComponent<Text>().text = playerInfo[i].playerId;
				playerInfoGrid[num].transform.GetChild(1).gameObject.SetActive(true);
				playerInfoGrid[num].transform.GetChild(2).gameObject.SetActive(true);
			}
			num++;
		}
		numOnLine += 1;
		playerPanel.transform.GetChild(0).Find("PageText").GetComponent<Text>().text = numOnLine + "/" + onLineCount;
	}



    #endregion


    //历史路单界面
    int numHistory = 1;

    #region  通用版按钮事件

    /// <summary>
    /// 上一页
    /// </summary>
    void BackBtnHistory()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
        {
            return;
        }
        int num = 0;
        for (int i = (numHistory - 2) * 8; i < ((numHistory - 2) * 8 + 8); i++)
        {
            historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
            historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
            historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
            historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
            historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
            historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
            historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
            historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
            historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
            historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
            historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
            historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
            historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
            num++;
        }

        numHistory -= 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }


    /// <summary>
    /// 下一页
    /// </summary>
    void NextBtnHistory()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
        {
            return;
        }
        int num = 0;
        for (int i = (numHistory) * 8; i < (numHistory) * 8 + 8; i++)
        {
            if (history_mpzzs.Count < i + 1)
            {
                historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(10).gameObject.SetActive(false);
                historyGo[num].transform.GetChild(11).gameObject.SetActive(false);
            }
            else
            {
                historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
                historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
                historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
                historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
                historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
                historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
                historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
                historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
                historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
                historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
                historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
                historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
                historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
            }
            num++;
        }
        numHistory += 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }


    /// <summary>
    /// 首页
    /// </summary>
    void FirstBtnHistory()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
                historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
                historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
                historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
                historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
                historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
                historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
                historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
                historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
                historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
                historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
                historyGo[i].transform.GetChild(10).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
                historyGo[i].transform.GetChild(11).gameObject.SetActive(true);
            }
        }
        numHistory = 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
    }

    /// <summary>
    /// 尾页
    /// </summary>
    void LastBtnHistory()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
        {
            return;
        }
        else
        {
            int num = 0;
            for (int i = (history_mpzzs.Count / 8) * historyCount - 1; i < (history_mpzzs.Count / 8) * historyCount; i++)
            {
                if (history_mpzzs.Count < i + 1)
                {

                    historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(10).gameObject.SetActive(false);
                    historyGo[num].transform.GetChild(11).gameObject.SetActive(false);
                }
                else
                {
                    historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
                    historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
                    historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
                    historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
                    historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
                    historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
                    historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
                    historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
                    historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
                    historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
                    historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
                    historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
                    historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
                }
                num++;
            }
        }
        numHistory = historyCount;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }

    #endregion

    #region  单挑用按钮事件
	/// <summary>
	/// 上一页
	/// </summary>
	void BackBtnHistory_mpzzs()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			return;
		}
		int num = 0;
		for (int i = (numHistory - 2) * 8; i < ((numHistory - 2) * 8 + 8); i++)
		{
			historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
			historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
			historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
			historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
			historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
			historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
			historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
			historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
			historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
			historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
			historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
			historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
			historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
			num++;
		}

		numHistory -= 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}


	/// <summary>
	/// 下一页
	/// </summary>
	void NextBtnHistory_mpzzs()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		int num = 0;
		for (int i = (numHistory) * 8; i < (numHistory) * 8 + 8; i++)
		{
			if (history_mpzzs.Count < i + 1)
			{
				historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(10).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(11).gameObject.SetActive(false);
			}
			else
			{
				historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
				historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
				historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
				historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
				historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
				historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
				historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
				historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
				historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
				historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
				historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
				historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
				historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
			}
			num++;
		}
		numHistory += 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}



	void FirstBtnHistory_mpzzs()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			return;
		}
		else
		{
			for (int i = 0; i < 8; i++)
			{
				historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
				historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
				historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
				historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
				historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
				historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
				historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
				historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
				historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
				historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
				historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
				historyGo[i].transform.GetChild(10).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
				historyGo[i].transform.GetChild(11).gameObject.SetActive(true);
			}
		}
		numHistory = 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
	}


	void LastBtnHistory_mpzzs()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		else
		{
			int num = 0;
			for (int i = (history_mpzzs.Count / 8) * historyCount-1; i < (history_mpzzs.Count / 8) * historyCount; i++)
			{
				if (history_mpzzs.Count < i + 1)
				{

					historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(10).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(11).gameObject.SetActive(false);
				}
				else
				{
					historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_mpzzs[i].num;
					historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_mpzzs[i].playerId;
					historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_mpzzs[i].round;
					historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_mpzzs[i].spade;
					historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_mpzzs[i].heart;
					historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_mpzzs[i].club;
					historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_mpzzs[i].diamond;
					historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_mpzzs[i].trump;
					historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_mpzzs[i].winPoker;
					historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_mpzzs[i].allBet;
					historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(10).GetComponent<Text>().text = history_mpzzs[i].winScore;
					historyGo[num].transform.GetChild(10).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(11).GetComponent<Text>().text = history_mpzzs[i].allScore;
					historyGo[num].transform.GetChild(11).gameObject.SetActive(true);
				}
				num++;
			}
		}
		numHistory = historyCount;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}

	#endregion

	#region  百家乐
	void BackBtnHistory_bl()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			Debug.Log("A1");
			return;
		}
		int num = 0;
		for (int i = (numHistory - 2) * 8; i < ((numHistory - 2) * 8 + 8); i++)
		{
			historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
			historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
			historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
			historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
			historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
			historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
			historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
			historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
			historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
			historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
			historyGo[num].transform.GetChild(9).gameObject.SetActive(true);
			num++;
		}

		numHistory -= 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}


	void NextBtnHistory_bl()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		int num = 0;
		for (int i = (numHistory  ) * 8; i < (numHistory) * 8 + 8; i++)
		{


			if (history_bl.Count < i + 1)
			{
				historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
			}
			else
			{
				historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
				historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
				historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
				historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
				historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
				historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
				historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
				historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
				historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
				historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
				historyGo[num].transform.GetChild(9).gameObject.SetActive(true);             
			}
			num++;
		}
		numHistory += 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}

	void FirstBtnHistory_bl()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			return;
		}
		else
		{
			for (int i = 0; i < 8; i++)
			{
				historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
				historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
				historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
				historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
				historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
				historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
				historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
				historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
				historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
				historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
				historyGo[i].transform.GetChild(9).gameObject.SetActive(true);
			}
		}
		numHistory = 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
	}

	void LastBtnHistory_bl()
        //按钮声音
	{
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		else
		{
			int num = 0;
			for (int i = (history_bl.Count / 8) * historyCount-1; i < (history_bl.Count / 8) * historyCount ; i++)
			{

				if (history_bl.Count < i+1)
				{
					historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(9).gameObject.SetActive(false);

				}
				else
				{
					historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_bl[i].num;
					historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_bl[i].playerId;
					historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_bl[i].round;
					historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_bl[i].zhuang;
					historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_bl[i].he;
					historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_bl[i].xian;
					historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_bl[i].kaiJiang;
					historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_bl[i].allScore;
					historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_bl[i].allWinScore;
					historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(9).GetComponent<Text>().text = history_bl[i].allGold;
					historyGo[num].transform.GetChild(9).gameObject.SetActive(true);

				}
				num++;
			}
		}

		numHistory = historyCount;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}

	#endregion

	#region 天地
	void BackBtnHistory_td()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			return;
		}
		int num = 0;
		for (int i = (numHistory - 2) * 8; i < ((numHistory - 2) * 8 + 8); i++)
		{
			historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
			historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
			historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
			historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
			historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
			historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
			historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
			historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
			historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
			historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
			historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
			num++;

		}

		numHistory -= 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}


	void NextBtnHistory_td()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		int num = 0;
		for (int i = (numHistory) * 8; i < (numHistory) * 8 + 8; i++)
		{


			if (history_td.Count < i + 1)
			{
				historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
				historyGo[num].transform.GetChild(9).gameObject.SetActive(false);
			}
			else
			{
				historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
				historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
				historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
				historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
				historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
				historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
				historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
				historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
				historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
				historyGo[num].transform.GetChild(8).gameObject.SetActive(true);

			}
			num++;
		}
		numHistory += 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}

	void FirstBtnHistory_td()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
		{
			return;
		}
		else
		{
			for (int i = 0; i < 8; i++)
			{
				historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
				historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
				historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
				historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
				historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
				historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
				historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
				historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
				historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
				historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
				historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
			}
		}
		numHistory = 1;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
	}

	void LastBtnHistory_td()
	{
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
		{
			return;
		}
		else
		{
			int num = 0;
			for (int i = (history_td.Count / 8) * historyCount-1; i < (history_td.Count / 8) * historyCount; i++)
			{

				if (history_td.Count < i + 1)
				{
					historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
					historyGo[num].transform.GetChild(9).gameObject.SetActive(false);

				}
				else
				{
					historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_td[i].num;
					historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_td[i].playerId;
					historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_td[i].round;
					historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_td[i].tian;
					historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_td[i].di;
					historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_td[i].kaiJiang;
					historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_td[i].allScore;
					historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_td[i].allWinScore;
					historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
					historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_td[i].allGold;
					historyGo[num].transform.GetChild(8).gameObject.SetActive(true);

				}
				num++;
			}
		}
		numHistory = historyCount;
		historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
	}
    #endregion

    #region 夏威夷
    void BackBtnHistory_xwy()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
        {
            return;
        }
        int num = 0;
        for (int i = (numHistory - 2) * 8; i < ((numHistory - 2) * 8 + 8); i++)
        {
            historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_xwy[i].num;
            historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_xwy[i].playerId;
            historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_xwy[i].round;
            historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_xwy[i].red;
            historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_xwy[i].grren;
            historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_xwy[i].kaiJiang == "0" ? "红" : "绿";
            historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_xwy[i].allScore;
            historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_xwy[i].allWinScore;
            historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
            historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_xwy[i].allGold;
            historyGo[num].transform.GetChild(8).gameObject.SetActive(true);
            num++;

        }

        numHistory -= 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }

    void NextBtnHistory_xwy()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
        {
            return;
        }
        int num = 0;
        for (int i = (numHistory) * 8; i < (numHistory) * 8 + 8; i++)
        {


            if (history_xwy.Count < i + 1)
            {
                for (int h = 0; h < historyGo[num].transform.childCount; h++)
                {
                    historyGo[num].transform.GetChild(h).gameObject.SetActive(false);
                }
                //historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
                //historyGo[num].transform.GetChild(8).gameObject.SetActive(false);
            }
            else
            {
                historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_xwy[i].num;
                historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_xwy[i].playerId;
                historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_xwy[i].round;
                historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_xwy[i].red;
                historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_xwy[i].grren;
                historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_xwy[i].kaiJiang == "0" ? "红" : "绿";
                historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_xwy[i].allScore;
                historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_xwy[i].allWinScore;
                historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
                historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_xwy[i].allGold;
                historyGo[num].transform.GetChild(8).gameObject.SetActive(true);

            }
            num++;
        }
        numHistory += 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }

    void FirstBtnHistory_xwy()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == 1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                historyGo[i].transform.GetChild(0).GetComponent<Text>().text = history_xwy[i].num;
                historyGo[i].transform.GetChild(0).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(1).GetComponent<Text>().text = history_xwy[i].playerId;
                historyGo[i].transform.GetChild(1).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(2).GetComponent<Text>().text = history_xwy[i].round;
                historyGo[i].transform.GetChild(2).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(3).GetComponent<Text>().text = history_xwy[i].red;
                historyGo[i].transform.GetChild(3).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(4).GetComponent<Text>().text = history_xwy[i].grren;
                historyGo[i].transform.GetChild(4).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(5).GetComponent<Text>().text = history_xwy[i].kaiJiang == "0" ? "红" : "绿";
                historyGo[i].transform.GetChild(5).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(6).GetComponent<Text>().text = history_xwy[i].allScore;
                historyGo[i].transform.GetChild(6).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(7).GetComponent<Text>().text = history_xwy[i].allWinScore;
                historyGo[i].transform.GetChild(7).gameObject.SetActive(true);
                historyGo[i].transform.GetChild(8).GetComponent<Text>().text = history_xwy[i].allGold;
                historyGo[i].transform.GetChild(8).gameObject.SetActive(true);
            }
        }
        numHistory = 1;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = "1/" + historyCount;
    }

    void LastBtnHistory_xwy()
    {
        //按钮声音
        Audiomanger._instenc.clickvoice();
        if (historyCount == 1 || numHistory == historyCount)
        {
            return;
        }
        else
        {
            int num = 0;
            for (int i = (history_xwy.Count / 8) * historyCount - 1; i < (history_xwy.Count / 8) * historyCount; i++)
            {

                if (history_xwy.Count < i + 1)
                {
                    for (int h = 0; h < historyGo[num].transform.childCount; h++)
                    {
                        historyGo[num].transform.GetChild(h).gameObject.SetActive(false);
                    }
                    //historyGo[num].transform.GetChild(0).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(1).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(2).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(3).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(4).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(5).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(6).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(7).gameObject.SetActive(false);
                    //historyGo[num].transform.GetChild(8).gameObject.SetActive(false);

                }
                else
                {
                    historyGo[num].transform.GetChild(0).GetComponent<Text>().text = history_xwy[i].num;
                    historyGo[num].transform.GetChild(0).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(1).GetComponent<Text>().text = history_xwy[i].playerId;
                    historyGo[num].transform.GetChild(1).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(2).GetComponent<Text>().text = history_xwy[i].round;
                    historyGo[num].transform.GetChild(2).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(3).GetComponent<Text>().text = history_xwy[i].red;
                    historyGo[num].transform.GetChild(3).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(4).GetComponent<Text>().text = history_xwy[i].grren;
                    historyGo[num].transform.GetChild(4).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(5).GetComponent<Text>().text = history_xwy[i].kaiJiang == "0" ? "红":"绿";
                    historyGo[num].transform.GetChild(5).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(6).GetComponent<Text>().text = history_xwy[i].allScore;
                    historyGo[num].transform.GetChild(6).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(7).GetComponent<Text>().text = history_xwy[i].allWinScore;
                    historyGo[num].transform.GetChild(7).gameObject.SetActive(true);
                    historyGo[num].transform.GetChild(8).GetComponent<Text>().text = history_xwy[i].allGold;
                    historyGo[num].transform.GetChild(8).gameObject.SetActive(true);

                }
                num++;
            }
        }
        numHistory = historyCount;
        historyPanel.transform.GetChild(1).Find("Page").GetComponent<Text>().text = numHistory + "/" + historyCount;
    }
    #endregion
}
