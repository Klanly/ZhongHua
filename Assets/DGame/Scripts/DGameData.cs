using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DGameData
{
    private static volatile DGameData instance;

    private DGameData() { }

    public static DGameData Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new DGameData();
            }

            return instance;
        }
    }

    /// <summary>
    /// 代理房间列表 ID
    /// </summary>
    public string[] RoomList_ID = new string[3];

    /// <summary>
    /// 代理房间列表 比列
    /// </summary>
    public string[] RoomList_Ratio = new string[3];

    /// <summary>
    /// 代理房间列表 房间名字
    /// </summary>
    public string[] RoomList_Title = new string[3];

    /// <summary>
    /// 选择的房间ID
    /// </summary>
    public string Room_ID { get; set; }

    /// <summary>
    /// 坐下来的用户名字 列表
    /// </summary>
    public string[] Trigger_Username = new string[30];

    /// <summary>
    /// 坐下来的用户座位编号 列表
    /// </summary>
    public int[] Trigger_SeatNum = new int[30];

    /// <summary>
    /// 坐下的用户 是否属于留机状态  1留机 其他不留机
    /// </summary>
    public int[] Trigger_Is_Machine = new int[30];

    /// <summary>
    /// 坐下来用户留机时间 秒数
    /// </summary>
    public int[] Trigger_Time = new int[30];

    /// <summary>
    /// 自己坐下的机子编号
    /// </summary>
    public string TriggerNum { get; set; }

    /// <summary>
    /// 机子ID
    /// </summary>
    public string TriggerID { get; set; }

    /// <summary>
    /// 有多少玩家在线
    /// </summary>
    public int PlayerNum { get; set; }

    /// <summary>
    /// 赔率列表
    /// </summary>
    public int[] OddList = new int[10];

    /// <summary>
    /// 用户分数
    /// </summary>
    public int Fraction { get; set; }

    /// <summary>
    /// 中奖分数
    /// </summary>
    public int WinMoney { get; set; }

    /// <summary>
    /// 用户押了多少分
    /// </summary>
    public int ChargeNum { get; set; }

    /// <summary>
    /// 这次下注的会话ID
    /// </summary>
    public string Periods_ID { get; set; }

    /// <summary>
    /// 是否翻牌
    /// </summary>
    public bool IsFlopCard = false;


    /// <summary>
    /// 开牌结果
    /// </summary>
    public string[] CardType = new string[5];

    /// <summary>
    /// 中了什么奖 
    /// </summary>
    public string CardTitle { get; set; }

    /// <summary>
    /// 中奖特效显示
    /// </summary>
    public bool IsWinEffect = false;

    /// <summary>
    /// 是否显示得分按钮
    /// </summary>
    public bool IsWin = false;

    /// <summary>
    /// 是否保留牌
    /// </summary>
    public string[] HoldData = new string[5];

    /// <summary>
    /// 是否点击开始或翻牌
    /// </summary>
    public bool IsStart = false;

    /// <summary>
    /// 比倍大小翻牌
    /// </summary>
    public bool IsFlopColumnBoard = false;

    /// <summary>
    /// 比倍结果 1  ---> 小   2  ---->  大
    /// </summary>
    public string Result_result { get; set; }

    /// <summary>
    /// 比倍的时候，牌子类型
    /// </summary>
    public string Result_Data { get; set; }

    //是否继续比倍
    public bool IsBiBei = false;

    /// <summary>
    /// 游戏模式 0：未开始模式  1：正常模式 2:比倍模式
    /// </summary>
    public int BiType = 0;

    /// <summary>
    /// 是否自动游戏
    /// </summary>
    public bool IsAutomatic = false;

    /// <summary>
    /// 翻牌次数 1：第一次翻牌  2：第二次翻牌
    /// </summary>
    public int FlopNum = 1;

    /// <summary>
    /// 比倍例牌显示
    /// </summary>
    public bool IsColumnBoard = false;

    /// <summary>
    /// 右边六张牌的结果
    /// </summary>
    public string[] str_result = new string[6];



    /// <summary>
    /// 网络错误
    /// </summary>
    public bool IsNetworkError = false;



    /// <summary>
    /// 1：扑克记录  2：比倍记录  3：出奖记录
    /// </summary>
    public int RecordTypeNum = 1;


    /// <summary>
    /// 扑克记录有多少条
    /// </summary>
    public int BetDataNum { get; set; }

    /// <summary>
    /// 扑克记录  第一次记录
    /// </summary>
    public string[] one_data = new string[15];

    /// <summary>
    /// 扑克记录  第二次记录
    /// </summary>
    public string[] two_data = new string[15];

    /// <summary>
    /// 保留牌位置
    /// </summary>
    public string[] retain_data = new string[15];

    /// <summary>
    /// 用户这局分数
    /// </summary>
    public string[] user_balance = new string[15];

    /// <summary>
    /// 下注分数
    /// </summary>
    public string[] bet_total = new string[15];

    /// <summary>
    /// 中奖分数
    /// </summary>
    public string[] win_total = new string[15];


    /// <summary>
    /// 比倍记录有多少 （页数）
    /// </summary>
    public int BiBeiDataNum { get; set; }

    /// <summary>
    /// 比倍记录有多少 （条）
    /// </summary>
    public int[] BiBeiDataNum2 = new int[15];

    /// <summary>
    /// 比倍押注
    /// </summary>
    public string[,] money = new string[15, 15];

    /// <summary>
    /// 比倍模式
    /// </summary>
    public string[,] doubley_sign = new string[15, 15];

    /// <summary>
    /// 猜大小
    /// </summary>
    public string[,] doubly_guessing_object = new string[15, 15];

    /// <summary>
    /// 比倍开牌结果
    /// </summary>
    public string[,] doubly_result = new string[15, 15];

    /// <summary>
    /// 出奖记录
    /// </summary>
    public string[] record_awards = new string[4];

    /// <summary>
    /// 有多少人登录中奖公告
    /// </summary>
    public int award_num { get; set; }

    /// <summary>
    /// 中奖公告的用户名字
    /// </summary>
    public string[] award_username = new string[100];

    /// <summary>
    /// 中奖公告的场地和桌子
    /// </summary>
    public string[] award_title = new string[100];

    /// <summary>
    /// 中奖公告的中奖类型
    /// </summary>
    public string[] award_type = new string[100];

    /// <summary>
    /// 中奖公告的中奖分数
    /// </summary>
    public string[] award_money = new string[100];


    public List<DGane_Modle> Model_List;

    public void EnterTheScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}
