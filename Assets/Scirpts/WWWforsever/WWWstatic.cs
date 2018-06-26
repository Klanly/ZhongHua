using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;


public enum listtype
{
    listforlogininfo=1,
    listforhallinfo=2,
    listforgameinfo=3
}
public enum action
{
    login=1,
    reganstion,
    isalive,
    gethallgame,
    getroomlist,
    
}

public class wwwinfo
{
    //public action returnstate;
    public int statetype;
    public string textinfo;//虽保留了整个类 但有可能会让传输的信息结构体变臃肿
}

public class WWWstatic : MonoBehaviour {

    //旧方法使用的委托与事件
    public delegate void mymessagetlist(UnityWebRequest unitywebinfo);
    public event mymessagetlist logincallback;    //登录事件
    public event mymessagetlist regantsionback;   //注册事件
    public event mymessagetlist isaliveeventback; //检查在线事件
    public event mymessagetlist gamelisteventback;//获取游戏列表事件
    public event mymessagetlist roomlistevnetback;//获取房间列表事件
    IEnumerator loginIE;
    IEnumerator reasgatinID;


    //新方法使用的委托与事件
    public delegate void mymessagetowwwlist(wwwinfo infomessage);

    public event mymessagetowwwlist gamestart;



    //public event mymessagetowwwlist logincallbackforww;

    //static WWWstatic _instence;
    public Queue<UnityWebRequest> listforlogininfo;//登录相关信息
    public Queue<UnityWebRequest> listforhallinfo;//游戏大厅相关信息
    public Queue<UnityWebRequest> listforgameinfo;//游戏中的相关信息

    public Queue<wwwinfo> listformwwwinfo;
    public GameObject lockgo;

    //public static WWWstatic Instence
    //{
    //    get
    //    {
    //        if (_instence == null)
    //        {
                
    //            _instence = new WWWstatic();
    //        }

    //        return _instence;
    //    }

       
    //}

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public  IEnumerator  sendcoutdown(string URL)
    {
       yield return   StartCoroutine(couwnt(URL));
    }

    //public void getuserinfoingame(string URL)
    //{
        
    //}


    //重新改写的新方法 v0.1
    //public void sendinfotolist(string URL, action state,bool isconver)
    //{
    //    StartCoroutine(wwwinfosend(URL, state, isconver));
    //}


    public  void sendWWW(string URL, listtype state,action act,bool isconver)
    {
        loginIE = wwwtosever(URL, state, act, isconver);
        StartCoroutine( loginIE);
        
    }

    public void stopieinlogin()
    {
        StopCoroutine(loginIE);
    }


    IEnumerator wwwtosever(string URL,listtype state,action act,bool isconver)
    {
        if (URL != null)
        {
            UnityWebRequest www = UnityWebRequest.Get(URL);
			Debug.Log (URL);
            yield return www.Send();


            wwwinfo temp = new wwwinfo();

            

            if (www.error==null) 
            {



                if (state == listtype.listforlogininfo)
                {
                    //temp.statetype = (int)state;
                    //temp.textinfo = www.downloadHandler.text;
                    LoginInfo.Instance().wwwinstance.listforlogininfo.Enqueue(www);
                }
                else if (state == listtype.listforhallinfo)
                {
                    //temp.statetype = (int)state;
                    //temp.textinfo = www.downloadHandler.text;
                    LoginInfo.Instance().wwwinstance.listforhallinfo.Enqueue(www);
                }
                else if (state == listtype.listforgameinfo)
                {
                    //temp.statetype = (int)state;
                    //temp.textinfo = www.downloadHandler.text;
                    LoginInfo.Instance().wwwinstance.listforgameinfo.Enqueue(www);
                }
                /// 使用了事件跟委托 进行了观察者模式的使用
                switch (act)
                {
                    case action.login:
                        if (logincallback != null)
                        {
                            logincallback(LoginInfo.Instance().wwwinstance.listforlogininfo.Dequeue());
                        }
                        break;
                    case action.reganstion:
                        if (regantsionback != null)
                        {
                            regantsionback(LoginInfo.Instance().wwwinstance.listforlogininfo.Dequeue());
                        }
                        break;
                    case action.isalive:
                        if (isaliveeventback != null)
                        {
                            isaliveeventback(LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
                        }

                        break;
                    case action.gethallgame:
                        if (gamelisteventback != null)
                        {
                            gamelisteventback(LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
                        }
                        break;
                    case action.getroomlist:
                        if (roomlistevnetback != null)
                        {
                            roomlistevnetback(LoginInfo.Instance().wwwinstance.listforhallinfo.Dequeue());
                        }
                        break;
                    default:
                        break;
                }
            }
            else 
            {
                if (isconver == true)
                {
                    StartCoroutine(wwwtosever(URL, state, act, isconver));

                }
            }
        }

            //Debug.Log("已进入对象");
        
    }

    //IEnumerator wwwinfosend(string URL, action state,bool isconver)
    //{

    //    ///实现的信息传递模块 首先获取从网络的信息并加返回的信息自定义成自己的类 随后进行处理通过标记为与事件调用对应的方法
    //    ///此处自定义类为 wwwinfo 里面包含了一个状态头与一个返回的信息内容 但因使用一个队列所有操纵时需要对队列进行加锁


    //    if (URL != null)
    //    {
    //        UnityWebRequest www = UnityWebRequest.Get(URL);
    //        yield return www.Send();


    //        if (www.error == null)
    //        {
    //            wwwinfo tempinfo = new wwwinfo();
    //            tempinfo.returnstate = state;
    //            tempinfo.textinfo = www;

    //            lock (lockgo)
    //            {
                    
    //            //同一对象多个方法同时调用考虑锁的问题
    //            LoginInfo.Instance().wwwinstance.listformwwwinfo.Enqueue(tempinfo);


    //            switch (LoginInfo.Instance().wwwinstance.listformwwwinfo.Peek().returnstate)
    //            {
    //                case action.login:

    //                    break;
    //                case action.reganstion:
    //                    break;
    //                case action.isalive:
    //                    break;
    //                case action.gethallgame:
    //                    break;
    //                case action.getroomlist:
    //                    break;
    //                default:
    //                    break;
    //            }

    //            }
    //        }
    //        else
    //        {
    //            //非正常的情况下重连
    //            if (isconver == true)
    //            {
    //                StartCoroutine(wwwinfosend(URL, state, isconver));

    //            }

    //        }
    //    }
    //}

    /// <summary>
    /// 重复监测时候有版本改变
    /// </summary>
    /// <param name="URL"></param>
    /// <returns></returns>
    IEnumerator couwnt(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.Send();

        JsonData jd= JsonMapper.ToObject(www.downloadHandler.text);
        if (www.error == null)
        {

            wwwinfo temp = new wwwinfo();

            if (jd["code"].ToString() == "200")
            {
                temp.statetype = (int)jd["info"]["is_open"];
                temp.textinfo = www.downloadHandler.text;

                if (listformwwwinfo.Count > 0)
                {

                    if (GameMagert.iscomeback==true)
                    {
                        Debug.LogError("中断之后弹出信息" + listformwwwinfo.Dequeue());
                        listformwwwinfo.Enqueue(temp);

                        //GameMagert.isupdatetime = true;
                        GameMagert.iscomeback = false;
                        GameMagert.swithonislistchange = true;
                        Debug.LogError("完成中断后的更新");
                        yield break;
                    }
                    ///当消息队列的
                    if (listformwwwinfo.Peek().statetype == temp.statetype)
                    {
                       
                    }
                    else
                    {
                        listformwwwinfo.Enqueue(temp);
                    }
                }
                else
                {
                    listformwwwinfo.Enqueue(temp);
                }
            }
            else
            {
                Debug.Log(jd["msg"].ToString());
                
            }


         
        }
        else
        {
            Debug.Log(www.error);
        }

    }

    //public void changelogin()
    //{
    //    if (logincallback != null)
    //    {
    //        logincallback(LoginInfo.Instance().wwwinstance.listforlogininfo.Dequeue());
    //    }
    //}
    //public void changereagation()
    //{
    //    if (regantsionback != null)
    //    {
    //        regantsionback(LoginInfo.Instance().wwwinstance.listforlogininfo.Dequeue());
    //    }
    //}
    //public void changegamechoose()
    //{
    //    if (isaliveeventback != null)
    //    {
    //        isaliveeventback(LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
    //    }
    //}
    //public void changegamechooselist()
    //{
    //    if (gamelisteventback != null)
    //    {
    //        gamelisteventback(LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
    //    }
    //}

    //public void change

}
