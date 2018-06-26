using UnityEngine;
using UnityEngine.UI;

public class DGame_ChooseRoom : MonoBehaviour
{
    public Text ID;

    public Text Gold;

    public Text[] Room_Ratio;

    private string url;

    private AudioSource Audio;

    private void Start()
    {
        Audio = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        OnUI();
    }

    void OnUI()
    {
        ID.text = LoginInfo.Instance().mylogindata.user_id;
        Gold.text = LoginInfo.Instance().mylogindata.ALLScroce;
        for (int i = 0; i < 3; i++)
        {
            Room_Ratio[i].text = "1金币->" + DGameData.Instance.RoomList_Ratio[i] + "分";
        }
    }

    /// <summary>
    /// 娱乐场 3
    /// </summary>
    public void Room01()
    {
        Audio.Play();
        DGameData.Instance.Room_ID = "3";
        url = "http://47.106.66.89:81/pj-api/room-start?game_id=12&room_id=3";
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 普通场 4
    /// </summary>
    public void Room02()
    {
        Audio.Play();
        DGameData.Instance.Room_ID = "4";
        url = "http://47.106.66.89:81/pj-api/room-start?game_id=12&room_id=4";
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }
    /// <summary>
    /// VIP场 6
    /// </summary>
    public void Room03()
    {
        Audio.Play();
        DGameData.Instance.Room_ID = "6";
        url = "http://47.106.66.89:81/pj-api/room-start?game_id=12&room_id=6";
        StartCoroutine(DGameTcpNet.Instance.SendVoid(url));
    }

    /// <summary>
    /// 放回游戏大厅
    /// </summary>
    public void OnQuit()
    {
        Audio.Play();
        DGameData.Instance.EnterTheScene("hallchooseroom");
    }
}
