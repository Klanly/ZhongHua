using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;

public class LoginInfo : MonoBehaviour {

    private static LoginInfo instance;
    private static GameObject loginGO;
    public WWWstatic wwwinstance;
    public int mytest;
    public LoginData mylogindata;

   

    public static LoginInfo Instance()
    {
        
            if (instance == null)
            {
                loginGO = new GameObject("LoginInfo");
                instance = loginGO.AddComponent(typeof(LoginInfo)) as LoginInfo;
                instance.wwwinstance = loginGO.AddComponent(typeof(WWWstatic)) as WWWstatic;
                instance.wwwinstance.listforlogininfo = new Queue<UnityWebRequest>();
                instance.wwwinstance.listforhallinfo = new Queue<UnityWebRequest>();
                instance.wwwinstance.listforgameinfo = new Queue<UnityWebRequest>();
                instance.wwwinstance.listformwwwinfo = new Queue<wwwinfo>();
                instance.mylogindata = new LoginData
                   (
                    /*"http://pkdt.frxrtf.cn/api/",*/
                    "http://47.106.66.89:81/api/",        //  		"http://jykj.weiec4.cn/api/",   //          	
                    "login?",                               // 2
                    "register?",                            // 3
                    "game-list",                            // 4
                    "room-list?",                           // 5 
                    "room-start?",                          // 6
                    "room-end?",                            // 7
                    "countdown-dt",                         // 8
                    "dt-win-list",                          // 9
                    "user-task-dt?",                        // 10
                    "bets-dt?",                             // 11
                    "dt-win-info?",                         // 12
                    "cancel-all?",                          // 13
                    "userinfo?",                            // 14
                    "version?",                             // 15
                    "password-chang?",                      // 16
                    "win-history?",                         // 17
                    "user-cut?",                            // 18
                    "user-cut-send?",                       // 19
                    "game-room-odds?",                      // 20
                    "mpzzs-bets?",                          // 21
                    "win-list?",                            // 22
                    "room-user-data?",                      // 23
                    "user-bets-data?",                      // 24
                    "win-info?",                            // 25
                    "mpzzs-cancel-all?",                    // 26
                    "bl-bets?",                             // 27
                    "bl-cancel-all?",                       // 28
                    "ds-bets?",                             // 29
                    "ds-cancel-all?",                       // 30
                    "elb-bets?",                            // 31
                    "elb-cancel-all?",                      // 32
                    "pj-bets?",                             // 33
                    "pj-cancel-all?",                       // 34
                    "lh-bets?",                             // 35
                    "lh-cancel-all?",                       // 36
                    "live-video?",                          // 37
                    "xwy-bets?",                            // 38
                    "xwy-cancel-all?",                      // 39
                    "dxb-bets?",                            // 40
                    "dxb-cancel-all?",                      // 41
                    "services-info"                         // 42

                    );
            //instance.mylogindata.username = "lkk";
            //instance.mylogindata.ALLScroce = "99999";
            //添加用户信息类（自定义）
            instance.mylogindata.game_id = new List<int>() { 1,2,3,4,5,6,7,8,9,10,11,12,13,14};
            instance.mylogindata.snid = new List<string>();
                DontDestroyOnLoad(loginGO);
            }
            return instance;

        

      
    }



    //可以用来判断连接状态
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewTcpNet.GetInstance().SocketQuit();
        }

        if (LoginData.IsLogin)
        {
            LoginData.OverTime += Time.deltaTime;
            if (LoginData.OverTime>=3f)
            {
                NewTcpNet.GetInstance().SocketQuit();
                //OnLogin onLo = new OnLogin("Login", mylogindata.user_id, mylogindata.room_id.ToString(),mylogindata.choosegame.ToString());
                //string str = JsonMapper.ToJson(onLo);

                NewTcpNet.GetInstance();

            }
        }

    }

    //清空单例中的用户信息
    public void cleanmylogindata()
   {
        if (instance.mylogindata != null)
        {

            instance.mylogindata.user_id = "";
            instance.mylogindata.token = "";
            instance.mylogindata.username = "";
            instance.mylogindata.ALLScroce = "";
            instance.mylogindata.login_ip = "";
            instance.mylogindata.telephone = "";
            instance.mylogindata.status = "";
            instance.mylogindata.userStatus = "";
            instance.mylogindata.coindown = 0;
            instance.mylogindata.room_id = 0;
            instance.mylogindata.choosegame = 0;
            instance.mylogindata.roomlitmit = "";
            instance.mylogindata.roomcount = "";
            instance.mylogindata.seating = "";
        }
  

   }
  public  void loguserinformation(string where)
    {
        StringBuilder temp = new StringBuilder();
        temp.Append(where + "\n");
        temp.Append( instance.mylogindata.user_id+"\n");
        temp.Append(instance.mylogindata.token + "\n");
        temp.Append(instance.mylogindata.username + "\n");
        temp.Append(instance.mylogindata.ALLScroce + "\n");
        temp.Append(instance.mylogindata.login_ip + "\n");
        temp.Append(instance.mylogindata.telephone + "\n");
        temp.Append(instance.mylogindata.status + "\n");
        Debug.Log(temp.ToString());
    }
    
    public void cheakisalive()
    {
        //this.wwwinstance.sendWWW()
    }


    //-------------版本更新监测---------
    //若一旦后台更新则自动返回登录界面
    public void cheakUPdate()
    {
        StartCoroutine(vesrionupdate());
    }

    IEnumerator vesrionupdate()
    {
        while (true)
        {
            yield return getversioningame(LoginInfo.Instance().mylogindata.URL + LoginInfo.Instance().mylogindata.VersioninfoAPI + "type=" + LoginInfo.Instance().mylogindata.gameType);


            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator getversioningame(string URL)
    {

        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();

        JsonData jd=JsonMapper.ToObject(www.downloadHandler.text);
        if (www.error == null && www.isDone)
        {
            if (jd["code"].ToString() == "200")
            {
#if UNITY_ANDROID
                if (this.mylogindata.version.Equals(jd["androidVersion"].ToString()))
                {
                    //Debug.Log("版本并未更新");
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
#endif
#if UNITY_IOS
                 if (this.mylogindata.iosversion.Equals(jd["iosVersion"].ToString()))
                {
                    //Debug.Log("版本并未更新");
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
#endif
            }
        }
    }



    
    /// <summary>
    /// 超时检测
    /// </summary>
    public void GetOnPing()
    {
        LoginData.IsOnPing = true;
        StartCoroutine(OnWebGet(LoginInfo.Instance().mylogindata.URL + "update-seat?"
            + "game_id=" + LoginInfo.Instance().mylogindata.choosegame
            + "&room_id=" + LoginInfo.Instance().mylogindata.room_id
            + "&user_id=" + LoginInfo.Instance().mylogindata.user_id));
    }

    IEnumerator OnWebGet(string url)
    {
        //等待10秒
        yield return new WaitForSeconds(10f);


        while (LoginData.IsOnPing)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.timeout = 1;
            yield return www.Send();
            Debug.Log("作为存在验证："+www.url);
            if (www.error == null)
            {
                JsonData jd;
                try
                {
                    jd = JsonMapper.ToObject(www.downloadHandler.text);
                    if (jd["code"].ToString() == "200")
                    {
                        if (bool.Parse(jd["bool"].ToString()))
                        {
                            //已超时需要离开房间
                            StartCoroutine(OnWebGet2());

                        }
                    }
                }
                catch (System.Exception)
                {

                }

            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator OnWebGet2()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoginInfo.Instance().mylogindata.URL +
                          "room-end?"
                          + "user_id=" + LoginInfo.Instance().mylogindata.user_id
                          + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame);
        yield return www.Send();
        if (www.error == null)
        {
            JsonData jd;
            try
            {
                jd = JsonMapper.ToObject(www.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    NewTcpNet.IsKick = true;
                    if (NewTcpNet.instance != null)
                    {
                        NewTcpNet.GetInstance().SocketQuit();
                    }
                    DisconnectPanel.GetInstance().Show();
                    DisconnectPanel.GetInstance().Modification("", "长时间未操作，你已被移除房间");
                }
            }
            catch
            {

            }
        }
    }
   
}
