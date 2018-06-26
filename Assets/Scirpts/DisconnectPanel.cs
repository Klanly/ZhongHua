using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 功能：断开连接后的操作界面
/// </summary>
public class DisconnectPanel : MonoBehaviour {

	private static DisconnectPanel instance;
	private Text TitleText;
	private Text ContentText;
	private Button ResetButton;
	private Button ReturnButton;

	public static DisconnectPanel GetInstance()
	{
		if(instance==null)
		{
			instance = Instantiate (Resources.Load<DisconnectPanel>("DisconnectPanel"),GameObject.Find("Canvas").transform);
			instance.TitleText = instance.transform.Find ("Title").GetComponent<Text>();
			instance.ContentText = instance.transform.Find ("Content").GetComponent<Text>();
			instance.ResetButton=instance.transform.Find ("Reset").GetComponent<Button>();
			instance.ReturnButton = instance.transform.Find ("Return").GetComponent<Button>();

			instance.ResetButton.onClick.AddListener (instance.Reset_Method);
			instance.ReturnButton.onClick.AddListener (instance.Return_Method);

			instance.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
			instance.GetComponent<RectTransform> ().sizeDelta = Vector2.zero;
			instance.GetComponent<RectTransform> ().localScale = Vector3.one;
		}


		return instance;
	}


//	public void AddParent(Transform tra)
//	{
//		instance.transform.SetParent (tra);
//		instance.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
//		instance.GetComponent<RectTransform> ().sizeDelta = Vector2.zero;
//		instance.GetComponent<RectTransform> ().localScale = Vector3.zero;
//	}


	public void Show()
	{
		this.transform.SetAsLastSibling ();
		this.gameObject.SetActive (true);
	}


	public void Hide()
	{
		this.gameObject.SetActive (false);
	}


	//修改标题和内容
	public void Modification(string title,string Content)
	{
		TitleText.text = title;
		ContentText.text = Content;

	}

	/// <summary>
	/// 按钮方法【重新登录】
	/// </summary>
	private void Reset_Method()
	{
		
//		if(NewTcpNet.instance!=null)
//		{
//			//检测是否已经断开 还是单纯被踢出房间
//			if (NewTcpNet.instance.GetConnectionStatus()) {
//				//还连接着
//				SceneManager.LoadSceneAsync(LoginInfo.Instance().mylogindata.choosegame+1);
//			} else {
//				//已经断开了
//				SceneManager.LoadSceneAsync(LoginInfo.Instance().mylogindata.choosegame+1);
//			}
//		}


	}

	/// <summary>
	/// 按钮方法【返回大厅】
	/// </summary>
	public void Return_Method()
	{
		if (NewTcpNet.instance != null) {
			//检测是否已经断开 还是单纯被踢出房间
			if (NewTcpNet.instance.GetConnectionStatus ()) {
				//还连接着
				NewTcpNet.instance.SocketQuit ();
				Audiomanger._instenc.GetComponent<AudioSource> ().clip = Audiomanger._instenc.backgroundmusic;
				Audiomanger._instenc.GetComponent<AudioSource> ().Play ();
				SceneManager.LoadScene (1);
			} else {
				//已经断开了
				NewTcpNet.instance.SocketQuit ();
				Audiomanger._instenc.GetComponent<AudioSource> ().clip = Audiomanger._instenc.backgroundmusic;
				Audiomanger._instenc.GetComponent<AudioSource> ().Play ();
				SceneManager.LoadScene (1);
			}
		} else {
			Audiomanger._instenc.GetComponent<AudioSource> ().clip = Audiomanger._instenc.backgroundmusic;
			Audiomanger._instenc.GetComponent<AudioSource> ().Play ();
			SceneManager.LoadScene (1);
		}

	}
}
