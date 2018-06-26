using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PTCChooseJiQiScript : MonoBehaviour
{
    public Text RoomName;
    public GameObject[] Seat;
    public Sprite[] SeatSprite;

    private int[] A = new int[30];

    public Text[] Name = new Text[30];
    public Text[] ID;

    private float time = 2;
    private string url;

    public GameObject NetworError;

    public AudioSource Audio;

    public AudioClip Clip;

    private void Start()
    {
        HGameData.Instance.IsNetworkError = false;
        url = "http://47.106.66.89:81/pj-api/room-user-data?" + "game_id=" + "11" + "&room_id=" + HGameData.Instance.RoomTitleID;
        Audiomanger._instenc.GetComponent<AudioSource>().clip = Clip;
        Audiomanger._instenc.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        OnUI();

        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            StartCoroutine(enumerator(url));
            if (HGameData.Instance.IsSign)
            {
                url = "http://47.106.66.89:81/pj-api/userinfo?user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + HGameData.Instance.Unionuid;
                StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
            }
        }

        if (HGameData.Instance.IsNetworkError)
        {
            NetworError.SetActive(true);
        }
    }

    void OnUI()
    {
        RoomName.text = HGameData.Instance.RoomTitle;
        A = new int[30];
        for (int i = 0; i < HGameData.Instance.TriggerNum; i++)
        {
            A[int.Parse(HGameData.Instance.TriggerSeatNum[i]) - 1] = 1;
        }
        for (int i = 0; i < 30; i++)
        {
            if (A[i] == 1)
            {
                Seat[i].GetComponent<Image>().sprite = SeatSprite[1];
                Name[i].text = HGameData.Instance.TriggerName[i];
                if (HGameData.Instance.Is_machine[i] == "1")
                {
                    if (HGameData.Instance.Time[i] > 60)
                    {
                        ID[i].text = (int)(HGameData.Instance.Time[i] / 60) + "分" + (HGameData.Instance.Time[i] % 60) + "秒";
                    }
                    else
                    {
                        if (HGameData.Instance.Time[i] == 0)
                        {
                            ID[i].text = "";
                        }
                        else
                        {
                            ID[i].text = HGameData.Instance.Time[i] + "秒";
                        }
                    }
                }
                else
                {
                    ID[i].text = "";
                }

                if (HGameData.Instance.TriggerName[i] == LoginInfo.Instance().mylogindata.username)
                {
                    Seat[i].GetComponent<Image>().sprite = SeatSprite[0];
                    Name[i].text = "";
                    ID[i].text = "";
                }
            }
            else
            {
                Seat[i].GetComponent<Image>().sprite = SeatSprite[0];
                Name[i].text = "";
                ID[i].text = "";
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

                    ///时时获取机子的情况  在机子列表页面轮询      
                    if (jd["userData"] != null)
                    {
                        HGameData.Instance.TriggerNum = jd["userData"].Count;
                        Debug.Log("一共有多少玩家在玩：" + HGameData.Instance.TriggerNum);
                    }
                    for (int i = 0; i < HGameData.Instance.TriggerNum; i++)
                    {
                        //if (jd["userData"][i]["user_id"] != null)
                        //{
                        //    HGameData.Instance.TriggerID[i] = jd["userData"][i]["user_id"].ToString();
                        //    Debug.Log("坐下的玩家ID：" + HGameData.Instance.TriggerID[i]);
                        //}
                        if (jd["userData"][i]["username"] != null)
                        {
                            HGameData.Instance.TriggerName[i] = jd["userData"][i]["username"].ToString();
                            Debug.Log("坐下来的玩家名字：" + HGameData.Instance.TriggerName[i]);
                        }
                        if (jd["userData"][i]["seat_number"] != null)
                        {
                            HGameData.Instance.TriggerSeatNum[i] = jd["userData"][i]["seat_number"].ToString();
                            Debug.Log("坐下的玩家座位的编号：" + HGameData.Instance.TriggerSeatNum[i]);
                        }

                        HGameData.Instance.Is_machine[i] = jd["userData"][i]["is_machine"].ToString();
                        HGameData.Instance.Time[i] = int.Parse(jd["userData"][i]["time"].ToString());
                    }
                }
                else
                {
                    HGameData.Instance.IsNetworkError = true;
                }
            }
        }

        catch
        {
            Debug.Log("JSON解析错误");
        }
    }


    /// <summary>
    /// 退出按钮
    /// </summary>
    public void OnQuitButton()
    {
        Audio.Play();
        if (HGameData.Instance.IsNetworkError)
            return;
        HGameData.Instance.EnterTheScene("ChooseRoom");
    }

    /// <summary>
    /// 网络按钮
    /// </summary>
    public void OnNetWorkButton()
    {
        Audio.Play();
        HGameData.Instance.EnterTheScene("hallchooseroom");
    }
}
