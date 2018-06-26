using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using LitJson;
using com.QH.QPGame.Lobby.Surfaces;

public class loginuser : MonoBehaviour
{
    public GameObject userpanel; //用户面板
    public GameObject restagaenpaenl; //注册面板
    public GameObject errorGo;// 错误提示面板
    public GameObject GOFA;//错误信息对象
    public loginsetting setting;//
    public GameObject loginwait;//屏蔽面板
    public MessageBoxPopup messg;//错误信息预设体
    public Toggle passwordre;//密码框


    public string Version;//版本信息
    public string iosVersion; // ios版本信息

	public Text Versiontext;

    private Button backfobutton;
    private Button backforebutton;
    private Button loginbutton;

    // Use this for initialization

    private void Awake()
    {
		
        if (loadingBack.GetComponent<Image>().sprite.name == "splash")
        {
            LoginInfo.Instance().mylogindata.gameType = "tianhua";
        }
        else
        {
            LoginInfo.Instance().mylogindata.gameType = "zhonghua";
        }
    }
    void Start()
    {
		#if UNITY_ANDROID 
		Versiontext.text="版本号："+Version;
		#elif UNITY_IOS
		Versiontext.text="版本号："+iosVersion;
		#endif
        StartCoroutine(ShowLoading());
        //callApplication("com.sina");
        LoginInfo.Instance();
        LoginInfo.Instance().mylogindata.version = Version;
        LoginInfo.Instance().mylogindata.iosversion = iosVersion;
        Screen.orientation = ScreenOrientation.Landscape;
        backfobutton = userpanel.transform.Find("loginbground").Find("backtof").GetComponent<Button>();
        backfobutton.onClick.AddListener(backfobutton.GetComponent<showWindow>().OnClick);
        backfobutton.onClick.AddListener(Audiomanger._instenc.clickvoice);
        backforebutton = restagaenpaenl.transform.Find("registartionbground").Find("backtof").GetComponent<Button>();
        backforebutton.onClick.AddListener(backforebutton.GetComponent<showWindow>().OnClick);

        backforebutton.onClick.AddListener(Audiomanger._instenc.clickvoice);
        loginbutton = userpanel.transform.GetChild(0).Find("Button").GetComponent<Button>();
        loginbutton.onClick.AddListener(load);
        loginbutton.onClick.AddListener(Audiomanger._instenc.clickvoice);

        Debug.Log(Application.identifier);

        //后台运行
        Application.runInBackground = true;

        LoginInfo.Instance().wwwinstance.logincallback += cheaklogininofo;
        LoginInfo.Instance().wwwinstance.regantsionback += cheakragininfo;
        //Debug.Log(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.VersioninfoAPI + "type=" + LoginInfo.Instance().mylogindata.gameType);
        StartCoroutine(getversion(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.VersioninfoAPI + "type=" + LoginInfo.Instance().mylogindata.gameType));
        //passwordre.onValueChanged.AddListener(readmenberpassword);
        if (PlayerPrefs.HasKey("username"))
        {
            setting.userinfotext.text = PlayerPrefs.GetString("username");
            setting.userpasswordtext.text = PlayerPrefs.GetString("password");
            passwordre.isOn = true;
        }
        else
        {
            passwordre.isOn = false;
        }

# if UNITY_ANDROID
        userpanel.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);

#endif

    }

    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            errormsgInstance("错误测试", GOFA);
//        }
//    }

    void readmenberpassword()
    {
        if (passwordre.isOn)
        {
            if (setting.userinfotext.text != "" && setting.userpasswordtext.text != "")
            {
                PlayerPrefs.SetString("username", setting.userinfotext.text);
                PlayerPrefs.SetString("password", setting.userpasswordtext.text);
                Debug.Log("记住账号密码");
            }
        }
        else
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("删除记住密码");
        }
    }


    //private void FixedUpdate()
    //{
        //if (LoginInfo.Instance().wwwinstance.listforlogininfo.Count > 0)
        //{
        //    if (LoginInfo.Instance().wwwinstance.listforlogininfo.Peek().statetype == 1)
        //    {

        //      LoginInfo.Instance().wwwinstance.changelogin();
        //    }

        //}
    //}


    IEnumerator getversion(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        Debug.Log(URL);
        yield return www.Send();

        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

        if (www.error == null && www.isDone)
        {
           
            if (jd["code"].ToString() == "200")
            {
#if UNITY_ANDROID
                if (Version.Equals(jd["androidVersion"].ToString()))
                {
                    LoginInfo.Instance().mylogindata.version = jd["androidVersion"].ToString();
                }
                else
                {
                    messg.Show("提示", "当前版本号与最新版本不一致，请重新下载游戏",
                            ButtonStyle.Confirm,
                           delegate (MessageBoxResult result)
                           {
                               switch (result)
                               {
                                   case MessageBoxResult.Timeout:
                                       {
                                           //SceneManager.LoadScene("scene_login");
                                           Application.OpenURL(jd["url"].ToString());
                                           break;
                                       }

                                   case MessageBoxResult.Confirm:
                                       {
                                           //SceneManager.LoadScene("scene_login");
                                           Application.OpenURL(jd["url"].ToString());
                                           break;
                                       }
                               }
                           },
                           10f);

                }
#endif
#if UNITY_IOS
                if (iosVersion.Equals(jd["iosVersion"].ToString()))
                {
                    LoginInfo.Instance().mylogindata.iosversion = jd["iosVersion"].ToString();
                }
                else
                {
                    messg.Show("提示", "当前版本号与最新版本不一致，请重新下载游戏",
                            ButtonStyle.Confirm,
                           delegate (MessageBoxResult result)
                           {
                               switch (result)
                               {
                                   case MessageBoxResult.Timeout:
                                       {
                                           //SceneManager.LoadScene("scene_login");
                                           Application.OpenURL(jd["url"].ToString());
                                           break;
                                       }
                                
                                   case MessageBoxResult.Confirm:
                                       {
                                           //SceneManager.LoadScene("scene_login");
                                           Application.OpenURL(jd["url"].ToString());
                                           break;
                                       }
                               }
                           },
                           10f);
                    
                }
#endif

            }
        }
        else
        {
            getversion(URL);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (LoginInfo.Instance().wwwinstance.listforlogininfo.Count > 0)
    //    {
    //        cheaklogininofo(LoginInfo.Instance().wwwinstance.listforlogininfo.Dequeue());
    //    }
    //}
    void load()
    {
        loginwait.SetActive(true);

        if (setting.userinfotext.text != "" && setting.userpasswordtext.text != "")
        {

            var temp = LoginInfo.Instance().mylogindata;
			Debug.Log (temp.URL + temp.loginAPI + "username=" + setting.userinfotext.text.ToString() + "&password=" + setting.userpasswordtext.text.ToString());
            LoginInfo.Instance().wwwinstance.sendWWW(temp.URL + temp.loginAPI + "username=" + setting.userinfotext.text.ToString() + "&password=" + setting.userpasswordtext.text.ToString(), listtype.listforlogininfo, action.login, true);
            StartCoroutine("isoverload");
            readmenberpassword();
            Debug.Log("给予登录信息");
        }
        else
        {
            if (setting.userinfotext.text == "")
            {
                loginerror("请输入用户名");
            }
            else if (setting.userpasswordtext.text == "")
            {
                loginerror("请输入密码");
            }
        }

    }

    string userName_;
    string userPassWord;
    public void ranagestion()
    {
        //有安全码
        //if (setting.reagenuserinfotext.text != "" && setting.reagenpassinfotext.text != "" && setting.reagenpassinfoagtext.text != null && setting.agenytext.text != ""&&setting.CAPTCHA.text!="")
        //{
        //    if (setting.reagenpassinfotext.text.ToString() == setting.reagenpassinfoagtext.text.ToString())
        //    {
        //        loginwait.SetActive(true);
        //        var temp = LoginInfo.Instance().mylogindata;
        //        StartCoroutine("isoverload");
        //        userName_ = setting.reagenuserinfotext.text;
        //        userPassWord = setting.reagenpassinfotext.text;
        //        LoginInfo.Instance().wwwinstance.sendWWW(temp.URL + temp.raganstionAPI + "username=" + setting.reagenuserinfotext.text.ToString() + "&password=" + setting.reagenpassinfotext.text.ToString() + "&uid=" + setting.agenytext.text.ToString() + "&safety_code=" + setting.CAPTCHA.text, listtype.listforlogininfo, action.reganstion, false);//
        //        Debug.Log("给予注册信息");
        //    }
        //    else if (!setting.reagenpassinfoagtext.text.Equals(setting.reagenpassinfotext.text))
        //    {
        //        loginerror("两次的密码不一致");
        //    }
        //}
        //else
        //{
        //    if (setting.reagenuserinfotext.text == "")
        //    {
        //        loginerror("请输入用户名");

        //    }
        //    else if (setting.reagenpassinfotext.text == "")
        //    {
        //        loginerror("请输入密码");

        //    }
        //    else if (setting.reagenpassinfoagtext.text == "")
        //    {
        //        loginerror("请再次输入密码");
        //    }
        //    else if (setting.agenytext.text == "")
        //    {
        //        loginerror("请输入推荐人");
        //    }
        //    else if (setting.CAPTCHA.text == "")
        //    {
        //        loginerror("请确定安全密码");
        //    }
        //}

        //无安全码          【有手动加的安全码】
        if (setting.reagenuserinfotext.text != "" && setting.reagenpassinfotext.text != "" && setting.reagenpassinfoagtext.text != null && setting.agenytext.text != "")
        {
            if (setting.reagenpassinfotext.text.ToString() == setting.reagenpassinfoagtext.text.ToString())
            {
                loginwait.SetActive(true);
                var temp = LoginInfo.Instance().mylogindata;
                StartCoroutine("isoverload");
                userName_ = setting.reagenuserinfotext.text;
                userPassWord = setting.reagenpassinfotext.text;
                LoginInfo.Instance().wwwinstance.sendWWW(temp.URL + temp.raganstionAPI + "username=" + setting.reagenuserinfotext.text.ToString() + "&password=" + setting.reagenpassinfotext.text.ToString() + "&uid=" + setting.agenytext.text.ToString() + "&safety_code=66666", listtype.listforlogininfo, action.reganstion, false);//+ "&safety_code=208"
                Debug.Log("给予注册信息");
            }
            else if (!setting.reagenpassinfoagtext.text.Equals(setting.reagenpassinfotext.text))
            {
                loginerror("两次的密码不一致");
            }
        }
        else
        {
            if (setting.reagenuserinfotext.text == "")
            {
                loginerror("请输入用户名");

            }
            else if (setting.reagenpassinfotext.text == "")
            {
                loginerror("请输入密码");

            }
            else if (setting.reagenpassinfoagtext.text == "")
            {
                loginerror("请再次输入密码");
            }
            else if (setting.agenytext.text == "")
            {
                loginerror("请输入推荐人");
            }
        }
    }

    private void OnDestroy()
    {
        LoginInfo.Instance().wwwinstance.logincallback -= cheaklogininofo;
        LoginInfo.Instance().wwwinstance.regantsionback -= cheakragininfo;
    }


    IEnumerator isoverload()
    {
        Debug.Log("超时计时开始");
        yield return new WaitForSeconds(20f);
        Debug.Log("超时结束");
        LoginInfo.Instance().wwwinstance.stopieinlogin();
        messg.Show("提示", "登录超时请检查网络",
                        ButtonStyle.Confirm,
                       delegate (MessageBoxResult result)
                       {
                           switch (result)
                           {
                               case MessageBoxResult.Timeout:
                                   {
                                       loginwait.SetActive(false);
                                       break;
                                   }

                               case MessageBoxResult.Confirm:
                                   {

                                       loginwait.SetActive(false);
                                       break;
                                   }
                           }
                       },
                       10f);
    }



    //登录总入口 用于检测登录之后加载场景
    IEnumerator loginskin()
    {
        yield return userinfocheak();
        if (true)
        {

            yield return loadtohall();

        }
    }
    //此方法用于登录检测
    IEnumerator userinfocheak()
    {
        if (setting.userinfotext.text != "" && setting.userpasswordtext.text != "")
        {

        }
        else
        {
            yield break;
        }


        yield return null;
    }
    //然后异步加载场景
    IEnumerator loadtohall()
    {
        var temp = SceneManager.LoadSceneAsync(1);
        yield return temp;
    }
    /// <summary>
    /// 在登录界面使用的错误信息生成
    /// </summary>
    /// <param name="value">错误信息</param>
    /// <param name="generated">父级对象</param>
    void errormsgInstance(string value, GameObject generated)
    {

        GameObject go = GameObject.Instantiate(errorGo, generated.transform);
        go.GetComponent<changetext>().changefrontsize(25);
        go.GetComponent<changetext>().msgset(value);

    }
    /// <summary>
    /// 登录错误信息并关闭遮挡面板
    /// </summary>
    /// <param name="value"></param>
    void loginerror(string value)
    {
        errormsgInstance(value, GOFA);
        loginwait.SetActive(false);
    }

    void cheaklogininofo(UnityWebRequest temp)
    {
        StopCoroutine("isoverload");
        Debug.Log(temp.downloadHandler.text);



        JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);
        if (jd["code"].ToString() == "200")
        {
            var usertemp = LoginInfo.Instance().mylogindata;
            usertemp.user_id = jd["UserInfo"]["user_id"].ToString();
            usertemp.token = jd["UserInfo"]["unionuid"].ToString();
            usertemp.username = jd["UserInfo"]["username"].ToString();
            usertemp.ALLScroce = jd["UserInfo"]["quick_credit"].ToString();
            usertemp.login_ip = jd["UserInfo"]["login_ip"].ToString();
            usertemp.status = jd["UserInfo"]["status"].ToString();

            StartCoroutine(loadtohall());

        }
        else
        {
            messg.Show("提示", jd["msg"].ToString(),
                            ButtonStyle.Confirm,
                           delegate (MessageBoxResult result)
                           {
                               switch (result)
                               {
                                   case MessageBoxResult.Timeout:
                                       {
                                           loginwait.SetActive(false);
                                           break;
                                       }

                                   case MessageBoxResult.Confirm:
                                       {

                                           loginwait.SetActive(false);

                                           break;
                                       }
                               }
                           },
                           10f);
        }


    }

    void cheakragininfo(UnityWebRequest temp)
    {

        if (temp != null)
        {
            JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                messg.Show("提示", "注册成功请重新登录",
                           ButtonStyle.Confirm,
                          delegate (MessageBoxResult result)
                          {
                              switch (result)
                              {
                                  case MessageBoxResult.Timeout:
                                      {
                                          loginwait.SetActive(false);
                                          break;
                                      }

                                  case MessageBoxResult.Confirm:
                                      {

                                          loginwait.SetActive(false);

                                          break;
                                      }
                              }
                              setting.userinfotext.text = userName_;
                              setting.userpasswordtext.text = userPassWord;
                          },
                          10f);
            }
            else
            {
                messg.Show("提示", jd["msg"].ToString(),
                          ButtonStyle.Confirm,
                         delegate (MessageBoxResult result)
                         {
                             switch (result)
                             {
                                 case MessageBoxResult.Timeout:
                                     {
                                         loginwait.SetActive(false);
                                         break;
                                     }

                                 case MessageBoxResult.Confirm:
                                     {

                                         loginwait.SetActive(false);
                                         break;
                                     }
                             }
                         },
                         10f);
            }

        }
    }
    //在对应的市场中寻找对应的应用
    public void openAPPinMarket(string appid)
    {
        appid = "com.sina.weibo";
#if UNITY_ANDROID
        //init AndroidJavaClass
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); ;
        AndroidJavaClass Intent = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri");

        // get currentActivity
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject jstr_content = new AndroidJavaObject("java.lang.String", "market://details?id=" + appid);

        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<AndroidJavaObject>("ACTION_VIEW"), Uri.CallStatic<AndroidJavaObject>("parse", jstr_content));

        currentActivity.Call("startActivity", intent);
#endif
    }

    //调用其他应用
    public void callApplication(string packageName, AndroidJavaObject currentActivity = null)
    {

        if (currentActivity == null)
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
        currentActivity.Call("startActivity", intent);
    }

    public void quitgame()
    {
#if UNITY_ANDROID


        Application.Quit();

#elif UNITY_IOS
      
         Application.Quit();
#endif
    }

    public GameObject loadingBack;
    public Slider loadingSlider;

    IEnumerator ShowLoading()
    {
        loadingBack.SetActive(true);
        loadingSlider.value = 0;
        while (true)
        {
            float a = Random.Range(0.1f, 0.6f);
            loadingSlider.value += a;
            if (loadingSlider.value >= 1)
            {
                loadingBack.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }

    }

}
