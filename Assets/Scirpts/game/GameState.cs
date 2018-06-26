using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState  {

}
/// <summary>
/// 游戏中的各种状态
/// </summary>
    public enum playstate
    {
        /// <summary>
        /// 单挑游戏状态切换 
        /// 对面无连接时
        /// 0.不显示
        /// 1.准备
        /// 2.下注
        /// 3.锁定
        /// 4.开奖
        /// 5.结算
        /// 6.休息
        /// </summary>
        betnull = 0,//无状态   0
        betready,//准备        1
        betin,//下注状态       2
        betover,//下注锁定     3
        betopen,//开奖状态     4
        betclearing,//结算阶段 5
        bettime,//休息状态     6
    }
