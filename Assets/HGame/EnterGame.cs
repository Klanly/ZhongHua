using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterGame : MonoBehaviour
{

    private string url = "";




    /// <summary>
    /// 进入火凤凰
    /// </summary>
    public void EnterHGame()
    {
        url = "http://47.106.66.89:81/pj-api/room-list?game_id=11&user_id=" + LoginInfo.Instance().mylogindata.user_id;

        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }


    /// <summary>
    /// 进入大字版
    /// </summary>
    public void EnterDGame()
    {
        url = "http://47.106.66.89:81/pj-api/room-list?game_id=12&user_id=" + LoginInfo.Instance().mylogindata.user_id;

        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }



    /// <summary>
    /// 进入3D动物
    /// </summary>
    public void Enter3DAnimals()
    {
        //  http://paiji-web.weiec4.cn/api/room-start?game_id=1&user_id=3&unionuid=201806081406562518896

        //url = "http://47.106.66.89:81/pj-api/room-list?game_id=11&user_id=" + LoginInfo.Instance().mylogindata.user_id;

        //StartCoroutine(HGameTcpNet.Instance.SendVoid(url));

        SceneManager.LoadScene(14);
    }

}
