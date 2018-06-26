using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenOrClose : MonoBehaviour
{
    public GameObject TuiChu;

   
    private void OnMouseDown()
    {
        TuiChu.SetActive(true);
    }
    
    /// <summary>
    /// 取消按钮
    /// </summary>
    public void OnOFFButton()
    {
        TuiChu.SetActive(false);
    }
}
