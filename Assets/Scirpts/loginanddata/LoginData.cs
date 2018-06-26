using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LoginData
{
    //用户信息
    public string user_id;//用户ID
    public string token;//登录时间戳
    public string username;//用户名
    public string ALLScroce;//玩家总值
    public string login_ip;//登录IP
    public string telephone;//电话数据 暂无用处
    public string status;//暂无用处
    public string userStatus;//暂无用处
    public int coindown;//当前的下注值
    public int room_id;//房间ID
    public int choosegame;//选择的游戏ID
    public string roomlitmit;//限红
    public string roomcount;//最小压分
    public string seating;//座位
    public string version;//版本号
    public string iosversion;
    public string gameModle; //游戏模式
    public string dropContent; // 期号
    public string farURL;       //远
    public string nearURL;      //近
    public string fullURL;      //全屏
    public string gameType;

    public static bool IsConnect;       //连接
    public static bool IsLogin;         //登录
    public static bool IsOnPing;        //验证是否启动
    public static float OverTime;       //检测时间

    public static bool IsVideoPlay;     //当前是否处于播放录像中


    public bool isOpenError = true;

    public bool isFull = true;
    public VideoSurface forDel;
    public string isAnchor; //0是观众 1是主播

    public List<int> game_id;//游戏类型ID
    public List<string> snid;
    public string servicesInfo;

    public bool picOrWord = true; // 开奖结果是图片还是文字  true为图片  false为文字
    //public bool isAbnormal_account;

    //---------游戏专用API接口---------通过构造时注入不允许编译时修改-----
    public readonly string URL;                                         // 1
    public readonly string loginAPI;                                    // 2
    public readonly string raganstionAPI;                               // 3
    public readonly string gamelistAPI;                                 // 4
    public readonly string roomlistAPI;                                 // 5
    public readonly string roominstartAPI;                              // 6
    public readonly string roominendAPI;                                // 7
    public readonly string counwtAPI;                                   // 8
    public readonly string winlistAPI;                                  // 9
    public readonly string gameinfoPollingAPI;                          // 10
    public readonly string betinAPI;                                    // 11
    public readonly string wingetforinterfroAPI;                        // 12
    public readonly string caneldownAPI;                                // 13
    public readonly string hallaliveAPI;                                // 14
    public readonly string VersioninfoAPI;                              // 15
    public readonly string changepasswordAPI;                           // 16
    public readonly string usercoininhistoryAPI;                        // 17
    public readonly string userCut;                                     // 18
    public readonly string userCutSend;                                 // 19
    public readonly string newInit;                                     // 20
    public readonly string BetDown_mpzzs;                               // 21
    public readonly string winHistory; //历史开奖信息                   // 22
    public readonly string playerOnLine;                                // 23
    public readonly string playerHistory; //玩家历史战绩                // 24
    public readonly string winInfo;                                     // 25
    public readonly string betCancel_mpzzs;                             // 26
    public readonly string betDown_bl;                                  // 27
    public readonly string betCancel_bl;                                // 28
    public readonly string betDown_ds;                                  // 29
    public readonly string betCancel_ds;                                // 30
    public readonly string betDown_elb;                                 // 31
    public readonly string betCancel_elb;                               // 32
    public readonly string betDown_td;                                  // 33
    public readonly string betCancel_td;                                // 34
    public readonly string betCancel_lh;                                // 35
    public readonly string betDown_lh;                                  // 36
    public readonly string liveVideo;                                   // 37
    public readonly string betDown_xwy;                                 // 38
    public readonly string betCancel_xwy;                               // 39
    public readonly string betDown_dxb;                                 // 40
    public readonly string betCancel_dxb;                               // 41
    public readonly string services;                                    // 42
 






    public LoginData
        (
          string url,
          string loginapi,
          string raganstionapi,
          string gamelistapi,
          string roomlistapi,
          string roominstart,
          string roominend,
          string counwtdown,
          string winlist,
          string gameinfoPolling,
          string betin,
          string wingetforinterform,
          string caneldown,
          string hallalive,
          string version,
          string changepassword,
          string coininhistory,
          string usercut,
          string usercutSend,
          string NewInit,
          string newBetDown,
          string winHistory,
          string playerOnLine,
          string playerHistory,
          string winInfo,
          string betCancel,
          string betDown_bl,
          string betCancel_bl,
          string betDown_ds,
          string betCancel_ds,
          string betDown_elb,
          string betCancel_elb,
          string betDown_td,
          string betCancel_td,
          string betDown_lh,
          string betCancel_lh,
          string liveVideo,
          string betDown_xwy,
          string betCancel_xwy,
          string betDown_dxb,
          string betCancel_dxb,
          string services


        )
    {
        URL = url;
        loginAPI = loginapi;
        raganstionAPI = raganstionapi;
        gamelistAPI = gamelistapi;
        roomlistAPI = roomlistapi;
        roominstartAPI = roominstart;
        roominendAPI = roominend;
        counwtAPI = counwtdown;
        winlistAPI = winlist;
        gameinfoPollingAPI = gameinfoPolling;
        betinAPI = betin;
        wingetforinterfroAPI = wingetforinterform;
        caneldownAPI = caneldown;
        hallaliveAPI = hallalive;
        VersioninfoAPI = version;
        changepasswordAPI = changepassword;
        usercoininhistoryAPI = coininhistory;
        userCut = usercut;
        userCutSend = usercutSend;
        this.newInit = NewInit;
        this.BetDown_mpzzs = newBetDown;
        this.winHistory = winHistory;
        this.playerOnLine = playerOnLine;
        this.playerHistory = playerHistory;
        this.winInfo = winInfo;
        this.betCancel_mpzzs = betCancel;
        this.betDown_bl = betDown_bl;
        this.betCancel_bl = betCancel_bl;
        this.betDown_ds = betDown_ds;
        this.betCancel_ds = betCancel_ds;
        this.betDown_elb = betDown_elb;
        this.betCancel_elb = betCancel_elb;
        this.betDown_td = betDown_td;
        this.betCancel_td = betCancel_td;
        this.betDown_lh = betDown_lh;
        this.betCancel_lh = betCancel_lh;
        this.liveVideo = liveVideo;
        this.betDown_xwy = betDown_xwy;
        this.betCancel_xwy = betCancel_xwy;
        this.betDown_dxb = betDown_dxb;
        this.betCancel_dxb = betCancel_dxb;
        this.services = services;
    }
}


