using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using LitJson;
public class ZoomEnter : MonoBehaviour {
 //   public string GameIDUrl = "http://3d-web.weiec4.cn/api/game-list";
    //登录接口
   
	//异步对象  
	private AsyncOperation _asyncOperation;

	public GameObject panel;

	private bool isload=false;
	//显示tips的文本
	//private Text _tip;
	//tips的集合
	//更新tips的时间间隔
	private const float _updateTime = 1f;
	//上次更新的时间
	private float _lastUpdateTime = 0;
	//滑动条
	private Slider _slider;
	//显示进度的文本
	private Text _progress;
	// Use this for initialization
	void Start ()
	{
		//_tip = GameObject.FindChild("Text").GetComponent<Text>();
		_slider = panel.transform.Find("Slider").GetComponent<Slider>();
		_progress =panel.transform.Find("Text").GetComponent<Text>();
       // DontDestroyOnLoad(this.gameObject);
	}

	public void OnClick(){
		
	}

    public void Login() {
       panel.SetActive (true);
		isload = true;
		StartCoroutine(LoadAsync("level_1017"));	
      
    }

    //得到数据
    public IEnumerator RecevedURL(string url)
    {

        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(url);
        //  ChipScript.ins.uilabel.text = url;
        www.timeout = 3;
        yield return www.Send();


        if (www.error == null && www.isDone)
        {
            if (www.responseCode == 200)
            {

                JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
                ReceiveData(jd,url);
             
                //        ChipScript.ins.uilabel.text = jd.ToJson().ToString();
            }
            else
            {

            }
        }
        else
        {
            ChipScript.ins.Tip("服务器繁忙");
        }
    }
    public void ReceiveData(JsonData jd, string str)
    {
        
        //登录
    }

	// Update is called once per frame
	void Update ()
	{
		//首先判断是否为空，其次判断是否加载完毕
		if (_asyncOperation != null && !_asyncOperation.allowSceneActivation && isload==true)
		{
			//开始更新tips
			if (Time.time - _lastUpdateTime >= _updateTime)
			{
				_lastUpdateTime = Time.time;

			}
		}
	}
	/// <summary>
	/// 设置加载标签
	/// </summary>

	/// <summary>
	/// 携程进行异步加载场景
	/// </summary>
	/// <param name="sceneName">需要加载的场景名</param>
	/// <returns></returns>
	IEnumerator LoadAsync(string sceneName)
	{
		//当前进度
		int currentProgress = 0;
		//目标进度
		int targetProgress = 0;
		_asyncOperation = Application.LoadLevelAsync(sceneName);
		//unity 加载90%
		_asyncOperation.allowSceneActivation = false;
		while (_asyncOperation.progress<0.9f)
		{
			targetProgress = (int) _asyncOperation.progress*100;
			//平滑过渡
			while (currentProgress<targetProgress)
			{
				++currentProgress;
				_progress.text= String.Format("{0}{1}",currentProgress.ToString(),"%");
				_slider.value = (float) currentProgress/100;
				yield return new WaitForEndOfFrame();
			}
		}
		//自行加载剩余的10%
		targetProgress = 100;
		while (currentProgress < targetProgress)
		{
			++currentProgress;
			_progress.text = String.Format("{0}{1}", currentProgress.ToString(), "%");
			_slider.value = (float)currentProgress / 100;
			yield return new WaitForEndOfFrame();
		}
		_asyncOperation.allowSceneActivation = true;
		panel.SetActive (false);
	}
}
