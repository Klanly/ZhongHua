using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//小型提示的脚本
public class changetext : MonoBehaviour {
    private GameObject go;
    //private Vector3 view;
    private void Awake()
    {
        go = this.gameObject;
    }


    // Use this for initialization
    void Start () {

        StartCoroutine(changeaphle());
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void msgset(string value)
    {
        go.GetComponent<Text>().text = value;
    }

    public void changefrontsize(int size)
    {
        go.GetComponent<Text>().fontSize = size; 
    }

    /// <summary>
    /// 字体修改由有色边透明 然后销毁
    /// </summary>
    /// <returns></returns>
    IEnumerator changeaphle()
    {
        int temp = 10;
        while (true)
        {
            go.GetComponent<RectTransform>().localPosition+=new Vector3(0,2,0);
            temp--;
            if (temp % 3 == 0)
            {
             go.GetComponent<Text>().color -= new Color(0, 0, 0, 0.02f);
             
            }

            if (temp <= 0)
            {
                temp = 10;
            }

         yield return new WaitForSeconds(0.05f);
            if (go.GetComponent<Text>().color.a <= 0)
            {
                break;
            }
        }
        Debug.Log("结束演示");
        DestroyObject(go, 0.2f);
    }
}
