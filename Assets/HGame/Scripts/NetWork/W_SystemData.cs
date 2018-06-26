using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能：游戏全局静态变量存放位置
/// </summary>
public class W_SystemData
{

    public static string Url = "";

    public static string LD_Url = "http://39.108.224.87:81/api/";
    public static string LX_Url = "http://39.108.224.87:81/practice-api/";

    //练习模式是否激活
    public static bool IsExercise;

    public enum GameType
    {
        /// <summary>
        /// 火凤凰
        /// </summary>
        HGane
    }

    public static GameType gametype = GameType.HGane;




}
