using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameObject : MonoBehaviour
{
    public bool IsDontDestroyOnLoad = true;                 //是否保留下来
    public bool DontCreateNewWhenBackToThisScene = true;    // 不要在回到这个场景时创建新的
    public static SaveGameObject Instance = null;

    void Awake()
    {
        if (this.IsDontDestroyOnLoad)
            GameObject.DontDestroyOnLoad(this);
    }
}
