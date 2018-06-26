using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketHttp : MonoBehaviour {
    private string m_info = string.Empty;
	// Use this for initialization
	void Start () {
        StartCoroutine(IPostData());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator IPostData()
    {

        WWWForm form = new WWWForm();
        //添加字段（键，值）  
        form.AddField("game_id", "1");
        form.AddField("user_id", "3");
        form.AddField("unionuid", "201806081406562518896");
        //向HTTP服务器提交Post数据，提交表单  
        WWW www = new WWW("http://3d-web.weiec4.cn/api/room-start?game_id=1&user_id=3&unionuid=201806081406562518896", form);

        //等待服务器的响应  
        yield return www;

        //如果出现错误  
        if (www.error != null)
        {
            //获取服务器的错误信息  
            m_info = www.error;
            yield return null;
        }

        //获取服务器的响应文本  
        m_info = www.text;
        GetData(m_info);
    }

    public void GetData(string str) {
        Debug.Log(str);
    
    }
}
