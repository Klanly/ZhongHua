using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{

    public GameObject _HelpPanel;


    /// <summary>
    /// 退出后台
    /// </summary>
    public void QuitButton()
    {
        _HelpPanel.SetActive(false);
    }
}
