using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeatButtonManage : MonoBehaviour
{

    private string SeatName;
    private string url; //地址

    public AudioSource Audio;

    private void Start()
    {
        SeatName = this.gameObject.name;
        // Debug.Log(SeatName);
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnButton);
    }

    /// <summary>
    /// 座位
    /// </summary>
    public void OnButton()
    {
        Audio.Play();
        HGameData.Instance.Seat_number = SeatName;
        url = "http://47.106.66.89:81/pj-api/trigger-start?" + "trigger_num=" + HGameData.Instance.Seat_number + "&room_id=" + HGameData.Instance.RoomTitleID + "&game_id=" + "11" + "&user_id=" + LoginInfo.Instance().mylogindata.user_id;
        StartCoroutine(HGameTcpNet.Instance.SendVoid(url));
    }
}
