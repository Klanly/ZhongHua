using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using LitJson;

public class hallInfo : MonoBehaviour {
    public GameObject playerScroecPanelGO;
    private Button playGO;
    public GameObject liScroecPanelGO;
    private Button liSpGO;

    public GameObject changepasswordGO;
    private Button chPDGO;

    public GameObject room1to1;
    public GameObject room1to10;

    public GameObject bettpenal;
    private Button closebet;
    private Button loadtologin;

    public List<userinfo> gamelistinfo;
    public Text username;
    public Text moeny;
    public LoginData temp;
    private int pagecout;

    public int Pagecout
    {
        get
        {
            return pagecout;
        }

        set
        {
            pagecout = value;
        }
    }

    void Awake()
    {
       
    }

    // Use this for initialization
    void Start () {

        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        temp = LoginInfo.Instance().mylogindata;
        playGO= playerScroecPanelGO.transform.Find("forbackbutton").GetComponent<Button>();
        playGO.onClick.AddListener(playGO.GetComponent<showWindow>().OnClick);

        liSpGO= liScroecPanelGO.transform.Find("forbackbutton").GetComponent<Button>();
        liSpGO.onClick.AddListener(liSpGO.GetComponent<showWindow>().OnClick);

        chPDGO= changepasswordGO.transform.Find("forbackbutton").GetComponent<Button>();
        chPDGO.onClick.AddListener(chPDGO.GetComponent<showWindow>().OnClick);

        loadtologin= bettpenal.transform.Find("Image").Find("quitButton").GetComponent<Button>();
        closebet   = bettpenal.transform.Find("Image").Find("closeButton").GetComponent<Button>();

        closebet.onClick.AddListener(closebet.GetComponent<showWindow>().OnClick);
        loadtologin.onClick.AddListener(torlogin);

        room1to1.transform.GetComponent<Button>().onClick.AddListener(toroom);
        room1to10.transform.GetComponent<Button>().onClick.AddListener(toroom2);
        setmoneyandname();

        gamelistinfo = new List<userinfo>();
        Pagecout = 0;
        for (int i = 0; i <27; i++)
        {
            userinfo temp = new userinfo();
            temp.username = "lili" + i;
            temp.time = "2017/1/1/" + i;
            temp.winbet = "win:" + i;
            gamelistinfo.Add(temp);

        }

        //StartCoroutine(hallalive());


    }
	
    IEnumerator hallalive()
    {
        while (true)
        {
            yield return getaliveinfo(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.hallaliveAPI + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + LoginInfo.Instance().mylogindata.token);

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator getaliveinfo(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();

        JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
        if (jd["code"].ToString() == "200")
        {

        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void toroom()
    {
        StartCoroutine(loadscen(2));
    }
    public void toroom2()
    {
        StartCoroutine(loadscen(3));
    }
    IEnumerator loadscen(int scren)
    {
        var temp= SceneManager.LoadSceneAsync(scren);
        
        yield return temp;
        if (temp.isDone)
        {
            Debug.Log("游戏加载完毕");
        }
    }

    void setmoneyandname()
    {
        username.text = temp.username;
        moeny.text = temp.ALLScroce;
    }

    public void torlogin()
    {
        StartCoroutine(loadloginscene());
    }

    IEnumerator loadloginscene()
    {
        var temp = SceneManager.LoadSceneAsync(0);
        yield return temp;
        if (temp.isDone)
        {
            Debug.Log("登录加载完毕");
        }
    }
   /// <summary>
   /// 初始化信息列表
   /// </summary>
   /// <param name="list"></param>
   public void getdatatoui(List<userinfo> list)
    {
        if (list.Count > 0)
        {
            //大于9的处理
            if (list.Count > 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    changeUIdataInListPanel(i, 0, list[i].username);
                    changeUIdataInListPanel(i, 1, list[i].time);
                    changeUIdataInListPanel(i, 2, list[i].winbet);
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    changeUIdataInListPanel(i, 0, list[i].username);
                    changeUIdataInListPanel(i, 1, list[i].time);
                    changeUIdataInListPanel(i, 2, list[i].winbet);
                }
            }

        }  
    }
    /// <summary>
    /// 修改定位
    /// </summary>
    /// <param name="uichild">定位UI列表中的第几位</param>
    /// <param name="datacout">修改哪个文本值</param>
    /// <param name="userinfodata">获取的修改值</param>
    void changeUIdataInListPanel(int uichild,int datacout,string userinfodata)
    {
        Transform temp = playerScroecPanelGO.transform.Find("listpanel");
        temp.GetChild(uichild).GetChild(datacout).GetComponent<Text>().text = userinfodata;

    }

    void cleangameinfo()
    {
        for (int i = 0; i < 9; i++)
        {
            changeUIdataInListPanel(i, 0, "");
            changeUIdataInListPanel(i, 1, "");
            changeUIdataInListPanel(i, 2, "");
        }
    }
    
    /// <summary>
    /// 下一页
    /// </summary>
   public void NextPage()
    {
        if (gamelistinfo.Count % 9 == 0)
        {
            if ((pagecout+1) < gamelistinfo.Count / 9)
            {
             pagecout++;

            }
            else
            {
                return;
            }
        }
        else
        {
            if ((pagecout + 1) < (gamelistinfo.Count / 9)+1)
            {
                pagecout++;

            }
            else
            {
                return;
            }
        }

        if (gamelistinfo.Count - (pagecout * 9) <0)
        {
            return;
        }
       


        cleangameinfo();
        changeuitopage(pagecout);
          
    }
    /// <summary>
    /// 上一页
    /// </summary>
    public void LastPage()
    {
        if (pagecout == 0)
        {
            return;
        }
        else
        {
            pagecout--;
        }
        cleangameinfo();
        changeuitopage(pagecout);

    }

    /// <summary>
    /// 更改列表的数据方法
    /// </summary>
    /// <param name="page"></param>
    void changeuitopage(int page)
    {
        int temp = page; //写一个数值用于存储页面信息
        if (temp >= 0)
        {
            temp = temp * 9;
        }
        else
        {
            return;
        }
        

        if (gamelistinfo.Count - temp < 9 && gamelistinfo.Count - temp > 0)
        {
            for (int i = temp; i < gamelistinfo.Count; i++)
            {
                changeUIdataInListPanel(i % 9, 0, gamelistinfo[i].username);
                changeUIdataInListPanel(i % 9, 1, gamelistinfo[i].time);
                changeUIdataInListPanel(i % 9, 2, gamelistinfo[i].winbet);
            }
        }

        else if (gamelistinfo.Count - temp >= 9)
        {

            for (int i = temp; i < temp + 9; i++)
            {
                changeUIdataInListPanel(i % 9, 0, gamelistinfo[i].username);
                changeUIdataInListPanel(i % 9, 1, gamelistinfo[i].time);
                changeUIdataInListPanel(i % 9, 2, gamelistinfo[i].winbet);
            }
        }



    }

}
