using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using UnityEngine.Networking;



/// <summary>
/// 历史数据结构
/// </summary>
[System.Serializable]
public class opendatesturt
{
   public  string prides;//期数
   public string opennumber;//开奖数据
   public string win_huase;// 中奖数据1
   public string win_huase2;//中奖数据2
}



//控制历史记录脚本
public class showhistorydata : MonoBehaviour {

    public List<opendatesturt> listcheak;//开奖数据列表
    public static int countpage=0;//当前的页数
    public GameObject dateGO;//开奖对象
    public Button PageUpButton;//上一页
    public Button PageDowmpButton;//下一页
    public Text nodatebutton;//错误提示文本

    private bool islessten=false;//是否关闭多余数据开关

    /// <summary>
    /// 每次启动该脚本就去调用存储好的输赢列表 （减少延迟率）
    /// </summary>
    private void OnEnable()
    {
        cheaklistifUPdate(GameMagert.Instance.Textwindata);
    }

    // Use this for initialization
    void Start () {
        PageUpButton.onClick.AddListener(delpage);
        PageDowmpButton.onClick.AddListener(addpage);
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    addpage();
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    delpage();
        //}

    }

   


    /// <summary>
    /// 接收开奖记录 跟据开奖记录信息对历史记录进行填充
    /// </summary>
    /// <param name="jsondata">游戏JSON记录</param>
    void cheaklistifUPdate(string jsondata)
    {
        if (jsondata != "")
        {
            JsonData jd= JsonMapper.ToObject(jsondata);
            if (jd["code"].ToString().Equals( "200"))
            {

                nodatebutton.gameObject.SetActive(false);


                if (jd["ArrList"].Count != listcheak.Count)
                {
                    listcheak.Clear();
                    for (int i = jd["ArrList"].Count-1; i >=0; i--)
                    {
                        opendatesturt temp = new opendatesturt();
                        temp.prides = jd["ArrList"][i]["dates"].ToString();
                        temp.opennumber = jd["ArrList"][i]["awards"].ToString();
                        temp.win_huase = jd["ArrList"][i]["win_text1"].ToString();
                        temp.win_huase2 = jd["ArrList"][i]["win_text2"].ToString();

                        listcheak.Add(temp);



                    }



                }
                if (listcheak.Count > 0)
                {
                 updateopenddate(countpage, listcheak);

                }

            }
            else
            {
            this.closeGo(0, 1);
            nodatebutton.text = "数据出错，请关闭再打开";

                Debug.Log("返回的参数有错");
            }
        }
        else
        {
            this.closeGo(0, 1);
            nodatebutton.text = "目前暂无数据请等会再打开";

        }

    }


    /// <summary>
    /// 根据开奖数据 设置所有的开奖记录图片
    /// </summary>
    /// <param name="pagecount">当前页</param>
    /// <param name="opedate">服务器数据数组</param>
    void updateopenddate(int pagecount,List<opendatesturt> opedate)
    {

        int start=0;//循环起始
        int end=0;//循环结束
        int thisforcount = 0;//循环总数


        if (opedate.Count > 0&&pagecount>=0)
        {
            //要是长度大于10且属于第一次的情况
            if (pagecount == 0&& opedate.Count-10>=10)
            {
                start = pagecount * 10;
                end = pagecount * 10 + 10;
                thisforcount = end - start;
                islessten = false;
                closeGo(0, 0);
            }
            //要是长度大于10且非第一次进的情况
            else if(opedate.Count-(pagecount * 10) >= 10)
            {
                start = pagecount * 10;
                end = pagecount * 10 + 10;
                thisforcount = end - start;
                islessten = false;
                closeGo(0, 0);
            }
            //循环一次长度小于10的情况
            else
            {
                start = pagecount * 10;
                int x;
                end = opedate.Count %((x=pagecount>0?pagecount * 10:10)) +  pagecount * 10;
                thisforcount = end - start;
                islessten = true;
            }
             
            //根据起始和结束填充图片
            for (    ; start < end; start++)
            {
                //添加期数
                dateGO.transform.GetChild(thisforcount - (end - start)).GetChild(0).GetComponent<Text>().text = opedate[start].prides;
                //添加开奖的数据
                setimgaefromstring( GameMagert.Instance.changestringtointlist(opedate[start].opennumber), dateGO.transform.GetChild(thisforcount - (end - start)).GetChild(1));
                //添加中奖的信息
                setopendateimage((int)GameMagert.Instance.changeTYPEtoint(opedate[start].win_huase), (int)GameMagert.Instance.changeTYPEtoint(opedate[start].win_huase2), dateGO.transform.GetChild(thisforcount - (end - start)).GetChild(2));

            }
            //根据开关决定要不要关闭多余的窗口
            if (islessten)
            {
                closeGo(thisforcount, 1);
                islessten = false;
            }


        }
    } 
    /// <summary>
    /// 设置当期开奖数据图片
    /// </summary>
    /// <param name="setmapdata">开奖数据数据组</param>
    /// <param name="taget"></param>
    void setimgaefromstring(int[] setmapdata,Transform taget)
    {

        for (int i = 0; i < taget.childCount; i++)
        {
            taget.GetChild(i).GetComponent<Image>().sprite = NumSpriteControl.Instances.num_Sprite[setmapdata[i]-1];
        }

    }

    //设置当期开奖结果图片
    void setopendateimage(int opendata1,int opendata2,Transform opedataGO)
    {


        opedataGO.GetChild(0).GetComponent<Image>().sprite = GameMagert.Instance.poketGOtoinit.transform.GetChild(0).GetChild(opendata1).GetComponent<Image>().sprite;
        opedataGO.GetChild(1).GetComponent<Image>().sprite = GameMagert.Instance.poketGOtoinit.transform.GetChild(0).GetChild(opendata2).GetComponent<Image>().sprite;


    }
    
    /// <summary>
    /// 下一页
    /// </summary>
    public void addpage()
    {
        if(countpage+1>= (listcheak.Count / 10f))
        {
            Debug.Log("已到达尾页，无法显示下一页");
        }
        else
        {
            countpage++;
            updateopenddate(countpage, listcheak);
        }
    }

    /// <summary>
    /// 上一页
    /// </summary>
    public void delpage()
    {
        if (countpage -1<0)
        {
            Debug.Log("已到达首页，无法显示上一页");
        }
        else
        {
            countpage--;
            updateopenddate(countpage, listcheak);
        }
    }



    //关闭对应预设体
    public void closeGo(int start,int taget)
    {
        for (int i = start; i <dateGO.transform.childCount; i++)
        {

            if (taget == 0)
            {
                dateGO.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                dateGO.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

      
            

    }

    //通过拖拽调用
    public void setcountdown()
    {
        countpage = 0;
    }


}
