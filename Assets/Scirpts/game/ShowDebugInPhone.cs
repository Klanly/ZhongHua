using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class logdata
{
    public string output = "";
    public string stack = "";
    
    public static logdata Init(string o, string s)
    {
        logdata log = new logdata();
        log.output = o;
        log.stack = s;
        return log;
    }
    public void Show(Color setcolor)
    {
        GUIStyle tempstyle = new GUIStyle();
        tempstyle.fontSize = 40;
        tempstyle.normal.textColor = setcolor;

        GUILayout.Label(output,tempstyle);
        //if (showstack)  
        GUILayout.Label(stack);
    }
}
/// <summary>  
/// 手机调试脚本  
/// 本脚本挂在一个空对象或转换场景时不删除的对象即可  
/// 错误和异常输出日记路径 Application.persistentDataPath  
/// </summary>  
public class ShowDebugInPhone : MonoBehaviour
{

    List<logdata> logDatas = new List<logdata>();//log链表  
    List<logdata> errorDatas = new List<logdata>();//错误和异常链表  
    List<logdata> warningDatas = new List<logdata>();//警告链表  
    private GUIStyle guilabel;

    static List<string> mWriteLogTxt = new List<string>();
    static List<string> mWriteErrorTxt = new List<string>();
    static List<string> mWriteWarningTxt = new List<string>();
    Vector2 uiLog;
    Vector2 uiError;
    Vector2 uiWarning;
    bool open = false;
    bool showLog = false;
    bool showError = false;
    bool showWarning = false;
    private string outpathLog;
    private string outpathError;
    private string outpathWarning;
    void Start()
    {
        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。  
        //Debug.Log(Application.persistentDataPath);
        
#if UNITY_EDITOR
        outpathLog = Application.dataPath + "/StreamingAssets" + "/outLog.txt";
        outpathError = Application.dataPath + "/StreamingAssets" + "/outLogError.txt";
        outpathWarning = Application.dataPath + "/StreamingAssets" + "/outLogWarining.txt";
#elif UNITY_IPHONE
        outpathLog = Application.dataPath +"/Raw"+"/outLog.txt";
        outpathError = Application.dataPath +"/Raw"+"/outLogError.txt";
        outpathWarning = Application.dataPath +"/Raw"+"/outLogWarining.txt";
#elif UNITY_ANDROID
        outpathLog = Application.persistentDataPath + "/outLog.txt";
        outpathError = Application.persistentDataPath + "/outLogError.txt";
        outpathWarning = Application.persistentDataPath + "/outLogWarining.txt";
#endif
        //每次启动客户端删除之前保存的Log  
        if (System.IO.File.Exists(outpathLog))
        {
            File.Delete(outpathLog);
        }
        if (System.IO.File.Exists(outpathError))
        {
            File.Delete(outpathError);
        }
        if (System.IO.File.Exists(outpathWarning))
        {
            File.Delete(outpathWarning);
        }
        //转换场景不删除  
        Object.DontDestroyOnLoad(gameObject);
        
    }
    void OnEnable()
    {
        //注册log监听  
        //Application.RegisterLogCallback(HangleLog);
        Application.logMessageReceived += (HangleLog);
    }

    void OnDisable()
    {
        // Remove callback when object goes out of scope  
        //当对象超出范围，删除回调。  
        Application.RegisterLogCallback(null);
    }
    void HangleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                logDatas.Add(logdata.Init(logString, stackTrace));
                mWriteLogTxt.Add(logString);
                mWriteLogTxt.Add(stackTrace);
                break;
            case LogType.Error:
            case LogType.Exception:
                errorDatas.Add(logdata.Init(logString, stackTrace));
                mWriteErrorTxt.Add(logString);
                mWriteErrorTxt.Add(stackTrace);
                break;
            case LogType.Warning:
                warningDatas.Add(logdata.Init(logString, stackTrace));
                mWriteWarningTxt.Add(logString);
                mWriteWarningTxt.Add(stackTrace);
                break;
        }
    }
    void Update()
    {
        //因为写入文件的操作必须在主线程中完成，所以在Update中才给你写入文件。  
        if (logDatas.Count > 0)
        {
            string[] temp = mWriteLogTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpathLog, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteLogTxt.Remove(t);
            }
        }

        if (errorDatas.Count > 0)
        {
            string[] temp = mWriteErrorTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpathError, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteErrorTxt.Remove(t);
            }
        }

        if (warningDatas.Count > 0)
        {
            string[] temp = mWriteWarningTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpathWarning, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteWarningTxt.Remove(t);
            }
        }

    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(">>Open", GUILayout.Height(150), GUILayout.Width(150)))
            open = !open;
        if (open)
        {
            if (GUILayout.Button("清理", GUILayout.Height(150), GUILayout.Width(150)))
            {
                logDatas = new List<logdata>();
                errorDatas = new List<logdata>();
                warningDatas = new List<logdata>();
            }
            if (GUILayout.Button("显示log日志:" + showLog, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showLog = !showLog;
                if (open == true)
                    open = !open;
            }
            if (GUILayout.Button("显示error日志:" + showError, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showError = !showError;
                if (open == true)
                    open = !open;
            }
            if (GUILayout.Button("显示warning日志:" + showWarning, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showWarning = !showWarning;
                if (open == true)
                    open = !open;
            }
        }
        GUILayout.EndHorizontal();
        if (showLog)
        {
            GUI.color = Color.white;
            
            uiLog = GUILayout.BeginScrollView(uiLog);
            foreach (var va in logDatas)
            {
                va.Show(Color.white);
            }
            GUILayout.EndScrollView();
        }
        if (showError)
        {
            GUI.color = Color.red;
            uiError = GUILayout.BeginScrollView(uiError);
            foreach (var va in errorDatas)
            {
                va.Show(Color.red);
            }
            GUILayout.EndScrollView();
        }
        if (showWarning)
        {
            GUI.color = Color.yellow;
            uiWarning = GUILayout.BeginScrollView(uiWarning);
            foreach (var va in warningDatas)
            {
                va.Show(Color.yellow);
            }
            GUILayout.EndScrollView();
        }
    }
}