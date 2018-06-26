using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager ins;
    /// <summary>
    /// UI 帮助，设置，退出
    /// </summary>
    public GameObject[] objs;

    public GameObject ZoomHead;

    public GameObject obj1;

    public GameObject obj2;

    public GameObject obj3;

    public string ZoomName = "animal_";

    public string ZXHName = "banker_";

    // Use this for initialization
    void Awake()
    {
        ins = this;
    }

    public void ClosePanel(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void ActivePanel(GameObject obj)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(false);
        }
        Debug.Log(obj.name);
        if (obj.name == "cunqu")
        {
            ChipScript.ins.CunQu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<UILabel>().text = ChipScript.ins.Credit.GetComponent<UILabel>().text;
            ChipScript.ins.CunQu.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<UILabel>().text = ChipScript.ins.Fraction.GetComponent<UILabel>().text;
        }
        obj.SetActive(true);

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// 得到角度
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public float GetEuler(GameObject obj)
    {
        return obj.transform.GetComponent<Transform>().localPosition.y;
    }

    /// <summary>
    /// 得到动物的名字
    /// </summary>
    /// <param name="str"></param>

    public void GetZoomName(string str)
    {

        // str = str.Substring(0, 1);
        switch (str)
        {
            case "L_green":
                str = "0_1";
                break;
            case "L_red":
                str = "0_0";
                break;
            case "L_yellow":
                str = "0_2";
                break;
            case "P_green":
                str = "1_1";
                break;
            case "P_red":
                str = "1_0";
                break;
            case "P_yellow":
                str = "1_2";
                break;
            case "M_green":
                str = "2_1";
                break;
            case "M_red":
                str = "2_0";
                break;
            case "M_yellow":
                str = "2_2";
                break;
            case "R_red":
                str = "3_0";
                break;
            case "R_green":
                str = "3_1";
                break;
            case "R_yellow":
                str = "3_2";
                break;
            default:
                break;
        }
        Debug.Log(str);
        SetHeadShow(true, str);

    }

    public string GetZoomName1(string str)
    {

        Debug.Log(str);
        switch (str)
        {
            case "L_green":
                str = "0_1";
                break;
            case "L_red":
                str = "0_0";
                break;
            case "L_yellow":
                str = "0_2";
                break;
            case "P_green":
                str = "1_1";
                break;
            case "P_red":
                str = "1_0";
                break;
            case "P_yellow":
                str = "1_2";
                break;
            case "M_green":
                str = "2_1";
                break;
            case "M_red":
                str = "2_0";
                break;
            case "M_yellow":
                str = "2_2";
                break;
            case "R_red":
                str = "3_0";
                break;
            case "R_green":
                str = "3_1";
                break;
            case "R_yellow":
                str = "3_2";
                break;
            default:
                break;
        }
        return ZoomName + str;

    }


    public void SetHeadShow(bool isshow, string str)
    {


        Debug.Log(ZoomName + str);
        ZoomHead.transform.GetChild(0).GetChild(0).GetComponent<UISprite>().spriteName = ZoomName + str;
        //	Debug.Log (PlayerData.ins.winnings_two_odds);
        //	Debug.Log (PlayerData.ins.winnings_three);
        Debug.Log(PlayerData.ins.winnings_three);
        Debug.Log(PlayerData.ins.winnings_four);
        ClickEvent.ins.SetNum(PlayerData.ins.winning_one, obj1.gameObject, 0);
        obj2.SetActive(false);
        obj3.SetActive(false);
        if (PlayerData.ins.win_type == "3")
        {

            for (int i = 0; i < obj2.transform.childCount; i++)
            {

                if (i == 0)
                {
                    obj2.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = GetZoomName1(ClickEvent.ins.GetAllZoomName(PlayerData.ins.winnings_two));
                    ClickEvent.ins.SetNum(PlayerData.ins.winnings_two_odds, obj2.gameObject, 0);
                }
                else
                {
                    obj2.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = GetZoomName1(ClickEvent.ins.GetAllZoomName(PlayerData.ins.winnings_three));
                    ClickEvent.ins.SetNum(PlayerData.ins.winnings_three_odds, obj2.gameObject, 1);

                }


            }
            obj2.SetActive(true);
        }
        else if (PlayerData.ins.win_type == "4")
        {



            for (int i = 0; i < obj3.transform.childCount; i++)
            {



                if (i == 0)
                {
                    obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = GetZoomName1(ClickEvent.ins.GetAllZoomName(PlayerData.ins.winnings_two));
                    Debug.Log(obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName);
                    ClickEvent.ins.SetNum(PlayerData.ins.winnings_two_odds, obj3.gameObject, 0);
                }
                else if (i == 1)
                {
                    obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = GetZoomName1(ClickEvent.ins.GetAllZoomName(PlayerData.ins.winnings_three));
                    Debug.Log(obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName);
                    ClickEvent.ins.SetNum(PlayerData.ins.winnings_three_odds, obj3.gameObject, 1);
                }
                else
                {
                    obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName = GetZoomName1(ClickEvent.ins.GetAllZoomName(PlayerData.ins.winnings_four));
                    Debug.Log(obj3.transform.GetChild(i).GetChild(0).GetComponent<UISprite>().spriteName);
                    ClickEvent.ins.SetNum(PlayerData.ins.winnings_four_odds, obj3.gameObject, 2);
                }
            }
            obj3.SetActive(true);
        }
        else
        {
            obj2.SetActive(false);
            obj3.SetActive(false);
        }

    }
    public void SetHeadZHX(string str)
    {

        if (str == "H")
        {
            str = "2";
        }
        else if (str == "X")
        {
            str = "1";
        }
        else if (str == "Z")
        {
            str = "0";
        }
        else
        {

        }
        ZoomHead.transform.GetChild(1).gameObject.SetActive(true);
        ZoomHead.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().spriteName = ZXHName + str;
        ClickEvent.ins.SetNum(PlayerData.ins.winning_ZHX, obj1.gameObject, 1);
    }

    void GetName(string str)
    {


    }
}
