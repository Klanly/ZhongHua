using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System;
using LitJson;

public class TcpNet
{
    private static TcpNet instance;
    Socket serverSocket;
	private string ClientIP = "47.106.66.89"; //IP地址
    IPAddress ip; //主机ip
    IPEndPoint iPEnd;
    string recvStr; 
    string sendStr;  
    byte[] recvData = new byte[1024]; //接收到的数据
    byte[] sendData = new byte[1024]; //发送的数据
    int recvLen = 0; //接收的数据的长度
    Thread connectThread; //连接线程
    Thread getRecvStrThread; //接收消息线程
    private Queue<string> queue = new Queue<string>(); //储存服务器发来的消息

	// Use this for initialization
	

    public static TcpNet GetInstance()
    {
        if (instance == null)
        {

            instance = new TcpNet();
        }
        return instance;
    }

    /// <summary>
    /// 构造函数
    /// 用来初始化链接
    /// </summary>
    public TcpNet()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ip = IPAddress.Parse(ClientIP);
        iPEnd = new IPEndPoint(ip, 7272);
        IAsyncResult result = serverSocket.BeginConnect(iPEnd, new AsyncCallback(ConnectCallBack), serverSocket);
        bool success = result.AsyncWaitHandle.WaitOne(5000, true);
        if (!success)
        {
            Debug.LogError("链接失败");
            //如果连接超时
            SocketQuit();
            

        }
        else
        {
            Debug.LogError("正在监听消息");
            //开启一个线程连接 接收服务器发来的消息
            connectThread = new Thread(new ThreadStart(SocketReceive));
        }
        Debug.Log("登录");
    }

    /// <summary>
    /// 连接回调
    /// </summary>
    /// <param name="ia"></param>
    void ConnectCallBack(IAsyncResult ia)
    {
        Debug.LogError("服务器连接成功");

        OnLogin login = new OnLogin("Login", LoginInfo.Instance().mylogindata.user_id, LoginInfo.Instance().mylogindata.room_id.ToString(), LoginInfo.Instance().mylogindata.choosegame.ToString());
        string str = JsonMapper.ToJson(login);
        SocketSend(str);

    }


    /// <summary>
    /// 连接退出
    /// </summary>
    void SocketQuit()
    {
        Debug.LogError("服务器断开连接");
    }

    void SocketReceive()
    {
        while (true)
        {
            if (!serverSocket.Connected)
            {
                //与服务器断开连接跳出循环
                Debug.Log("服务器断开连接");
                serverSocket.Close();
                break;
            }
            try
            {
                //接收数据保存至bytes当中
                recvData = new byte[4096];
                recvLen = serverSocket.Receive(recvData);
                //if (recvLen <= 0)
                //{
                //    //serverSocket.Close();
                //    break;
                //}
                if (recvData.Length > 0)
                {
                    recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                    Debug.LogError("服务器信息"+recvStr);
                }
                
            }
            catch (Exception)
            {
                Debug.LogError("错误");
                throw;
            }
        }
    }

    public void SocketSend(string str)
    {
        sendData = new byte[4096];
        sendData = Encoding.UTF8.GetBytes(str);
        if (!serverSocket.Connected)
        {
            serverSocket.Close();
            return;
        }
        try
        {
            IAsyncResult asyncSend = serverSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallBack), serverSocket);
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                serverSocket.Close();
              
                Debug.LogError("Failed to SendMessage server.");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    void SendCallBack(IAsyncResult ar)
    {
        Debug.LogError("发送成功");
    }

}



