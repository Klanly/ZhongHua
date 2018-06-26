using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{

    static int SCREEN_WIDTH = 960;
    static int SCREEN_HEIGHT = 640;
    int mScreenWidth;
    int mScreenHeight;

    public UIRoot root;
    // Use this for initialization
    void Awake()
    {
        mScreenWidth = SCREEN_WIDTH;
        mScreenHeight = SCREEN_HEIGHT;
        root = GetComponent<UIRoot>();
        ScaleScreen();
#if UNITY_EDITOR
        UICamera.onScreenResize += ScaleScreen;
#endif
    }

    void ScaleScreen()
    {
        if ((float)Screen.width / (float)Screen.height < (float)SCREEN_WIDTH / (float)SCREEN_HEIGHT)
        {
            root.manualHeight = SCREEN_WIDTH * Screen.height / Screen.width;
        }
        else
        {
            root.manualHeight = (int)SCREEN_HEIGHT;
        }
    }
}