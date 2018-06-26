using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using LitJson;
using System.Text;
using System.Threading;

public class NewTcpNet {

	public static bool IsKick;
	public static bool IsQuit;

    //客户端负责接收服务端发来的数据消息的线程
    Thread threadClient = null;
    //创建客户端套接字，负责连接服务器
    Socket socketClient = null;

    //异步加载相关脚本
    Loom _loom=null;

    //连接检测脚本
    OverDetection detection = null;

    public int prot = 7272;                 //声明int类型变量值为8080
//	public string ip = "127.0.0.1";    
	public string ip = "47.106.66.89";                 //声明int类变量ip，值为"127.0.0.1"


    public static NewTcpNet instance;   //声明Cliet类型变量instance
                                        // Use this for initialization
    public static NewTcpNet GetInstance()
    {
        
        if (instance == null)
        {

            instance = new NewTcpNet();
        }
        return instance;
    }




    /// <summary>
    /// 开启服务
    /// </summary>
    /// <param name="data"></param>
    public NewTcpNet()
    {

        isPlayOver = false;
        //loom = Loom.Current;
        //获得文本框中的IP地址对象
        IPAddress address = IPAddress.Parse(ip);
        //创建包含IP和端口的网络节点对象
        IPEndPoint endpoint = new IPEndPoint(address, prot);
        //创建客户端套接字，负责连接服务器
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        _loom = new GameObject().AddComponent<Loom>();
        _loom.name = "Loom";


        try
        {
            //客户端连接到服务器
            socketClient.Connect(endpoint);
			IsKick=false;
			IsQuit=false;
        }
        catch (SocketException ex)
        {
			Debug.Log("客户端连接服务器发生异常：" + ex.Message);
			DisconnectPanel.GetInstance ().Show ();
			DisconnectPanel.GetInstance ().Modification ("","连接服务器失败！！");
        }
        catch (Exception ex)
        {
			Debug.Log("客户端连接服务器发生异常：" + ex.Message);
			DisconnectPanel.GetInstance ().Show ();
			DisconnectPanel.GetInstance ().Modification ("","连接服务器失败！！");
        }
        Loom.RunAsync(
            () =>
            {
                threadClient = new Thread(ReceiveMsg/*ReceiveMessage*/);  //将ReceiveMsg传入
                threadClient.IsBackground = true;     //设置threadClient变量IsBackground为true
                threadClient.Start();                   //调用Start方法
            }
        );


        OnLogin login = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
        string str = JsonMapper.ToJson(login);
        SendMessage(str);
		//调用SendMessage发送消息到服务器
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="data"></param>
    public void SendMessage(string data)
    {
        //将字符串转成方便网络传送的二进制数组
        byte[] arrMsg = Encoding.UTF8.GetBytes(data);
        byte[] arrMsgSend = new byte[arrMsg.Length];

        Debug.Log("发送登录："+data);

        //Buffer.BlockCopy(arrMsg, 0, arrMsgSend, 1, arrMsg.Length);
        try
        {
            socketClient.Send(arrMsg);
        }
        catch (SocketException ex)
        {
			Debug.Log("客户端发送消息时发生异常：" + ex.Message);

        }
        catch (Exception ex)
        {
			Debug.Log("客户端发送消息时发生异常：" + ex.Message);

        }
    }

    //接收服务器返回的消息
    void ReceiveMsg()
    {
        while (true)
        {
            //定义一个接收消息用的字节数组缓冲区（2M大小）
            byte[] arrMsgRev = new byte[8192];
            //将接收到的数据存入arrMsgRev,并返回真正接收到数据的长度
            int length = -1;
            try
            {
                length = socketClient.Receive(arrMsgRev);

            }
            catch (SocketException ex)
            {
                //System.Console.WriteLine("客户端接收消息时发生异常：" + ex.Message);
              
//				socketClient.Shutdown (SocketShutdown.Both);
//				socketClient.Close ();
				if(!IsKick&&!IsQuit)
				{
					Loom.QueueOnMainThread
					(
						() => {
							//不是被踢 执行显示
							Debug.Log("接收异常"+ ex.ToString());
							DisconnectPanel.GetInstance ().Show ();
							DisconnectPanel.GetInstance ().Modification ("", "与服务器断开连接");
						}
					);
				}
                break;
            }
            catch (Exception ex)
            {
//                System.Console.WriteLine("客户端接收消息时发生异常：" + ex.Message);
              
				if(!IsKick&&!IsQuit)
				{
					Loom.QueueOnMainThread
					(
						() => {
							//不是被踢 执行显示
							Debug.Log("接收异常"+ ex.ToString());
							DisconnectPanel.GetInstance ().Show ();
							DisconnectPanel.GetInstance ().Modification ("", "与服务器断开连接");
						}
					);
				}

                break;
            }

            //此时是将数组的所有元素（每个字节）都转成字符串，而真正接收到只有服务端发来的几个字符
            string strMsgReceive = Encoding.UTF8.GetString(arrMsgRev, 0, length);
            //Debug.Log("接收的原消息格式："+strMsgReceive);
            //Debug.LogError(strMsgReceive);
            //DanTiao.instance.MessageManage(strMsgReceive);
            string[] newStr = strMsgReceive.Split(new string[] { "xx==" }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.LogError(newStr.Length);

            for (int i = 0; i < newStr.Length; i++)
            {
                Debug.Log("接收到的数据为：" + newStr[i]);
                


                try
                {
                    JsonData jd = JsonMapper.ToObject(newStr[i]);
                    LoginData.IsLogin = true;
                    LoginData.OverTime = 0;

                }
                catch (Exception)
                {
                    LoginData.IsLogin = true;
                    LoginData.OverTime = 2;
                }

                Message(newStr[i]);

            }

        }
    }



  	//获得连接状态
	public bool GetConnectionStatus()
	{
		return socketClient.Connected;
	}


    //退出用
    public  void SocketQuit()
    {
        //清理异步处理脚本
        if (_loom!=null)
        {
            MonoBehaviour.Destroy(_loom.gameObject);
        }

        LoginData.IsConnect = false;
        LoginData.IsLogin = false;
        LoginData.IsOnPing = false;
        LoginData.OverTime = 0f;

        //需要添加一个关于离开房间的短链接

        //关闭线程  
        if (threadClient != null)
        {
            threadClient.Interrupt();
            threadClient.Abort();
            threadClient = null;
            instance = null;
        }

        //if (getRecvStrThread != null)
        //{
        //    getRecvStrThread.Interrupt();
        //    getRecvStrThread.Abort();
        //}
        //最后关闭服务器  
        if (socketClient != null)
            /*< span style = "white-space:pre;" > </ span > */
            socketClient.Close();
       
    }

    



	

    #region test

    int mode = LoginInfo.Instance().mylogindata.choosegame;
    public void Message(string str)
    {
		JsonData jd = JsonMapper.ToObject(str);

		//if(jd["type"].ToString()=="ping")
		//{
           
  //      }

		switch (mode)
		{
		case 1:
			//DisposeMpzzs(str);
			DisposelDt(jd);
			break;
		case 2:
			DisposeBl(jd); //百家乐
			break;
		case 3:
			DisposeDs(jd);//单双
			break;
		case 4:
			DisposeElb(jd);
			break;
		case 5:
			DisposeTd(jd);
			break;
		case 6:
			
			break;
		case 7:
			DisposeLh(jd); //此为暂定为龙虎
			break;
		case 8:
			DisposeDZBl (jd);//单张百乐
			break;
            case 10:    //夏威夷
           DisposeXwy(jd);
           break;
		case 111:
			DisposeDxb(jd); //大小豹  TODO
			break;
		case 13: //幸运六狮
                DisposeXYLS(jd);
			break;

		}
    }

    #endregion


    #region mpzzs


    public static string countDown;  //剩余时间
    public static string periods;   //回合数
    public static string season; // 轮数


    #region 总下注
    public static List<string> dnum = new List<string>();


    public static bool isUpdateAllDnum = false;
    #endregion

    public static bool isUpdateRate;



    public static bool isUpdate = false;

    int resend;
    public static bool isFirst = false;



    bool isPlayOver;
	bool IsGet;
    /// <summary>
    ///初期产品
    ///性能低下
    /// </summary>
    /// <param name="str"></param>
    /// 
    void DisposeMpzzs(string str)
    {
      
        JsonData jd = JsonMapper.ToObject(str);
        switch (jd["type"].ToString())
        {
		case "Periods-mpzzs":		//轮询
			//彩金

				
                if (jd["is_empty"].ToString() == "1")
                {
				DanTiao.instance.isOnClear = true;

                }
                if (jd["is_win"].ToString() == "0")
                {
				//底下那张牌出现
				if(IsGet)
				{
					IsGet = false;
					if(jd["open_deal"].ToString()=="0")
					{
						
					}else if(jd["open_deal"].ToString()=="1")
					{
						
					}else if(jd["open_deal"].ToString()=="2")
					{

					}
						
				}
                    LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                    countDown = jd["countdown"].ToString();
                    periods = jd["periods"].ToString();
                    season = jd["season"].ToString();
                    Loom.QueueOnMainThread
                        (
                             () =>
                             {
                                 if (countDown != "0" && !isPlayOver)
                                 {
                                     Audiomanger._instenc.PlayTip(0);
                                     isPlayOver = true;
                                 }
                                 else if (countDown == "0" && isPlayOver)
                                 {
                                     Audiomanger._instenc.PlayTip(1);
							DanTiao.isFengPan = true;
                                     isPlayOver = false;

                                 }
                             }
                        );
                   
                    if (!isUpdate)
                    {
                        isUpdate = true;
                    }
				if (DanTiao.winInfo != "")
                    {
					DanTiao.winInfo = "";
                    }
                    resend = 0;
                    if (!isFirst)
                    {
                        isFirst = true;
                    }
                }
                else if (jd["is_win"].ToString() == "2" && resend != 1 && jd["winnings"].ToString() != "")
                {
                    if (jd["winnings"].ToString() == "")
                    {
					DanTiao.isFengPan = true;
                        countDown = jd["countdown"].ToString();
                    }
                    else
                    {
					DanTiao.isFengPan = false;
                        
                        if (isFirst)
                        {
						DanTiao.is_WinTwo = true;
						DanTiao.winInfo = str;
                            resend = 1;

                        }
                        else
                        {
                            LoginInfo.Instance().mylogindata.dropContent = jd["drop_date"].ToString();
                            countDown = jd["countdown"].ToString();
                            periods = jd["periods"].ToString();
                            season = jd["season"].ToString();
                            if (!isUpdate)
                            {
                                isUpdate = true;
                            }
                        }
                    }
                    

                }


                break;

            case "odd-list-mpzzs":		//下注
                //if (jd["0"]["id"].ToString() == LoginInfo.Instance().mylogindata.user_id)
                //{
                //    return;
                //}
                //Debug.LogError(str);
                dnum.Clear();
                for (int i = 0; i < jd["Oddlist"].Count; i++)
                {
                    //Debug.LogError(jd["Oddlist"][i].ToString() + "*****");
                    dnum.Add(jd["Oddlist"][i].ToString());
                }

                //dnum.Add(jd["Oddlist"][0].ToString());
                //dnum.Add(jd["Oddlist"][1].ToString());
                //dnum.Add(jd["Oddlist"][2].ToString());
                //dnum.Add(jd["Oddlist"][3].ToString());
                //dnum.Add(jd["Oddlist"][4].ToString());
                ////Debug.LogError(jd["Oddlist"][0].ToString());
                isUpdateAllDnum = true;
                break;
            case "start-mpzzs":		//初始化
                for (int i = 0; i < DanTiao.betId.Length; i++)
                {
                    DanTiao.betId[i] = jd["oddlist"][i]["id"].ToString();
                    DanTiao.rateInfo[i] = jd["oddlist"][i]["rate"].ToString();
                    DanTiao.allDnumInfo[i] = jd["oddlist"][i]["dnum"].ToString();
                    DanTiao.selfDnumInfo[i] = jd["oddlist"][i]["user_dnum"].ToString();
                }
                isUpdateRate = true;
                break;


        }
    }
    #endregion

	#region 单挑
	void DisposelDt(JsonData jd)
	{
		switch (jd["type"].ToString()) {
		case "start-mpzzs":		//初始化
			Loom.QueueOnMainThread
			(
				() =>
				{
                    //背景音乐调整
                    Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.GameBG;
                    Audiomanger._instenc.GetComponent<AudioSource>().Play();
                    DanTiao2.instance.UpdateSuit(jd);
				}
			);
			break;
		case "Periods-mpzzs":		//轮询
			Loom.QueueOnMainThread
			(
				() =>
				{
					DanTiao2.instance.PollingPeriods(jd);
				}
			);
			break;
		case "odd-list-mpzzs":		//下注
			Loom.QueueOnMainThread
			(
				() =>
				{
					DanTiao2.instance.OddList(jd);
				}
			);
			break;
		}
	}

	#endregion

    #region bl

    void DisposeBl(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "start-bl"://初始化

                Loom.QueueOnMainThread
           (
               () =>
               {
                   //背景音乐调整
                   Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.GameBG;
                   Audiomanger._instenc.GetComponent<AudioSource>().Play();
                   BaiJiaLe2.instance.UpdateSuit(jd);
               }
           );

                break;
            case "Periods-bl"://游戏轮询信息
                Loom.QueueOnMainThread
                (
                    () =>
                    {
                        BaiJiaLe2.instance.PollingPeriods(jd);
                    }
                );
                break;
            case "odd-list-bl"://历史记录
                Loom.QueueOnMainThread
                (
                    () =>
                    {
                        BaiJiaLe2.instance.OnOddList(jd);
                    }
                );
                break;
        }
    }


    IEnumerator waitIE(JsonData jd)
    {
        yield return new WaitForSeconds(1f);
        DisposeBl(jd);
    }

    #endregion

    #region ds
    void DisposeDs(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-ds":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            DanShuang.instance.PollingPeriods(jd);
                        }
                    );           
                break;
            case "start-ds":        //初始化
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            //背景音乐调整
                            Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.dangshuangBG;
                            Audiomanger._instenc.GetComponent<AudioSource>().Play();
                           DanShuang.instance.UpdateSuit(jd);
                        }
                    );
                break;
            case "odd-list-ds":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            DanShuang.instance.OddList(jd);
                        }
                    );
                break;
        }
    }
    #endregion

    #region 208
    void DisposeElb(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-elb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            TZE.instance.PollingPeriods(jd);
                        }
                    );
                break;
            case "start-elb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            //背景音乐调整
                            Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.GameBG;
                            Audiomanger._instenc.GetComponent<AudioSource>().Play();
                            TZE.instance.UpdateSuit(jd);
                        }
                    );
                break;
            case "odd-list-elb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            TZE.instance.OddList(jd);
                        }
                    );
                break;
        }
    }
    #endregion

    #region TianDi
    void DisposeTd(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-pj":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            TianDi.instance.PollingPeriods(jd);
                        }
                    );
                break;
            case "start-pj":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            TianDi.instance.UpdateSuit(jd);
                        }
                    );
                break;
            case "odd-list-pj":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            TianDi.instance.OddList(jd);
                        }
                    );
                break;
        }

    }
    #endregion

    #region 夏威夷
    void DisposeXwy(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-xwy":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            NewXiaWeiYi.instance.PollingPeriods(jd);
                        }
                    );
                break;
            case "start-xwy":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            NewXiaWeiYi.instance.UpdateSuit(jd);
                        }
                    );
                break;
            case "odd-list-xwy":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            NewXiaWeiYi.instance.OddList(jd);
                        }
                    );
                break;
        }

    }
    #endregion

    #region 龙虎
    void DisposeLh(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-lh":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
					NewLongHu .Instance.PollingPeriods(jd);		//轮询
                        }
                    );
                break;
            case "start-lh":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
					NewLongHu .Instance.UpdateSuit(jd);	//初始化
                        }
                    );
                break;
            case "odd-list-lh":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
					NewLongHu .Instance.OddList(jd);	//下注信息
                        }
                    );
                break;
        }

    }
    #endregion

    #region 大小豹
    void DisposeDxb(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-dxb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            DXB.instance.PollingPeriods(jd);
                        }
                    );
                break;
            case "start-dxb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            DXB.instance.UpdateSuit(jd);
                        }
                    );
                break;
            case "odd-list-dxb":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            DXB.instance.OddList(jd);
                        }
                    );
                break;
        }

    }
    #endregion

    #region 缺一门
    void DisposeQym(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "Periods-qym":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            QueYiMen.instance.PollingPeriods(jd);
                        }
                    );
                break;
            case "start-qym":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            QueYiMen.instance.UpdateSuit(jd); //TODODO
                        }
                    );
                break;
            case "odd-list-qym":
                Loom.QueueOnMainThread
                    (
                        () =>
                        {
                            QueYiMen.instance.OddList(jd); //TODODO
                        }
                    );
                break;
        }

    }
    #endregion

	#region 单张百乐
	void DisposeDZBl(JsonData jd)
	{

		switch (jd["type"].ToString())
		{
		case "Periods-dzbl":
			Loom.QueueOnMainThread
			(
				() =>
				{
					BaiJiaLe3.instance.PollingPeriods(jd);
				}
			);
			break;
		case "start-dzbl":
			Loom.QueueOnMainThread
			(
				() =>
				{
                    //背景音乐调整
                    Audiomanger._instenc.GetComponent<AudioSource>().clip = Audiomanger._instenc.GameBG;
                    Audiomanger._instenc.GetComponent<AudioSource>().Play();
                    BaiJiaLe3.instance.UpdateSuit(jd); 
				}
			);
			break;
		case "odd-list-dzbl":
			Loom.QueueOnMainThread
			(
				() =>
				{
					BaiJiaLe3.instance.OnOddList(jd); 
				}
			);
			break;
		}
	}
    #endregion

    #region 幸运六狮
    void DisposeXYLS(JsonData jd)
    {
        switch (jd["type"].ToString())
        {
            case "start-sd":
                Loom.QueueOnMainThread
                 (
                () =>
                {
                    LoginData.IsLogin = true;
                    LoginData.OverTime = 0;
                    ClickEvent.ins.InsInterf(jd);
                }
                 );

                break;
            case "Periods-sd":
                Loom.QueueOnMainThread
                (
                 () =>
                 {
                     LoginData.IsLogin = true;
                     LoginData.OverTime = 0;
                     ClickEvent.ins.EnterRoomInterf(jd);
                 }
                 );
                break;
            case "odd-list-sd":

                break;
        }
    }
    #endregion

    /* ///<summary>
     ///接收消息
     ///</summary>
     private void ReceiveMessage()
     {
         while (true)
         {
             //接受消息头（消息长度4字节）
             int HeadLength = 4;
             //存储消息头的所有字节数
             byte[] recvBytesHead = new byte[HeadLength];
             //如果当前需要接收的字节数大于0，则循环接收
             while (HeadLength > 0)
             {
                 byte[] recvBytes1 = new byte[4];
                 //将本次传输已经接收到的字节数置0
                 int iBytesHead = 0;
                 //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
                 if (HeadLength >= recvBytes1.Length)
                 {
                     iBytesHead = socketClient.Receive(recvBytes1, recvBytes1.Length, 0);
                 }
                 else
                 {
                     iBytesHead = socketClient.Receive(recvBytes1, HeadLength, 0);
                 }
                 //将接收到的字节数保存
                 recvBytes1.CopyTo(recvBytesHead, recvBytesHead.Length - HeadLength);
                 //减去已经接收到的字节数
                 HeadLength -= iBytesHead;
             }
             //接收消息体（消息体的长度存储在消息头的4至8索引位置的字节里）
             byte[] bytes = new byte[4];
             Array.Copy(recvBytesHead, 0, bytes, 0, 4);
             int BodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0));
             //存储消息体的所有字节数
             byte[] recvBytesBody = new byte[BodyLength];
             //如果当前需要接收的字节数大于0，则循环接收
             while (BodyLength > 0)
             {
                 byte[] recvBytes2 = new byte[BodyLength < 1024 ? BodyLength : 1024];
                 //将本次传输已经接收到的字节数置0
                 int iBytesBody = 0;
                 //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
                 if (BodyLength >= recvBytes2.Length)
                 {
                     iBytesBody = socketClient.Receive(recvBytes2, recvBytes2.Length, 0);
                 }
                 else
                 {
                     iBytesBody = socketClient.Receive(recvBytes2, BodyLength, 0);
                 }
                 //将接收到的字节数保存
                 recvBytes2.CopyTo(recvBytesBody, recvBytesBody.Length - BodyLength);
                 //减去已经接收到的字节数
                 BodyLength -= iBytesBody;
             }
             //一个消息包接收完毕，解析消息包
             UnpackData(recvBytesHead, recvBytesBody);
         }
     }
     /// <summary>
     /// 解析消息包
     /// </summary>
     /// <param name="Head">消息头</param>
     /// <param name="Body">消息体</param>
     public static void UnpackData(byte[] Head, byte[] Body)
     {
         byte[] bytes = new byte[4];
         Array.Copy(Head, 0, bytes, 0, 4);
         Debug.Log("接收到数据包中的校验码为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

         //bytes = new byte[8];
         //Array.Copy(Head, 8, bytes, 0, 8);
         //Debug.Log("接收到数据包中的身份ID为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt64(bytes, 0)));

         //bytes = new byte[4];
         //Array.Copy(Head, 16, bytes, 0, 4);
         //Debug.Log("接收到数据包中的数据主命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

         //bytes = new byte[4];
         //Array.Copy(Head, 20, bytes, 0, 4);
         //Debug.Log("接收到数据包中的数据子命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

         //bytes = new byte[4];
         //Array.Copy(Head, 24, bytes, 0, 4);
         //Debug.Log("接收到数据包中的数据加密方式为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

         bytes = new byte[Body.Length];
         for (int i = 0; i < Body.Length;)
         {
             byte[] _byte = new byte[4];
             Array.Copy(Body, i, _byte, 0, 4);
             i += 4;
             int num = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_byte, 0));

             _byte = new byte[num];
             Array.Copy(Body, i, _byte, 0, num);
             i += num;
             Debug.Log("接收到数据包中的数据有：" + Encoding.UTF8.GetString(_byte, 0, _byte.Length));
         }
     }*/

}
