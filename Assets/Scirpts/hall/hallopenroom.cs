using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using LitJson;
using System;
using com.QH.QPGame.Lobby.Surfaces;
using System.Collections.Generic;

public class hallopenroom : MonoBehaviour
{
    public GameObject roompanel;//房间面板
    public GameObject gamechoosepanel;//选择游戏面板
    public GameObject roomgo;//房间存储对象
    public GameObject roomparefabsGo;//房间创建预设体
    public GameObject waitmask;//屏蔽面板
    public GameObject gametypeGO;//游戏类型GO
    public GameObject tiltleGO;//标题预设体
    public MessageBoxPopup messg;//错误信息预设体
    public List<InputField> passwordchangetext;

    public List<InputField> CAPTCHAchangetext;
    public GameObject scalego;//对大厅进行缩放
    private LoginInfo _gamelogininfo;
    public Text servicesOne;
    public Text servicesTwo;
    
	public Notice notice;

    // Use this for initialization
    void Start()
    {

        //初始化过快获取宽 高值可能会相反
        //Screen.orientation = ScreenOrientation.Portrait;

        StartCoroutine(ShowLoading());
        StartCoroutine(GetServices(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.services));
        Debug.Log("大厅屏幕宽" + Screen.height);

        int tempwidth = Screen.height;
        changescaleinscren(tempwidth);
        _gamelogininfo = LoginInfo.Instance();
        LoginInfo.Instance().wwwinstance.gamelisteventback += GetgameidAndgamelist;
        LoginInfo.Instance().wwwinstance.roomlistevnetback += changeroomlist;

		//单双特有
		//notice.AddNotice();

        try
        {
            StartCoroutine(gameinit());

        }
        catch (Exception)
        {

            throw;
        }
        init();
        tiltleGO.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _gamelogininfo.mylogindata.username;
        //tiltleGO.transform.GetChild(2).GetComponent<Text>().text = _gamelogininfo.mylogindata.*****;
        tiltleGO.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = _gamelogininfo.mylogindata.ALLScroce;
        StartCoroutine(hallalive());
        LoginInfo.Instance().cheakUPdate();
    }

    void init()
    {
        //直接进行游戏选择按钮的初始化
        //gametypeGO.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate ()
        //{
        //    getroomlist(
        //            LoginInfo.Instance().mylogindata.game_id[0],
        //            LoginInfo.Instance().mylogindata.user_id);
        //});
        for (int i = 0; i < gametypeGO.transform.childCount; i++)
        {
            int num = i;
            gametypeGO.transform.GetChild(num).GetComponent<Button>().onClick.AddListener(delegate ()
            {
                try
                {
                    getroomlist(
                    LoginInfo.Instance().mylogindata.game_id[num],
                    LoginInfo.Instance().mylogindata.user_id);
                }
                catch (Exception)
                {

                }
               
            });
        }

        EnterBtn.onClick.AddListener(buttonclick);
        //gametypeGO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate ()
        //{
        //    getroomlist(
        //         LoginInfo.Instance().mylogindata.game_id[0],
        //         LoginInfo.Instance().mylogindata.user_id);
        //});
        //gametypeGO.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate()
        //{
        //    getroomlist(
        //            LoginInfo.Instance().mylogindata.game_id[2],
        //            LoginInfo.Instance().mylogindata.user_id);
        //});
        //gametypeGO.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate()
        //{
        //    getroomlist(
        //            LoginInfo.Instance().mylogindata.game_id[3],
        //            LoginInfo.Instance().mylogindata.user_id);
        //});
        //}
    }

    //根据某些比例修改屏幕缩放
    void changescaleinscren(int changewith)
    {
        //if (changewith == 480|| changewith == 720)
        //{
        //    scalego.transform.localScale = new Vector3(0.91f, 1, 1);
        //}
        //else if(changewith==1080&&Screen.width == 2160)
        //{
        //    scalego.transform.localScale = new Vector3(0.83f, 1, 1);
        //}
        //else if (changewith == 1080 && Screen.width == 1920)
        //{
        //    scalego.transform.localScale = new Vector3(0.91f, 1, 1);
        //}
        //else
        //{
        //    scalego.transform.localScale = new Vector3(1, 1, 1);
        //}
    }

    //是否还在存活
    IEnumerator hallalive()
    {
        while (true)
        {
            yield return getaliveinfo(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.hallaliveAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + LoginInfo.Instance().mylogindata.token);
            if (roompanel.activeSelf)
            {
                _gamelogininfo.wwwinstance.sendWWW(_gamelogininfo.mylogindata.URL + _gamelogininfo.mylogindata.roomlistAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame, listtype.listforhallinfo, action.getroomlist, true);
            }
            yield return new WaitForSeconds(5f);
        }
    }


    //存活服务器连接
    IEnumerator getaliveinfo(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
		//Debug.Log (URL);
        yield return www.Send();

        Debug.Log(www.downloadHandler.text);
        if (www.error == null)
        {


            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {

            }
            else
            {

                messg.Show("账号异常", jd["msg"].ToString(), ButtonStyle.Confirm,
                    delegate (MessageBoxResult call)
                    {
                        switch (call)
                        {

                            case MessageBoxResult.Cancel:
                                LoginInfo.Instance().cleanmylogindata();
                                SceneManager.LoadScene(0);
                                break;
                            case MessageBoxResult.Confirm:
                                LoginInfo.Instance().cleanmylogindata();
                                SceneManager.LoadScene(0);
                                break;


                        }
                    }, 0f);



            }
        }
        else
        {
            messg.Show("异常", "网络异常，即将退出游戏", ButtonStyle.Confirm,
                  delegate (MessageBoxResult call)
                  {
                      switch (call)
                      {

                          case MessageBoxResult.Cancel:
                              LoginInfo.Instance().cleanmylogindata();
                              SceneManager.LoadScene(0);
                              break;
                          case MessageBoxResult.Confirm:
                              LoginInfo.Instance().cleanmylogindata();
                              SceneManager.LoadScene(0);
                              break;
                          case MessageBoxResult.Timeout:
                              LoginInfo.Instance().cleanmylogindata();
                              SceneManager.LoadScene(0);
                              break;

                      }
                  }, 5f);
        }
    }



    //获取游戏列表
    IEnumerator gameinit()
    {
        _gamelogininfo.wwwinstance.sendWWW(_gamelogininfo.mylogindata.URL + _gamelogininfo.mylogindata.gamelistAPI/*+"user_id="+_gamelogininfo.mylogindata.user_id*/, listtype.listforgameinfo, action.gethallgame, true);
        yield return new WaitForSeconds(5f);
    }


    // Update is called once per frame
    void Update()
    {

    }

    //将对应的的事件注销
    private void OnDestroy()
    {
        LoginInfo.Instance().wwwinstance.gamelisteventback -= GetgameidAndgamelist;
        LoginInfo.Instance().wwwinstance.roomlistevnetback -= changeroomlist;
    }

    Dictionary<string, List<string>> EnterRoom = new Dictionary<string, List<string>>();
    GameObject goToggle;
    string toggleName;
    /// <summary>
    /// 对房间的进入按钮事件进行重新监听
    /// </summary>
    /// 
    void clickintoscreen(JsonData jd)
    {
        EnterRoom.Clear();
		if(jd["code"].ToString()=="200")
		{
			if (jd["roomList"].Count > 0)
			{
				for (int i = 0; i < roomgo.transform.childCount; i++)
				{
					int num = i;
					List<string> info = new List<string>();
					info.Add(jd["roomList"][num]["id"].ToString());
					info.Add(jd["roomList"][num]["red_limit"].ToString());
					info.Add(jd["roomList"][num]["min_bet"].ToString());

					EnterRoom.Add(roomgo.transform.GetChild(num).name, info);

				}


				//    for (int i = 0; i < roomgo.transform.childCount; i++)
				//    {
				//        Transform temp = roomgo.transform.GetChild(i);
				//        //Debug.LogWarning(temp.GetComponent<Button>().onClick.GetPersistentEventCount());
				//        temp.GetComponent<Button>().onClick.RemoveAllListeners();
				//    }
			}


			if (roomgo.transform.childCount > 0)
			{


				for (int i = 0; i < roomgo.transform.childCount; i++)
				{
					int num = i;
					roomgo.transform.GetChild(num).GetComponent<Toggle>().onValueChanged.AddListener
					(
						(bool Value) =>
						{
							EnterTogglRoom(Value, num);
						}
					);



				}
			}
		}
    }


    public Button EnterBtn;

    void EnterTogglRoom(bool value,int num)
    {
        if (value)
        {
            if (goToggle != null && toggleName != roomgo.transform.GetChild(num).transform.name)
            {
                goToggle.GetComponent<Toggle>().isOn = false;
                toggleName = roomgo.transform.GetChild(num).transform.name;

            }
            else
            {
                toggleName = roomgo.transform.GetChild(num).transform.name;

            }
            goToggle = roomgo.transform.GetChild(num).gameObject;
        }
        else
        {
            goToggle = null;
            toggleName = null;
        }
    }

    //进入房间的监听
    void buttonclick()
    {
        if (toggleName == null || toggleName == "")
        {
            return;
        }
        //if()
        /*if (roomID == "公开房")
        {
            roomID = "1";
        }*/
        List<string> rua;
        EnterRoom.TryGetValue(toggleName,out rua);
        LoginInfo.Instance().mylogindata.room_id = Convert.ToInt32(rua[0]); //Convert.ToInt32(roomID);
        LoginInfo.Instance().mylogindata.roomlitmit = rua[1]; //roomlitmitcode;//限红
        LoginInfo.Instance().mylogindata.roomcount = rua[2]; // roomcount;//最小压分

        StartCoroutine(loadgame());
    }

    //加载游戏
    IEnumerator loadgame()
    {

        yield return StartCoroutine(goinroom(_gamelogininfo.mylogindata.URL + _gamelogininfo.mylogindata.roominstartAPI + "user_id=" + _gamelogininfo.mylogindata.user_id + "&unionuid=" + _gamelogininfo.mylogindata.token + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame + "&room_id=" + LoginInfo.Instance().mylogindata.room_id));

    }

    //获取对应方法的信息
    void GetgameidAndgamelist(UnityWebRequest temp)
    {
        if (temp.downloadHandler.text != "")
        {
            JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);

            if (jd["code"].ToString() == "200")
            {

                for (int i = 0; i < jd["GameList"].Count; i++)
                {


                    //LoginInfo.Instance().mylogindata.game_id.Add(Convert.ToInt32(jd["GameList"][i]["id"].ToString()));
                    LoginInfo.Instance().mylogindata.snid.Add(jd["GameList"][i]["snid"].ToString());
                    if (jd["GameList"][i]["is_open"].ToString() == "1")
                    {
                        gametypeGO.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                        gametypeGO.transform.GetChild(i).GetComponent<Button>().enabled = true;


                    }
                    else
                    {
                        gametypeGO.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                        gametypeGO.transform.GetChild(i).GetComponent<Button>().enabled = false;
                    }

                }
            }
            else
            {
                StartCoroutine(gameinit());
            }

        }
    }

    //游戏选择按钮绑定方法
    public void getroomlist(int game_id, string userid)
    {
        LoginInfo.Instance().mylogindata.choosegame = game_id;
#if UNITY_ANDROID
        StartCoroutine(GetUrl
            (
            LoginInfo.Instance().mylogindata.URL +
            LoginInfo.Instance().mylogindata.liveVideo +
            "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
            "&user_id=" + LoginInfo.Instance().mylogindata.user_id
            )
            );
#elif UNITY_IOS
         StartCoroutine(GetUrl
            (
            LoginInfo.Instance().mylogindata.URL + 
            LoginInfo.Instance().mylogindata.liveVideo + 
            "game_id=" +  LoginInfo.Instance().mylogindata.choosegame + 
            "&user_id=" + LoginInfo.Instance().mylogindata.user_id      
            )
            );
#endif
        //Debug.Log (_gamelogininfo.mylogindata.URL + _gamelogininfo.mylogindata.roomlistAPI + "user_id=" + userid + "&game_id=" + game_id);
        _gamelogininfo.wwwinstance.sendWWW(_gamelogininfo.mylogindata.URL + _gamelogininfo.mylogindata.roomlistAPI + "user_id=" + userid + "&game_id=" + game_id, listtype.listforhallinfo, action.getroomlist, true);


        waitmask.gameObject.SetActive(true);
    }
    IEnumerator GetUrl(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log ("url:"+url);
        yield return www.Send();
      
        if (www.error == null)
        {

            if (www.responseCode == 200)//(jd["code"].ToString() == "200")
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                try
                {
                    if (jd["anchor"].ToString() == "1")
                    {
                        LoginInfo.Instance().mylogindata.nearURL = jd["jin"].ToString();
                        LoginInfo.Instance().mylogindata.farURL = jd["jin"].ToString();
                        try
                        {
                            LoginInfo.Instance().mylogindata.fullURL = jd["quan"].ToString();
                        }
                        catch (Exception)
                        {
                            LoginInfo.Instance().mylogindata.fullURL= jd["jin"].ToString();
                        }
                        LoginInfo.Instance().mylogindata.isAnchor = jd["anchor"].ToString();
                    }
                    else
                    {
                        LoginInfo.Instance().mylogindata.farURL = jd["yuan"].ToString();
                        LoginInfo.Instance().mylogindata.nearURL = jd["jin"].ToString();
                        try
                        {
                            LoginInfo.Instance().mylogindata.fullURL = jd["quan"].ToString();
                        }
                        catch (Exception)
                        {
                            LoginInfo.Instance().mylogindata.fullURL = jd["jin"].ToString();
                        }
                        LoginInfo.Instance().mylogindata.isAnchor = jd["anchor"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    
                }
                



            }
        else
            {
#if UNITY_ANDROID
                StartCoroutine(GetUrl
                    (
                    LoginInfo.Instance().mylogindata.URL +
                    LoginInfo.Instance().mylogindata.liveVideo +
                    "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                    "&user_id=" + LoginInfo.Instance().mylogindata.user_id                   
                    )
                    );
#elif UNITY_IOS
         StartCoroutine(GetUrl
            (
            LoginInfo.Instance().mylogindata.URL + 
            LoginInfo.Instance().mylogindata.liveVideo + 
            "game_id=" +LoginInfo.Instance().mylogindata.choosegame + 
            "&user_id=" + LoginInfo.Instance().mylogindata.user_id      
            )
            );
#endif
            }

        }
        else
        {
#if UNITY_ANDROID
            StartCoroutine(GetUrl
                (
                LoginInfo.Instance().mylogindata.URL +
                LoginInfo.Instance().mylogindata.liveVideo +
                "game_id=" + LoginInfo.Instance().mylogindata.choosegame +
                "&user_id=" + LoginInfo.Instance().mylogindata.user_id
                )
                );
#elif UNITY_IOS
         StartCoroutine(GetUrl
            (
            LoginInfo.Instance().mylogindata.URL + 
            LoginInfo.Instance().mylogindata.liveVideo + 
            "game_id=" +LoginInfo.Instance().mylogindata.choosegame + 
            "&user_id=" + LoginInfo.Instance().mylogindata.user_id      
            )
            );
#endif
        }
    }

    //获取游戏房间
    void changeroomlist(UnityWebRequest data)
    {
        //Debug.LogWarning(data.downloadHandler.text);

        Debug.Log(data.url);

        if (roompanel.activeSelf == true)
        {
			if(data.error==null)
			{
				//此处进行一个对比 对比实时在线的人数
				string temp = data.downloadHandler.text;

				if (temp != "")
				{
					JsonData jd = JsonMapper.ToObject(temp);
					if (jd["code"].ToString() == "200")
					{

						//for (int i = 0; i < jd["roomList"].Count; i++)
						//{
						//    //string temptext = jd["roomList"][i]["exist_number"].ToString() + "      ：" + jd["roomList"][i]["number_limit"].ToString();
						//    if (!temptext.Equals(roomgo.transform.GetChild(i).GetChild(4).GetComponent<Text>().text))
						//    {
						//        roomgo.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = temptext;
						//        Debug.Log("更新成功");
						//    }
						//    else
						//    {
						//        Debug.Log("无需更新");
						//    }
						//}
						clickintoscreen(jd);
						//gamechoosepanel.SetActive(false);
						waitmask.SetActive(false);
						roompanel.SetActive(true);

					}
					else
					{
						Debug.Log("更新错误，等待下一次");
					}
				}
			}

        }
        else
        {
			if(data.error==null)
			{
				string temp = data.downloadHandler.text;

				if (temp != "")
				{
					JsonData jd = JsonMapper.ToObject(temp);
					if (jd["code"].ToString() == "200")
					{



						int cout = jd["roomList"].Count;

						int temptype = 1;
						for (int i = 0; i < jd["roomList"].Count; i++)
						{

							if (i >= roomgo.transform.childCount)
							{
								temptype = 2;
							}
							else if (i < roomgo.transform.childCount)
							{
								temptype = 1;
							}
							setroomcountorcreatnewroom
							(
								jd["roomList"][i]["number_limit"].ToString(),
								jd["roomList"][i]["id"].ToString(),
								jd["roomList"][i]["exist_number"].ToString(),
								jd["roomList"][i]["min_bet"].ToString(),
								jd["roomList"][i]["red_limit"].ToString(),
								i,
								temptype

							);




							if (cout < roomgo.transform.childCount)
							{
								closebutton(cout);
							}
							else
							{

							}
							clickintoscreen(jd);
							//gamechoosepanel.SetActive(false);
							waitmask.SetActive(false);
							roompanel.SetActive(true);
						}




					}
					else
					{
						//closebutton(0);
						messg.Show("错误", "房间已关闭", ButtonStyle.Confirm,
							delegate (MessageBoxResult call)
							{
								switch (call)
								{

								case MessageBoxResult.Cancel:

									break;
								case MessageBoxResult.Confirm:

									break;
								case MessageBoxResult.Timeout:

									break;

								}
							}, 5f);
					}

					///结束输入之后打开面板

					clickintoscreen(jd);
					//gamechoosepanel.SetActive(false);
					waitmask.SetActive(false);
					//roompanel.SetActive(true);
				}
			}

        }
    }

    //创建游戏房间  并设置信息
    void setroomcountorcreatnewroom(string number, string roomnumber, string onlineCount, string min_bet, string red_limit, int countnow, int swithtype)
    {

        if (swithtype == 1)
        {
            roomgo.transform.GetChild(countnow).gameObject.SetActive(true);
            /*if (roomnumber == "1")
            {
                roomnumber = "公开房";
            }*/
            roomgo.transform.GetChild(countnow).GetChild(1).GetComponent<Text>().text = "房间号：" + roomnumber + "    限红：" + red_limit;
            //roomgo.transform.GetChild(countnow).GetChild(2).GetComponent<Text>().text = red_limit;
            //roomgo.transform.GetChild(countnow).GetChild(3).GetComponent<Text>().text = min_bet;
            //roomgo.transform.GetChild(countnow).GetChild(4).GetComponent<Text>().text = onlineCount + "      ：" + 8;
        }
        else
        {
            Transform temp = GameObject.Instantiate(roomparefabsGo, roomgo.transform).transform;
            temp.name = temp.name.ToString() + countnow.ToString();
            //roomgo.transform.GetChild(countnow).GetChild(1).GetComponent<Text>().text = roomnumber;
            //roomgo.transform.GetChild(countnow).GetChild(2).GetComponent<Text>().text = red_limit;
            //roomgo.transform.GetChild(countnow).GetChild(3).GetComponent<Text>().text = min_bet;
            //roomgo.transform.GetChild(countnow).GetChild(4).GetComponent<Text>().text = onlineCount + "      ：" + 8;
            roomgo.transform.GetChild(countnow).GetChild(1).GetComponent<Text>().text = "房间号：" + roomnumber + "    限红：" + red_limit;

        }
    }

    //关闭不要的显示的房间对象
    void closebutton(int temp)
    {
        for (int i = temp; i < roomgo.transform.childCount; i++)
        {
            Destroy(roomgo.transform.GetChild(i).gameObject);
            //roomgo.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void getvoice()
    {
        Audiomanger._instenc.clickvoice();
    }


    //返回游戏选择
    public void closeroompaenl()
    {
        gamechoosepanel.SetActive(true);
        LoginInfo.Instance().mylogindata.choosegame = 0;
        LoginInfo.Instance().mylogindata.room_id = 0;
        for (int i = 0; i < roomgo.transform.childCount; i++)
        {
            Destroy(roomgo.transform.GetChild(i).gameObject);
          
        }
        roompanel.SetActive(false);
    }

    //进入房间之前先与服务器通信获取数据
    IEnumerator goinroom(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
		Debug.Log ("加入房间"+url);
        yield return www.Send();

        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                //LoginInfo.Instance().mylogindata.servicesInfo = jd["wechat_name"].ToString();
				switch (LoginInfo.Instance().mylogindata.choosegame)       //进入游戏
				{
                    case 1://单挑
                        yield return SceneManager.LoadSceneAsync(2);
                        break;
                    case 2://百家乐
                        yield return SceneManager.LoadSceneAsync(3);
                        break;
                    case 3://单双
                        yield return SceneManager.LoadSceneAsync(4);
                        break;
                    case 4://208
                        yield return SceneManager.LoadSceneAsync(5);
                        break;
                    case 5://天地
                        yield return SceneManager.LoadSceneAsync(6);
                        break;
                    case 6://缺一门
                        yield return SceneManager.LoadSceneAsync(7);
                        break;
                    case 7://龙虎
                        yield return SceneManager.LoadSceneAsync(8);
                        break;
                    case 8://单张百乐
                        yield return SceneManager.LoadSceneAsync(9);
                        break;
                    case 9:

                        break;
                    case 10:
                        yield return SceneManager.LoadSceneAsync(11);//夏威夷
                        break;

                    case 111:  //TODO
					    yield return SceneManager.LoadSceneAsync(10);
					break;

				}

            }
            else
            {
                messg.Show("错误", jd["msg"].ToString(), ButtonStyle.Confirm, delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 0f);
            }

        }
        else
        {
            Debug.Log("进入房间，发生错误请重新进入");
        }
    }

    //返回登录界面处理
    public void gotologin()
    {
        LoginInfo.Instance().cleanmylogindata();
        Audiomanger._instenc.clickvoice();
        SceneManager.LoadScene(0);
    }

    //修改密码
    public void changepassword()
    {

        if (passwordchangetext[0].text == "" || passwordchangetext[1].text == "" || passwordchangetext[2].text == "")
        {
            messg.Show("密码修改", "请正确填入密码", ButtonStyle.Confirm,
                delegate (MessageBoxResult call)
                {
                    switch (call)
                    {

                        case MessageBoxResult.Cancel:

                            break;
                        case MessageBoxResult.Confirm:

                            break;
                        case MessageBoxResult.Timeout:

                            break;

                    }
                }, 0f);
            return;
        }

        StartCoroutine(changepassword(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.changepasswordAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&password=" + passwordchangetext[0].text + "&newpassword=" + passwordchangetext[1].text + "&confirmpassword=" + passwordchangetext[2].text));
    }

    //修改密码协程
    IEnumerator changepassword(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
        if (www.error == null)
        {
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < passwordchangetext.Count; i++)
                {
                    passwordchangetext[i].text = "";
                }
                messg.Show("密码修改", jd["msg"].ToString(), ButtonStyle.Confirm,
               
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
            }
            else
            {
                messg.Show("异常", jd["msg"].ToString(), ButtonStyle.Confirm,
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
            }
        }
        else
        {
            messg.Show("异常", "网络异常，修改密码失败请等会重试", ButtonStyle.Confirm,
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
        }

    }

    //修改支付验证用
    public void changeCAPTCHA()
    {

        if (CAPTCHAchangetext[0].text == "" || CAPTCHAchangetext[1].text == "" )
        {
            
            messg.Show("密码修改", "请正确填入密码", ButtonStyle.Confirm,
                delegate (MessageBoxResult call)
                {
                    switch (call)
                    {

                        case MessageBoxResult.Cancel:

                            break;
                        case MessageBoxResult.Confirm:

                            break;
                        case MessageBoxResult.Timeout:

                            break;

                    }
                }, 0f);
            return;
        }

        StartCoroutine(changeCAPTCHA(LoginInfo.Instance().mylogindata.URL + "mdify-security?" + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&one_safety_code=" + CAPTCHAchangetext[0].text + "&two_safety_code=" + CAPTCHAchangetext[1].text));
    }

    //修改支付验证协程
    IEnumerator changeCAPTCHA(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        Debug.Log(www.url);
        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
        if (www.error == null)
        {
            if (jd["code"].ToString() == "200")
            {
                for (int i = 0; i < CAPTCHAchangetext.Count; i++)
                {
                    CAPTCHAchangetext[i].text = "";
                }
                messg.Show("支付验证修改", jd["msg"].ToString(), ButtonStyle.Confirm,
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
            }
            else
            {
                messg.Show("异常", jd["msg"].ToString(), ButtonStyle.Confirm,
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
            }
        }
        else
        {
            messg.Show("异常", "网络异常，修改安全码失败请等会重试", ButtonStyle.Confirm,
                 delegate (MessageBoxResult call)
                 {
                     switch (call)
                     {

                         case MessageBoxResult.Cancel:

                             break;
                         case MessageBoxResult.Confirm:

                             break;
                         case MessageBoxResult.Timeout:

                             break;

                     }
                 }, 5f);
        }

    }


    public GameObject loadingBack;
    public Slider loadingSlider;

    IEnumerator ShowLoading()
    {
        loadingBack.SetActive(true);
        loadingSlider.value = 0;
        while (true)
        {
            float a = UnityEngine.Random.Range(0.1f, 0.6f);
            loadingSlider.value += a;
            if (loadingSlider.value >= 1)
            {
                loadingBack.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }


    }

    IEnumerator GetServices(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                servicesOne.text = jd["services_one"].ToString();
                servicesTwo.text = jd["services_two"].ToString();
            }
            else
            {
                StartCoroutine(GetServices(url));
            }
        }
        else
        {
            StartCoroutine(GetServices(url));
        }
    }

}
