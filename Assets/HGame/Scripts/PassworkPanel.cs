using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassworkPanel : MonoBehaviour
{
    public GameObject passworkPanel;

    /// <summary>
    /// 确定按钮
    /// </summary>
    public void ConfirmButton()
    {
        //对密码进行处理
    }


    /// <summary>
    /// 退出按钮（取消按钮）
    /// </summary>
    public void QuitButton()
    {
        passworkPanel.SetActive(false);
    }

}
