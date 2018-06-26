using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePolling : MonoBehaviour
{

    private float times;

    string url = "";


    private void Start()
    {
        url = "http://47.106.66.89:81/pj-api/update-seat" + "?game_id=" + "11"+ "&user_id=" + LoginInfo.Instance().mylogindata.user_id + "&room_id=" + HGameData.Instance.RoomTitleID;
    }


    private void Update()
    {
        times += Time.deltaTime;
        if (times > 2)
        {
            times = 0;
            StartCoroutine(enumerator(url));
            if (HGameData.Instance.IsSign)
            {
                url = "http://47.106.66.89:81/pj-api/userinfo?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + HGameData.Instance.Unionuid;
                StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
            }
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

                    if (jd["code"] != null)
                    {
                        Debug.Log("code:" + jd["code"].ToString());
                        if (jd["code"].ToString() != "200")
                        {

                        }
                    }
                    if (jd["bool"] != null)
                    {
                        Debug.Log("bool:" + jd["bool"].ToString());
                        switch (jd["bool"].ToString())
                        {
                            case "false":
                                //不管
                                break;
                            case "true":
                                //用户超时未操作 清楚当前用户
                                HGameData.Instance.IsNetworkError = true;
                                break;
                        }
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

}
