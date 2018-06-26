using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public GameObject QuitPanel;
    private bool isQuitPanel = false; //退出面板
    public GameObject SoundPanel;
    private bool isSoundPanel = false;//音效面板
    public GameObject StopPanel;
    private bool isStopPanel = false; //留机面板
    public GameObject UpDownPanel;
    private bool isUpDownPanel = true;//上下分面板（存/取分）
    public GameObject RecordPanel;
    public Text[] RecordData;
    private bool isRectdPanel = false; //历史记录面板

    public GameObject[] OddsObj;
    public GameObject[] Rainbow;
    public GameObject Oddeffecf;
    public GameObject PromptText;


    /// <summary>
    /// 倍率显示
    /// </summary>
    public Text[] OddsText;

    /// <summary>
    /// 倍率显示
    /// </summary>
    public Text[] OddsText2;

    /// <summary>
    /// 短连接
    /// </summary>
    string url = "";

    public Image[] Card;
    public Sprite reverseBoard;
    public GameObject[] HOLD;

    public Text quick_credit;
    public Text fraction;
    public Text CREDIT;
    public Text BET;

    public GameObject AutomaticButton;
    public Sprite[] AutomaticSprite;
    public Text WIN;
    public GameObject WinButton;
    public GameObject Startbutton;
    public GameObject SmallButton;
    public Sprite[] SmallButtonSprite;
    public GameObject BigButton;
    public Sprite[] BigButtonSprite;
    public GameObject FillButton;
    public Sprite[] FillButtonSprite;
    public GameObject BanBiButton;
    public GameObject ShuangBiButton;
    public GameObject BiButton;

    /// <summary>
    /// 是否开始统计赢的分数
    /// </summary>
    private bool IsWin = false;

    /// <summary>
    /// 是否正在比倍
    /// </summary>
    private bool IsBiBei = false;

    private int doubly_type = 1;//比倍类型   1-->半比倍  2----> 一倍   3  ---->  两倍  保存
    private int doubly_data = 1;//比倍结果   1  ---> 小   2  ---->  大

    /// <summary>
    /// 未开始面板
    /// </summary>
    public GameObject LOGO;
    public GameObject board_Game_Card;

    /// <summary>
    /// 正常面板
    /// </summary>
    public GameObject Board_Game;

    /// <summary>
    /// 比倍面板
    /// </summary>
    public GameObject Board_Contrast;

    private int ChargeNum = 0;

    public Text BiPromptText;

    public GameObject FireCycle;

    public GameObject NetworkPrompt;

    public AudioSource Audio;

    public AudioClip BGclip;

    public GameObject winAudio;

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Start()
    {
        ChargeNum = 0;
        HGameData.Instance.FlopNum = 1;
        HGameData.Instance.Fraction = 0;
        HGameData.Instance.IsNetworkError = false;
        HGameData.Instance.IsAutomatic = false;
        HGameData.Instance.IsNetworkError = false;
        Audio.volume = PlayerPrefs.GetFloat("sliderTwo");
        Audiomanger._instenc.GetComponent<AudioSource>().clip = BGclip;
        Audiomanger._instenc.GetComponent<AudioSource>().Play();
    }



    void Update()
    {
        OnUI();
        WinEffect();
        Win();

        if (HGameData.Instance.IsNetworkError)
        {
            HGameData.Instance.IsAutomatic = false;
            NetworkPrompt.SetActive(true);
        }

        switch (HGameData.Instance.BiType)
        {
            case 0:
                for (int i = 0; i < 12; i++)
                {
                    OddsObj[i].SetActive(false);
                    Rainbow[i].SetActive(false);
                }
                HGameData.Instance.HoldData = new string[5];
                HGameData.Instance.IsWinEffect = false;
                for (int i = 0; i < 5; i++)
                {
                    Card[i].sprite = reverseBoard;
                }
                HGameData.Instance.ChargeNum = 0;
                HGameData.Instance.FlopNum = 1;
                LOGO.SetActive(true);
                Board_Game.SetActive(true);
                board_Game_Card.SetActive(false);
                Board_Contrast.SetActive(false);
                SmallButton.GetComponent<Image>().sprite = SmallButtonSprite[2];
                BigButton.GetComponent<Image>().sprite = BigButtonSprite[2];
                FillButton.GetComponent<Image>().sprite = FillButtonSprite[0];

                break;
            case 1:  //正常模式
                LOGO.SetActive(false);
                board_Game_Card.SetActive(true);
                Board_Game.SetActive(true);
                Board_Contrast.SetActive(false);
                SmallButton.GetComponent<Image>().sprite = SmallButtonSprite[2];
                BigButton.GetComponent<Image>().sprite = BigButtonSprite[2];
                FillButton.GetComponent<Image>().sprite = FillButtonSprite[0];
                break;
            case 2://比倍模式         
                if (HGameData.Instance.IsBiBei)
                {
                    SmallButton.SetActive(false);
                    BigButton.SetActive(false);
                    FillButton.SetActive(false);
                    BiButton.SetActive(true);
                    BanBiButton.SetActive(true);
                    ShuangBiButton.SetActive(true);
                }
                else
                {
                    Board_Contrast.SetActive(true);
                    Board_Game.SetActive(false);
                    SmallButton.GetComponent<Image>().sprite = SmallButtonSprite[0];
                    BigButton.GetComponent<Image>().sprite = BigButtonSprite[0];
                    FillButton.GetComponent<Image>().sprite = FillButtonSprite[2];
                }
                break;
        }

        if (HGameData.Instance.IsAutomatic)
        {
            AutomaticButton.GetComponent<Image>().sprite = AutomaticSprite[1];
            if (Startbutton.activeSelf)
            {
                if (HGameData.Instance.IsFlopCard == false && IsWin == false && HGameData.Instance.IsStart == false)
                    StartButton();
            }
            if (WinButton.activeSelf)
            {
                if (HGameData.Instance.IsFlopCard == false && IsWin == false)
                    OnWinButton();
            }
        }
        else
        {
            AutomaticButton.GetComponent<Image>().sprite = AutomaticSprite[0];
        }
    }

    void WinEffect()
    {
        if (HGameData.Instance.IsWinEffect)
        {
            if (HGameData.Instance.IsFlopCard == false)
            {
                switch (HGameData.Instance.CardTitle)
                {
                    case "对子":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[0].SetActive(true);

                        }
                        else
                        {
                            OddsObj[0].SetActive(true);

                        }
                        break;
                    case "两对":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[1].SetActive(true);
                        }
                        else
                        {
                            OddsObj[1].SetActive(true);
                        }
                        break;
                    case "三条":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[2].SetActive(true);
                        }
                        else
                        {
                            OddsObj[2].SetActive(true);
                        }
                        break;
                    case "顺子":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[3].SetActive(true);
                        }
                        else
                        {
                            OddsObj[3].SetActive(true);
                        }
                        break;
                    case "同花":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[4].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[4].SetActive(true);
                        }
                        break;
                    case "葫芦":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[5].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[5].SetActive(true);
                        }
                        break;
                    case "小四梅":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[6].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[6].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    case "大四梅":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[7].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[7].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    case "同花小顺":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[8].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[8].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    case "五梅":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[9].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[9].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    case "同花大顺":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[10].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[10].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    case "五鬼":
                        if (HGameData.Instance.FlopNum == 2)
                        {
                            Rainbow[11].SetActive(true);
                            Oddeffecf.SetActive(true);
                        }
                        else
                        {
                            OddsObj[11].SetActive(true);
                            FireCycle.SetActive(true);
                        }
                        break;
                    default:
                        for (int i = 0; i < 12; i++)
                        {
                            Rainbow[i].SetActive(false);
                            OddsObj[i].SetActive(false);
                        }
                        Oddeffecf.SetActive(false);
                        break;
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    Rainbow[i].SetActive(false);
                    OddsObj[i].SetActive(false);
                }

                Oddeffecf.SetActive(false);
            }
        }
    }

    void Win()
    { ///开始统计分数
        if (IsWin)
        {
            if (HGameData.Instance.WinMoney > 0)
            {
                //1分1分的从赢的分数加到用户分数
                HGameData.Instance.WinMoney -= 10;
                HGameData.Instance.Fraction += 10;
                winAudio.SetActive(true);
            }
            else
            {
                winAudio.SetActive(false);
                HGameData.Instance.IsBiBei = false;
                HGameData.Instance.str_result = new string[6];
                HGameData.Instance.IsWinEffect = false;
                if (HGameData.Instance.BiType == 2)
                    HGameData.Instance.BiType = 0;
                else
                    Invoke("OnCardStart", 0.5f);
                for (int i = 0; i < 12; i++)
                {
                    OddsObj[i].SetActive(false);
                    Rainbow[i].SetActive(false);
                }
                IsWin = false;
                IsBiBei = false;
                HGameData.Instance.IsWin = false;
                WinButton.SetActive(false);
                Startbutton.SetActive(true);
                FillButton.SetActive(true);
                BanBiButton.SetActive(false);
                BiButton.SetActive(false);
                ShuangBiButton.SetActive(false);
                SmallButton.SetActive(true);
                BigButton.SetActive(true);
                HGameData.Instance.HoldData = new string[5];
            }
        }
    }

    void OnCardStart()
    {
        HGameData.Instance.IsCardStart = true;
    }

    /// <summary>
    /// 初始化倍率UI
    /// </summary>
    void StartOdd()
    {
        HGameData.Instance.IsWinEffect = false;
        HGameData.Instance.CardTitle = "";
        for (int i = 0; i < 12; i++)
        {
            OddsObj[i].SetActive(false);
            Rainbow[i].SetActive(false);
        }
        Oddeffecf.SetActive(false);
    }

    /// <summary>
    /// UI更新
    /// </summary>
    void OnUI()
    {
        switch (HGameData.Instance.FlopNum)
        {
            case 1:
                PromptText.GetComponent<Text>().text = "<color=#FFFF00>押 注 </color>或 <color=#458b00>开 始</color>";
                break;
            case 2:
                PromptText.GetComponent<Text>().text = "<color=#458b00>保 留 </color>或 <color=#458b00>开 牌</color>";
                break;
            case 3:
                if (HGameData.Instance.WinMoney != 0)
                    PromptText.GetComponent<Text>().text = "<color=#458b00>得 分 </color>或 <color=#458b00>比 倍</color>";
                else
                    PromptText.GetComponent<Text>().text = "<color=#458b00>保 留 </color>或 <color=#458b00>开 牌</color>";
                break;
        }

        if (isQuitPanel) { QuitPanel.SetActive(true); } else { QuitPanel.SetActive(false); }
        if (isSoundPanel) { SoundPanel.SetActive(true); } else { SoundPanel.SetActive(false); }
        if (isStopPanel) { StopPanel.SetActive(true); } else { StopPanel.SetActive(false); }
        if (isUpDownPanel) { UpDownPanel.SetActive(true); } else { UpDownPanel.SetActive(false); }
        if (isRectdPanel) { /*RecordPanel.SetActive(true);*/ } else { RecordPanel.SetActive(false); }
        for (int i = 0; i < 12; i++)
        {
            if (HGameData.Instance.ChargeNum != 0)
            {
                OddsText[i].text = (int.Parse(HGameData.Instance.Multiplying[i]) * HGameData.Instance.ChargeNum).ToString();
                OddsText2[i].text = (int.Parse(HGameData.Instance.Multiplying[i]) * HGameData.Instance.ChargeNum).ToString();
            }
            else
            {
                OddsText[i].text = HGameData.Instance.Multiplying[i];
                OddsText2[i].text = HGameData.Instance.Multiplying[i];
            }
        }
        quick_credit.text = LoginInfo.Instance().mylogindata.ALLScroce;
        fraction.text = HGameData.Instance.Fraction.ToString();
        CREDIT.text = HGameData.Instance.Fraction.ToString();
        WIN.text = HGameData.Instance.WinMoney.ToString();
        BET.text = HGameData.Instance.ChargeNum.ToString();
        if (HGameData.Instance.IsWin)
        {
            if (HGameData.Instance.BiType == 2)
            {
                FillButton.SetActive(true);

            }
            else
            {
                SmallButton.SetActive(false);
                BigButton.SetActive(false);
                FillButton.SetActive(false);
                BiButton.SetActive(true);
                BanBiButton.SetActive(true);
                ShuangBiButton.SetActive(true);
            }
            Startbutton.SetActive(false);
            WinButton.SetActive(true);
        }
        else
        {
            WinButton.SetActive(false);
            Startbutton.SetActive(true);
            FillButton.SetActive(true);
            SmallButton.SetActive(true);
            BigButton.SetActive(true);
            BiButton.SetActive(false);
            BanBiButton.SetActive(false);
            ShuangBiButton.SetActive(false);
        }
        for (int i = 0; i < RecordData.Length; i++)
        {
            RecordData[i].text = HGameData.Instance.RecordData[i];
        }
    }

    IEnumerator enumerator(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

                    Debug.Log(jd["code"].ToString());
                    if (jd["code"].ToString() == "200")
                    {

                    }
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["quick_credit"].ToString();
                    Debug.Log(LoginInfo.Instance().mylogindata.ALLScroce);

                    HGameData.Instance.Fraction = int.Parse(jd["fraction"].ToString());
                    Debug.Log("用户分数：" + HGameData.Instance.Fraction);
                }
            }
        }
        catch
        {
            Debug.Log("Json解析错误");
            HGameData.Instance.IsNetworkError = true;
        }
    }


    IEnumerator SendRecord(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    if (jd["msg"].ToString() == "获取成功")
                    {
                        RecordPanel.SetActive(true);  //显示记录面板
                    }
                    HGameData.Instance.RecordData[0] = jd["data"]["五鬼"].ToString();
                    HGameData.Instance.RecordData[1] = jd["data"]["同花大顺"].ToString();
                    HGameData.Instance.RecordData[2] = jd["data"]["五梅"].ToString();
                    HGameData.Instance.RecordData[3] = jd["data"]["同花小顺"].ToString();
                    HGameData.Instance.RecordData[4] = jd["data"]["大四梅"].ToString();
                    HGameData.Instance.RecordData[5] = jd["data"]["小四梅"].ToString();
                    HGameData.Instance.RecordData[6] = jd["data"]["葫芦"].ToString();
                    HGameData.Instance.RecordData[7] = jd["data"]["同花"].ToString();
                    HGameData.Instance.RecordData[8] = jd["data"]["顺子"].ToString();
                    HGameData.Instance.RecordData[9] = jd["data"]["三条"].ToString();
                    HGameData.Instance.RecordData[10] = jd["data"]["两对"].ToString();
                    HGameData.Instance.RecordData[11] = jd["data"]["对子"].ToString();
                    HGameData.Instance.RecordData[12] = jd["data"]["win_number"].ToString();
                    HGameData.Instance.RecordData[13] = jd["data"]["total_number"].ToString();
                    HGameData.Instance.RecordData[14] = jd["data"]["gl_one"].ToString();
                    HGameData.Instance.RecordData[15] = jd["data"]["yin"].ToString();
                    HGameData.Instance.RecordData[16] = jd["data"]["wan"].ToString();
                    HGameData.Instance.RecordData[17] = jd["data"]["gl_two"].ToString();
                }
            }
        }
        catch
        {
            Debug.Log("Json解析错误");
            HGameData.Instance.IsNetworkError = true;
        }
    }

    IEnumerator SendBiBei(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    if (jd["code"].ToString() == "200")
                    {
                        IsBiBei = true;
                        HGameData.Instance.BiType = 2;
                        Debug.Log("执行3");
                        FillButton.SetActive(false);
                        BiButton.SetActive(false);
                        BanBiButton.SetActive(false);
                        ShuangBiButton.SetActive(false);
                        SmallButton.SetActive(true);
                        BigButton.SetActive(true);
                        Startbutton.SetActive(false);
                        WinButton.SetActive(true);
                        HGameData.Instance.IsColumnBoard = true;
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        HGameData.Instance.str_result[i] = jd["result"][i].ToString();
                        Debug.Log("右边的牌为第" + (i + 1) + "张：" + HGameData.Instance.str_result[i]);
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Json解析错误");
            HGameData.Instance.IsNetworkError = true;
        }
    }


    #region 按钮事件

    /// <summary>
    ///网络按钮
    /// </summary>
    public void OnNetWorkButton()
    {
        Audio.Play();
        //退出当前房间界面，调整到选房间界面
        //url = "http://47.106.66.89:81/pj-api/trigger-ent?" + "game_id=" + "11" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + HGameData.Instance.RoomTitleID + "&trigger_id=" + HGameData.Instance.Trigger_id;
        //StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
        // GameData.Instance.EnterTheScene("DaTing");
        HGameData.Instance.IsStart = false;
        HGameData.Instance.IsNetworkError = false;
        NetworkPrompt.SetActive(false);

    }

    /// <summary>
    /// 自动按钮
    /// </summary>
    public void OnAutomaticButton()
    {
        Audio.Play();
        if (HGameData.Instance.Fraction != 0)
            HGameData.Instance.IsAutomatic = !HGameData.Instance.IsAutomatic;
        else
            HGameData.Instance.IsAutomatic = false;
    }

    /// <summary>
    /// 押大按钮
    /// </summary>
    public void OnBigButton()
    {
        Audio.Play();
        string b = Onstr_result(HGameData.Instance.str_result);
        url = "http://47.106.66.89:81/pj-api/room-doubly" + "?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&periods_id=" + HGameData.Instance.PeriodsID + "&doubly_type=" + doubly_type + "&doubly_data=" + "2" + "&str_result=" + b;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }



    /// <summary>
    /// 押小按钮
    /// </summary>
    public void OnSmallButton()
    {
        Audio.Play();
        string b = Onstr_result(HGameData.Instance.str_result);
        url = "http://47.106.66.89:81/pj-api/room-doubly" + "?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&periods_id=" + HGameData.Instance.PeriodsID + "&doubly_type=" + doubly_type + "&doubly_data=" + "1" + "&str_result=" + b;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }

    string Onstr_result(string[] data)
    {
        Audio.Play();
        string b = "";
        for (int i = 0; i < data.Length; i++)
        {
            b += data[i].TrimEnd('-');
            b += ',';
        }
        b = b.TrimEnd(',');
        b = b.Replace("-", "");
        return b;
    }

    /// <summary>
    /// 双比倍按钮
    /// </summary>
    public void OnShuangBiButton()
    {
        Audio.Play();
        HGameData.Instance.IsBiBei = false;
        doubly_type = 3;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        HGameData.Instance.Fraction -= HGameData.Instance.WinMoney;
        HGameData.Instance.WinMoney = HGameData.Instance.WinMoney * 2;
        BiPromptText.text = "双 比 倍";
    }

    /// <summary>
    /// 半比倍按钮
    /// </summary>
    public void OnBanBiButton()
    {
        Audio.Play();
        HGameData.Instance.IsBiBei = false;
        doubly_type = 1;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        HGameData.Instance.WinMoney = HGameData.Instance.WinMoney / 2;
        HGameData.Instance.Fraction += HGameData.Instance.WinMoney;
        BiPromptText.text = "半 比 倍";
    }

    /// <summary>
    /// 比倍按钮
    /// </summary>
    public void OnBiButton()
    {
        Audio.Play();
        HGameData.Instance.IsBiBei = false;
        doubly_type = 2;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        BiPromptText.text = "比 倍";
    }

    /// <summary>
    /// 押分按钮
    /// </summary>
    public void OnFillButton()
    {
        Audio.Play();
        if (HGameData.Instance.Fraction == 0)
        {
            Up_Down_Button();
            return;
        }
        else
        {
            if (HGameData.Instance.BiType == 0)
            {
                Debug.Log("执行4");
                HGameData.Instance.BiType = 1;
                HGameData.Instance.FlopNum = 1;
            }
            if (HGameData.Instance.BiType == 3)
                return;
            if (HGameData.Instance.FlopNum != 1)
                return;

            ChargeNum += 100;
            if (ChargeNum > 1000)
            {
                ChargeNum -= 100;
                return;
            }
            HGameData.Instance.ChargeNum = ChargeNum;

        }

    }

    /// <summary>
    /// 得分按钮
    /// </summary>
    public void OnWinButton()
    {
        Audio.Play();
        IsWin = true;   //开始得分统计
    }



    /// <summary>
    /// 开始按钮
    /// </summary>
    public void StartButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsFlopCard || IsWin)
            return;
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
        switch (HGameData.Instance.FlopNum)
        {
            case 1:
                //第一次翻牌
                if (HGameData.Instance.Fraction != 0)
                {
                    if (HGameData.Instance.Fraction > 100)
                    {
                        if (ChargeNum == 0)
                            HGameData.Instance.ChargeNum = 100;
                        else
                            HGameData.Instance.ChargeNum = ChargeNum;
                        url = "http://47.106.66.89:81/pj-api/hfh-find-walking-algorithm?" + "room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&num=" + HGameData.Instance.ChargeNum + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&seat_number=" + HGameData.Instance.Seat_number;
                    }
                    else
                    {
                        url = "http://47.106.66.89:81/pj-api/hfh-find-walking-algorithm?" + "room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&num=" + HGameData.Instance.Fraction + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&seat_number=" + HGameData.Instance.Seat_number;
                    }
                    StartOdd();
                    HGameData.Instance.HoldData = new string[5];
                    for (int i = 0; i < 5; i++)
                    {
                        HOLD[i].SetActive(false);
                    }
                    Debug.Log("执行五");
                    HGameData.Instance.BiType = 1;
                    HGameData.Instance.IsStart = true;
                    StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
                }
                else
                {
                    Debug.Log("用户余额不足");
                }

                break;
            case 2:
                //第二次翻牌
                StartOdd();
                HGameData.Instance.IsStart = true;
                url = "http://47.106.66.89:81/pj-api/hfh-two-walking-algorithm?room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&num=" + HGameData.Instance.ChargeNum + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&periods_id=" + HGameData.Instance.PeriodsID + "&retain=" + OnRetain(HGameData.Instance.HoldData);
                StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
                break;
        }
    }

    string OnRetain(string[] str)
    {
        string b = "";

        for (int i = 0; i < 5; i++)
        {
            if (str[i] == (i + 1).ToString())
            {
                b += (i + 1).ToString();
            }
            else
            {
                b += "";
            }
        }

        if (b == "")
            b = "100";

        return b;
    }

    /// <summary>
    /// 退出按钮
    /// </summary>
    public void QuitButton()
    {
        Audio.Play();
        isQuitPanel = !isQuitPanel;
        // isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
    }

    /// <summary>
    /// 确定按钮 
    /// </summary>
    public void QuitPanelConfirmButton()
    {
        Audio.Play();
        //退出当前房间界面，调整到选房间界面
        url = "http://47.106.66.89:81/pj-api/trigger-ent?" + "game_id=" + "11" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + HGameData.Instance.RoomTitleID + "&trigger_id=" + HGameData.Instance.Trigger_id;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 取消按钮
    /// </summary>
    public void QuitPanelCancelButton()
    {
        Audio.Play();
        isQuitPanel = false;
    }

    /// <summary>
    /// 音效按钮
    /// </summary>
    public void SoundButton()
    {
        isQuitPanel = false;
        isSoundPanel = !isSoundPanel;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
        //if (GameData.Instance.NatureMusic == 0)
        //{
        //    GameData.Instance.NatureMusic = 1;
        //    PlayerPrefs.SetInt("NatureMusic", 1);
        //    PlayerAudioClip.instance.OnStopAudio();
        //    SoundPanel.sprite = SoundSprites[1];
        //}
        //else
        //{
        //    GameData.Instance.NatureMusic = 0;
        //    PlayerPrefs.SetInt("NatureMusic", 0);
        //    PlayerAudioClip.instance.OnPlay();
        //    SoundPanel.sprite = SoundSprites[0];
        //}
    }

    /// <summary>
    /// 留机（暂停）按钮
    /// </summary>
    public void StopButton()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = !isStopPanel;
        isUpDownPanel = false;
        isRectdPanel = false;
    }

    /// <summary>
    /// 留机（暂停）面板  确定
    /// </summary>
    public void StopPanel_Yes()
    {
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/stay-machine?game_id=11&room_id=" + HGameData.Instance.RoomTitleID + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&trigger_id=" + HGameData.Instance.Trigger_id;

        StartCoroutine(StopGame(url));

    }

    public GameObject StopError;

    IEnumerator StopGame(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);

                    if (jd["code"].ToString() == "200")
                    {
                        if (jd["msg"].ToString() == "留机成功")
                        {
                            HGameData.Instance.EnterTheScene("PTCChooseJiQi");
                        }
                    }
                    else
                    {
                        StopError.SetActive(true);
                    }

                }
            }
        }
        catch
        {
            Debug.Log("开牌超过1000手才能留机");
            StopError.SetActive(true);
        }
    }

    public void StopGameError()
    {
        StopError.SetActive(false);
    }

    /// <summary>
    /// 留机（暂停）面板  取消  
    /// </summary>
    public void StopPanel_No()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
    }

    /// <summary>
    /// 上下分按钮
    /// </summary>
    public void Up_Down_Button()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = !isUpDownPanel;
        isRectdPanel = false;
    }

    /// <summary>
    /// 上分按钮
    /// </summary>
    public void Up_Down_Panel_Up()
    {
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/top-score?" + "room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id;
        StartCoroutine(enumerator(url));
    }

    /// <summary>
    /// 存分按钮
    /// </summary>
    public void Up_Down_Panel_Down()
    {
        Audio.Play();
        if (HGameData.Instance.Fraction != 0)
        {
            url = "http://47.106.66.89:81/pj-api/lower-score?" + "room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id;
            StartCoroutine(enumerator(url));
        }
    }

    /// <summary>
    /// 历史记录按钮
    /// </summary>
    public void RecordButton()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = !isRectdPanel;
        if (isRectdPanel)
        {
            url = "http://47.106.66.89:81/pj-api/room-record" + "?room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&seat_number=" + HGameData.Instance.Seat_number;
            StartCoroutine(SendRecord(url));
        }
    }

    /// <summary>
    /// 历史面板关闭按钮
    /// </summary>
    public void RecordPanelCloseButton()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
    }

    /// <summary>
    /// 背景按钮
    /// </summary>
    public void BGButton()
    {
        Audio.Play();
        isQuitPanel = false;
        isSoundPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRectdPanel = false;
    }

    #endregion
}
