using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
using com.QH.QPGame.Lobby.Surfaces;


//获取的倒计时数据
[System.Serializable]
public class countdowdata
{
    public int    is_open;//处于的游戏状态  值：0 1 2
    public string periods;//期数
    public int    couwttime;//倒计时
    public poketstate wintype1;//对应花色结果1
    public poketstate wintype2;//对应花色结果2

    //public int    wincode;//对应数据

    public string last_periods;//上期的期数
    public string TotalCount;//今日期数
    public string RemainData;//剩余期数
    public string last_winnumber;//上期中奖号码
    public string is_win;//开奖标志
    public string win_number;//当期的开奖号码
    public int    EWinTime;//开奖结束后的倒计时
    public int    WinnerCountdown;//包括封盘时间的倒计时
  
}


public class GameMagert : MonoBehaviour {


    static GameMagert _instance; 
    public List<Text> uesrinscoercout;//花色次数左边
    public List<Text> userinscoercoutright;//花色次数右边
    public List<Text> userscorein;//玩家下注显示 
    public List<Text> uiallintext;//全体下注显示区域
    public List<Button> buttonlist;//按钮列表
    public List<Text> titlettextlist;//标题信息列表
    public List<Button> canellist;//取消按钮列表
    public List<Image> imagechange;//图案列表
    public Text statetext;//状态文字条
    public Text coundowntext;//倒计时文字
    public GameObject pokerlist;//扑克池列表
    public GameObject pokerlist2;//扑克池列表2
    public GameObject poketGOtoinit;//存放所有的牌的预设对象 需要时打开获取并复制
    public GameObject userGo;//用户Go
    public GameObject error_left;//错误信息提示左侧GO
    public GameObject error_right;//错误信息提示右侧GO
    public Button cnealGO;//左区域取消按钮
    public Button cnealGO2;//右区域取消按钮
    public Text AllScroceText;//总分组件
    public Text WinScroceInText;//总压分组件
    public int waittimeui;//等待时间
    public GameObject errorGo; //错误信息预设体
    public Button quit; //返回按钮
    //public uniweblist uniwebtoopen; //uniweb插件脚本对象
    public string URL; //目标的URL
    public List<GameObject> buttonmask;//按钮阻挡层
    public InputField inputtext;//输入框
    public GameObject winpanel;//赢钱之后的面板
    public GameObject winpanel2;//赢钱之后的面板2
    public Text gonggao;//公告
    public MessageBoxPopup messg;//消息框预设体
    public static bool swithonislistchange;//是否有新消息添加
    public countdowdata nowdata;//获取从服务器获得的此刻的游戏数据
    public playstate ChangeState;//状态机 变换状态
    public delegate void cheakpoket();//检测扑克池子是否正确列表
    public event cheakpoket cheak;//检测事件
    public int severcount;//设定服务器的开牌总数 （显示卡牌的上线）
    public Toggle fullwindowGO;//全屏开关
    public Slider audiosource;//音量调整开关
    public Toggle voicetoggle;//声音开关
    public GameObject changeimageGO;//确认提交按钮
    public Button canelconfirmGO;//取消下注
    public GameObject useropenGO;//用户下注历史界面
    public GameObject canelhint;//取消提示面板
    public Text canelhinttext;//提示文本
    public GameObject timeover;//当前一局即66期结束
    public Button rushpanel;//刷新按钮
    public Text limittext;//限红文本
    public Text lessdown;//最低下注文本
    public Text thistimecoutText;//本次下注总分文本


    private delegate void cheakupdatemoney();
    private event cheakupdatemoney cheakindang;

    //定期刷新
    private delegate void isfinishiAndRushView();
    private event isfinishiAndRushView rushviewevent;

    private delegate void onetime_date(string opendate);
    private event onetime_date openinfrist;

    public int count;
    public int Count
    {
        get
        {
            return count;
        }
    }
    public string value_;
    public string value
    {
        get
        {
            return value_;
        }
    }



    private bool fristopen; //是否为第一次加载网页
    private playstate NowState;//状态机 此刻状态
    private int countsize;//牌池中显示的位置 范围：（0-101） （重点修改）
    private bool isfrist;//是否为第一次初始化牌池
    private int[] listrange;//随意加分数组
    private int tempc;//用于临时储存随机数值值
    private int[] poketcount;//牌色次数
    private int[] poketcount2;//另一组牌色次数
    private int winCodeforSever;//用于获取服务器得分数据
    private int winCodeforUi;//用于UI显示 最好分离
    private double allScroceForSever;//用于获取服务器总分数据
    private double allScroceForUi;//用于UI显示 最好分离
    protected int[] historyuserin;//用于存储和修改曾经下过的数值并用于实际计算（确认投注可用 功能待定）
    protected int historyallin; //历史总下注 用于金钱计算(可能用实际的服务器数据代替)
    private List<poketstate> severpokethuase;//从服务器获取的花色                (重点修改）
    private List<poketstate> severpokethuase2;//从服务器获取的花色另一个池子  
    private int[] betinlist;//本地下注数组
    private bool openinnexttime;//是否需要做服务检查
    //private double beiallinvalue;//本地下注总值
   

    private List<int> severpoketcode;//从服务器获取的牌的值
    private IEnumerator counwtIDE;//临时倒计时协程
    private List<int> buttonIDcode;//对应的按钮ID
    private List<string> buttonrate;//对应的赔率
    private int gotohallint; //断线错误次数
    private  bool isPause; //是否暂停标记
    private  bool isFocus;//是否失焦标记
    private bool isopendatafromsever=true;//是否成功开奖 true 为正常开奖 false为开奖预备
    private int erroropendatacout;//错误开奖的次数 （当积累到一定次数之后将把玩家弹出房间）;
    public static bool iscomeback=false;//是否 属于返回
    public static bool isupdatetime = false; //是否 更新时间标记位
    private bool isopennextdata=false;//是否跳期记号
    private Vector3 startcont;//公告其实位置
    private Vector3 endcont;//公告结束位置 （注意超过界面的字体将不被渲染）
    private bool isfullopenwindows;//直播开奖全屏开关
    private bool fristuserin; //是否需要获取当局玩家下注开关
    private bool isbetincheak;
    private bool istimeover=false;//是否结束一局
    public int gonggaocount;
    private int countouit;//计算触发次数
    private int rushin=0;
    private bool isfristopennumber=true;
    private string textwindata="";//开奖需要传递的数据
    private int updatenowcount=0;//输赢列表更新计数器
    private int updatelimitcount=3;//计数器限制
    private int updateusercoutnowcount = 101;//用户下注更新计数器
    private int updateusercoutlimitcount = 100;//计数器限制
    private bool isopenanmimation = true;//延迟更新用户总金额更新开关
    private static bool fixeddatestringswith=false;//修正期数开关
    private bool isopendatenocheakswith=false;//是否当期不检测的开关

    public Button tuiShuiShowButton; //显示退水面板按钮

    public Button tuiShuiButton;  //退水按钮

    public Text tuiShuiRate; //退水率

    public Text tuiShuiRateNum; // 退水额度

    public GameObject TuiShuiPanel; //退水面板


    Vector3 vecFir;

    //public GameObject servicesBtn; // 客服button

    public Text servicesPanelText; //客服面板

    public List<Text> rateList; // 赔率面板

    bool isFirstRefreashRate; //是否第一次刷新赔率

    public GameObject grid; // 牌色格子预制体

    public Transform gridNewPar; //牌色格子对象池父物体
    public List<GameObject> gridNew; //对象池里所有牌色格子
    int aGrid;

    public ScrollRect[] gridScrool;
    public GameObject uiGoPanel; // 为适配ipad


    bool agr = false;
    bool bgr;
    bool isChangeKaiJiang; // 更改开奖背景音乐
    bool isChangeXiaZhu; // 更改下注背景音乐


    public Text mainText;


    public static GameMagert Instance
    {
        get
        {
            return _instance;
        }
        
       

    }

    /// <summary>
    /// 引用
    /// </summary>
    public string Textwindata
    {
        get
        {
            return textwindata;
        }

        set
        {
            textwindata = value;
        }
    }


    private void Awake()
    {
        _instance = this;


    }
    //初始化数值参数

    
    void OnEnable()
    {
        isPause = false;
        isFocus = false;
    }

    

    //初始化数值
    void init()
    {
        mainText.text = "切换 " + LoginInfo.Instance().mylogindata.roomcount;
        vecFir = gonggao.rectTransform.localPosition += new Vector3(gonggao.preferredWidth, 0, 0);
        ChangeState = playstate.betready;
        isfrist = true;
        listrange = new int[] { 10, 50, 100, 500 };
        poketcount = new int[] { 0, 0, 0, 0, 0 };
        poketcount2 = new int[] { 0, 0, 0, 0 };
        tempc = 0;
        betinlist = new int[] { 0, 0, 0, 0, 0 };
        gonggaocount = 0;
        LoginInfo.Instance();

        servicesPanelText.text = LoginInfo.Instance().mylogindata.servicesInfo;
        aGrid = 0;
        //agr = pokerlist.transform.childCount;
        //bgr = pokerlist2.transform.childCount;
        fristopen = true;
        fristuserin = true;
        isbetincheak = true;
        openinnexttime = false;
        isFirstRefreashRate = true;
        bgr = true;
        isChangeKaiJiang = false;
        isChangeXiaZhu = false;
        LoginInfo.Instance().mylogindata.isOpenError = true;
        countouit = 0;
        rushin = 0;
        //uniwebtoopen.URL = URL;
        LoginInfo.Instance().mylogindata.coindown =int.Parse( LoginInfo.Instance().mylogindata.roomcount);
        historyuserin = new int[5];
        swithonislistchange = true;
        severpoketcode = new List<int>();
        severpokethuase = new List<poketstate>();
        severpokethuase2 = new List<poketstate>();
        nowdata = new countdowdata();
        buttonrate = new List<string>();
        buttonIDcode = new List<int>();
        erroropendatacout = 0;
        //头部信息初始化
        titlettextlist[0].text = LoginInfo.Instance().mylogindata.username;
        titlettextlist[1].text = "0";
        titlettextlist[2].text = "0";
        titlettextlist[3].text = "0";
        titlettextlist[4].text = "0";
        titlettextlist[5].text = "0";
        limittext.text = LoginInfo.Instance().mylogindata.roomlitmit;//限红 
        lessdown.text = LoginInfo.Instance().mylogindata.roomcount;//最小压分
        gotohallint = 0;
        
        //titlettextlist[5].text = UnityEngine.Random.Range(1, 9).ToString();
        //视频尺寸处理
        //IOS视频处理需要自定义尺寸

        startcont = gonggao.rectTransform.localPosition;
        endcont = startcont;

        isfullopenwindows = false;
        //添加监听 对应的按钮监听 全屏开关 背景音调节 声音开关
        fullwindowGO.onValueChanged.AddListener(fullwindowcontorl);
        voicetoggle.onValueChanged.AddListener(closeclickvoice);
        audiosource.onValueChanged.AddListener(backmusicchange);
        //canelconfirmGO.onClick.AddListener(CanelBetIn);
        //rushpanel.onClick.AddListener(refrshview);
        //Debug.Log(gonggao.preferredWidth);
        cheak += cheakpoketpoolisture;
        cheakindang += cheakifmoney;

        tuiShuiShowButton.onClick.AddListener(OnShowTuiShui);
        tuiShuiButton.onClick.AddListener(OnTuiShui);
    }

    


    // Use this for initialization
    void Start() {
        
        init();
        Screen.orientation = ScreenOrientation.Landscape;
        if (Screen.width == 2048 && Screen.height == 1536 || Screen.width == 1536 && Screen.height == 2048) //适配ipad air2
        {
            uiGoPanel.transform.localScale = new Vector3(0.81f, 1, 1);
        }
        Debug.Log(System.DateTime.Now.Hour);
      
        


        //Debug.Log();
        getpoint( Convert.ToDouble(LoginInfo.Instance().mylogindata.ALLScroce));
      
        setUserUIinfo();
        addlistenrt();
        
        StartCoroutine(polling());
        //Debug.Log(gonggao.preferredHeight);
        StartCoroutine(gonggaoanmation());

        //Debug.Log(gonggao.preferredWidth);
    }

    /// <summary>
    /// 修改用户总数的显示
    /// </summary>
    /// <param name="temp"></param>
    void getpoint(Double temp)
    {
        allScroceForSever = temp;
        allScroceForUi = allScroceForSever;
        setallsroce(allScroceForUi);
    }
    /// <summary>
    /// 只用于修改UI金钱
    /// </summary>
    /// <param name="temp"></param>
    void changetextmoney(Double temp)
    {
        allScroceForUi = temp;
        setallsroce(allScroceForUi);
    }
    /// <summary>
    /// 对整个数组进行修改
    /// </summary>
    /// <param name="list">需要修改的数组 长度需要一致</param>
    void changecountall(List<poketstate> list)
    {
        for (int i = 0; i <list.Count; i++)
        {
            if ((int)list[i] > 3)
            {
             poketcount[(int)list[i]%4]++;
             poketcount[4]++;
            }
            else
            {
                poketcount[(int)list[i]]++;
            }
            //遍历之前先将原有的总数进行添加
         }
    }
    void changecountallright(List<poketstate> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //遍历之前先将原有的总数进行添加
            poketcount2[(int)list[i]%4]++;
        }
    }


    /// <summary>
    /// 对总次数数值进行单个添加
    /// </summary>
    /// <param name="state">需要修改的状态</param>
    void changecountall(poketstate state)
    {
        if ((int)state > 3)
        {
            poketcount[(int)state % 4]++;
            poketcount[4]++;
        }
        else
        {
            poketcount[(int)state]++;
        }
    }

 /// <summary>
 /// 对右边区域花色出现次数进行计数
 /// </summary>
 /// <param name="state"></param>
    void changecountallright(poketstate state)
    {
        poketcount2[(int)state%4]++;
    }


    /// <summary>
    /// 在全局修复时数据进行一次存储提供个历史记录方法使用 间隔时间为updatelimitcount*2秒
    /// </summary>
    /// <param name="text"></param>
    void opendataUpdate(string text)
    {
        if (text!= "")
        {
            updatenowcount++;
            if (updatenowcount < updatelimitcount&&this.textwindata!="")
            {

                
            }
            else
            {
                this.Textwindata = text;
                updatenowcount = 0;
                Debug.Log("已经更新开奖数据到开奖结果");
            }
        }
    }



     
    ///检查是否需要修复
    ///此方法用于一个全局修复扑克池中的数据
    IEnumerator cheakwillfixed(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();


        opendataUpdate(www.downloadHandler.text);

        Debug.Log("开奖数据："+ www.downloadHandler.text);

        JsonData jd = getdataforjson(www.downloadHandler.text);
        if (www.error == null)
        {
            if (NowState != playstate.betclearing)
            {

                if (isopendatenocheakswith)
                {

                }
                else
                {





                    if (jd["code"].ToString() == "200")
                    {
                        //当服务器数据小于客户端数据时
                        if (jd["ArrList"].Count < countsize)
                        {
                            startclosepoket(0);
                            Debug.LogError("启用开奖多期纠正");
                            yield return StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 1));
                        }
                        else
                        {
                            Debug.LogError("未启用纠正");
                        }

                        yield return new WaitForSeconds(1f);

                        if (NowState == playstate.betclearing)
                        {
                            yield break;
                        }

                        if (jd["ArrList"].Count > countsize)
                        {
                            startclosepoket(0);
                            Debug.LogError("启用开奖漏期纠正");
                            yield return StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 1));
                        }
                    }
                }
            }

        }
    }

    /// <summary>
    /// 公告动画
    /// </summary>
    /// <returns></returns>
    IEnumerator gonggaoanmation()
    {
        
       
        while (true)
        {

            gonggao.rectTransform.localPosition -= new Vector3(2, 0, 0);
            //endcont -= new Vector3(2, 0, 0);
            yield return new WaitForSeconds(0.05f);
            //if ( startcont.x-endcont.x > gonggao.preferredWidth+1000)
            //{
            //    gonggao.rectTransform.localPosition = startcont;
            //    endcont = startcont;
            //    gonggaocount++;

            //}
            //Debug.Log("公告" + gonggao.rectTransform.localPosition.x);
            //Debug.Log(vecFir.x - gonggao.preferredWidth / 2f - 486.5f - 5);
            if (gonggao.rectTransform.localPosition.x <= (vecFir.x - gonggao.preferredWidth / 2f - 486.5f - 5))
            {
                gonggao.rectTransform.localPosition = (vecFir + new Vector3(gonggao.preferredWidth / 2f , 0, 0));
                gonggao.transform.localScale = Vector3.zero;
                endcont = startcont;
                gonggaocount++;
                //yield return new WaitForSeconds(0.05f);
                
            }

        }

        
    }
    /// <summary>
    /// 轮询的方法 获取用户数据 获取来自服务器的数据
    /// </summary>
    /// <returns></returns>
    IEnumerator polling()
    {
       
            while (true)
            {


                if (System.DateTime.Now.Hour < 4)
                {
                    if (titlettextlist[3].text.Equals("48"))
                    {
                        isopendatenocheakswith = true;
                    }
                    else
                    {
                       isopendatenocheakswith = false;
                    }
                    changeimageGO.transform.GetChild(0).gameObject.SetActive(false);
                    changeimageGO.transform.GetChild(6).gameObject.SetActive(true);
                }
                else
                {
                    isopendatenocheakswith = false;
                    changeimageGO.transform.GetChild(0).gameObject.SetActive(true);
                    changeimageGO.transform.GetChild(6).gameObject.SetActive(false);


                }



            try
                {
                    StartCoroutine(getdatainitformsever(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.gameinfoPollingAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + LoginInfo.Instance().mylogindata.room_id + "&unionuid=" + LoginInfo.Instance().mylogindata.token));
                    StartCoroutine(LoginInfo.Instance().wwwinstance.sendcoutdown(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.counwtAPI));
                    if (isfrist == false)
                    {
                      
                       
                            StartCoroutine(cheakwillfixed(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI));

                        
                    }

                    StartCoroutine(GetAnnouncementInfo(LoginInfo.Instance().mylogindata.URL + "notice?" + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame));


                if (updateusercoutnowcount < updateusercoutlimitcount )
                {
                    updateusercoutnowcount++;

                }
                else
                {
                    StartCoroutine(rewww(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.usercoininhistoryAPI + "plate=1" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + LoginInfo.Instance().mylogindata.room_id, true));
                    StartCoroutine(rewww(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.usercoininhistoryAPI + "plate=2" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + LoginInfo.Instance().mylogindata.room_id, false));
                    updateusercoutnowcount = 0;
                    Debug.Log("已经更新用户下注数据");
                }




            }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                yield return new WaitForSeconds(2f);
            if (bgr)
            {
                gridScrool[0].verticalNormalizedPosition = 0;
                gridScrool[1].verticalNormalizedPosition = 0;
                bgr = false;
            }
            }
      
       
    }

    /// <summary>
    /// 获取服务器数据并传入用户记录面板
    /// </summary>
    /// <param name="url"></param>
    /// <param name="leftorright"></param>
    /// <returns></returns>
    IEnumerator rewww(string url, bool leftorright)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.error == null)
        {

         useropenGO.GetComponent<outtextstlye>().addtexttoui(www.downloadHandler.text, leftorright);
        }
        else
        {   //以防此次更新出错
            StartCoroutine(rewww(url, leftorright));
        }

    }



    /// <summary>
    /// 获取用户数据并作为心跳测试的连接
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator getdatainitformsever(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.error == null)
        {
            JsonData jd= JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                gotohallint = 0;
                Double tdou =Convert.ToDouble(jd["Userinfo"]["quick_credit"].ToString());
                LoginInfo.Instance().mylogindata.ALLScroce = tdou.ToString();

                if (isFirstRefreashRate)
                {
                    for (int i = 0; i < rateList.Count; i++)
                    {
                        rateList[i].text = "x" + jd["Oddlist"][i]["rate"].ToString();
                        if (i == 8)
                        {
                            isFirstRefreashRate = false;
                        }
                    }
                    
                }

                if (isopenanmimation)
                {

                //目前的数值与服务器数值不等时替换
                if (tdou != allScroceForSever)
                {
                    
                    getpoint(tdou);
                    //LoginInfo.Instance().mylogindata.ALLScroce = tdou.ToString();
                }
                }

                //每次轮询都更新下注的总情况
                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    uiallintext[i].text = jd["Oddlist"][i]["dnum"].ToString();
                }


                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    //限制只在第一次进行初始化按钮的其他信息 第二次即超过9则不再初始化
                    if (buttonIDcode.Count > 9&&buttonrate.Count>9)
                    {
                        break;   
                    }
                    else
                    {
                        buttonIDcode.Add(Convert.ToInt32( jd["Oddlist"][i]["id"].ToString()));
                        buttonrate.Add(jd["Oddlist"][i]["rate"].ToString());
                    }

                }
                //退出房间后将重新获取当前用户的当局下注数据

                if (fristuserin==true)
                {
                    rushin++;
                    for (int i = 0; i < jd["Oddlist"].Count; i++)
                    {
                       userscorein[i].text= jd["Oddlist"][i]["user_dnum"].ToString();
                    }
                    Debug.Log("更新当局的开奖数据");
                    if (rushin >= 2)
                    {
                     fristuserin = false;

                    }
                }
                
                
             

                //退出重进后需要一个
                if (openinnexttime == true)
                {
                    for (int i = 0; i < jd["Oddlist"].Count; i++)
                    {
                        userscorein[i].text = jd["Oddlist"][i]["user_dnum"].ToString();
                    }
                    Debug.Log("对用户当前值进行检测");
                    openinnexttime = false;
                }


                if (jd["BetTotal"].ToString() != "")
                {
                 WinScroceInText.text ="总压分："+ jd["BetTotal"].ToString();

                }

                //更新目前房间的总人数
                //string temptext = jd["UsersCount"].ToString();

                //if (!titlettextlist[5].text.Equals(temptext))
                //{
                //    titlettextlist[5].text = temptext;
                //}

            }
            else
            {
                messg.Show("异常", jd["msg"].ToString(), ButtonStyle.Confirm,
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
                }, 0f);

               
            }
        }
        else
        {
            //getdatainitformsever(url);
            //断线连接
            gotohallint++;
            if (gotohallint > 3)
            {
                gotohallint = 0;
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
           /* UnityWebRequest www_ = UnityWebRequest.Get("");
            yield return www.Send();
            if (www.error == null)
            {
                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")               
                {
                    jd["lua"] = "print";
                }
            }*/

        }
    }


    // Update is called once per frame
 





    /// <summary>
    /// 状态监听(考虑使用消息状态执行一个监听 观察者模式）
    /// </summary>
    private void FixedUpdate()
    {
        if (agr /*< pokerlist.transform.childCount*/)
        {
            //if (bgr && pokerlist.transform.childCount>= 47)
            //{
            //    gridScrool[0].verticalNormalizedPosition = 0;
            //    gridScrool[1].verticalNormalizedPosition = 0;
            //    bgr = false;  
            //}
            gridScrool[0].verticalNormalizedPosition = 0;
            gridScrool[1].verticalNormalizedPosition = 0;
            agr =/* pokerlist.transform.childCount*/ false;
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    startclosepoket(0);

        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    NumSpriteControl.Instances.StartImage();
        //    Debug.Log("按下了开始按钮");
        //}



        ///用于监测脚本状态是否有变化
        if (NowState == ChangeState)//检查状态变化
        {

        }
        else
        {
            NowState = ChangeState;
            changeState(NowState);
        }

        //用于监测轮询服务器后存在有效消息队列中的信息
        //每次队列中只能存在一个有效消息
        if (LoginInfo.Instance().wwwinstance.listformwwwinfo.Count > 0)
        {
            
            if (LoginInfo.Instance().wwwinstance.listformwwwinfo.Count == 1)
            {
                if (swithonislistchange)
                {
                    //当只有一个状态时只切换到这个状态
                    getinfoandsetstate(LoginInfo.Instance().wwwinstance.listformwwwinfo.Peek());
                    //Debug.LogError("当前弹出的状态" + LoginInfo.Instance().wwwinstance.listformwwwinfo.Peek().textinfo);
                    swithonislistchange = false;
                }
                else
                {
                    //Debug.Log("没有需要更新的状态");
                }

            }
            //若有效消息超过2则弹出前一个消息 只保留后一个消息
            else if(LoginInfo.Instance().wwwinstance.listformwwwinfo.Count >1)
            {
               wwwinfo temp=  LoginInfo.Instance().wwwinstance.listformwwwinfo.Dequeue();
                //getinfoandsetstate(temp);
                //Debug.LogError("弹出的信息" + temp.textinfo);

                //LoginInfo.Instance().wwwinstance.listformwwwinfo.Peek();
              
                
                swithonislistchange = true;
            }
        }

    }


    /// <summary>
    /// 监听消息队列是否存在消息并执行相应的方法
    /// </summary>
    /// <param name="info">自定义的数据类型 内部存储了对应的状态方法与数据</param>
    void getinfoandsetstate(wwwinfo info)
    {
        ///状态1：下注中
        ///状态2：封盘
        ///状态3：等待开奖
        ///状态4：开奖

        ///新游戏状态0：下注中
        ///新游戏状态1：封盘
        ///新游戏状态2：开奖




        switch (info.statetype)
        {
            case 0:
                getdatatojson(info.textinfo);
                changestate(playstate.betin);
                break;
            case 1:
                getdatatojson(info.textinfo);
                changestate(playstate.betover);
                break;
            //case 3:
            //    getdatatojson(info.textinfo);
            //    changestate(playstate.betopen);
            //    break;
            case 2:
                getdatatojson(info.textinfo);
                changestate(playstate.betclearing);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 解析所有返回的数据
    /// </summary>
    /// <param name="temp">来自服务器的JSON字符串</param>
    void getdatatojson(string temp)
    {

        Debug.Log(temp);
        JsonData jd=JsonMapper.ToObject(temp);
        if (jd["code"].ToString() == "200")
        {
            nowdata.is_open = (int)jd["info"]["is_open"];
            //先判断是否从中断进入
            if (isopendatafromsever == false)
            {
                if (nowdata.periods != null && !nowdata.periods.Equals(jd["info"]["dates"].ToString()))
                {
                    //期数是否相等 
                    isopennextdata = true; //相等则 true 为已跳期则进入不正常开奖 否则进入正常开奖
                }
            }
            nowdata.periods = jd["info"]["dates"].ToString();
            //是否在下注期间且倒计时是否已经为零
            if ((nowdata.is_open == 0||nowdata.is_open==1) && nowdata.couwttime != 0)
            {
                //对比来自服务器的数据是否与现在存储的数值相等
                if (nowdata.couwttime != (int)jd["info"]["WinnerCountdown"])
                {
                    isupdatetime = true;
                    Debug.Log("更改数据");
                }
            }
            nowdata.couwttime = (int)jd["info"]["WinnerCountdown"];
            nowdata.last_periods = jd["info"]["last_dates"].ToString();
            nowdata.last_winnumber = jd["info"]["last_winnumber"].ToString();
            nowdata.TotalCount = jd["info"]["TotalCount"].ToString();
            nowdata.RemainData = jd["info"]["RemainData"].ToString();
            //在跳期的状态确定后 对下注之后两个阶段的处理
            if (isopennextdata == true)
            {
                if (nowdata.is_open == 1)
                {
                    //检测输赢列表需要
                    cheak();
                }
                for (int i = 0; i < userscorein.Count; i++)
                {

                    Debug.LogError("跳期清理用户下注");
                    userscorein[i].text = "0";

                    openinnexttime = true;
                }
                if (nowdata.is_open != 2)
                {
                    isopennextdata = false;
                }


            }

            //nowdata.season = jd["info"]["season"].ToString();
            //nowdata.dates = jd["info"]["dates"].ToString();

            //只有在开奖的阶段才去接收这个返回值的数据
            if (nowdata.is_open==2)
            {
                nowdata.wintype1 =changeTYPEtoint( jd["info"]["win_text1"].ToString());
                nowdata.wintype2 = changeTYPEtoint(jd["info"]["win_text2"].ToString());
                
                nowdata.win_number = jd["info"]["win_number"].ToString();
                Debug.Log(temp + "is_open = 2");
                nowdata.EWinTime = (int)jd["info"]["EWinTime"];

                //nowdata.wincode = Convert.ToInt16( jd["info"]["winnings"].ToString())-1;
            }
            else
            {
                //nowdata.wincode =0;
                nowdata.wintype1 = poketstate.isnull;
                nowdata.wintype2 = poketstate.isnull;
                nowdata.win_number = "";
                nowdata.EWinTime = 0;
            }
            //第一次进入时需要显示上期的开奖号码
            if (isfristopennumber)
            {
                if (nowdata.win_number != "")
                {
                    NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.win_number));
                }
                else
                {
                NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.last_winnumber));

                }
                isfristopennumber = false;
            }
            //titlettextlist[3].text = nowdata.season;
            //titlettextlist[4].text = nowdata.dates;
            titlettextlist[1].text = nowdata.last_periods;//上局期数
            titlettextlist[2].text = nowdata.TotalCount;//今日总局数
            titlettextlist[3].text = nowdata.RemainData;//当前期数
            titlettextlist[4].text = nowdata.periods;//下局期数





        }
    }


    



    /// <summary>
    /// 状态切换
    /// </summary>
    /// <param name="state"></param>
    /// 实际用到的只有 准备 下注 锁定 等待 结算
    void changeState(playstate state) //考虑加入重复状态中去调用
    {
        switch (state)
        {
            case playstate.betnull:
                Debug.Log("无状态等待上线");
                //用于不开奖时的显示界面
                break;
            case playstate.betready:
                betready();
                Debug.Log("准备状态进行中");
                
                break;
            case playstate.betin:
                betin();
                Debug.Log("下注状态进行中");
                //下注进行中
                break;
            case playstate.betover:
                Debug.Log("锁定状态进行中");
                betover();
                //下注结算
                break;
            case playstate.betopen:
                Debug.Log("等待开奖状态进行中");
                betwaitopen();
                //开奖状态
                break;
            case playstate.betclearing:
                Debug.Log("结算状态进行中");
                betcleaning();
                //结算暂停
                break;
            case  playstate.bettime:
                Debug.Log("休息状态进行中");
                //休息暂停
                bettime();
                break;
                
        }
    }
    /// <summary>
    /// 准备状态
    /// </summary>
    void betready()
    {
        //StopAllCoroutines();
        //第一次进入游戏进行特别的初始化
        if (isfrist)
        {
            startclosepoket(0);
            isfrist = false;
            StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 1));
        }
        else
        {
            startclosepoket(countsize);
        }
        thistimecoutText.text = "本局总得分：0";
        statetext.text = "准备中";
        cleanallinanduserin();
        cleanhistorydata();
        
        //StartCoroutine(waitsecnd(playstate.betin));
    }







    /// <summary>
    /// 下注状态
    /// </summary>
    void betin()
    {
        if (!isChangeXiaZhu)
        {
            Audiomanger._instenc.ChangeBGMusic();
            //isChangeKaiJiang = !isChangeKaiJiang;
            isChangeXiaZhu = !isChangeXiaZhu;
            if (isChangeKaiJiang == true)
            {
                isChangeKaiJiang = false;
            }
        }
        if (counwtIDE != null)
        {
         StopCoroutine(counwtIDE);

        }
        thistimecoutText.text = "本局总得分：0";
        statetext.text = "请下注";
        statetext.color = Color.red;
        if (fristuserin==false&&isbetincheak==false)
        {

            Debug.Log("非当局开奖直接清空");
            cleanallinanduserin();
            cleanhistorydata();
        }
        else
        {
            Debug.Log("当局开奖不清空");
            isbetincheak = false;
        }

        //是否为第一次加载判断
        if (fristopen == true)
        {
            //#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
            
            
            //uniwebtoopen.OpenButtonClicked();
            //#endif
            fristopen = false;
            //#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
            //uniwebtoopen.hidewindos();
            //uniwebtoopen.reload();
            //#endif
        }
        else
        {
            

        }

        if (istimeover)
        {
            //#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
            //uniwebtoopen.reload();
            //#endif
            //istimeover = false;
        }

        isopenanmimation = true;
        //else
        //{
        //    uniwebtoopen.shownomrolview();
        //}
        if (nowdata.couwttime > 0)
        {
          waittimeui = nowdata.couwttime+1;
        }
        counwtIDE = betincountdown();
        StartCoroutine(counwtIDE);
    }


    //修改当失去游戏焦点 暂停游戏时的方法
    //重新获取服务器游戏列表并添加入池子
    void changefromdataislivergame()
    {

        //startclosepoket(0);
        StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 0));
            Debug.Log("未正常开奖");
         
        
    }

    /// <summary>
    /// 切换游戏错误累计
    /// </summary>
    void erroroutgame()
    {
        erroropendatacout++;
        if (erroropendatacout > 4)
        {
            erroropendatacout = 0;
            messg.Show("异常", "您由于在游戏期间离开次数过多，请退出房间后再进入", ButtonStyle.Confirm,
            delegate (MessageBoxResult call)
            {
                switch (call)
                {

                    case MessageBoxResult.Cancel:
                        tohall();
                        break;
                    case MessageBoxResult.Confirm:
                        tohall();
                        break;
                    case MessageBoxResult.Timeout:
                        tohall();
                        break;
                
                }
            }, 0f);
        }
    }

  
    /// <summary>
    /// 下注结束状态
    /// </summary>
    void betover()
    {
        //if (counwtIDE != null)
        //{
        // StopCoroutine(counwtIDE);
        //}

        if (fristopen)
        {
            if (nowdata.couwttime > 0)
            {
                waittimeui = nowdata.couwttime +1;
            }
            counwtIDE = betincountdown();
            StartCoroutine(counwtIDE);
            fristopen = false;
        }
        thistimecoutText.text = "本局总得分：0";
        NumSpriteControl.Instances.StartImage();


        isopendatafromsever = true;
        isopenanmimation = false;
        statetext.text = "已封盘";
        statetext.color = Color.green;
        if (!isChangeKaiJiang)
        {
            Audiomanger._instenc.ChangeBGMusic_KaiJiang();
            isChangeKaiJiang = !isChangeKaiJiang;
            if (isChangeXiaZhu == true)
            {
                isChangeXiaZhu = false;
            }
            //isChangeXiaZhu = !isChangeXiaZhu;
        }
        //StartCoroutine(waitsecnd(playstate.betopen));

    }
    /// <summary>
    /// 等待开奖状态
    /// </summary>
    void betwaitopen()
    {

        isbetincheak = false;
        if (counwtIDE != null)
        {
          StopCoroutine(counwtIDE);
        }

        isopendatafromsever = true;
        if (isopennextdata == true)
        {

            //跳过之后有可能协程没跑完需要提前清理 
            //for (int i = 0; i < uiallintext.Count; i++)
            //{

            //    userscorein[i].text = "0";//用户下注清零
            //    Debug.Log("跳期之后清除所有的下注数据");

            //}
            isopennextdata = false;
        }

        if (fristopen)
        {
//#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8


//            uniwebtoopen.OpenButtonClicked();
//#endif
//            fristopen = false;
//#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
//            uniwebtoopen.hidewindos();
//            uniwebtoopen.reload();
//#endif
        }
        else
        {
            //uniwebtoopen.reload();
        }
        //severpokethuase

        statetext.text = "当前状态：等待开奖中";

    }

    /// <summary>
    /// 结算状态 
    /// </summary>
    void betcleaning()
    {
        isbetincheak = false;
        if (counwtIDE != null)
        {
            StopCoroutine(counwtIDE);
        }
        statetext.text = "开奖中";


        if (nowdata.EWinTime > 0)
        {
            waittimeui = nowdata.EWinTime +1;
        }
        counwtIDE = betincountdown();
        StartCoroutine(counwtIDE);



        if (isopennextdata == true)
        {
            //不修正
            
        }
        else
        {
            isopendatafromsever = true;

        }

       

        //检查数据是否已存在
        try
        {
            isopenanmimation = false;
            if (/*nowdata.wintype == poketstate.isnull||*/isopendatafromsever == false)
            {

                StartCoroutine(opentolist(1));

            }
            else
            {
                StartCoroutine(opentolist(0));
            }
        }
        catch (Exception ex)
        {
            Debug.Log("结算错误：" + ex.Message);
        }

        //StartCoroutine(waitsecnd(playstate.bettime));
    }
    /// <summary>
    /// 休息状态
    /// </summary>
    void bettime()
    {
        
        statetext.text = "当前状态：休息";
    }


    /// <summary>
    /// 公告信息更新
    /// </summary>
    /// <param name="URL"></param>
    /// <returns></returns>
    IEnumerator GetAnnouncementInfo(string URL)
    {
        UnityWebRequest temp = UnityWebRequest.Get(URL);
        yield return temp.Send();
        var jd= getdataforjson(temp.downloadHandler.text);
        if (jd["code"].ToString() == "200")
        {
            ///将读取到的公告信息进行一个循环并进行调整
            
            if (gonggaocount  >= jd["List"].Count)
            {
                gonggaocount = 0;
            }
            if (gonggao.text != "" && !gonggao.text.Equals(jd["List"][gonggaocount]["site"].ToString()))
            {
                gonggao.text = jd["List"][gonggaocount]["site"].ToString();
                gonggao.rectTransform.localPosition = (vecFir + new Vector3(gonggao.preferredWidth / 2f, 0, 0));                
                gonggao.transform.localScale = Vector3.one;
            }
            else
            {
                Debug.Log("公告不需要更新");
            }
        }
        else
        {
            gonggao.text = "暂无公告";
            //gonggao.rectTransform.localPosition = (vecFir + new Vector3(gonggao.preferredWidth / 2f, 0, 0));
            //gonggao.transform.localScale = Vector3.one;
        }

    }
    /// <summary>
    /// 获取JSON数据
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    JsonData getdataforjson(string text)
    {
        JsonData temp;
        temp= JsonMapper.ToObject(text);
        return temp;
    }



    /// <summary>
    /// 将牌添加到池子中
    /// </summary>
    /// <returns></returns>
    IEnumerator opentolist(int toggle)
    {

        //yield return new WaitForSeconds(1f);
        if (toggle == 1)
        {
            //startclosepoket(0);
            yield return StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 0));
            Debug.LogError("错误的进入了开奖");
            NumSpriteControl.Instances.StopImage(changestringtointlist(nowdata.win_number));

            isopendatafromsever = true;
        }
        else if(toggle==0)
        {
            Debug.Log("正确的进入开奖状态");
            //yield return StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 2));
            //自定义开启全屏播放
            if (isfullopenwindows)
            {
              yield return StartCoroutine(change());
            }

            yield return new WaitForSeconds(3f);
            yield return starsetwinnumber(changestringtointlist(nowdata.win_number));
            NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.win_number));


            yield return new WaitForSeconds(3f);
            //闪烁的动画效果处理
            if ((int)nowdata.wintype1 >= 4)
            {
                StartCoroutine(buttomwinanimation((int)nowdata.wintype1%4));
                StartCoroutine(buttomwinanimation(4));
                
            }
           
            else
            {
                StartCoroutine(buttomwinanimation((int)nowdata.wintype1));
            }
            StartCoroutine(buttomwinanimation((int)(nowdata.wintype2)%4+5));


            //开奖正确添加
            isopendatafromsever = true;
            yield return addpoket();
            showpoketcount(poketcount);
            showpoketcountright(poketcount2);
            //yield return new WaitForSeconds(3f);
            StartCoroutine(getwindataandopenpanel(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.wingetforinterfroAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&periods=" + nowdata.periods));
           
        }

        updatenowcount = 4;
        updateusercoutnowcount = 101;
        isopennextdata = false;
        yield return new WaitForSeconds(1f);
        cleanallinanduserin();

        //if (titlettextlist[4].text == "66")
        //{
        //    uniwebtoopen.hidewindos();
        //    timeover.SetActive(true);
        //    istimeover = true;
        //}
        //isopendatafromsever = true;
      
    }
    IEnumerator change()
    {
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
        //uniwebtoopen.reload();
        //uniwebtoopen.changeviewonopengame();
        #endif
        yield return new WaitForSeconds(5f);
        //uniwebtoopen.reload();
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
        //uniwebtoopen.changeonopenfnish();
        #endif
    }


   //用户赢取的面板
    IEnumerator getwindataandopenpanel(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
        Debug.Log(www.downloadHandler.text);
        if (www.error==null)
        {
            if(!jd["periods"].ToString().Equals(nowdata.periods))
            {
                yield break;
            }

            if (jd["code"].ToString() == "200")
            {
                if (Convert.ToDouble(jd["WinTotal1"].ToString()) > 0f)
                {
                winpanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "恭喜你本局赢取了：" + jd["WinTotal1"].ToString() + "分";
                winpanel.SetActive(true);
                //StartCoroutine(colsewindows(winpanel));
                }

                if (Convert.ToDouble(jd["WinTotal2"].ToString()) > 0f)
                {
                winpanel2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "恭喜你本局赢取了：" + jd["WinTotal2"].ToString() + "分";
                winpanel2.SetActive(true);
                //StartCoroutine(colsewindows(winpanel2));
                }

                if(Convert.ToDouble(jd["WinTotal"].ToString()) > 0f)
                {
                    thistimecoutText.text = "本局总得分："+ jd["WinTotal"].ToString();
                }
                else
                {
                    thistimecoutText.text = "本局总得分：0";
                }


            }
            else 
            {
                
            }

        }
        else
        {
            StartCoroutine(getwindataandopenpanel(url));
        }

        yield return new WaitForSeconds(1f);
         isopenanmimation = true;

    }

    IEnumerator colsewindows(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        if (go != null)
        {
            go.SetActive(false);
        }
    }


    //大厅轮询在线
    IEnumerator gamealive(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();

        JsonData jd = getdataforjson(www.downloadHandler.text);
        if (jd["code"].ToString() == "200")
        {

        }
        else
        {
            tohall();

        }
    }

    /// <summary>
    /// 调用检测是否有跳期的数据
    /// </summary>
    void cheakpoketpoolisture()
    {
        //startclosepoket(0);
        StartCoroutine(initpoketlist(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.winlistAPI, 0));
    }



    /// <summary>
    /// 添加牌执行方法
    /// </summary>
    /// <returns></returns>
    IEnumerator addpoket()
    {
        if (countsize == severcount)
        {
            startclosepoket(0);
            for (int i = 0; i < poketcount.Length; i++)
            {

                poketcount[i] = 0;
                poketcount2[i] = 0;
            }
        }
        //int a = UnityEngine.Random.Range(0, 13);
        //int b = UnityEngine.Random.Range(0, 5);
        //Debug.Log("-----随机值" + a);
        //Debug.Log("-----花色" + b);
        //yield return new WaitForSeconds(1f);
        poketActivetolist(nowdata.wintype1, countsize,pokerlist/*, nowdata.wincode*/);
        poketActivetolist(nowdata.wintype2, countsize, pokerlist2);
  
      


        countsize++;
        if ((int)nowdata.wintype1 >= 4)
        {
          poketcount[(int)nowdata.wintype1 % 4]++;
          poketcount[4]++;
        }
        else
        {
         poketcount[(int)nowdata.wintype1]++;
        }
        if ((int)nowdata.wintype2 >= 5)
        {
          
        }
        else
        {
            Debug.Log((int)nowdata.wintype2);
            Debug.Log(poketcount2.Length);
            try
            {
                poketcount2[(int)nowdata.wintype2]++;
            }
            catch (Exception)
            {

                throw;
            }
        
        }



        yield return null;
        //yield return waitsecnd(playstate.betclearing);

    }



    /// <summary>
    /// 倒计时协程
    /// </summary>
    /// <returns></returns>
    IEnumerator betincountdown()
    {

        while (true)
        {

            if (isupdatetime == true)
            {
                waittimeui = nowdata.couwttime-1;
                Debug.Log("数据为" + nowdata.couwttime);
                changefromdataislivergame();
                isupdatetime = false;
            }

            yield return new WaitForSeconds(1f);
            coundowntext.text =/* "当前状态：倒计时" +*/ (waittimeui--).ToString();
            if (waittimeui <= 0)
            {
                yield break;
            }

            //rangerom();
        }
        
      
    }



    /// <summary>
    /// 切换开关
    /// </summary>
    /// <param name="state">切换的下一个状态</param>
    /// <returns></returns>
    IEnumerator waitsecnd(playstate state)
    {
        yield return new WaitForSeconds(1f);
        ChangeState = state;
    }
   /// <summary>
   /// 场景转换
   /// </summary>
   /// <param name="state"></param>
    void changestate(playstate state)
    {
        ChangeState = state;
    }




    /// <summary>
    /// 根据数量关闭相应的扑克牌预设体
    /// </summary>
    /// <param name="countpoket"> </param>
    void startclosepoket(int countpoket)
    {
        for (int i = pokerlist.transform.childCount - 1; i > -1; i--)
        {
            if (i == countpoket - 1)
            {
                break;
            }
            else
            {
                pokerlist.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = pokerlist2.transform.childCount - 1; i > -1; i--)
        {
            if (i == countpoket - 1)
            {
                break;
            }
            else
            {
                pokerlist2.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < aGrid; i++)
        {
            gridNew[i].SetActive(false);
            gridNew[i].transform.SetParent(gridNewPar);
        }

        Debug.Log(aGrid);

        aGrid = countpoket;
        countsize = countpoket;
    }



    /// <summary>
    /// 添加单个牌至池子中
    /// </summary>
    /// <param name="poketstate">牌的花色</param>
    /// <param name="size">池子中预设体的位置</param>
    /// <param name="code">对应的牌的大小</param>
    /// 因实际需要去掉了数字的传入 并其他相关的也一并注释 需要重新修改的地方的话需要再次修改注释
    /// 需要重写此方法
    /// 
    void poketActivetolist(poketstate poketstate, int size/*, int code*/,GameObject poketlistGO)
    {
        //Debug.LogWarning(poketlistGO.transform.childCount);
        //Debug.LogWarning("size" + size);
        if (size >=48 /*&& size > poketlistGO.transform.childCount - 1*/)
        {
            //int a = (size - 48) / 6 ;
            //if (((size - 48) % 6) >= 0)
            //{
            //    a += 1;
            //}
            //for (int i = 0; i < a * 6; i++)
            //{
            gridNew[aGrid].gameObject.SetActive(true);
            //poketlistGO.AddChild();
            gridNew[aGrid].transform.SetParent(poketlistGO.transform);
            //if (agr == aGrid )
            //{
            //   /* Debug.Log(*//*poketlistGO.transform.parent.parent */gridScrool[0]/*.GetComponent<ScrollRect>()*/.verticalNormalizedPosition = 0;
            //    //if (gridScrool[0].GetComponent<>)
            //    //{

            //    //}
            //    //for (int i = 0; i < gridScrool.Length; i++)
            //    //{
            //    //    gridScrool[0].verticalNormalizedPosition = 0;
            //    //    Debug.LogWarning(gridScrool[i].verticalNormalizedPosition);
            //    //}
            //    //poketlistGO.transform.parent.parent
            //    //Debug.LogWarning(poketlistGO.transform.parent.parent);

            agr = true;
            //    agr += 10;


            //}
            //if (bgr == aGrid)
            //{
            //    gridScrool[1]/*poketlistGO.transform.parent.parent.GetComponent<ScrollRect>()*/.verticalNormalizedPosition = 0;
            //    if (gridScrool[1].verticalNormalizedPosition == 1)
            //    {
            //        gridScrool[1].verticalNormalizedPosition = 0;
            //    }
            //    bgr += 10;
            //}

            aGrid++;
        //}


    }
        //打开对应花色
        GameObject temphuse =  poketGOtoinit.transform.GetChild(0).GetChild((int)poketstate).gameObject;
        temphuse.SetActive(true);
        GameObject tempsize;
        //通过对应花色决定牌的颜色为红或者为黑
        if ((int)poketstate==0|| (int)poketstate == 2)
        {
            tempsize= poketGOtoinit.transform.GetChild(1).GetChild(0).gameObject;
        }

        //else if ((int)poketstate==4)
        //{
        //    tempsize =null;
        //    poketGOtoinit.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        //}

        else
        {
            tempsize = poketGOtoinit.transform.GetChild(1).GetChild(1).gameObject;
        }
        if (tempsize != null)
        {

          tempsize.SetActive(true);

        }
        ////小于3属于花色 大于3属于JOKEY
        //if ((int)poketstate <4)
        //{
        //  pokerlist.transform.GetChild(size).GetComponent<Image>().sprite = poketGOtoinit.transform.GetChild((int)poketstate).GetChild(code).GetComponent<Image>().sprite;
        //  pokerlist.transform.GetChild(size).gameObject.SetActive(true);
        //}
        //else
        //{
        //    pokerlist.transform.GetChild(size).GetComponent<Image>().sprite = poketGOtoinit.transform.GetChild((int)poketstate).GetComponent<Image>().sprite;
        //    pokerlist.transform.GetChild(size).gameObject.SetActive(true);
        //}
        //poketGOtoinit.transform.GetChild((int)poketstate).gameObject.SetActive(false);

        //给予花色
        poketlistGO.transform.GetChild(size).GetChild(0).GetComponent<Image>().sprite = temphuse.GetComponent<Image>().sprite;
        
        //此代码根据客户需求改动 

        //给予数字
        //if (tempsize != null)
        //{
        //  pokerlist.transform.GetChild(size).GetChild(1).GetComponent<Image>().sprite = tempsize.transform.GetChild(code).GetComponent<Image>().sprite;
        //}
        //else
        //{
        //    pokerlist.transform.GetChild(size).GetChild(1).GetComponent<Image>().sprite = poketGOtoinit.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite;
        //}
        poketlistGO.transform.GetChild(size).gameObject.SetActive(true);
        closepoketGO();
        

    }
    /// <summary>
    /// 关闭所有花色预设体
    /// </summary>
    void closepoketGO()
    {
       Transform temp=  poketGOtoinit.transform.GetChild(0);
        temp.GetChild(0).gameObject.SetActive(false);
        temp.GetChild(1).gameObject.SetActive(false);
        temp.GetChild(2).gameObject.SetActive(false);
        temp.GetChild(3).gameObject.SetActive(false);
        temp.GetChild(4).gameObject.SetActive(false);

        temp = poketGOtoinit.transform.GetChild(1);

        temp.GetChild(0).gameObject.SetActive(false);
        temp.GetChild(1).gameObject.SetActive(false);
        temp.GetChild(2).gameObject.SetActive(false);


    }

    void cleanallin()
    {

    }

    /// <summary>
    /// 随机下注添加
    /// </summary>
    void rangerom()
    {
       tempc = 0;
       int tempa= UnityEngine.Random.Range(0,4);
       int tempb = UnityEngine.Random.Range(0, 5);
        tempc +=listrange[tempa];
        if (uiallintext[tempb].text.Equals(""))
        {
            uiallintext[tempb].text = tempc.ToString();
        }
        else
        {
         uiallintext[tempb].text =(Convert.ToInt16(uiallintext[tempb].text)+ tempc).ToString();
        }
    }
    /// <summary>
    /// 修改总下注
    /// </summary>
    /// <param name="value">下注单次筹码值</param>
    /// <param name="round">对应的UI总值</param>
    void chagerom(int value,int round)
    {
        if (uiallintext[round].text.Equals(""))
        {
            uiallintext[round].text = value.ToString();
        }
        else
        {
            uiallintext[round].text = (Convert.ToInt16(uiallintext[round].text) + value).ToString();
        }
    }

    
    /// <summary>
    /// 显示牌色次数方法
    /// </summary>
    /// <param name="list"></param>
    void showpoketcount(int[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            uesrinscoercout[i].text = list[i].ToString();
        }
      
    }

    void showpoketcountright(int[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            userinscoercoutright[i].text = list[i].ToString();
        }
    }



    GameObject goLeft; //为下面的方法，方便销毁
    GameObject goRight;
    /// <summary>
    /// 生成错误并传递信息
    /// </summary>
    /// <param name="value">对应的错误信息</param>
    /// 
    void senderror(string value)
    {
        if (!LoginInfo.Instance().mylogindata.isOpenError)
        {
            return;
        }
        if (goLeft != null)
        {
            Destroy(goLeft);
        }
        //此重载可先定义父级后生成
        GameObject go=  GameObject.Instantiate(errorGo, error_left.transform,true);
        go.transform.localPosition = Vector3.zero;
        goLeft = go;
        goLeft.transform.GetChild(0).GetComponent<Text>().text = value;

        goLeft.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goLeft.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);

        //go.GetComponent<changetext>().msgset(value);
    }


     void senderror_right(string value)
    {
        if (!LoginInfo.Instance().mylogindata.isOpenError)
        {
            return;
        }
        if (goRight != null)
        {
            Destroy(goRight);
        }
        GameObject go = GameObject.Instantiate(errorGo, error_right.transform,true);
        go.transform.localPosition = Vector3.zero;
        goRight = go;
        goRight.transform.GetChild(0).GetComponent<Text>().text = value;

        goRight.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goRight.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);

        //go.GetComponent<changetext>().msgset(value);
    }

    /// <summary>
    /// 添加按钮监听
    /// </summary>
    void addlistenrt()
    {
            //循环添加可使用迭代器 但循环展开则更快 
            buttonlist[0].onClick.AddListener(SPclick);
            buttonlist[1].onClick.AddListener(HTclick);
            buttonlist[2].onClick.AddListener(CBclick);
            buttonlist[3].onClick.AddListener(DMclick);
            buttonlist[4].onClick.AddListener(JKclick);
            buttonlist[5].onClick.AddListener(SPRclick);
            buttonlist[6].onClick.AddListener(HTRclick);
            buttonlist[7].onClick.AddListener(CBRclick);
            buttonlist[8].onClick.AddListener(DMRclick);
            cnealGO.onClick.AddListener(delegate ()
            {
                canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods,1);
            });
            cnealGO.onClick.AddListener(Audiomanger._instenc.clickvoice);
            cnealGO2.onClick.AddListener(delegate ()
            {
                canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods, 2);
            });
            cnealGO2.onClick.AddListener(Audiomanger._instenc.clickvoice);

            quit.onClick.AddListener(tohall);
            quit.onClick.AddListener(Audiomanger._instenc.clickvoice);
    }

    /// <summary>
    /// 初始化用户UI
    /// </summary>
    void setUserUIinfo()
    {
        setallsroce(allScroceForUi);
    } 

    /// <summary>
    /// 每次完成结算 对数据进行初始化
    /// </summary>
    void cleanallinanduserin()
    {
        for (int i = 0; i < uiallintext.Count; i++)
        {
            uiallintext[i].text = "0";//总下注清零
            userscorein[i].text = "0";//用户下注清零
            //historyuserin[i] = 0;//用户历史下注清零
        }

         historyallin = 0;//用于下注总分清零
    }


    /// <summary>
    /// 设置总分
    /// </summary>
    /// <param name="value"></param>
    void setallsroce(double value)
    {
        AllScroceText.text = value.ToString();
    }
    /// <summary>
    /// 添加正数或者负数进行对总分加减
    /// </summary>
    /// <param name="value"></param>
    void changeallsroce(int value)
    {
        allScroceForUi += value;
        allScroceForUi= Mathf.Clamp((float)allScroceForUi, 0, 99999999f);
        //senderror("allScroceForUi");
        setallsroce(allScroceForUi);
    }
    /// <summary>
    /// 筹码点击
    /// </summary>
    /// <param name="value"></param>
    void returnciontologininfo(int value)
    {
        LoginInfo.Instance().mylogindata.coindown = value;
    }


    /// <summary>
    /// 按钮执行
    /// </summary>
    /// <param name="value">不同的按钮输入参数</param>
    void btnaddclick(int value,int id,string huase)
    {

        //if (allScroceForUi < 0)
        //{
        //    cheakindang();
        //}

        if (NowState != playstate.betin)
        {
            if (value <= 4)
            {
              senderror("未到下注时间");
            }
            else
            {
                senderror_right("未到下注时间");
            }
            return;
        }
        if (allScroceForUi <int.Parse(LoginInfo.Instance().mylogindata.roomcount))
        {
            changeallsroce(0);

            if (value <= 4)
            {
                senderror("余额不足或未满足最低下分要求");
            }
            else
            {
                senderror_right("余额不足或未满足最低下分要求");
            }
            return;
        }


        //if (LoginInfo.Instance().mylogindata.coindown != 0)
        //{

        //    if (allScroceForUi - LoginInfo.Instance().mylogindata.coindown > 0)
        //    {

        //        betlistadd(value, LoginInfo.Instance().mylogindata.coindown);
        //        AddCoinToText(value, LoginInfo.Instance().mylogindata.coindown);
        //        changeallsroce(-LoginInfo.Instance().mylogindata.coindown);

        //    }
        //    else
        //    {
        //        senderror("下注失败，超过目前金额");
        //        //之后小于100大于10可全额押注在此处理

        //    }

        //}
        ///------------------------分割线-------------------

        //if (LoginInfo.Instance().mylogindata.coindown > 0)
        //{

        //    if (allScroceForUi < LoginInfo.Instance().mylogindata.coindown)
        //    {
        //        senderror("余额不足");
        //    }
        //    else
        //    {
        //        if (value == 4)
        //        {
        //            int temp = Convert.ToInt32(userscorein[4].text);

        //            if (LoginInfo.Instance().mylogindata.coindown == 500)
        //            {
        //                senderror("王的下注不能使用500");
        //                return;
        //            }
        //            else if(
        //                (temp+LoginInfo.Instance().mylogindata.coindown) > 100
        //                )

        //            {
        //                senderror("王的下注总额超过了100");
        //                return;
        //            }
        //        }

        //if (userscorein[value].text.Equals(""))
        //{
        //    userscorein[value].text = LoginInfo.Instance().mylogindata.coindown.ToString();
        //}
        //else
        //{
        //    userscorein[value].text = (Convert.ToInt16(userscorein[value].text) + LoginInfo.Instance().mylogindata.coindown).ToString();

        //}
        //修改总分
        //修改
        //chagerom(LoginInfo.Instance().mylogindata.coindown, value);
        //        //每次下注的记录
        //        historyuserinlist(LoginInfo.Instance().mylogindata.coindown, value);
        //    }
        //}
        //else
        //{
        //    senderror("请先选择筹码");
        //}

        buttonmask[value].SetActive(true);

        try
        {

            
            if (allScroceForUi < 100 && LoginInfo.Instance().mylogindata.coindown == 100)
            {
                StartCoroutine(betincoin(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.betinAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&num=" + allScroceForUi + "&room_id=" + LoginInfo.Instance().mylogindata.room_id + "&drop_content=" + huase + "&id=" + id, value, allScroceForUi));
            }
            else
            {
                StartCoroutine(betincoin(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.betinAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&num=" + LoginInfo.Instance().mylogindata.coindown + "&room_id=" + LoginInfo.Instance().mylogindata.room_id + "&drop_content=" + huase + "&id=" + id, value, LoginInfo.Instance().mylogindata.coindown));
            }

            //if (allScroceForUi < 100 && LoginInfo.Instance().mylogindata.coindown == 100)
            //{
            //    StartCoroutine(betincoin(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.betinAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&num=" + allScroceForUi + "&room_id=" + LoginInfo.Instance().mylogindata.room_id + "&drop_content=" + huase + "&id=" + id, value, allScroceForUi));
            //}
            //else
            //{

            //}


        }
        catch (Exception ex)
        {
            Debug.Log("下注的错误" + ex.Message);
        }
    }


    private void ConfirmToSubmit()
    {
        if(betinlist[0]>0|| betinlist[1] > 0 || betinlist[2] > 0 || betinlist[3] > 0 || betinlist[4] > 0)
        {

            //需要先处理要发送的数据成为JSON格式 发送成功后清空本地Betlist 并更新当前的下注情况和玩家的总金额 
 
            //StartCoroutine();
        }
        else
        {
            //senderror("您未下注,请重新下注");
        }
    }

    //IEnumerator sendbetin(string url)
    //{
    //    UnityWebRequest www = UnityWebRequest.Post();
    //    yield return www.Send();
    //}


    private void CanelBetIn()
    {
         if (betinlist[0] > 0 || betinlist[1] > 0 || betinlist[2] > 0 || betinlist[3] > 0 || betinlist[4] > 0)
        {
            //清空betlist 并想办法修正Text数值成为原来的数值 最后回复玩家金额数值;
            for (int i = 0; i < betinlist.Length; i++)
            {
              SubCoinToText(i, betinlist[i]);
            }
            changetextmoney(allScroceForSever);
            BetListClean();
            
        }
        else
        {
            //senderror("未下注，无法取消");
        }
    }


    /// <summary>
    /// 对文本数组添加
    /// </summary>
    /// <param name="TextSqueacn"></param>
    /// <param name="AddValue"></param>
    private void AddCoinToText(int TextSqueacn,int AddValue)
    {
        userscorein[TextSqueacn].text = (Convert.ToInt32(userscorein[TextSqueacn].text) + AddValue).ToString();
    }

    /// <summary>
    /// 对文本值进行减少
    /// </summary>
    /// <param name="TextSqueacn"></param>
    /// <param name="AddValue"></param>
    private void SubCoinToText(int TextSqueacn, int AddValue)
    {
        userscorein[TextSqueacn].text = (Convert.ToInt32(userscorein[TextSqueacn].text) - AddValue).ToString();
    }






    /// <summary>
    /// 取消按钮
    /// </summary>
    /// <param name="user_id">玩家ID</param>
    /// <param name="ratedate">目前期数</param>
    void canelallin(string user_id,string ratedate,int plate)
    {
        string tempplate = "";
        if (NowState != playstate.betin)
        {
            if (plate == 1)
            {
             senderror("已超过了下注时间无法取消");

            }
            else{
                senderror_right("已超过了下注时间无法取消");
            }
            return;
        }
        if (plate == 1)
        {
          buttonmask[9].SetActive(true);
          tempplate = "1";
        }
        else if(plate==2)
        {
            buttonmask[10].SetActive(true);
            tempplate = "2";
        }



        try
        {
            StartCoroutine(canelbetin(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.caneldownAPI + "user_id=" + user_id + "&drop_date=" + ratedate + "&room_id=" + LoginInfo.Instance().mylogindata.room_id+ "&plate="+tempplate,plate));
        }
        catch (Exception ex)
        {
            Debug.Log("下注取消的错误信息" + ex.Message);
        }
    }







    IEnumerator canelbetin(string URL,int plate)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();

        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

        if (www.error == null)
        {
            //string temptext="";



            if (jd["code"].ToString().Equals("200"))
            {

                for (int i = 0; i < jd["DnumList"].Count; i++)
                {



                    //userscorein[i].text = jd["DnumList"][i]["user_dnum"].ToString();
                    int temp = changeTYPEtointforcanel(jd["DnumList"][i]["num"].ToString());

                    userscorein[temp].text= jd["DnumList"][i]["user_dnum"].ToString();

                }

            }
            //canelhinttext.text = "";

            //canelhinttext.text = temptext;

            //canelhint.SetActive(true);


            //if (jd["code"].ToString() == "200")
            //{
            //    //取消后修改总值
            //    Double temp = Convert.ToDouble (jd["Userinfo"]["quick_credit"].ToString());
            //    if (temp != 0)
            //    {
            //    getpoint(temp);
            //    }
            //    Debug.Log("取消成功");
            //    for (int i = 0; i < userscorein.Count; i++)
            //    {
            //     userscorein[i].text = "0";
            //    }

            //    for (int i = 0; i <jd["RoomList"].Count; i++)
            //    {
            //        if (jd["RoomList"].Count > 0)
            //        {
            //            uiallintext[i].text = jd["RoomList"][i]["dnum"].ToString();
            //        }
            //    }

            //}
            //else
            //{
            //    senderror(jd["msg"].ToString());
            //}
             


            Debug.LogError("取消执行");
        }
        else
        {
            StartCoroutine(canelbetin(URL,plate));
        }
        if (plate == 1)
        {
           buttonmask[9].SetActive(false);

        }
        else if(plate==2)
        {
            buttonmask[10].SetActive(false);
        }
    }

    




    IEnumerator betincoin(string URL,int value,Double invalue)
    {
        UnityWebRequest TEMP = UnityWebRequest.Get(URL);
        yield return TEMP.Send();
        if (TEMP.error == null)
        {
            JsonData jd =JsonMapper.ToObject(TEMP.downloadHandler.text);
            if (jd["code"].ToString()=="200")
            {
                    //ui玩家显示
                    changeallsroce(-(int)invalue);
                    Debug.Log("下注成功");
                    //玩家下注
                    if (userscorein[value].text.Equals(""))
                    {
                        userscorein[value].text = LoginInfo.Instance().mylogindata.coindown.ToString();
                    }
                    else
                    {
                        userscorein[value].text = (Convert.ToInt16(userscorein[value].text) + invalue).ToString();
                    }
                    //总下注
                    if (uiallintext[value].text.Equals(""))
                    {
                        uiallintext[value].text = LoginInfo.Instance().mylogindata.coindown.ToString();
                    }
                    else
                    {
                        uiallintext[value].text = (Convert.ToInt16(uiallintext[value].text) + invalue).ToString();
                    }
            }
            else
            {
                if (value <= 4)
                {
                 senderror(jd["msg"].ToString());

                }
                else
                {
                 senderror_right(jd["msg"].ToString());
                }
            }
            buttonmask[value].SetActive(false);
        }
        //cheakindang();
    }

    private void cheakifmoney()
    {
        if (allScroceForUi < 0)
        {
            allScroceForUi =0;
            setallsroce(allScroceForUi);
        }
    }



    //----------------------------------------按钮监听方法-------------------------------
    void SPclick()
    {
        btnaddclick(0,buttonIDcode[0],"A");
        buttonlist[0].GetComponent<AudioSource>().Play();
    }

    void HTclick()
    {
        btnaddclick(1,buttonIDcode[1], "B");
        buttonlist[1].GetComponent<AudioSource>().Play();
    }

    void CBclick()
    {
        btnaddclick(2, buttonIDcode[2], "C");
        buttonlist[2].GetComponent<AudioSource>().Play();
    }


    void DMclick()
    {
        btnaddclick(3, buttonIDcode[3], "D");
        buttonlist[3].GetComponent<AudioSource>().Play();
    }

    void JKclick()
    {
        //此方法与设置一个间接的方法一致 （只是用匿名委托来跳转方法)
        //list[3].onClick.AddListener(delegate ()
        //{
        //    clicktocion(4);
        //});

        //if (Convert.ToInt16(userscorein[4].text) >= 100)
        //{
        //    senderror("王最大下注只能为100分");
        //    return;
        //}
        btnaddclick(4, buttonIDcode[4], "E");
        buttonlist[4].GetComponent<AudioSource>().Play();
    }

    void SPRclick()
    {
        //此方法与设置一个间接的方法一致 （只是用匿名委托来跳转方法)
        //list[3].onClick.AddListener(delegate ()
        //{
        //    clicktocion(4);
        //});

        //if (Convert.ToInt16(userscorein[4].text) >= 100)
        //{
        //    senderror("王最大下注只能为100分");
        //    return;
        //}
        btnaddclick(5, buttonIDcode[5], "F");
        buttonlist[5].GetComponent<AudioSource>().Play();
    }
    void HTRclick()
    {
        //此方法与设置一个间接的方法一致 （只是用匿名委托来跳转方法)
        //list[3].onClick.AddListener(delegate ()
        //{
        //    clicktocion(4);
        //});

        //if (Convert.ToInt16(userscorein[4].text) >= 100)
        //{
        //    senderror("王最大下注只能为100分");
        //    return;
        //}
        btnaddclick(6, buttonIDcode[6], "G");
        buttonlist[6].GetComponent<AudioSource>().Play();
    }
    void CBRclick()
    {
        btnaddclick(7, buttonIDcode[7], "H");
        buttonlist[7].GetComponent<AudioSource>().Play();
    }
    void DMRclick()
    {
        btnaddclick(8, buttonIDcode[8], "I");
        buttonlist[8].GetComponent<AudioSource>().Play();
    }



    //void SPcanel()
    //{
    //    canelallin(LoginInfo.Instance().mylogindata.user_id,nowdata.periods, "A");
    //}
    //void HTcanel()
    //{
    //    canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods, "B");
    //}
    //void CBcanel()
    //{
    //    canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods, "C");
    //}
    //void DMcanel()
    //{
    //    canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods, "D");
    //}
    //void JKcanel()
    //{
    //    canelallin(LoginInfo.Instance().mylogindata.user_id, nowdata.periods, "E");
    //}





    //------------------筹码按钮反馈-------------
    //void cion10toggle(bool isbool)
    //{
    //    if (isbool)
    //    {
    //        returnciontologininfo(10);
    //    }
    //    else
    //    {
    //        returnciontologininfo(0);
    //    }
    //}

    //void cion50toggle(bool isbool)
    //{
    //    if (isbool)
    //    {
    //        returnciontologininfo(50);
    //    }
    //    else
    //    {
    //        returnciontologininfo(0);
    //    }
    //}
    //void cion100toggle(bool isbool)
    //{
    //    if (isbool)
    //    {
    //        returnciontologininfo(100);
    //    }
    //    else
    //    {
    //        returnciontologininfo(0);
    //    }
    //}
    //void cion500toggle(bool isbool)
    //{
    //    if (isbool)
    //    {
    //        returnciontologininfo(500);
    //    }
    //    else
    //    {
    //        returnciontologininfo(0);
    //    }
    //}

    //改变下注筹码
    public void changecointo100()
    {
        Audiomanger._instenc.clickvoice();
        returnciontologininfo(100);

    }

    //public void playcont(GameObject go)
    //{

    //    Audiomanger._instenc.playfromGo(go);
    //}

    public void changecointo10()
    {
        Audiomanger._instenc.clickvoice();
        returnciontologininfo(int.Parse(lessdown.text));

    }

    /// <summary>
    /// 取消所有下注
    /// </summary>
    //public void canelallin()
    //{
        
    //    if (NowState != playstate.betin)
    //    {
    //        senderror("已开始结算不可取消");
    //        return;
    //    }

    //    if (historyallin > 0)
    //    {
    //        changeallsroce(historyallin);
    //        historyallin = 0;
    //        for (int i = 0; i < uiallintext.Count; i++)
    //        {
    //            chagerom(-historyuserin[i], i);
    //        }
    //        cleanhistorydata();
    //    }
    //    else
    //    {
    //        senderror("前先下注再取消");
    //        return;
    //    }
    //}

    
    //public void changeurl()
    //{
    //    URL = inputtext.text;
    //    this.transform.GetComponent<uniweblist>().reload();
    //}


    /// <summary>
    /// 每次下注都往历史下注数值中添加值
    /// </summary>
    /// <param name="coin">下注的值</param>
    /// <param name="round">对应的下注花色位置</param>
    private void historyuserinlist(int coin,int round)
    {
        historyuserin[round] += coin;
        historyallin += coin;
    }

    void cleanhistorydata()
    {
        for (int i = 0; i < uiallintext.Count; i++)
        {
            userscorein[i].text = "0";
            //historyuserin[i] = 0;
        }
    }

    /// <summary>
    /// 退出至大厅
    /// </summary>
    public void tohall()
    {
        //StartCoroutine(gooutroom(LoginInfo.Instance().mylogindata.URL+ LoginInfo.Instance().mylogindata.roominendAPI+"user_id="+LoginInfo.Instance().mylogindata.user_id));
        StartCoroutine(loadloginscene());
    }

    IEnumerator loadloginscene()
    {

        var temp = SceneManager.LoadSceneAsync(1);

        yield return temp;
        if (temp.isDone)
        {
            Debug.Log("登录加载完毕");
        }
    }
    //返回大厅
    IEnumerator gooutroom(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.error == null)
        {
            yield return loadloginscene();
        }
        else
        {
            senderror("返回大厅错误，请重试");
        }
    }


    /// <summary>
    /// 处理来自服务器的牌堆资源
    /// </summary>
    /// <param name="URL"></param>
    /// <param name="toggle"> 1 用于初始化 2 用于检查开奖的期数跳跃 0 用于更新缺少的期数</param>
    /// <returns></returns>
    IEnumerator initpoketlist(string URL, int toggle)
    {


        lock (this.gameObject)
        {

            UnityWebRequest temp = UnityWebRequest.Get(URL);
            yield return temp.Send();
            JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);

            Debug.Log("进入游戏初始化");
            severpoketcode.Clear();

            severpokethuase.Clear();
            severpokethuase2.Clear();
            if (temp.error == null && temp.isDone == true)
            {
                if (temp.downloadHandler.text != "")
                {
                    if (jd["code"].ToString() == "200")
                    {
                        if (jd["ArrList"].Count > 0)
                        {
                            for (int i = 0; i < jd["ArrList"].Count; i++)
                            {
                                severpokethuase.Add(changeTYPEtoint(jd["ArrList"][i]["win_text1"].ToString()));
                                severpokethuase2.Add(changeTYPEtoint(jd["ArrList"][i]["win_text2"].ToString()));
                                //severpoketcode.Add(Convert.ToInt16(jd["ArrList"][i][0].ToString()) - 1);
                            }
                        }
                        //循环结束
                        //修改目前牌堆的出现次数

                    }
                }
                else
                {
                    messg.Show("异常", "无法获取以往期数，请重新进入", ButtonStyle.Confirm,
                       delegate (MessageBoxResult call)
                       {
                           switch (call)
                           {
                               case MessageBoxResult.Cancel:
                                   tohall();
                                   break;
                               case MessageBoxResult.Confirm:
                                   tohall();
                                   break;
                               case MessageBoxResult.Timeout:
                                   tohall();
                                   break;
                           }
                       }, 0f);
                }

            }
            else
            {
                StartCoroutine(initpoketlist(URL, toggle));
            }

            //全局修正的时候也需要清空
            if (toggle == 1)
            {
                //aGrid = 0;
                for (int i = 0; i < poketcount.Length; i++)
                {
                    poketcount[i] = 0;
                }
                for (int i = 0; i < poketcount2.Length; i++)
                {

                    poketcount2[i] = 0;
                }
                changecountall(severpokethuase);

                showpoketcount(poketcount);

                changecountallright(severpokethuase2);

                showpoketcountright(poketcount2);
                for (int i = 0; i < severpokethuase.Count; i++)
                {
                    poketActivetolist(severpokethuase[i], countsize,pokerlist/*, 0*/);
                    poketActivetolist(severpokethuase2[i], countsize, pokerlist2/*, 0*/);
                    countsize++;

                }

                agr = true;
                //gridScrool[0].verticalNormalizedPosition = 0;
                //gridScrool[1].verticalNormalizedPosition = 0;


            }
            else if (toggle == 0)
            {
                int tempi;
                if (severpokethuase.Count - countsize > 0)
                {
                    tempi = countsize;
                }
                else
                {
                    tempi = 0;
                    Debug.LogError("此次检查无效");
                }

                if (tempi > 0)
                {

                    Debug.Log("不直接初始化,只做列表更新 属性纠正开错");
                    for (int i = tempi; i < severpokethuase.Count; i++)
                    {
                        if (countsize == severcount)
                        {
                            startclosepoket(0);
                            for (int j = 0; j < poketcount.Length; j++)
                            {
                                poketcount[j] = 0;
                                poketcount2[j] = 0;
                            }
                        }
                        poketActivetolist(severpokethuase[i], countsize,pokerlist/*, 0*/);
                        poketActivetolist(severpokethuase2[i], countsize, pokerlist2/*, 0*/);
                        changecountall(severpokethuase[i]);
                        changecountallright(severpokethuase2[i]);

                        countsize++;

                    }
                    //if (isopennextdata)
                    //{

                        
                    //    for (int i = 0; i < uiallintext.Count; i++)
                    //    {
                    //        Debug.LogError("修复来自全局修正的清零");
                    //        userscorein[i].text = "0";//用户下注清零
                    //    }
                    //}
                    //异常修正
                    erroroutgame();
                    showpoketcount(poketcount);
                    showpoketcountright(poketcount2);
                }

                //}
                //else if (toggle == 2) //用于如果跳跃了多期之后的问题
                //{
                //    int tempi;
                //    if (severpokethuase.Count - countsize >= 2)
                //    {
                //        if (severpokethuase.Count - countsize >= 4)
                //        {
                //            messg.Show("异常", "错过多期请退出房间后再进入", ButtonStyle.Confirm,
                //            delegate (MessageBoxResult call)
                //            {
                //             switch (call)
                //             {
                //              case MessageBoxResult.Cancel:
                //                        tohall();
                //                        break;
                //              case MessageBoxResult.Confirm:
                //                        tohall();
                //                        break;
                //              case MessageBoxResult.Timeout:
                //                        tohall();
                //                        break;
                //             }
                //            }, 0f);
                //        }

                //        tempi = countsize;
                //    }
                //    else
                //    {
                //        tempi = 0;
                //        Debug.LogError("此次检查无效");
                //    }

                //    if (tempi > 0)
                //    {
                //        Debug.Log("不直接初始化,只做列表更新属于跳期开错");
                //        for (int i = tempi; i < severpokethuase.Count-1; i++)
                //        {
                //            if (countsize == severcount)
                //            {
                //                startclosepoket(0);
                //                for (int j = 0; j < poketcount.Length; j++)
                //                {
                //                    poketcount[j] = 0;
                //                }
                //            }
                //            poketActivetolist(severpokethuase[i], countsize/*, 0*/);
                //            changecountall(severpokethuase[i]);
                //            countsize++;

                //        }

                //        showpoketcount(poketcount);
                //    }
            }
        }


    }

    /// <summary>
    /// 修改透明度
    /// </summary>
    /// <param name="ButtomSequence"></param>
    /// <returns></returns>
    IEnumerator buttomwinanimation(int ButtomSequence)
    {
        
        for (int i = 0; i < 5; i++)
        {
            imagechange[ButtomSequence].GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1);
            yield return new WaitForSeconds(0.2f);
            imagechange[ButtomSequence].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator starsetwinnumber(int[] winnumber)
    {


        for (int i = 0; i < winnumber.Length; i++)
        {
           
            NumSpriteControl.Instances.StopImage(winnumber[i]);
            yield return new WaitForSeconds(0.5f);


        }

       
    }



    /// <summary>
    /// 本地投注数组添加值
    /// </summary>
    /// <param name="sequence">数组序列</param>
    /// <param name="value">数据</param>
    private void betlistadd(int sequence,int value)
    {
        if (betinlist != null)
        {
            betinlist[sequence] += value;
        }
        else
        {
            Debug.Log("添加值出错");
        }
    }

    /// <summary>
    /// 清除本地下注列表
    /// </summary>
    private void BetListClean()
    {
        for (int i = 0; i < betinlist.Length; i++)
        {
            betinlist[i] = 0;
        }
    }
    /// <summary>
    /// 刷新视频窗口
    /// </summary>
    private void refrshview()
    {
//#if UNITY_ANDROID
//        uniwebtoopen.hidewindos();
//        uniwebtoopen.reload();
//#elif UNITY_IOS
//        uniwebtoopen.openagin();
//#endif
    }


    /// <summary>
    /// 调节开奖时候的直播窗口大小
    /// </summary>
    /// <param name="ison"></param>
    private void fullwindowcontorl(bool ison)
    {
        if (ison)
        {
            isfullopenwindows = true;
        }
        else
        {
            isfullopenwindows = false;
        }
    }

    /// <summary>
    /// 调节背景音乐大小
    /// </summary>
    /// <param name="value"></param>
    private void backmusicchange(float value)
    {
        Audiomanger._instenc.GetComponent<AudioSource>().volume = value;
    }

    /// <summary>
    /// 关闭按键音效
    /// </summary>
    /// <param name="isclose"></param>
    private void closeclickvoice(bool isclose)
    {
        if (isclose)
        {

        }
        else
        {

        }
    }
   //当期开奖数据字符串转换为数组
   public int[] changestringtointlist(string temp)
    {
        string temp2 = temp;

        string[] temp3 = temp2.Split(new char[] { ',' });
        int[] temp4 = new int[10];
        for (int i = 0; i < temp3.Length; i++)
        {
            temp4[i] = int.Parse(temp3[i]);
        }
        //Debug.Log(temp3);
        return temp4;

    }

    




    ///通过字符串获取对应的状态值
    public poketstate changeTYPEtoint(string temp)
    {
        poketstate retuenint;
        if (temp.Equals("A1"))
        {
            retuenint = poketstate.spdaes;
        }
        else if(temp.Equals("B1"))
        {
            retuenint = poketstate.Hearts;
        }
        else if (temp.Equals("C1"))
        {
            retuenint = poketstate.Culb;
        }
        else if (temp.Equals("D1"))
        {
            retuenint = poketstate.Diamond;
        }
        //else if (temp.Equals("E"))
        //{
        //    retuenint = poketstate.jokeyR;
        //}
        else if (temp.Equals("A2"))
        {
            retuenint = poketstate.spdaesJR;

        }
        else if (temp.Equals("B2"))
        {
            retuenint = poketstate.HeartsJR;

        }
        else if (temp.Equals("C2"))
        {
            retuenint = poketstate.CulbJR;

        }
        else if (temp.Equals("D2"))
        {
            retuenint = poketstate.DiamondJR;

        }
        else
        {
            retuenint = 0;
        }
        return retuenint;
    }
    /// <summary>
    /// 将字符转换为对应数字
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    private int changeTYPEtointforcanel(string temp)
    {
        int retuenint;
        if (temp.Equals("A"))
        {
            retuenint = 0;
        }
        else if (temp.Equals("B"))
        {
            retuenint =1;
        }
        else if (temp.Equals("C"))
        {
            retuenint = 2;
        }
        else if (temp.Equals("D"))
        {
            retuenint = 3;
        }
        else if (temp.Equals("E"))
        {
            retuenint = 4;
        }
        else if (temp.Equals("F"))
        { 
            retuenint =5;

        }
        else if (temp.Equals("G"))
        {
            retuenint = 6;

        }
        else if (temp.Equals("H"))
        {
            retuenint = 7;

        }
        else if (temp.Equals("I"))
        {
            retuenint = 8;

        }
        else
        {
            retuenint = 0;
        }
        return retuenint;
    }





    //用于监听移动设备游戏进入后台的处理
    void OnApplicationPause()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        Debug.Log("OnApplicationPause  " + isPause + "  " + isFocus);


        if (!isPause)
        {
            Debug.Log("强制暂停时，事件");
            //pauseTime();
        }
        else
        {
            isFocus = true;
        }
        isPause = true;
#endif
    }

    //游戏重新聚焦的监听
    void OnApplicationFocus()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        Debug.Log("OnApplicationFocus  " + isPause + "  " + isFocus);


        //if (isPause == false || (isFocus == false && isPause == false))
        //{
        //    messg.Show("异常", "由于您切换至游戏外，请重新进入房间", ButtonStyle.Confirm,
        //  delegate (MessageBoxResult call)
        //  {
        //      switch (call)
        //      {

        //          case MessageBoxResult.Cancel:
        //              tohall();
        //              break;
        //          case MessageBoxResult.Confirm:
        //              tohall();
        //              break;
        //          case MessageBoxResult.Timeout:
        //              tohall();
        //              break;

        //      }
        //  }, 0f);
        //    return;
        //}



        if (isPause)
        {
            isFocus = true;
          
        }
#if UNITY_IPHONE
        countouit++;
        if (countouit > 1)
        {
            countouit = 0;
            if (isPause == false && isFocus == false)
            {
                messg.Show("异常", "由于您长时间的退出，请重新进入房间", ButtonStyle.Confirm,
              delegate (MessageBoxResult call)
              {
                  switch (call)
                  {

                      case MessageBoxResult.Cancel:
                          tohall();
                          break;
                      case MessageBoxResult.Confirm:
                          tohall();
                          break;
                      case MessageBoxResult.Timeout:
                          tohall();
                          break;

                  }
              }, 0f);
                return;
            }

        }
#endif
        if (isFocus)
        {
            Debug.Log("“启动”手机时，事件");
            //resumeList();
            isopendatafromsever = false;
            iscomeback = true;
            //if (uniwebtoopen != null)
            //{
            //    uniwebtoopen.reload();
            //}
            isPause = false;
            isFocus = false;
        }


      
#endif
    }
    /// <summary>
    /// 销毁时处理响应的事件与数据
    /// </summary>
    private void OnDestroy()
    {
        LoginInfo.Instance().wwwinstance.listformwwwinfo.Clear();
        LoginInfo.Instance().mylogindata.room_id = 0;
        cheak -= cheakpoketpoolisture;
    }

  
    //IEnumerator TuiShui(string url)
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //}

    void OnShowTuiShui()
    {
        
        StartCoroutine(ShowTuiShui(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.userCut + "user_id=" + LoginInfo.Instance().mylogindata.user_id));
    }
    IEnumerator ShowTuiShui(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
            {
                tuiShuiRate.text = jd["Proportion"].ToString() + "‰";
                tuiShuiRateNum.text = jd["UserCut"].ToString();
                TuiShuiPanel.SetActive(true);
            }
        }
    }

    void OnTuiShui()
    {
        Debug.Log(Convert.ToDouble(tuiShuiRateNum.text.ToString()));
        if (Convert.ToDouble(tuiShuiRateNum.text.ToString()) >= 100)
        {
            StartCoroutine(TuiShui(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.userCutSend + "user_id=" + LoginInfo.Instance().mylogindata.user_id));
        }
        else
        {
            senderror("退水金额未达到最低标准");
        }
        
    }
    IEnumerator TuiShui(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.error == null)
	    {
		    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
            if (jd["code"].ToString() == "200")
	        {
		         tuiShuiRateNum.text = "0";
	        }
            senderror("退水成功！");
	    }
    
    }

    //public void OnOpenServicesPanel()
    //{
    //    servicesPanel.gameObject.SetActive(true);

    //    //StartCoroutine(OpenServicesPanel(""));
    //}
    //IEnumerator OpenServicesPanel(string url)
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    yield return www.Send();
    //    if (www.error == null)

    //    {
    //        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
    //        if (jd["code"].ToString() == "200")
    //        {
    //            servicesPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = jd[""].ToString();
    //            servicesPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = jd[""].ToString();
                
    //            servicesBtn.transform.GetChild(0).GetComponent<Text>().text = "客服";
    //        }
            
    //    }
    //}

}
/// <summary>
/// 花色枚举
/// </summary>
public enum poketstate
{
    spdaes,
    Hearts,
    Culb,
    Diamond,
    //jokeyR,
    //jokeyB,
    //isnull,
    spdaesJR,
    HeartsJR,
    CulbJR,
    DiamondJR,
    isnull
}
