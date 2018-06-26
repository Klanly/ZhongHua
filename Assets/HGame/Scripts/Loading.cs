using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Image loading;

    private float num = 0;

    private void Update()
    {
        if (num < 1)
            num += 0.01f;
        loading.fillAmount = num;

        if (num >= 1)
        {
            //进度条满足，跳转场景
            SceneManager.LoadScene("Register");
        }
    }
}
