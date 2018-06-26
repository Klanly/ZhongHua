using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DGame_SeatButton : MonoBehaviour
{

    private string SeatName;

    private string url;

    public AudioSource Audio;

    private void Start()
    {
        SeatName = this.gameObject.name;

        Button btn = this.GetComponent<Button>();

        btn.onClick.AddListener(OnButton);
    }

    /// <summary>
    /// 座位
    /// </summary>
    public void OnButton()
    {
        Audio.Play();
        DGameData.Instance.TriggerNum = SeatName;
        url = "http://47.106.66.89:81/pj-api/trigger-start?" + "trigger_num=" + DGameData.Instance.TriggerNum + "&room_id=" + DGameData.Instance.Room_ID + "&game_id=" + "12" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id;
        Debug.Log(url);
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }
}
