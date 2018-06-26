using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HGameData
{

    private static volatile HGameData instance;
    private static readonly object obj = new object();

    private HGameData() { }
    public static HGameData Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new HGameData();
            }
            return instance;
        }
    }

    /// <summary>
    /// 0.翻牌模式  1.竞猜大小模式
    /// </summary>
    public int BoardType
    {
        get
        {
            return _boardType;
        }

        set
        {
            _boardType = value;
        }
    }

    /// <summary>
    /// 临时会话密令
    /// </summary>
    public string Unionuid { get; set; }

    /// <summary>
    /// 账号状态 1：正常 其余则禁用
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 游戏ID 列表
    /// </summary>
    private string[] gameID = new string[3];

    /// <summary>
    /// 游戏是否打开 0：不开启  1：正常
    /// </summary>
    private string[] gameOpen = new string[3];

    private string[] _roomID = new string[3];

    /// <summary>
    /// 进入的房间ID
    /// </summary>
    public string RoomTitleID { get; set; }

    /// <summary>
    /// 房间名称 列表
    /// </summary>
    private string[] roomName = new string[3];

    /// <summary>
    /// 房间金币转换比列 列表
    /// </summary>
    private string[] roomRatio = new string[3];

    /// <summary>
    /// 房间类型名称
    /// </summary>
    public string RoomTitle { get; set; }

    /// <summary>
    /// 当前房间机子的总数量（有多少个座位）
    /// </summary>
    public string TriggerTotal { get; set; }

    /// <summary>
    /// 当前房间一共有多少人在玩
    /// </summary>
    public int TriggerNum { get; set; }

    /// <summary>
    /// 坐下来的玩家ID 列表
    /// </summary>
    private string[] tTriggerID = new string[51];

    /// <summary>
    /// 坐下来的玩家 是否留机 1留机  2不留机
    /// </summary>
    private string[] is_machine = new string[51];

    /// <summary>
    /// 坐下的玩家剩余留机时间
    /// </summary>
    private int[] time = new int[51];

    /// <summary>
    /// 自己进入的机子编号
    /// </summary>
    public string Seat_number { get; set; }

    /// <summary>
    /// 机子的id
    /// </summary>
    public string Trigger_id { get; set; }

    /// <summary>
    /// 坐下来的玩家名字 列表
    /// </summary>
    private string[] triggerName = new string[51];

    /// <summary>
    /// 坐下来的玩家余额 列表
    /// </summary>
    private string[] triggerGold = new string[51];

    /// <summary>
    /// 坐下来的玩家座位编号 列表
    /// </summary>
    private string[] triggerSeatNum = new string[51];

    /// <summary>
    /// 房间ID 列表
    /// </summary>
    public string[] RoomID
    {
        get
        {
            return _roomID;
        }

        set
        {
            _roomID = value;
        }
    }
    /// <summary>
    /// 坐下来的玩家座位编号 列表
    /// </summary>
    public string[] TriggerSeatNum
    {
        get
        {
            return triggerSeatNum;
        }

        set
        {
            triggerSeatNum = value;
        }
    }

    /// <summary>
    /// 坐下来的玩家余额 列表
    /// </summary>
    public string[] TriggerGold
    {
        get
        {
            return triggerGold;
        }

        set
        {
            triggerGold = value;
        }
    }

    /// <summary>
    /// 坐下来的玩家名字 列表
    /// </summary>
    public string[] TriggerName
    {
        get
        {
            return triggerName;
        }

        set
        {
            triggerName = value;
        }
    }

    /// <summary>
    /// 坐下来的玩家ID 列表
    /// </summary>
    public string[] TriggerID
    {
        get
        {
            return tTriggerID;
        }

        set
        {
            tTriggerID = value;
        }
    }

    /// <summary>
    /// 房间名称 列表
    /// </summary>
    public string[] RoomName
    {
        get
        {
            return roomName;
        }

        set
        {
            roomName = value;
        }
    }

    /// <summary>
    /// 房间金币转换比列 列表
    /// </summary>
    public string[] RoomRatio
    {
        get
        {
            return roomRatio;
        }

        set
        {
            roomRatio = value;
        }
    }


    /// <summary>
    /// 游戏是否打开 0：不开启  1：正常
    /// </summary>
    public string[] GameOpen
    {
        get
        {
            return gameOpen;
        }

        set
        {
            gameOpen = value;
        }
    }

    /// <summary>
    /// 翻牌 牌子类型
    /// </summary>
    public string[] CardType
    {
        get
        {
            return cardType;
        }

        set
        {
            cardType = value;
        }
    }

    /// <summary>
    /// 让牌子初始化为背面
    /// </summary>
    public bool IsCardStart = true;


    /// <summary>
    /// 比倍的时候，牌子类型
    /// </summary>
    public string Result_Data { get; set; }

    /// <summary>
    /// 比倍结果 1  ---> 小   2  ---->  大
    /// </summary>
    public string Result_result { get; set; }

    private string[] cardType = new string[5];

    /// <summary>
    /// 是否翻牌
    /// </summary>
    public bool IsFlopCard = false;
    /// <summary>
    /// 用户分数
    /// </summary>
    public int Fraction;

    /// <summary>
    /// 中奖分数
    /// </summary>
    public int WinMoney;

    /// <summary>
    /// 是否显示得分按钮
    /// </summary>
    public bool IsWin = false;

    /// <summary>
    /// 倍率
    /// </summary>
    public string Odds { get; set; }

    /// <summary>
    /// 中奖特效显示
    /// </summary>
    public bool IsWinEffect = false;

    /// <summary>
    /// 中了什么奖 
    /// </summary>
    public string CardTitle { get; set; }

    /// <summary>
    /// 这次下注的会话id
    /// </summary>
    public string PeriodsID { get; set; }

    /// <summary>
    /// 是否保留牌
    /// </summary>
    public string[] HoldData = new string[5];

    /// <summary>
    /// 记录数据 0:五鬼  1:同花大顺 2:五梅  3:同花小顺  4:大四梅  5:小四梅  6:葫芦  7:同花  8:顺子  9:三条  10:两对  11:对子  12:总赢次数  13:总玩次数  14:中奖  15:总赢分数  16:总玩分数  17:游戏                 
    /// </summary>
    public string[] RecordData = new string[18];

    /// <summary>
    /// 0：对子倍率1   1：两对倍率2   2：三条倍率3   3：顺子倍率5   4：同花倍率7   5：葫芦倍率10   6：小四梅倍率40   7：大四梅倍率80   8：同花小顺倍率120   9：五梅倍率250   10：同花大顺倍率500   11：五鬼倍率1000
    /// </summary>
    public string[] Multiplying
    {
        get
        {
            return _multiplying;
        }

        set
        {
            _multiplying = value;
        }
    }

    /// <summary>
    /// 坐下来的玩家 是否留机 1留机  2不留机
    /// </summary>
    public string[] Is_machine
    {
        get
        {
            return is_machine;
        }

        set
        {
            is_machine = value;
        }
    }

    /// <summary>
    /// 坐下的玩家剩余留机时间
    /// </summary>
    public int[] Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    /// <summary>
    /// 翻牌次数 1：第一次翻牌  2：第二次翻牌
    /// </summary>
    public int FlopNum = 1;

    /// <summary>
    /// 比倍例牌显示
    /// </summary>
    public bool IsColumnBoard = false;
    /// <summary>
    /// 比倍大小翻牌
    /// </summary>
    public bool IsFlopColumnBoard = false;

    /// <summary>
    /// 右边六张牌的结果
    /// </summary>
    public string[] str_result = new string[6];

    /// <summary>
    /// 游戏模式 0：未开始模式  1：正常模式 2:比倍模式
    /// </summary>
    public int BiType = 0;

    //是否继续比倍
    public bool IsBiBei = false;

    /// <summary>
    /// 是否点击开始或翻牌
    /// </summary>
    public bool IsStart = false;

    /// <summary>
    /// 是否自动游戏
    /// </summary>
    public bool IsAutomatic = false;

    /// <summary>
    /// 用户押多少分
    /// </summary>
    public int ChargeNum = 0;

    /// <summary>
    /// 网络错误
    /// </summary>
    public bool IsNetworkError = false;

    /// <summary>
    /// 是否登录成功
    /// </summary>
    public bool IsSign = false;

    private int _boardType = 0;
    private int _registerErrorType = 0;
    private string _playerPasswork = "";
    private string _playerName = "用户名字";
    private string[] _multiplying = new string[12];
    private int _gameMusic = 0;
    private int _natureMusic = 0;


    /// <summary>
    /// 读取游戏数据（本地记录）
    /// </summary>
    public void ReadData()
    {
        //GameMusic = PlayerPrefs.GetInt("GameMusic");
        //NatureMusic = PlayerPrefs.GetInt("NatureMusic");
        //PlayerID = PlayerPrefs.GetString("PlayerID");
        //PlayerName = PlayerPrefs.GetString("PlayerName");
        //PlayerPasswork = PlayerPrefs.GetString("PlayerPasswork");
    }


    /// <summary>
    /// 保存游戏数据(本地记录)
    /// </summary>
    public void SaveData()
    {
        //PlayerPrefs.SetInt("GameMusic", GameMusic);
        //PlayerPrefs.SetInt("NatureMusic", NatureMusic);
        //PlayerPrefs.SetString("PlayerID", PlayerID);
        //PlayerPrefs.SetString("PlayerName", PlayerName);
        //PlayerPrefs.SetString("PlayerPasswork", PlayerPasswork);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 清楚数据（本地记录）
    /// </summary>
    public void ClearData()
    {
        //GameMusic = 0;
        //NatureMusic = 0;
        //PlayerID = "";
        //PlayerName = "";
        SaveData(); //保存数据
    }


    /// <summary>
    /// 进入指定场景
    /// </summary>
    /// <param name="Name">游戏场景名字</param>
    public void EnterTheScene(string Name)
    {
        SceneManager.LoadScene(Name);
    }
}
