using UnityEngine;
using System.Collections;

public class Serialportmanagement : MonoBehaviour
{
    public bool IsDontDestroyOnLoad = true;             //是否保留下来
    public bool DontCreateNewWhenBackToThisScene = true;// 不要在回到这个场景时创建新的
    public static Serialportmanagement Instance = null;

    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            
            return;
        }
        Instance = this;
        if (this.IsDontDestroyOnLoad)
            GameObject.DontDestroyOnLoad(this);
    }
}
