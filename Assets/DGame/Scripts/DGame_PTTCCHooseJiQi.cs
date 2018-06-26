using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DGame_PTTCCHooseJiQi : MonoBehaviour
{
    public Text RoomName;

    public Text[] PlayerName;

    public Text[] PlayerTime;

    public GameObject[] Seat;
    public Sprite[] SeatSprite;

    private int[] A = new int[30];


    private float time;
    string url = "";

    private AudioSource Audio;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        url = "http://47.106.66.89:81/pj-api/room-user-data?game_id=12&room_id=" + DGameData.Instance.Room_ID;
    }

    private void Update()
    {
        OnUI();

        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            StartCoroutine(SendVoid(url));
        }
    }

    void OnUI()
    {
        A = new int[30];
        for (int i = 0; i < DGameData.Instance.PlayerNum; i++)
        {
            A[DGameData.Instance.Trigger_SeatNum[i] - 1] = 1;
        }
        for (int i = 0; i < 30; i++)
        {
            if (A[i] == 1)
            {
                Seat[i].GetComponent<Image>().sprite = SeatSprite[1];
                PlayerName[i].text = DGameData.Instance.Trigger_Username[i];
                if (DGameData.Instance.Trigger_Is_Machine[i] == 1)
                {
                    if (DGameData.Instance.Trigger_Time[i] > 60)
                    {
                        PlayerTime[i].text = (DGameData.Instance.Trigger_Time[i] / 60) + "分" + (DGameData.Instance.Trigger_Time[i] % 60) + "秒";
                    }
                    else
                    {
                        if (DGameData.Instance.Trigger_Time[i] != 0)
                            PlayerTime[i].text = DGameData.Instance.Trigger_Time[i] + "秒";
                        else
                            PlayerTime[i].text = "";

                    }
                }
                else
                {
                    PlayerTime[i].text = "";
                }
                if (DGameData.Instance.Trigger_Username[i] == LoginInfo.Instance().mylogindata.username)
                {
                    Seat[i].GetComponent<Image>().sprite = SeatSprite[0];
                    PlayerName[i].text = "";
                    PlayerTime[i].text = "";
                }
            }
            else
            {
                Seat[i].GetComponent<Image>().sprite = SeatSprite[0];
                PlayerName[i].text = "";
                PlayerTime[i].text = "";
            }
        }

        switch (DGameData.Instance.Room_ID)
        {
            case "3":
                RoomName.text = "娱乐场";
                break;
            case "4":
                RoomName.text = "普通场";
                break;
            case "6":
                RoomName.text = "VIP场";
                break;
        }
    }

    public IEnumerator SendVoid(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return www.Send();

        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {
                try
                {
                    JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                    DGameData.Instance.PlayerNum = jd["userData"].Count;
                    for (int i = 0; i < jd["userData"].Count; i++)
                    {
                        DGameData.Instance.Trigger_Username[i] = jd["userData"][i]["username"].ToString();
                        DGameData.Instance.Trigger_SeatNum[i] = int.Parse(jd["userData"][i]["seat_number"].ToString());
                        DGameData.Instance.Trigger_Is_Machine[i] = int.Parse(jd["userData"][i]["is_machine"].ToString());
                        DGameData.Instance.Trigger_Time[i] = int.Parse(jd["userData"][i]["time"].ToString());
                    }
                }
                catch
                {
                    Debug.Log("当前座位没有人");
                    DGameData.Instance.Trigger_Username = new string[30];
                    DGameData.Instance.Trigger_SeatNum = new int[30];
                    DGameData.Instance.Trigger_Is_Machine = new int[30];
                    DGameData.Instance.Trigger_Time = new int[30];
                }
            }
        }
    }

    /// <summary>
    /// 退出按钮
    /// </summary>
    public void OnQuit()
    {
        Audio.Play();
        DGameData.Instance.EnterTheScene("DGame_ChooseRoom");
    }

}
