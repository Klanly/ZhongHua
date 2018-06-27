using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;


public class AndroidImpl : MonoBehaviour
{
    private AndroidJavaObject jo;

    public Button[] btn;
    public Text[] text;

#if UNITY_IPHONE
        /* Interface to native implementation */
        [DllImport ("__Internal")]
        private static extern void _copyTextToClipboard(string text);
#endif


    void Start()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            int num = i;
            btn[num].onClick.AddListener(delegate ()
            {
                OnCopy(text[num].text);
            });
        }

#if UNITY_ANDROID

        AndroidJavaClass jc = new AndroidJavaClass("product.company.com.unity_exchange.MainActivity"); //和java代码包名统一  
        jo = jc.CallStatic<AndroidJavaObject>("GetInstance", gameObject.name); //Main Camera  

#endif
    }

    public void OnCopy(string text)
    {
       if (text == "")
        {
            text = "太阳骑士日神仙";
        }
       
         
        //if (text == "") return;


#if UNITY_ANDROID
        jo.Call("onClickCopy", text);
#elif UNITY_IPHONE
         _copyTextToClipboard(text);
#endif
    }


}
