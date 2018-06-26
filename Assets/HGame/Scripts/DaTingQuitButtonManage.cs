using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaTingQuitButtonManage : MonoBehaviour
{

    public GameObject ChangePasswordPanel; //修改密码面板

    /// <summary>
    /// 切换账号
    /// </summary>
    public void SwitchAccountButton()
    {
        //退出账号

        //返回登录界面
        SceneManager.LoadScene("Register");
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public void ChangePasswordButton()
    {
        //打开修改密码菜单  

        
    }

    /// <summary>
    /// 关闭游戏
    /// </summary>
    public void QuitButton()
    {
        //退出账号

        //关闭游戏
        Application.Quit();
    }

}
