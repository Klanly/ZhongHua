using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openUserPanel : MonoBehaviour
{
    public hallInfo openhallinfo;

    private void OnEnable()
    {
        openhallinfo.getdatatoui(openhallinfo.gamelistinfo);
        openhallinfo.Pagecout = 0;
    }
}
