using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DGame_GameManage : MonoBehaviour
{

    private string url;

    public GameObject[] OddListObj;
    public Text[] OddList;
    public Text AllScore;
    public Text Fraction;
    public Text Fraction2;
    public Text WinMoney;
    public Text BET;



    public GameObject QuitPanel;
    private bool isQuitPanel = false;
    public GameObject StopPanel;
    private bool isStopPanel = false;
    public GameObject UpDownPanel;
    private bool isUpDownPanel = true;
    public GameObject RecordPanel;
    private bool isRecordPanel = false;
    public GameObject RankingPanel;
    private bool isRankingPanel = false;

    public GameObject wining;
    public GameObject winEffectIng;

    /// <summary>
    /// 纸牌组
    /// </summary>
    public Sprite[] boardGroup;

    /// <summary>
    /// 反面纸牌
    /// </summary>
    public Sprite[] reverseBoard;


    /// <summary>
    /// 纸牌
    /// </summary>
    public Image[] boards;

    /// <summary>
    /// 翻牌序号 0.第一张 1.第二张 2.第三张 3.第四张 4.第五张
    /// </summary>
    private int CardNum = 0;

    /// <summary>
    /// 翻牌帧数动画帧数
    /// </summary>
    private int FlopFrame = 0;

    /// <summary>
    /// 翻牌速度计时
    /// </summary>
    private float FlopTimes;

    /// <summary>
    /// 翻牌速度
    /// </summary>
    public float FlopSpeed;

    private bool isCardStart = false;

    public GameObject[] HOLD;

    public AudioClip BgClip;
    public AudioClip[] Clip;
    private AudioSource Audio;



    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Start()
    {
        ChargeNum = 0;
        DGameData.Instance.Model_List = new List<DGane_Modle>();
        DGameData.Instance.FlopNum = 1;
        DGameData.Instance.Fraction = 0;
        DGameData.Instance.IsNetworkError = false;
        DGameData.Instance.IsAutomatic = false;
        Audio = this.GetComponent<AudioSource>();
        Audiomanger._instenc.GetComponent<AudioSource>().clip = BgClip;
        Audiomanger._instenc.GetComponent<AudioSource>().Play();

    }

    /// <summary>
    /// 让牌子初始化为背面
    /// </summary>
    public bool IsCardStart = true;

    public GameObject NetworkPrompt;

    private void Update()
    {
        OnUI();
        WinEffect();
        OnWin();

        if (DGameData.Instance.IsNetworkError)
        {
            DGameData.Instance.IsAutomatic = false;
            NetworkPrompt.SetActive(true);
        }

        if (DGameData.Instance.IsFlopCard)
        {
            OnFlop(ref CardNum);
        }
        if (isCardStart)
        {
            isCardStart = false;
            for (int i = 0; i < 5; i++)
            {
                boards[i].sprite = reverseBoard[0];
                HOLD[i].SetActive(false);
            }
            DGameData.Instance.FlopNum = 1;
            if (DGameData.Instance.IsAutomatic == false)
                DGameData.Instance.BiType = 0;
            DGameData.Instance.IsStart = false;
        }

        if (DGameData.Instance.IsFlopCard == false)
        {
            for (int i = 0; i < 5; i++)
            {
                if (DGameData.Instance.HoldData[i] == (i + 1).ToString())
                {
                    HOLD[i].SetActive(true);
                }
                else
                {
                    HOLD[i].SetActive(false);
                }
            }
        }

        switch (DGameData.Instance.BiType)
        {
            case 0:
                for (int i = 0; i < 10; i++)
                {
                    OddListObj[i].SetActive(true);
                }
                DGameData.Instance.HoldData = new string[5];
                DGameData.Instance.IsWinEffect = false;
                for (int i = 0; i < 5; i++)
                {
                    boards[i].GetComponent<Image>().sprite = reverseBoard[0];
                }
                DGameData.Instance.ChargeNum = 0;
                DGameData.Instance.FlopNum = 1;
                Board_Game.SetActive(true);
                Board_Contrast.SetActive(false);
                SmallButton.GetComponent<Image>().sprite = SmallButtonSprite[2];
                BigButton.GetComponent<Image>().sprite = BigButtonSprite[2];
                FillButton.GetComponent<Image>().sprite = FillButtonSprite[0];

                break;
            case 1:  //正常模式
                Board_Game.SetActive(true);
                Board_Contrast.SetActive(false);
                SmallButton.GetComponent<Image>().sprite = SmallButtonSprite[2];
                BigButton.GetComponent<Image>().sprite = BigButtonSprite[2];
                FillButton.GetComponent<Image>().sprite = FillButtonSprite[0];
                break;
            case 2://比倍模式         
                if (DGameData.Instance.IsBiBei)
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

        if (DGameData.Instance.IsAutomatic)
        {
            AutomaticButton.GetComponent<Image>().sprite = AutomaticSprite[1];
            if (StartButton.activeSelf)
            {
                if (DGameData.Instance.IsFlopCard == false && IsWin == false && DGameData.Instance.IsStart == false)
                    Button_Start();
            }
            if (WinButton.activeSelf)
            {
                if (DGameData.Instance.IsFlopCard == false && IsWin == false)
                    OnWinButton();
            }
        }
        else
        {
            AutomaticButton.GetComponent<Image>().sprite = AutomaticSprite[0];
        }





        if (DGameData.Instance.IsColumnBoard)
        {
            BiBeiPrompt.SetActive(false);
            DGameData.Instance.IsColumnBoard = false;
            int a = 0;
            for (int i = 0; i < columnBoard.Length; i++)
            {
                a = ColumnBoard_Num(i);
                columnBoard[i].sprite = boardGroup[a];
            }
        }

        if (DGameData.Instance.IsFlopColumnBoard)
        {
            BiFlopTime += Time.deltaTime;
            if (BiFlopTime > 0.06)
            {
                BiFlopTime = 0;
                guessingBoard.sprite = reverseBoard[BiFlopNum];
                BiFlopNum++;
            }
            if (BiFlopNum >= reverseBoard.Length)
            {
                Audio.clip = Clip[1];
                Audio.Play();
                Debug.Log("得出比倍结果");
                BiFlopNum = 0;
                DGameData.Instance.IsFlopColumnBoard = false;

                guessingBoard.sprite = boardGroup[GuessingBoard_Num()];
                BiBeiPrompt.SetActive(true);
                if (DGameData.Instance.WinMoney == 0)
                {
                    Audio.clip = Clip[3];
                    Audio.Play();
                    BiBeiPrompt.GetComponent<Image>().sprite = BiBeiSprite[1];
                    Invoke("OnStart", 1.5f);
                }
                else
                {
                    Audio.clip = Clip[2];
                    Audio.Play();
                    BiBeiPrompt.GetComponent<Image>().sprite = BiBeiSprite[0];
                }
            }
        }
    }
    public Image[] columnBoard;    //例外显示纸牌


    public GameObject AutomaticButton;
    public Sprite[] AutomaticSprite;

    private float BiFlopTime;//翻牌时间
    private int BiFlopNum;   //翻牌序列帧

    public Image guessingBoard;  //竞猜纸牌

    public GameObject BiBeiPrompt;
    public Sprite[] BiBeiSprite;

    void OnStart()
    {
        guessingBoard.sprite = reverseBoard[0];
        DGameData.Instance.BiType = 0;
        Debug.Log("执行2");
        DGameData.Instance.IsWin = false;
        BiBeiPrompt.SetActive(false);
    }

    int GuessingBoard_Num()
    {
        int a = 0;
        string[] b = DGameData.Instance.Result_Data.Split('-');
        switch (DGameData.Instance.Result_Data[0])
        {
            case 'A':
                //黑桃              
                a = int.Parse(b[1]) - 1;
                break;
            case 'B':
                //红桃
                a = int.Parse(b[1]) + 12;
                break;
            case 'C':
                a = int.Parse(b[1]) + 25;
                //梅花
                break;
            case 'D':
                //方块
                a = int.Parse(b[1]) + 38;
                break;
            case 'E':
                //鬼牌
                a = 52;
                break;
            default:
                Debug.Log("解析牌类错误，不属于ABCDE类牌");
                a = 52;
                break;
        }
        return a;
    }


    int ColumnBoard_Num(int num)
    {
        int a = 0;
        string[] b = DGameData.Instance.str_result[num].Split('-');
        switch (DGameData.Instance.str_result[num][0])
        {
            case 'A':
                //黑桃              
                a = int.Parse(b[1]) - 1;
                break;
            case 'B':
                //红桃
                a = int.Parse(b[1]) + 12;
                break;
            case 'C':
                a = int.Parse(b[1]) + 25;
                //梅花
                break;
            case 'D':
                //方块
                a = int.Parse(b[1]) + 38;
                break;
            case 'E':
                //鬼牌
                a = int.Parse(b[1]) + 51;
                break;
            default:
                Debug.Log("解析牌类错误，不属于ABCDE类牌");
                a = 52;
                break;
        }
        return a;
    }

    /// <summary>
    /// 正常面板
    /// </summary>
    public GameObject Board_Game;
    public Sprite[] SmallButtonSprite;
    public Sprite[] BigButtonSprite;
    public Sprite[] FillButtonSprite;


    /// <summary>
    /// 比倍面板
    /// </summary>
    public GameObject Board_Contrast;


    private float EffectTime;

    void WinEffect()
    {
        if (DGameData.Instance.IsWinEffect)
        {
            if (DGameData.Instance.IsFlopCard == false)
            {
                EffectTime += Time.deltaTime;
                if (EffectTime >= 0.1)
                {
                    EffectTime = 0;
                    switch (DGameData.Instance.CardTitle)
                    {
                        case "对子":
                            if (OddListObj[0].activeSelf)
                                OddListObj[0].SetActive(false);
                            else
                                OddListObj[0].SetActive(true);
                            break;
                        case "两对":
                            if (OddListObj[1].activeSelf)
                                OddListObj[1].SetActive(false);
                            else
                                OddListObj[1].SetActive(true);
                            break;
                        case "三条":
                            if (OddListObj[2].activeSelf)
                                OddListObj[2].SetActive(false);
                            else
                                OddListObj[2].SetActive(true);
                            break;
                        case "顺子":
                            if (OddListObj[3].activeSelf)
                                OddListObj[3].SetActive(false);
                            else
                                OddListObj[3].SetActive(true);
                            break;
                        case "同花":
                            if (OddListObj[4].activeSelf)
                                OddListObj[4].SetActive(false);
                            else
                                OddListObj[4].SetActive(true);
                            break;
                        case "葫芦":
                            if (OddListObj[5].activeSelf)
                                OddListObj[5].SetActive(false);
                            else
                                OddListObj[5].SetActive(true);
                            break;
                        case "四梅":
                            if (OddListObj[6].activeSelf)
                                OddListObj[6].SetActive(false);
                            else
                                OddListObj[6].SetActive(true);
                            break;
                        case "同花小顺":
                            if (OddListObj[7].activeSelf)
                                OddListObj[7].SetActive(false);
                            else
                                OddListObj[7].SetActive(true);
                            break;
                        case "五梅":
                            if (OddListObj[8].activeSelf)
                                OddListObj[8].SetActive(false);
                            else
                                OddListObj[8].SetActive(true);
                            break;
                        case "同花大顺":
                            if (OddListObj[9].activeSelf)
                                OddListObj[9].SetActive(false);
                            else
                                OddListObj[9].SetActive(true);
                            break;
                        default:
                            for (int i = 0; i < 10; i++)
                            {
                                OddListObj[i].SetActive(true);
                            }
                            winEffectIng.SetActive(false);
                            break;

                    }
                }
                winEffectIng.SetActive(true);
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    OddListObj[i].SetActive(true);
                }
                winEffectIng.SetActive(false);
            }
        }
    }

    void OnWin()
    {
        if (IsWin)
        {
            if (DGameData.Instance.WinMoney > 0)
            {
                //1分1分的从赢的分数加到用户分数
                DGameData.Instance.WinMoney -= 10;
                DGameData.Instance.Fraction += 10;
                wining.SetActive(true);
            }
            else
            {
                wining.SetActive(false);
                DGameData.Instance.IsBiBei = false;
                DGameData.Instance.str_result = new string[6];
                DGameData.Instance.IsWinEffect = false;
                if (DGameData.Instance.BiType == 2)
                    DGameData.Instance.BiType = 0;
                else
                    Invoke("OnCardStart", 0.5f);
                for (int i = 0; i < 10; i++)
                {
                    OddListObj[i].SetActive(true);
                }
                winEffectIng.SetActive(false);
                IsWin = false;
                IsBiBei = false;
                DGameData.Instance.IsWin = false;
                WinButton.SetActive(false);
                StartButton.SetActive(true);
                FillButton.SetActive(true);
                BanBiButton.SetActive(false);
                BiButton.SetActive(false);
                ShuangBiButton.SetActive(false);
                SmallButton.SetActive(true);
                BigButton.SetActive(true);
                DGameData.Instance.HoldData = new string[5];
            }
        }
    }

    /// <summary>
    /// 是否正在比倍
    /// </summary>
    private bool IsBiBei = false;

    /// <summary>
    /// 播放翻牌动画
    /// </summary>
    /// <param name="num"></param>
    void OnFlop(ref int num)
    {
        if (DGameData.Instance.HoldData[num] == (num + 1).ToString() && DGameData.Instance.FlopNum == 3)
        {
            FlopFrame = 0;
            int a = Card_Num(num);
            boards[num].GetComponent<Image>().sprite = boardGroup[a];
            num++;
            if (num > 4)
            {
                num = 0;
                DGameData.Instance.IsFlopCard = false;
                DGameData.Instance.IsStart = false;
                if (DGameData.Instance.WinMoney == 0)
                {
                    Invoke("OnCardStart", 0.5f);
                }
            }
        }
        else
        {
            if (FlopFrame < reverseBoard.Length)
            {
                FlopTimes += Time.deltaTime;
                if (FlopTimes >= FlopSpeed)
                {
                    FlopTimes = 0;
                    boards[num].GetComponent<Image>().sprite = reverseBoard[FlopFrame];
                    FlopFrame++;
                }
            }
            else
            {
                FlopFrame = 0;
                Audio.clip = Clip[1];
                Audio.Play();
                int a = Card_Num(num);
                boards[num].GetComponent<Image>().sprite = boardGroup[a];

                //成功翻牌，开始下一张牌 翻牌
                num++;
                if (num > 4)
                {
                    num = 0;
                    DGameData.Instance.IsFlopCard = false;
                    if (DGameData.Instance.FlopNum == 3 && DGameData.Instance.WinMoney == 0)
                    {
                        Invoke("OnCardStart", 0.5f);
                    }
                }
            }
        }
    }



    void OnCardStart()
    {
        isCardStart = true;
        DGameData.Instance.FlopNum = 1;
    }


    int Card_Num(int num)
    {
        int a = 0;
        string[] b = DGameData.Instance.CardType[num].Split('-');
        //正常类型的牌
        switch (DGameData.Instance.CardType[num][0])
        {
            case 'A':
                //黑桃              
                a = int.Parse(b[1]) - 1;
                break;
            case 'B':
                //红桃
                a = int.Parse(b[1]) + 12;
                break;
            case 'C':
                a = int.Parse(b[1]) + 25;
                //梅花
                break;
            case 'D':
                //方块
                a = int.Parse(b[1]) + 38;
                break;
            case 'E':
                //鬼牌
                a = int.Parse(b[1]) + 51;
                break;
            default:
                Debug.Log("解析牌类错误，不属于ABCDE类牌");
                a = 52;
                break;
        }
        return a;
    }


    public void Button_Up()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        if (float.Parse(LoginInfo.Instance().mylogindata.ALLScroce) <= 0)
            return;
        url = "http://47.106.66.89:81/pj-api/top-score?room_id=" + DGameData.Instance.Room_ID + "&game_id=12&user_id=" + LoginInfo.Instance().mylogindata.user_id;
        StartCoroutine(AllScroce(url));
    }

    public void Button_Down()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        if (DGameData.Instance.Fraction <= 0)
            return;
        url = "http://47.106.66.89:81/pj-api/lower-score?room_id=" + DGameData.Instance.Room_ID + "&game_id=12&user_id=" + LoginInfo.Instance().mylogindata.user_id;
        StartCoroutine(AllScroce(url));
    }

    public IEnumerator AllScroce(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();

        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
                try
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    LoginInfo.Instance().mylogindata.ALLScroce = jd["quick_credit"].ToString();
                    DGameData.Instance.Fraction = int.Parse(jd["fraction"].ToString());
                }
                catch
                {
                    DGameData.Instance.IsNetworkError = true;
                    Debug.Log("上下分错误");
                }
            }
        }
    }

    public void Button_Quit()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        ClosePanel();
        isQuitPanel = true;
    }

    public void Button_Quit_Quit()
    {
        url = "http://47.106.66.89:81/pj-api/trigger-ent?game_id=12&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID + "&trigger_id=" + DGameData.Instance.TriggerID;
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }

    public void Button_UpDown()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        ClosePanel();
        isUpDownPanel = true;
    }

    public void Button_Record()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        ClosePanel();
        Debug.Log("点击到记录按钮");
        // isRecordPanel = true;
        switch (DGameData.Instance.RecordTypeNum)
        {
            case 1:
                Button_Record_01();
                break;
            case 2:
                Button_Record_02();
                break;
            case 3:
                Button_Record_03();
                break;
        }
    }


    public void Button_Record_01()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        Debug.Log("点击到扑克按钮");
        url = "http://47.106.66.89:81/pj-api/poker-record?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID;
        StartCoroutine(SendRecord_1(url));
    }

    public void Button_Record_02()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/doubly-record?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID;
        StartCoroutine(SendRecord_2(url));
    }

    public void Button_Record_03()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/prize-record?room_id=" + DGameData.Instance.Room_ID;
        StartCoroutine(SendRecord_3(url));
    }


    public IEnumerator SendRecord_1(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();

        try
        {
            DGameData.Instance.RecordTypeNum = 1;
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    isRecordPanel = true;
                    RecordPanelList[0].SetActive(true);
                    RecordPanelList[1].SetActive(false);
                    RecordPanelList[2].SetActive(false);
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    DGameData.Instance.BetDataNum = jd["betData"].Count;
                    if (DGameData.Instance.BetDataNum >= 15)
                    {
                        DGameData.Instance.BetDataNum = 15;
                        for (int i = 0; i < DGameData.Instance.BetDataNum; i++)
                        {
                            try { DGameData.Instance.one_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["one_data"].ToString(); }
                            catch { DGameData.Instance.one_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.two_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["two_data"].ToString(); }
                            catch { DGameData.Instance.two_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.retain_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["retain_data"].ToString(); }
                            catch { DGameData.Instance.retain_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.bet_total[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["bet_total"].ToString(); }
                            catch { DGameData.Instance.bet_total[15 - i - 1] = ""; }
                            try { DGameData.Instance.win_total[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["win_total"].ToString(); }
                            catch { DGameData.Instance.win_total[15 - i - 1] = ""; }
                            try { DGameData.Instance.user_balance[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["user_balance"].ToString(); }
                            catch { DGameData.Instance.user_balance[15 - i - 1] = ""; }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < DGameData.Instance.BetDataNum; i++)
                        {
                            try { DGameData.Instance.one_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["one_data"].ToString(); }
                            catch { DGameData.Instance.one_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.two_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["two_data"].ToString(); }
                            catch { DGameData.Instance.two_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.retain_data[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["retain_data"].ToString(); }
                            catch { DGameData.Instance.retain_data[15 - i - 1] = ""; }
                            try { DGameData.Instance.bet_total[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["bet_total"].ToString(); }
                            catch { DGameData.Instance.bet_total[15 - i - 1] = ""; }
                            try { DGameData.Instance.win_total[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["win_total"].ToString(); }
                            catch { DGameData.Instance.win_total[15 - i - 1] = ""; }
                            try { DGameData.Instance.user_balance[15 - i - 1] = jd["betData"][jd["betData"].Count - i - 1]["user_balance"].ToString(); }
                            catch { DGameData.Instance.user_balance[15 - i - 1] = ""; }
                        }
                    }
                }
            }
        }
        catch
        {


        }


    }

    public IEnumerator SendRecord_2(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            DGameData.Instance.RecordTypeNum = 2;
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    isRecordPanel = true;
                    RecordPanelList[1].SetActive(true);
                    RecordPanelList[0].SetActive(false);
                    RecordPanelList[2].SetActive(false);
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    isRecordPanel = true;

                    DGameData.Instance.Model_List = new List<DGane_Modle>();
                    DGameData.Instance.BiBeiDataNum = jd["data"].Count;
                    print(DGameData.Instance.BiBeiDataNum);
                    if (DGameData.Instance.BiBeiDataNum > 15)
                    {
                        DGameData.Instance.BiBeiDataNum = 15;
                    }
                    for (int i = 0; i < DGameData.Instance.BiBeiDataNum; i++)
                    {
                        DGane_Modle md = new DGane_Modle();
                        md.ID = i;
                        md.newList = new List<DGane_Modle_1>();
                        for (int j = 0; j < jd["data"][i].Count; j++)
                        {
                            DGane_Modle_1 md_1 = new DGane_Modle_1();
                            md_1.money = (int)float.Parse(jd["data"][i][j]["money"].ToString());
                            md_1.doubly_sign = jd["data"][i][j]["doubly_sign"].ToString();
                            md_1.doubly_guessing_object = jd["data"][i][j]["doubly_guessing_object"].ToString();
                            md_1.doubly_result = jd["data"][i][j]["doubly_result"].ToString();
                            md.newList.Add(md_1);
                        }
                        DGameData.Instance.Model_List.Add(md);
                    }
                    //for (int i = 0; i < DGameData.Instance.Model_List.Count; i++)
                    //{
                    //    print(DGameData.Instance.Model_List[i].money + "sssss" + DGameData.Instance.Model_List[i].ID);
                    //}
                    //DGameData.Instance.BiBeiDataNum = jd["data"].Count;
                    //if (DGameData.Instance.BiBeiDataNum > 15)
                    //{
                    //    DGameData.Instance.BiBeiDataNum = 15;
                    //}
                    //for (int i = 1; i <= DGameData.Instance.BiBeiDataNum; i++)
                    //{
                    //    DGameData.Instance.BiBeiDataNum2[16 - i] = jd["data"][i].Count;
                    //    if (DGameData.Instance.BiBeiDataNum2[16 - i] > 15)
                    //    {
                    //        DGameData.Instance.BiBeiDataNum2[16 - i] = 15;
                    //    }
                    //    for (int x = 1; x <= DGameData.Instance.BiBeiDataNum2[16 - i]; x++)
                    //    {
                    //        try { DGameData.Instance.money[16 - i, x - 1] = jd["data"][16 - i][x - 1]["money"].ToString(); }
                    //        catch { DGameData.Instance.money[16 - i, x - 1] = "0"; }
                    //        try { DGameData.Instance.doubley_sign[16 - i, x - 1] = jd["data"][16 - i][x - 1]["doubly_sign"].ToString(); }
                    //        catch { DGameData.Instance.doubley_sign[16 - i, x - 1] = ""; }
                    //        try { DGameData.Instance.doubly_guessing_object[16 - i, x - 1] = jd["data"][16 - i][x - 1]["doubly_guessing_object"].ToString(); }
                    //        catch { DGameData.Instance.doubly_guessing_object[16 - i, x - 1] = ""; }
                    //        try { DGameData.Instance.doubly_result[16 - i, x - 1] = jd["data"][16 - i][x - 1]["doubly_result"].ToString(); }
                    //        catch { DGameData.Instance.doubly_result[16 - i, x - 1] = ""; }

                    //    }
                    //}
                }
            }
        }
        catch
        {
            Debug.Log("比倍记录出错");
            DGameData.Instance.BiBeiDataNum = 0;
            DGameData.Instance.BiBeiDataNum2 = new int[15];
        }
    }

    public IEnumerator SendRecord_3(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            DGameData.Instance.RecordTypeNum = 3;
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    try
                    {
                        isRecordPanel = true;
                        RecordPanelList[2].SetActive(true);
                        RecordPanelList[0].SetActive(false);
                        RecordPanelList[1].SetActive(false);
                        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                        isRecordPanel = true;
                        DGameData.Instance.record_awards[0] = jd["data"]["五梅"].ToString();
                        DGameData.Instance.record_awards[1] = jd["data"]["同花大顺"].ToString();
                        DGameData.Instance.record_awards[2] = jd["data"]["同花小顺"].ToString();
                        DGameData.Instance.record_awards[3] = jd["data"]["四梅"].ToString();
                    }
                    catch
                    {
                        HGameData.Instance.IsNetworkError = true;

                    }
                }
            }
        }
        catch
        {

        }
    }


    public GameObject[] RecordPanelList;


    public void Button_Ranking()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        ClosePanel();
        url = "http://47.106.66.89:81/pj-api/grand-prix-notice?game_id=12";
        StartCoroutine(SendRanking(url));
    }

    public IEnumerator SendRanking(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();
        try
        {
            isRankingPanel = true;
            if (www.error == null && www.isDone)
            {
                if (www.responseCode == 200)
                {
                    try
                    {
                        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                        DGameData.Instance.award_num = jd["List"].Count;
                        for (int i = 0; i < 100; i++)
                        {
                            if (i < DGameData.Instance.award_num)
                            {
                                try { DGameData.Instance.award_username[i] = jd["List"][i]["username"].ToString(); }
                                catch { DGameData.Instance.award_username[i] = "错误名字"; }
                                try { DGameData.Instance.award_title[i] = jd["List"][i]["title"].ToString(); }
                                catch { DGameData.Instance.award_title[i] = "错误位置"; }
                                try { DGameData.Instance.award_type[i] = jd["List"][i]["type"].ToString(); }
                                catch { DGameData.Instance.award_type[i] = "错误奖"; }
                                try { DGameData.Instance.award_money[i] = jd["List"][i]["money"].ToString(); }
                                catch { DGameData.Instance.award_money[i] = "错误分数"; }
                            }
                        }
                    }
                    catch
                    {
                        HGameData.Instance.IsNetworkError = true;
                    }
                }
            }
        }
        catch
        {

        }
    }



    public void Button_Stop()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        ClosePanel();
        isStopPanel = true;
    }

    /// <summary>
    /// 临时押分
    /// </summary>
    private int ChargeNum = 0;

    /// <summary>
    /// 押分按钮
    /// </summary>
    public void Button_Fill()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        if (DGameData.Instance.Fraction <= 0)
        {
            Button_UpDown();
            return;
        }
        else
        {
            if (DGameData.Instance.BiType == 0)
            {
                Debug.Log("执行4");
                DGameData.Instance.BiType = 1;
                DGameData.Instance.FlopNum = 1;
            }
            if (DGameData.Instance.BiType == 3)
                return;
            if (DGameData.Instance.FlopNum != 1)
                return;

            ChargeNum += 100;
            if (ChargeNum > 1000)
            {
                ChargeNum -= 100;
                return;
            }
            DGameData.Instance.ChargeNum = ChargeNum;

        }

    }

    /// <summary>
    /// 是否开始统计赢的分数
    /// </summary>
    private bool IsWin = false;

    /// <summary>
    /// 得分按钮
    /// </summary>
    public void Button_Win()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        IsWin = true;   //开始得分统计
    }

    /// <summary>
    /// 自动按钮
    /// </summary>
    public void OnAutomaticButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        if (DGameData.Instance.Fraction != 0)
            DGameData.Instance.IsAutomatic = !DGameData.Instance.IsAutomatic;
        else
            DGameData.Instance.IsAutomatic = false;
    }


    /// <summary>
    /// 押大按钮
    /// </summary>
    public void OnBigButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        string b = Onstr_result(DGameData.Instance.str_result);
        url = "http://47.106.66.89:81/pj-api/room-doubly" + "?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID + "&game_id=" + "12" + "&periods_id=" + DGameData.Instance.Periods_ID + "&doubly_type=" + doubly_type + "&doubly_data=" + "2" + "&str_result=" + b;
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }


    /// <summary>
    /// 押小按钮
    /// </summary>
    public void OnSmallButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        string b = Onstr_result(DGameData.Instance.str_result);
        url = "http://47.106.66.89:81/pj-api/room-doubly" + "?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID + "&game_id=" + "12" + "&periods_id=" + DGameData.Instance.Periods_ID + "&doubly_type=" + doubly_type + "&doubly_data=" + "1" + "&str_result=" + b;
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }

    string Onstr_result(string[] data)
    {
        Audio.clip = Clip[0];
        Audio.Play();
        string b = "";
        for (int i = 0; i < data.Length; i++)
        {
            b += data[i].TrimEnd('-');
            b += ',';
        }
        b = b.TrimEnd(',');
        return b;
    }

    /// <summary>
    /// 双比倍按钮
    /// </summary>
    public void OnShuangBiButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        DGameData.Instance.IsBiBei = false;
        doubly_type = 3;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        DGameData.Instance.Fraction -= DGameData.Instance.WinMoney;
        DGameData.Instance.WinMoney = DGameData.Instance.WinMoney * 2;
        BiPromptText.text = "双 比 倍";
    }

    /// <summary>
    /// 半比倍按钮
    /// </summary>
    public void OnBanBiButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        DGameData.Instance.IsBiBei = false;
        doubly_type = 1;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        DGameData.Instance.WinMoney = DGameData.Instance.WinMoney / 2;
        DGameData.Instance.Fraction += DGameData.Instance.WinMoney;
        BiPromptText.text = "半 比 倍";
    }

    /// <summary>
    /// 比倍按钮
    /// </summary>
    public void OnBiButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        DGameData.Instance.IsBiBei = false;
        doubly_type = 2;
        //显示比倍界面
        url = "http://47.106.66.89:81/pj-api/doubly";
        StartCoroutine(SendBiBei(url));
        BiPromptText.text = "比 倍";
    }

    private int doubly_type = 1;//比倍类型   1-->半比倍  2----> 一倍   3  ---->  两倍  保存
    private int doubly_data = 1;//比倍结果   1  ---> 小   2  ---->  大
    public Text BiPromptText;

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

                    IsBiBei = true;
                    DGameData.Instance.BiType = 2;
                    Debug.Log("执行3");
                    FillButton.SetActive(false);
                    BiButton.SetActive(false);
                    BanBiButton.SetActive(false);
                    ShuangBiButton.SetActive(false);
                    SmallButton.SetActive(true);
                    BigButton.SetActive(true);
                    StartButton.SetActive(false);
                    WinButton.SetActive(true);
                    DGameData.Instance.IsColumnBoard = true;

                    for (int i = 0; i < 6; i++)
                    {
                        DGameData.Instance.str_result[i] = jd["result"][i].ToString();
                        Debug.Log("右边的牌为第" + (i + 1) + "张：" + DGameData.Instance.str_result[i]);
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Json解析错误");
            DGameData.Instance.IsNetworkError = true;
        }
    }

    /// <summary>
    /// 得分按钮
    /// </summary>
    public void OnWinButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        IsWin = true;   //开始得分统计
    }

    /// <summary>
    /// 开始按钮
    /// </summary>
    public void Button_Start()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        if (DGameData.Instance.IsFlopCard || IsWin)
            return;
        if (DGameData.Instance.Fraction <= 0)
        {
            Button_UpDown();
            return;
        }
        ClosePanel();
        switch (DGameData.Instance.FlopNum)
        {
            case 1:
                //第一次翻牌
                if (DGameData.Instance.Fraction != 0)
                {
                    if (DGameData.Instance.Fraction > 100)
                    {
                        if (ChargeNum == 0)
                            DGameData.Instance.ChargeNum = 100;
                        else
                            DGameData.Instance.ChargeNum = ChargeNum;
                    }
                    else
                    {
                        DGameData.Instance.ChargeNum = DGameData.Instance.Fraction;
                    }
                    url = "http://47.106.66.89:81/pj-api/dzb-find-walking-algorithm?room_id=" + DGameData.Instance.Room_ID + "&game_id=12&num=" + DGameData.Instance.ChargeNum + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&seat_number=" + DGameData.Instance.TriggerNum;
                    DGameData.Instance.FlopNum = 2;
                    StartOdd();
                    DGameData.Instance.HoldData = new string[5];
                    for (int i = 0; i < 5; i++)
                    {
                        HOLD[i].SetActive(false);
                    }
                    DGameData.Instance.BiType = 1;
                    DGameData.Instance.IsStart = true;
                    Debug.Log(url);
                    Debug.Log("第一次开牌");
                    StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
                }
                else
                {
                    Debug.Log("用户余额不足");
                }

                break;
            case 2:
                //第二次翻牌
                StartOdd();
                DGameData.Instance.FlopNum = 3;
                DGameData.Instance.IsStart = true;
                url = "http://47.106.66.89:81/pj-api/dzb-two-walking-algorithm?room_id=" + DGameData.Instance.Room_ID + "&game_id=12&num=" + DGameData.Instance.ChargeNum + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&periods_id=" + DGameData.Instance.Periods_ID + "&retain=" + OnRetain(DGameData.Instance.HoldData);
                Debug.Log("第二次开牌");
                Debug.Log(url);
                StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
                break;
        }
    }

    /// <summary>
    /// 初始化倍率UI
    /// </summary>
    void StartOdd()
    {
        DGameData.Instance.IsWinEffect = false;
        DGameData.Instance.CardTitle = "";
        for (int i = 0; i < 10; i++)
        {
            OddListObj[i].SetActive(true);
        }
        winEffectIng.SetActive(false);
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
    ///网络按钮
    /// </summary>
    public void OnNetWorkButton()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/trigger-ent?game_id=12&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + DGameData.Instance.Room_ID + "&trigger_id=" + DGameData.Instance.TriggerID;
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 留机（暂停）面板  确定
    /// </summary>
    public void StopPanel_Yes()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        url = "http://47.106.66.89:81/pj-api/stay-machine?game_id=12&room_id=" + DGameData.Instance.Room_ID + "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&trigger_id=" + DGameData.Instance.TriggerID;

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
                            DGameData.Instance.EnterTheScene("DGame_PTCChooseJiQi");
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
    /// 关闭全部面板
    /// </summary>
    public void ClosePanel()
    {
        Audio.clip = Clip[0];
        Audio.Play();
        isQuitPanel = false;
        isStopPanel = false;
        isUpDownPanel = false;
        isRecordPanel = false;
        isRankingPanel = false;
    }

    public GameObject PromptText;

    void OnUI()
    {
        switch (DGameData.Instance.FlopNum)
        {
            case 1:
                PromptText.GetComponent<Text>().text = "<color=#FFFF00>押 注 </color>或 <color=#458b00>开 始</color>";
                break;
            case 2:
                PromptText.GetComponent<Text>().text = "<color=#458b00>保 留 </color>或 <color=#458b00>开 牌</color>";
                break;
            case 3:
                if (DGameData.Instance.WinMoney != 0)
                    PromptText.GetComponent<Text>().text = "<color=#458b00>得 分 </color>或 <color=#458b00>比 倍</color>";
                else
                    PromptText.GetComponent<Text>().text = "<color=#458b00>保 留 </color>或 <color=#458b00>开 牌</color>";
                break;
        }

        if (isQuitPanel) { QuitPanel.SetActive(true); } else { QuitPanel.SetActive(false); }
        if (isStopPanel) { StopPanel.SetActive(true); } else { StopPanel.SetActive(false); }
        if (isUpDownPanel) { UpDownPanel.SetActive(true); } else { UpDownPanel.SetActive(false); }
        if (isRecordPanel) { RecordPanel.SetActive(true); } else { RecordPanel.SetActive(false); }
        if (isRankingPanel) { RankingPanel.SetActive(true); } else { RankingPanel.SetActive(false); }

        for (int i = 0; i < 10; i++)
        {
            if (DGameData.Instance.ChargeNum != 0)
                OddList[i].text = (DGameData.Instance.OddList[i] * DGameData.Instance.ChargeNum).ToString();
            else
                OddList[i].text = DGameData.Instance.OddList[i].ToString();
        }

        AllScore.text = LoginInfo.Instance().mylogindata.ALLScroce;
        Fraction.text = DGameData.Instance.Fraction.ToString(); Fraction2.text = DGameData.Instance.Fraction.ToString();
        WinMoney.text = DGameData.Instance.WinMoney.ToString();
        BET.text = DGameData.Instance.ChargeNum.ToString();

        if (DGameData.Instance.IsWin)
        {
            if (DGameData.Instance.BiType == 2)
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
            StartButton.SetActive(false);
            WinButton.SetActive(true);
        }
        else
        {
            WinButton.SetActive(false);
            StartButton.SetActive(true);
            FillButton.SetActive(true);
            SmallButton.SetActive(true);
            BigButton.SetActive(true);
            BiButton.SetActive(false);
            BanBiButton.SetActive(false);
            ShuangBiButton.SetActive(false);
        }
    }

    public GameObject FillButton;
    public GameObject SmallButton;
    public GameObject BigButton;
    public GameObject BiButton;
    public GameObject BanBiButton;
    public GameObject WinButton;
    public GameObject ShuangBiButton;
    public GameObject StartButton;

}