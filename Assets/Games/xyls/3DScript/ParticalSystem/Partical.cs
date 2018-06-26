using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partical:MonoBehaviour  {

    /// <summary>
    /// 例子单例类
    /// </summary>

    public static Partical instance;

    public GameObject WinPartical;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 例子关闭
    /// </summary>

    public  void OrOpen(bool isshow) {
        WinPartical.SetActive(isshow);
    }
}
