using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNet : MonoBehaviour
{

    private string text = "http://47.106.66.89:81/pj-api/login?username=zhou&password=123456";

    public void StartButton()
    {
        StartCoroutine(HGameTcpNet.Instance.SendVoid(text));
    }
}
