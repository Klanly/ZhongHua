using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipScript : MonoBehaviour
{
    public static ChipScript ins;

    public float gapTime = 0.5f; //闪烁的间隔时间，在Unity中修改
    private float temp;

    public bool isflase = true;

    bool IsDisplay = true;
    /// <summary>
    /// 红黄绿动物
    /// </summary>
    public GameObject[] objs;

    public GameObject HZX;

    public static NewTcpNet net;

    //private string StrZoomName = string.Empty;

    //private string StrHZXName = string.Empty;

    public GameObject UserName;
    /// <summary>
    /// 押注
    /// </summary>
    public UIButton[] btns;

    /// <summary>
    /// 动画头像按钮
    /// </summary>
    public GameObject[] ChipNumberBtn;

    //提示面板
    public GameObject TipPanel;

    // public GameObject BetBtn;
    /// <summary>
    /// 动画头像面板
    /// </summary>
    public GameObject BetPanel;
    /// <summary>
    /// 压分我
    /// </summary>
    /// 
    private string str = "20";
    private int p1;
    int waitime;
    //各种动物
    public GameObject Animals;
    //存取分数按钮
    public GameObject CunQu;
    //

    public GameObject yazhu;
    /// <summary>
    /// 结果页面
    /// </summary>
    public GameObject Result;
    //安卓级打印
    // public UILabel uilabel;
    //得分
    public UILabel Score;
    //动物得分
    //  public UILabel dongwuScore;
    //庄闲和得分
    //  public UILabel ZHXScore;
    //JX奖励
    public UILabel JXLabel;
    /// <summary>
    /// 设置精灵的布尔值
    /// </summary>
    private bool SetSpriteBool = true;

    //用户余额
    public GameObject Credit;
    //用户分数
    public GameObject Fraction;
    //彩金
    public UILabel Caijing;
    ////提示面板
    //public GameObject TipPanel;

    GameObject ZoomGo;
    GameObject XZH;
    string[] StrName;

    //房间记录
    public GameObject roomRecord;
    //
    //  private List<GameObject> Go = new List<GameObject>();
    //按钮
    public List<GameObject> list = new List<GameObject>();
    /// <summary>
    /// 初始化
    /// </summary>
    void Awake()
    {
        ins = this;
        Application.runInBackground = true;
        UserName.GetComponent<UILabel>().text = LoginInfo.Instance().mylogindata.username;


        //    StartCoroutine(ShowImage());

        for (int i = 0; i < ChipNumberBtn.Length; i++)
        {
            //ChipNumberBtn[i].transform.GetComponent<B>().onClick.add
            UIEventListener.Get(ChipNumberBtn[i]).onClick = OnClick;
        }

        for (int i = 0; i < btns.Length; i++)
        {
            UIEventListener.Get(btns[i].gameObject).onClick = OnChipClick;
        }
        // SetSprite(false);
    }
    void Start()
    {
        Defalut();
        //for (int j = 0; j < ChipScript.ins.objs.Length - 1; j++)
        //{
        //    for (int k = 0; k < 3; k++)
        //    {
        //        list.Add(ChipScript.ins.objs[j].transform.GetChild(k).gameObject);
        //    }
        //    Debug.Log(ChipScript.ins.objs[j].transform.name);

        //}


    }
    public void Defalut()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                objs[i].transform.GetChild(j).transform.GetChild(2).GetComponent<UILabel>().text = "0";
            }
        }
        for (int i = 0; i < 3; i++)
        {
            HZX.transform.GetChild(i).GetChild(2).transform.GetComponent<UILabel>().text = "0";
        }

    }
    /// <summary>
    /// 初始化赔率数据
    /// </summary>
    public void DefalutNumber(string str, int i, int j, string str1)
    {
        //    objs[j].transform.GetChild(i).transform.GetChild(2).GetComponent<UILabel>().text = "0";

        objs[j].transform.GetChild(i).transform.GetChild(1).GetComponent<UILabel>().text = str;
        objs[j].transform.GetChild(i).transform.GetChild(3).GetComponent<UILabel>().text = str1;

    }

    //上一次记录
    public void LastDefalutNumber(string str, int i, int j, string str2, string str1)
    {
        objs[j].transform.GetChild(i).transform.GetChild(2).GetComponent<UILabel>().text = str2;

        objs[j].transform.GetChild(i).transform.GetChild(1).GetComponent<UILabel>().text = str;
        objs[j].transform.GetChild(i).transform.GetChild(3).GetComponent<UILabel>().text = str1;

    }
    public void LastDeFalutHZX(string str, int i, string str1, string str2)
    {
        HZX.transform.GetChild(i).transform.GetChild(2).GetComponent<UILabel>().text = str1;
        HZX.transform.GetChild(i).transform.GetChild(1).GetComponent<UILabel>().text = str;
        HZX.transform.GetChild(i).transform.GetChild(3).GetComponent<UILabel>().text = str2;
    }
    public void DeFalutHZX(string str, int i, string str1)
    {
        //     HZX.transform.GetChild(i).transform.GetChild(2).GetComponent<UILabel>().text = "0";
        HZX.transform.GetChild(i).transform.GetChild(1).GetComponent<UILabel>().text = str;
        HZX.transform.GetChild(i).transform.GetChild(3).GetComponent<UILabel>().text = str1;
    }

    /// <summary>
    /// 点击事件下注
    /// </summary>
    /// <param name="obj"></param>
    public void OnClick(GameObject obj)
    {
        if (bonuUI.ins.time > 0 && ClickEvent.ins.Game == false)
        {
            //  SetBet(str, obj);
            //Debug.Log("11");
            //  CunQu.SetActive(true);

            if (int.Parse(Fraction.transform.GetComponent<UILabel>().text) < int.Parse(str))
            {
                Tip("你的分数余额不足\r\n请充值继续");
            }
            else
            {
                ClickEvent.ins.XiaZhu = "http://47.106.66.89:81/sd-api/sd-bets?room_id=" + PlayerData.ins.room_id + "&user_id=" + PlayerData.ins.userid + "&num=" + str + "&sign_num=" + obj.transform.GetComponent<YAbei>().num + "&id=" + obj.transform.GetComponent<YAbei>().id;
                StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.XiaZhu, 7));
            }

            //  StartCoroutine(ClickEvent.ins.GoForm(ClickEvent.ins.EnterRoomUrl));
        }
        else
        {
            Tip("非押注时间");
        }

    }
    //提示面板显示隐藏
    public void Tip(string str)
    {


        TipPanel.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = str;
        TipPanel.transform.GetChild(0).GetComponent<TweenPosition>().ResetToBeginning();
        TipPanel.transform.localPosition = new Vector3(0, 0, 0);
        TipPanel.SetActive(true);
        if (str != "请等待下一局.")
        {
            TipPanel.transform.GetChild(0).GetComponent<TweenPosition>().enabled = true;


        }
        else
        {
            Debug.Log(TipPanel.transform.GetChild(0).GetChild(0).name);
            TipPanel.transform.GetChild(0).GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
            TipPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        }

    }

    //登录
    public void LoginUser()
    {
        // StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.LoginUrl, 9));

    }
    //得到用户信息
    public void ReceivedUserInfo()
    {
        if (PlayerData.ins.userid == string.Empty || PlayerData.ins.unionuid == string.Empty)
        {
            Tip("传送数据不完整" + "\r\n" + "登录接口未获取");
        }
        else
        {
            ClickEvent.ins.UserInfoUrl = "http://3d-web.weiec4.cn/api/userinfo?user_id=" + PlayerData.ins.userid + "&unionuid=" + PlayerData.ins.unionuid;
            StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.UserInfoUrl, 2));
        }

    }

    //上分
    public void AddScore()
    {
        ClickEvent.ins.AddScore = "http://47.106.66.89:81/sd-api/top-score?user_id=" + PlayerData.ins.userid + "&room_id=" + PlayerData.ins.room_id + "&game_id=" + PlayerData.ins.game_id;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.AddScore, 3));
    }
    //下分
    public void XiaFen()
    {
        ClickEvent.ins.xiaScore = "http://47.106.66.89:81/sd-api/lower-score?user_id=" + PlayerData.ins.userid + "&room_id=" + PlayerData.ins.room_id + "&game_id=" + PlayerData.ins.game_id;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.xiaScore, 4));
    }
    //取消押分
    public void CanCleYaFen()
    {
        ClickEvent.ins.CancleYaFen = "http://47.106.66.89:81/sd-api/sd-cancel-all?room_id=" + PlayerData.ins.room_id + "&user_id=" + PlayerData.ins.userid;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.CancleYaFen, 5));
    }
    //续压
    public void ContinueScore()
    {
        if (PlayerData.ins.drop_date == string.Empty)
        {
            Tip("未押注，不能续压");
        }
        else
        {
            ClickEvent.ins.Continue = "http://47.106.66.89:81/sd-api/continuous-pressure?room_id=" + PlayerData.ins.room_id + "&user_id=" + PlayerData.ins.userid + "&game_id=" + PlayerData.ins.game_id + "&drop_date=" + PlayerData.ins.drop_date;
            StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.Continue, 6));
        }


    }
    //上一次记录
    public void Recored()
    {

        ClickEvent.ins.LastRecord = "http://47.106.66.89:81/sd-api/laet-time-record?room_id=" + PlayerData.ins.room_id + "&user_id=" + PlayerData.ins.userid + "&game_id=" + PlayerData.ins.game_id + "&drop_date=" + PlayerData.ins.drop_date;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.LastRecord, 11));


    }
    //获取游戏ID
    public void ReceivedGameID()
    {
        //     StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.GameIDUrl,8));
    }

    //房间记录
    public void RoomRecord()
    {
        ClickEvent.ins.roomRecord = "http://47.106.66.89:81/sd-api/now-room-date?" + "game_id=" + PlayerData.ins.game_id + "&room_id=" + PlayerData.ins.room_id;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.roomRecord, 10));
    }

    //中奖情况
    public void GetWinning()
    {
        ClickEvent.ins.Winning = "http://47.106.66.89:81/sd-api/user-win-and-lose?room_id=" + PlayerData.ins.room_id + "&user_id=" + PlayerData.ins.userid + "&game_id=" + PlayerData.ins.game_id + "&drop_date=" + PlayerData.ins.drop_date;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.Winning, 12));
    }
    public void SetDouble(string[] str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            for (int j = 0; j < ChipNumberBtn.Length; j++)
            {
                ReceiveNum(str[i], 1, ChipNumberBtn[j]);
            }
        }

    }

    /// <summary>
    /// 自己押注
    /// </summary>
    /// <param name="str"></param>
    /// <param name="obj"></param>
    public void SetBet(string str, GameObject obj)
    {
        string strnum = (int.Parse(obj.transform.GetChild(2).GetComponent<UILabel>().text) + int.Parse(str)).ToString();
        ReceiveNum(strnum, 2, obj);
    }
    /// <summary>
    /// 押注总数
    /// </summary>
    /// <param name="str"></param>
    /// <param name="obj"></param>
    public void SetTotal_Bet(string str, GameObject obj)
    {
        ReceiveNum(str, 3, obj);
    }
    //得到押注总数
    public void ReceiveNum(string str, int num, GameObject obj)
    {
        obj.transform.GetChild(num).GetComponent<UILabel>().text = str;
    }
    /// <summary>
    /// 押注的图标
    /// </summary>
    /// <param name="obj"></param>

    void OnChipClick(GameObject obj)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        obj.transform.GetChild(1).gameObject.SetActive(true);
        if (obj.name == "chip01")
        {
            str = "20";
        }
        else if (obj.name == "chip02")
        {
            str = "50";
        }
        else
        {
            str = "100";
        }
    }
    /// <summary>
    /// 押注页面显示
    /// </summary>
    public void MoveToChip(GameObject go)
    {

        GetWinning();
        ClickEvent.ins.IsStart = true;

        //Debug.Log ("3345");


        Debug.Log(PlayerData.ins.ZoomName);
        for (int i = 0; i < objs.Length; i++)
        {

            if (PlayerData.ins.ZoomName.Substring(0, 1) == objs[i].name.Substring(0, 1).ToString())
            {
                StrName = PlayerData.ins.ZoomName.Split('_');
                // Debug.Log(str[1].ToUpper());

                for (int j = 0; j < 3; j++)
                {

                    if (StrName[1].ToUpper() == objs[i].transform.GetChild(j).name.ToUpper())
                    {
                        ZoomGo = objs[i].transform.GetChild(j).gameObject;

                        break;
                    }

                }


            }
        }

        for (int i = 0; i < HZX.transform.childCount; i++)
        {
            if (PlayerData.ins.InterZHX == HZX.transform.GetChild(i).name.Substring(0, 1).ToString())
            {
                XZH = HZX.transform.GetChild(i).gameObject;
                break;
            }
        }
        Recored();
        StartCoroutine(ShowImage());
        BetPanel.transform.GetComponent<TweenPosition>().enabled = true;
        SetBtnPanelShow();


    }

    public void GetAllZoom()
    {


    }



    public void Btn_OmClick(GameObject go)
    {

        go.transform.GetChild(0).gameObject.SetActive(false);
    }
  
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isshow"></param>
    public void SetBtnPanelShow()
    {
        yazhu.transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// 闪耀次数
    /// </summary>
    /// <param name="obj"></param>
    public IEnumerator ShowImage()
    {
      
        int i = 0;
        while (true)
        {
            temp += Time.deltaTime;
            if (temp >= gapTime && i<6)
            {
                if (IsDisplay)
                {
                 
                    if (PlayerData.ins.win_type == "1" || PlayerData.ins.win_type == "2")
                    {
                       
                        ZoomGo.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else if (PlayerData.ins.win_type == "4")
                    {


                        if (StrName[1].ToUpper() == "RED")
                        {
                            p1 = 0;
                        }
                        else if (StrName[1].ToUpper() == "YELLOW")
                        {
                            p1 = 2;

                        }
                        else if (StrName[1].ToUpper() == "GREEN")
                        {
                            p1 = 1;
                        }
                        else
                        {


                        }
                       
                        objs[0].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(true);
                        objs[1].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(true);
                        objs[2].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(true);
                        objs[3].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(true);
                       
                    }
                    else if (PlayerData.ins.win_type == "3")
                    {

                        if (StrName[0].ToUpper() == "L")
                        {
                            p1 = 0;
                        }
                        else if (StrName[0].ToUpper() == "P")
                        {
                            p1 = 1;

                        }
                        else if (StrName[0].ToUpper() == "M")
                        {
                            p1 = 2;
                        }
                        else if (StrName[0].ToUpper() == "R")
                        {
                            p1 = 3;

                        }
                        objs[p1].transform.GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                        objs[p1].transform.GetChild(1).transform.GetChild(4).gameObject.SetActive(true);
                        objs[p1].transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(true);

                    }
                    XZH.transform.GetChild(4).gameObject.SetActive(true);

                    IsDisplay = false;
                    temp = 0;
                    i++;
                }
                else
                {
              
                    if (PlayerData.ins.win_type == "1" || PlayerData.ins.win_type == "2")
                    {
                        ZoomGo.transform.GetChild(4).gameObject.SetActive(false);
                    }
                    else if (PlayerData.ins.win_type == "4")
                    {


                        if (StrName[1].ToUpper() == "RED")
                        {
                            p1 = 0;
                        }
                        else if (StrName[1].ToUpper() == "YELLOW")
                        {
                            p1 = 2;

                        }
                        else if (StrName[1].ToUpper() == "GREEN")
                        {
                            p1 = 1;
                        }
                        else
                        {


                        }
                        objs[0].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(false);

                        objs[1].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(false);

                        objs[2].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(false);

                        objs[3].transform.GetChild(p1).transform.GetChild(4).gameObject.SetActive(false);


                    }
                    else if (PlayerData.ins.win_type == "3")
                    {
                        if (StrName[0].ToUpper() == "L")
                        {
                            p1 = 0;
                        }
                        else if (StrName[0].ToUpper() == "P")
                        {
                            p1 = 1;

                        }
                        else if (StrName[0].ToUpper() == "M")
                        {
                            p1 = 2;
                        }
                        else if (StrName[0].ToUpper() == "R")
                        {
                            p1 = 3;

                        }

                        objs[p1].transform.GetChild(0).transform.GetChild(4).gameObject.SetActive(false);
                        objs[p1].transform.GetChild(1).transform.GetChild(4).gameObject.SetActive(false);
                        objs[p1].transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(false);
                    }
                    XZH.transform.GetChild(4).gameObject.SetActive(false);

                    IsDisplay = true;
                    temp = 0;
                }
            }
            if (i >= 6)
            {

              //  ChipScript.ins.SetSprite(false);
                SetResult(true);

                UIManager.ins.GetZoomName(PlayerData.ins.ZoomName);
                UIManager.ins.SetHeadZHX(PlayerData.ins.InterZHX);
           
                //if (int.Parse(PlayerData.ins.DataTime) >= 3)
                //{
                //    waitime = int.Parse(PlayerData.ins.DataTime);
                //    yield return new WaitForSeconds(waitime - 1);
                //}
                //else
                //{
                  
                //}
                yield return new WaitForSeconds(2);
               
                SetResult(false);
                ZPan.ins.ResetUI();
                ZPan.ins.SetShow();
                //   UIManager.ins.SetHeadShow(false, "");
                ZPan.ins.ResetZoomPosition();
                ClickEvent.ins.IsStart = false;
                //                Go.Clear();
            
                break;
            }

            yield return null;
        }

    }

    //Sprite 按钮
    public void SetResult(bool isshow)
    {
        Result.SetActive(isshow);

    }

    /// <summary>
    /// 设置赔率面板的显示隐藏
    /// </summary>
    /// <param name="isShow"></param>

    public void SetSprite(bool isShow)
    {


        for (int i = 0; i < objs.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                objs[i].transform.GetChild(j).transform.GetChild(4).gameObject.SetActive(isShow);
            }

        }
        for (int i = 0; i < HZX.transform.childCount; i++)
        {

            HZX.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(isShow);


        }
        // SetSpriteBool = isShow;
    }



    public string GetName(string str)
    {
        return str;
    }
}
