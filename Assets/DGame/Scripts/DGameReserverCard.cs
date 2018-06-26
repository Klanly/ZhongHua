using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DGameReserverCard : MonoBehaviour
{

    private string SeatName;
    private GameObject HOLD;

    private void Start()
    {
        SeatName = this.gameObject.name;
        try
        {
            HOLD = transform.Find("HLLD").gameObject;
        }
        catch
        {
            Debug.Log("寻找错误");
        }

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnButton);
    }


    public void OnButton()
    {
        if (DGameData.Instance.FlopNum != 2)
            return;
        if (HOLD.activeSelf == true)
        {
            DGameData.Instance.HoldData[int.Parse(SeatName) - 1] = "";
            Debug.Log("第" + SeatName + "张牌不保留");
            // HOLD.SetActive(false);
        }
        else
        {
            DGameData.Instance.HoldData[int.Parse(SeatName) - 1] = SeatName;
            // HOLD.SetActive(true);
            Debug.Log("第" + SeatName + "张牌保留");
        }
    }
}
