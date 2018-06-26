using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoom : MonoBehaviour
{
    public Text ID;
    public Text Gold;

    public Text[] GameRatio;


    private float times;
    string url;

    public GameObject NetworkError;

    public AudioClip clip;

    public AudioSource Audio;

    private void Start()
    {
        HGameData.Instance.IsNetworkError = false;
        Audiomanger._instenc.GetComponent<AudioSource>().clip = clip;
        Audiomanger._instenc.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        OnUI();

        if (HGameData.Instance.IsNetworkError)
        {
            NetworkError.SetActive(true);
        }


        ///网络连接正常
        times += Time.deltaTime;
        if (times > 2)
        {
            times = 0;
            ////每隔2轮询一次 用户信息 
            //url = "http://47.106.66.89:81/pj-api/userinfo?" + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + HGameData.Instance.Unionuid;
            //StartCoroutine(HGameTcpNet.Instance.SendVoid(url));

            //if (HGameData.Instance.IsSign)
            //{
            //    url = "http://47.106.66.89:81/pj-api/userinfo?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + HGameData.Instance.Unionuid;
            //    StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
            //}
        }
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    void OnUI()
    {
        ID.text = LoginInfo.Instance().mylogindata.user_id;
        Gold.text = LoginInfo.Instance().mylogindata.ALLScroce;
        for (int i = 0; i < 3; i++)
        {
            GameRatio[i].text = "1金币->" + HGameData.Instance.RoomRatio[i] + "分";
        }
    }


    /// <summary>
    /// 娱乐场按钮
    /// </summary>
    public void CasinoButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsNetworkError)
            return;
        HGameData.Instance.RoomTitleID = "3";
        url = "http://47.106.66.89:81/pj-api/room-start?" + "game_id=" + "11" + "&room_id=" + HGameData.Instance.RoomTitleID;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 普通场按钮
    /// </summary>
    public void CommonButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsNetworkError)
            return;
        HGameData.Instance.RoomTitleID = "4";
        url = "http://47.106.66.89:81/pj-api/room-start?" + "game_id=" + "11" + "&room_id=" + HGameData.Instance.RoomTitleID;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// VIP按钮
    /// </summary>
    public void VIPButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsNetworkError)
            return;
        HGameData.Instance.RoomTitleID = "6";
        url = "http://47.106.66.89:81/pj-api/room-start?" + "game_id=" + "11" + "&room_id=" + HGameData.Instance.RoomTitleID;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 网络按钮 
    /// </summary>
    public void OnNetworkButton()
    {
        Audio.Play();
        HGameData.Instance.EnterTheScene("hallchooseroom");
    }


    /// <summary>
    /// 退出按钮
    /// </summary>
    public void QuitButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsNetworkError)
            return;
        HGameData.Instance.EnterTheScene("hallchooseroom");
    }

}
