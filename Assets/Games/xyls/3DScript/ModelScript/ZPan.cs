using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPan : MonoBehaviour
{


    public float endAngle = 100;//旋转停止位置，相对y坐标方向的角度

    public Vector3 targetDir;//目标点的方向向量
    bool isMoving = false;//是否在旋转
    public float speed = 0;//当前的旋转速度
    public float maxSpeed = 0.02f;//最大旋转速度
    public float minSpeed = 0.002f;//最小旋转速度
    float rotateTimer = 2;//旋转计时器
    public int moveState = 0;//旋转状态，旋转，减速

    bool isMoving1 = false;//是否在旋转
    public float speed1 = 0;//当前的旋转速度
    public float maxSpeed1 = 0.018f;//最大旋转速度
    public float minSpeed1 = 0.002f;//最小旋转速度
    public int moveState1 = 0;//旋转状态，旋转，减速
    // public int keepTime = 27;//旋转减速前消耗的时间

    int i = 0;
    //按照顺时针方向递增



    private enum TipType
    {
        zhuang,
        xian,
        he
    }
    ///// <summary>
    /////  动物名字
    ///// </summary>
    //public string str;
    ///// <summary>
    ///// 庄和闲
    ///// </summary>
    //public string ZHX=string.Empty;

    private TipType tiptype = TipType.zhuang;

    public Transform zp;

    public GameObject Banker;
    //持续时间
    public float time;

    /// <summary>
    /// 是否开始
    /// </summary>
    /// 
    public static ZPan ins;
    private float zpInitSpeed;
    private float ModelSpeed;
    private float mdelta = 0.5f;

    private GameObject MainCamera;

    public Transform Model;

    /// <summary>
    /// 记录刚开始的动画的位置
    /// </summary>
    private List<Vector3> PosVec3 = new List<Vector3>();

    /// <summary>
    /// 记录角度
    /// </summary>

    private List<Vector3> EulerVec3 = new List<Vector3>();

    private Vector3 ToEuler = new Vector3();

    private Vector3 CameraPos = new Vector3();

    private Vector3 CameraEuler = new Vector3();

    //public string ZoomName;
    //相同颜色
    private List<GameObject> ListGo = new List<GameObject>();
    private List<string> compareTo = new List<string>();
    //大四喜
    private List<GameObject> SiXIGo = new List<GameObject>();
    /// <summary>
    /// 具体转到哪个动物
    /// </summary>
    private GameObject DongWuGo;

    private bool isstart = false;


    //轮子
    public GameObject Gunlun;

    /// <summary>
    /// 动物模型动画
    /// </summary>
    public GameObject[] Zoom;

    private Animator Ani;

    // 目标
    [HideInInspector]
    public float target_dongwu;


    void Awake()
    {
        ins = this;
    }

    // Use this for initialization
    void Start()
    {


        //    ZoomName = Zoom[Random.RandomRange(0,Zoom.Length)].name;



        zpInitSpeed = Random.Range(100, 200);
        MainCamera = GameObject.Find("Main Camera") as GameObject;

        //   zpInitSpeed = 0;
        ModelSpeed = Random.Range(100, 200);
        GetTransfrom();
    }

    // Update is called once per frame
    /// <summary>
    /// 转动转盘
    /// </summary>
    void Update()
    {

        if (LoginData.IsLogin)
        {
            LoginData.OverTime += Time.deltaTime;
            if (LoginData.OverTime >= 3f)
            {


                NewTcpNet.GetInstance().SocketQuit();
                NewTcpNet.GetInstance();


            }
        }


      
        //JSTime();
    }
    void FixedUpdate() {

        if (isMoving)
        {
            if (moveState == 1 && (time >= 0 || getAngle() < 270))
            {//如果旋转时间小于旋转保持时间，或者大于旋转保持时间但是与停止方向角度小于270，继续保持旋转
                //	Debug.Log(getAngle());

                //rotateTimer -= Time.deltaTime;

                if (speed < maxSpeed) speed += 0.008f;
                zp.transform.Rotate(new Vector3(0, speed, 0));
                //  Model.transform.Rotate(new Vector3(0, -speed, 0));
            }
            else
            {//减速旋转，知道停止在目标位置
                //	Debug.Log("152152152152");
                moveState = 2;

                //      Debug.Log(zp.transform.localEulerAngles.y + "       " + endAngle);
                if (speed > minSpeed)
                    speed -= 7 * speed / 50;
                //if (Mathf.Abs(Model.transform.localEulerAngles.y)%360!=0)
                //Model.transform.Rotate(new Vector3(0, -speed, 0));
                if (Mathf.Abs(zp.transform.localEulerAngles.y - endAngle) > 2)

                    //Debug.Log ("59" + getAngle ());
                    zp.transform.Rotate(new Vector3(0, speed, 0));
                else
                {//stop

                    endMove();



                }
            }
            if (isMoving1 == true)
            {
                if (moveState1 == 1)
                {

                    if (speed1 < maxSpeed) speed1 += 0.005f;
                    Model.transform.Rotate(new Vector3(0, -speed1, 0));
                }

                else
                {

                    if (speed1 > minSpeed)
                        speed1 -= 7 * speed1 / 100;
                    if (Mathf.Abs(Model.transform.localEulerAngles.y) >= 1)
                        Model.transform.Rotate(new Vector3(0, -speed1, 0));
                    else
                    {
                        Model.transform.localEulerAngles = new Vector3(0, 0, 0);
                        EndMove1();
                    }
                }

            }

        }
    }
    /// <summary>
    /// 得到动物属性以及相机的初始化
    /// </summary>
    void GetTransfrom()
    {
        for (int i = 0; i < Zoom.Length; i++)
        {
            PosVec3.Add(Zoom[i].transform.localPosition);
            EulerVec3.Add(Zoom[i].transform.localEulerAngles);
        }
        CameraPos = MainCamera.transform.localPosition;
        CameraEuler = MainCamera.transform.localEulerAngles;

    }



    //}
    //复位运算

    public void Received(bool ishow)
    {
        StartMove();
        //        zp.transform.GetComponent<TweenRotation>().enabled = ishow;

        //  if (Model.transform.GetComponent<TweenRotation>().tweenGroup ==1) {
        // Model.transform.GetComponent<TweenRotation>().enabled = ishow;
        //   }
        StartMove1();
        Banker.gameObject.transform.GetComponent<TweenPosition>().ResetToBeginning();
        Banker.gameObject.GetComponent<TweenPosition>().style = UITweener.Style.Loop;
        Banker.gameObject.GetComponent<TweenPosition>().from = new Vector3(0, -60, 0);
        Banker.gameObject.GetComponent<TweenPosition>().to = new Vector3(0, 60, 0);
        Banker.gameObject.transform.GetComponent<TweenPosition>().enabled = ishow;

    }


    public void shiJian(float time)
    {
        if (time > 0 && time <= 2)
        {
            GetVec3();
            GetLunziEuler();
            JsEuler();
      
        }
        else if ((time == 3 || time == 4) && isstart == false)
        {
            isstart = true;
            moveState1 = 0;

        }
    }
    public void JSXZ()
    {
        int rand = Random.RandomRange(1, 3);
        Model.transform.GetComponent<TweenRotation>().to = new Vector3(0, -360 * rand, 0);
        if (Model.transform.localEulerAngles.y % 15 != 0)
        {
            Model.transform.GetComponent<TweenRotation>().from = Model.transform.localEulerAngles;
        }
        if (rand == 1)
        {
            Model.transform.GetComponent<TweenRotation>().duration = 3;
        }
        else
        {
            Model.transform.GetComponent<TweenRotation>().duration = 6;
        }
        Model.transform.GetComponent<TweenRotation>().style = UITweener.Style.Once;


        Model.transform.GetComponent<TweenRotation>().enabled = false;


  

    }



    public Vector3 GetVec3()
    {
        Debug.Log(PlayerData.ins.ZoomName);
        if (PlayerData.ins.ZoomName == string.Empty)
        { ChipScript.ins.Tip("代码错误"); }
        else
        {
            for (int i = 0; i < ChipScript.ins.Animals.transform.childCount; i++)
            {
                if (ChipScript.ins.Animals.transform.GetChild(i).name == PlayerData.ins.ZoomName)
                {
                    ListGo.Add(ChipScript.ins.Animals.transform.GetChild(i).gameObject);
                }

            }
            //  Debug.Log(ListGo.Count);
            DongWuGo = ListGo[Random.RandomRange(0, 2)].gameObject;
            print(DongWuGo.name);
            ToEuler = DongWuGo.transform.localEulerAngles;

            endAngle = DongWuGo.transform.localEulerAngles.y;//获得目标位置相对y坐标方向的角度
            targetDir = calculateDir(endAngle);//获得目标位置方向向量
            return ToEuler;
        }
        return ToEuler;
    }

    //旋转指定位置
    public void RotateEuler()
    {

        //  zp.transform.GetComponent<TweenRotation>().enabled = true;

        zp.transform.GetComponent<TweenRotation>().from = zp.transform.localEulerAngles;

        zp.transform.GetComponent<TweenRotation>().to = ToEuler;



        int Eulery = Mathf.Abs(Mathf.CeilToInt(zp.transform.localEulerAngles.y));
        int GetVec3Y = Mathf.Abs(Mathf.CeilToInt(ToEuler.y));

        if (Mathf.Abs(Eulery - GetVec3Y) >= 120 && Mathf.Abs(Eulery - GetVec3Y) <= 180)
        {

            zp.transform.GetComponent<TweenRotation>().duration = 2f;
        }
        else if (Mathf.Abs(Eulery - GetVec3Y) > 60 && Mathf.Abs(Eulery - GetVec3Y) < 120)
        {
            zp.transform.GetComponent<TweenRotation>().duration = 1.5f;
        }
        else
        {
            zp.transform.GetComponent<TweenRotation>().duration = 1f;
        }
        zp.transform.GetComponent<TweenRotation>().style = UITweener.Style.Once;
    
    }

    //接着旋转
    public void RotateDaSIXi()
    {


        SiXIGo[i].transform.GetComponent<TweenScale>().ResetToBeginning();
      
        SiXIGo[i].transform.GetComponent<TweenScale>().enabled = true;

        Ani = SiXIGo[i].GetComponent<Animator>();
        Ani.SetBool("Win", true);
        Ani.SetBool("Idle", false);
        SiXIGo[i].GetComponent<Animator>().enabled = true;

        SiXIGo[i].transform.Find("Plane").gameObject.SetActive(true);
        i += 1;
    }


    public void RotateZp()
    {


     
        if (SiXIGo.Count != 0)
        {
            if (i < SiXIGo.Count)
            {

                Vector3 Vecding;
                zp.transform.GetComponent<TweenRotation>().from = zp.transform.localEulerAngles;
                if (SiXIGo[i].transform.localEulerAngles.y <= zp.transform.localEulerAngles.y)
                {
                    Vecding = new Vector3(0, SiXIGo[i].transform.localEulerAngles.y + 360, 0);
                }
                else
                {
                    Vecding = new Vector3(0, SiXIGo[i].transform.localEulerAngles.y, 0);
                }

                zp.transform.GetComponent<TweenRotation>().to = Vecding;
                zp.transform.GetComponent<TweenRotation>().style = UITweener.Style.Once;
                zp.transform.GetComponent<TweenRotation>().duration = 5f;
                zp.transform.GetComponent<TweenRotation>().ResetToBeginning();
                zp.transform.GetComponent<TweenRotation>().enabled = true;

            }
            else
            {

                zp.transform.GetComponent<TweenRotation>().enabled = false;
                Partical.instance.OrOpen(true);
                ChipScript.ins.MoveToChip(SiXIGo[SiXIGo.Count - 1]);
                SiXIGo.Clear();
            }

        }
    }


    //大四喜接下来的动作
    public void ChangeEuler()
    {

        for (int i = 0; i < SiXIGo.Count; i++)
        {
            // RotateEuler(SiXIGo[i].transform.localEulerAngles);
        }


    }
    //滚轮角度
    public void GunRotate()
    {
        Gunlun.transform.localEulerAngles = new Vector3(0, 0, 180);
        Gunlun.transform.GetComponent<TweenRotation>().from = new Vector3(0, 0, 180);
        Gunlun.transform.GetComponent<TweenRotation>().to = new Vector3(0, 0, 0);
        Gunlun.transform.GetComponent<TweenRotation>().ResetToBeginning();
        Gunlun.transform.GetComponent<TweenRotation>().enabled = true;
  //      XiaoLun();
    }
    //上面的两个小轮
    public void XiaoLun()
    {

      

        Gunlun.transform.GetChild(6).transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().ResetToBeginning();
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().ResetToBeginning();

        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().style = UITweener.Style.Loop;
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().style = UITweener.Style.Loop;
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().from = new Vector3(0, 180, 180);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().from = new Vector3(0, 0, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().to = new Vector3(360, 180, 180);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().to = new Vector3(360, 0, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().enabled = true;
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().enabled = true;
    }

    //中奖zhuanlun
    public void GetLunziEuler()
    {
        Vector3 Left = new Vector3();
        Vector3 Right = new Vector3();
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().from = new Vector3(Gunlun.transform.GetChild(6).transform.GetChild(0).transform.localEulerAngles.x, 180, 180);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().from = new Vector3(Gunlun.transform.GetChild(6).transform.GetChild(1).transform.localEulerAngles.x, 0, 0);
        if (PlayerData.ins.win_type == "1")
        {
            Left = new Vector3(60, 180, 180);
            Right = new Vector3(0, 0, 0);
        }
        else if (PlayerData.ins.win_type == "2")
        {
            Left = new Vector3(60, 180, 180);
            Right = new Vector3(60, 0, 0);
        }
        else if (PlayerData.ins.win_type == "3")
        {
            Left = new Vector3(120, 180, 180);
            Right = new Vector3(120, 0, 0);
        }
        else
        {
            Left = new Vector3(180, 180, 180);
            Right = new Vector3(180, 0, 0);

        }
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().to = Left;
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().to = Right;
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().style = UITweener.Style.Once;
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().style = UITweener.Style.Once;
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetComponent<TweenRotation>().ResetToBeginning();
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetComponent<TweenRotation>().ResetToBeginning();
    }

    //庄和闲得到角度
    void JsEuler()
    {
        Banker.transform.GetComponent<TweenPosition>().enabled = false;
        Vector3 Vec;
        if (PlayerData.ins.InterZHX == "H")
        {
            Vec = new Vector3(0, 60, 0);
        }
        else if (PlayerData.ins.InterZHX == "X")
        {
            Vec = new Vector3(0, 0, 0);
        }
        else
        {
            Vec = new Vector3(0, -60, 0);
        }
        
        Banker.gameObject.GetComponent<TweenPosition>().from = Banker.gameObject.transform.localPosition;
        Banker.gameObject.GetComponent<TweenPosition>().to = Vec;
        Banker.gameObject.GetComponent<TweenPosition>().style = UITweener.Style.Once;
        Banker.transform.GetComponent<TweenPosition>().enabled = true;

    }

    public void SameClass(GameObject obj, bool isshow)
    {
        Ani = obj.GetComponent<Animator>();
        Ani.SetBool("Win", isshow);
        Ani.SetBool("Idle", !isshow);
        obj.GetComponent<Animator>().enabled = true;
        obj.transform.Find("Plane").gameObject.SetActive(isshow);
        if (isshow == true) {
          
            obj.GetComponent<TweenPosition>().ResetToBeginning();
            obj.GetComponent<TweenScale>().ResetToBeginning();
            obj.GetComponent<TweenRotation>().ResetToBeginning();
        }
        obj.GetComponent<TweenPosition>().enabled = isshow;
        obj.GetComponent<TweenScale>().enabled = isshow;
        obj.GetComponent<TweenRotation>().enabled = isshow;
        Partical.instance.OrOpen(isshow);

    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="name"></param>
    public void PlayAnimation()
    {

        //平常奖
        if (PlayerData.ins.win_type == "1")
        {
        
            DongWuGo.GetComponent<TweenPosition>().from = DongWuGo.transform.localPosition;
            DongWuGo.GetComponent<TweenPosition>().to = new Vector3(0, 5, 0);
            DongWuGo.GetComponent<TweenRotation>().to = new Vector3(0, (180f - Model.transform.eulerAngles.y), 0);

            MainCamera.GetComponent<TweenPosition>().ResetToBeginning();
            MainCamera.GetComponent<TweenPosition>().enabled = true;
            SameClass(DongWuGo, true);
         
        }

         //JX奖
        else if (PlayerData.ins.win_type == "2")
        {



            DongWuGo.GetComponent<TweenPosition>().from = DongWuGo.transform.localPosition;
            DongWuGo.GetComponent<TweenPosition>().to = new Vector3(0, 5, 0);
            DongWuGo.GetComponent<TweenRotation>().to = new Vector3(0, (180f - Model.transform.eulerAngles.y), 0);

            MainCamera.GetComponent<TweenPosition>().ResetToBeginning();
            MainCamera.GetComponent<TweenPosition>().enabled = true;
            SameClass(DongWuGo, true);
      
            ChipScript.ins.JXLabel.text = "X" + PlayerData.ins.handsel_total;
            ChipScript.ins.JXLabel.GetComponent<TweenPosition>().from = new Vector3(0, 60, 0);
            ChipScript.ins.JXLabel.GetComponent<TweenPosition>().to = new Vector3(0, -250, 0);
            ChipScript.ins.JXLabel.GetComponent<TweenScale>().from = new Vector3(1, 1, 1);
            ChipScript.ins.JXLabel.GetComponent<TweenScale>().to = new Vector3(1.3f, 1.3f, 1.3f);

            ChipScript.ins.JXLabel.GetComponent<TweenPosition>().ResetToBeginning();
            ChipScript.ins.JXLabel.GetComponent<TweenScale>().ResetToBeginning();
            ChipScript.ins.JXLabel.GetComponent<TweenPosition>().enabled = true;
            ChipScript.ins.JXLabel.GetComponent<TweenScale>().enabled = true;
            ChipScript.ins.JXLabel.gameObject.SetActive(true);

        }
        
        //大四喜
        else if (PlayerData.ins.win_type == "4")
        {
        


            Ani = DongWuGo.GetComponent<Animator>();
            Ani.SetBool("Win", true);
            Ani.SetBool("Idle", false);
            DongWuGo.GetComponent<Animator>().enabled = true;
            DongWuGo.transform.Find("Plane").gameObject.SetActive(true);

            string[] str = DongWuGo.name.Split('_');
            string[] str1;
            for (int j = 0; j < ChipScript.ins.Animals.transform.childCount - 1; j++)
            {

                str1 = ChipScript.ins.Animals.transform.GetChild(j).name.Split('_');

                if (str[0] != str1[0] && str[1] == str1[1])
                {
                    if (compareTo.Contains(ChipScript.ins.Animals.transform.GetChild(j).gameObject.name))
                    {

                    }
                    else
                    {

                        compareTo.Add(ChipScript.ins.Animals.transform.GetChild(j).gameObject.name);
                        SiXIGo.Add(ChipScript.ins.Animals.transform.GetChild(j).gameObject);

                    }

                }
                DongWuGo.GetComponent<TweenScale>().enabled = true;
            }

          
        }
        //大三元
        else if (PlayerData.ins.win_type == "3")
        {
          

            Ani = DongWuGo.GetComponent<Animator>();
            Ani.SetBool("Win", true);
            Ani.SetBool("Idle", false);
            DongWuGo.GetComponent<Animator>().enabled = true;
            DongWuGo.transform.Find("Plane").gameObject.SetActive(true);
            string[] str = DongWuGo.name.Split('_');
            string[] str1;
            for (int j = 0; j < ChipScript.ins.Animals.transform.childCount - 1; j++)
            {

                str1 = ChipScript.ins.Animals.transform.GetChild(j).name.Split('_');

                if (str[0] == str1[0] && str[1] != str1[1])
                {

                    if (compareTo.Contains(ChipScript.ins.Animals.transform.GetChild(j).gameObject.name))
                    {

                    }
                    else
                    {

                        compareTo.Add(ChipScript.ins.Animals.transform.GetChild(j).gameObject.name);
                        SiXIGo.Add(ChipScript.ins.Animals.transform.GetChild(j).gameObject);

                    }



                }

            }
            DongWuGo.GetComponent<TweenScale>().enabled = true;

        }
        //else { }

        //    str = ChipScript.ins.GetName(name);

    }

    /// <summary>
    /// 重置位置
    /// </summary>

    public void ResetZoomPosition()
    {

        for (int i = 0; i < Zoom.Length; i++)
        {
            SameClass(Zoom[i], false);
            Zoom[i].transform.localPosition = PosVec3[i];
            Zoom[i].transform.localScale = new Vector3(1, 1, 1);
            Zoom[i].transform.localEulerAngles = EulerVec3[i];
        }
        MainCamera.transform.localPosition = CameraPos;
        MainCamera.transform.localEulerAngles = CameraEuler;


    }
    //
    public void ResetUI()
    {
        if (ChipScript.ins.JXLabel.gameObject.activeSelf)
        {
            ChipScript.ins.JXLabel.transform.localPosition = new Vector3(0, 60, 0);
            ChipScript.ins.JXLabel.gameObject.SetActive(false);
        }


        //ModelSpeed = Random.Range(350, 450);
        //   Banker.transform.GetComponent<TweenPosition>().enabled = true;

        //  UIManager.ins.SetHeadShow(false, null);
        isstart = false;
        ListGo.Clear();
        PlayerData.ins.win_type = string.Empty;
        ClickEvent.ins.isqyfen = false;
        ChipScript.ins.Defalut();
        PlayerData.ins.ZoomName = string.Empty;
        PlayerData.ins.InterfZoomName = string.Empty;
        ClickEvent.ins.ReSetPosition();
        SiXIGo.Clear();
        compareTo.Clear();
        SetShow();
        GetZoom();
        ResetLunzi();
        UIManager.ins.obj2.SetActive(false);
        UIManager.ins.obj3.SetActive(false);
        zp.transform.localEulerAngles = new Vector3(0, 0, 0);
        Model.transform.localEulerAngles = new Vector3(0, 0, 0);
        Partical.instance.OrOpen(false);
        i = 0;
    }

    public void ResetLunzi()
    {

        Gunlun.transform.GetChild(6).transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, 180);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetChild(1).localEulerAngles = new Vector3(0, 180, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetChild(1).localEulerAngles = new Vector3(0, 180, 180);
        Gunlun.transform.GetChild(6).transform.localEulerAngles = new Vector3(0, 0, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 180);
        Gunlun.transform.GetChild(6).transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
        Gunlun.transform.GetChild(6).transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        Gunlun.transform.GetChild(6).transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        Gunlun.transform.localEulerAngles = new Vector3(0, 0, 180);
        //Gunlun.transform.GetComponent<TweenRotation> ().ResetToBeginning ();

        //	Gunlun.transform.GetComponent<TweenRotation> ().enabled = true;
        Banker.gameObject.transform.localPosition = new Vector3(0, 0, 0);


    }

    public void GetZoom()
    {

        for (int i = 0; i < Zoom.Length; i++)
        {
            if (Zoom[i].GetComponent<Animator>().enabled = false)
            {
                Zoom[i].GetComponent<Animator>().enabled = true;
            }

        }
    }

    public void SetShow()
    {
        foreach (Transform item in bonuUI.ins.TimePanel.transform)
        {
            ///			Debug.Log (item.gameObject);
            if (!item.transform.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
            }

        }
    }






    #region 计算当前对象y方向与目标方向的夹角



    float getAngle()
    {
        return calAngle(targetDir, transform.forward);//计算y轴方向的旋转角度
    }
    //计算从dir1旋转到dir2的角度

    float calAngle(Vector3 dir1, Vector3 dir2)
    {
        float angle = Vector3.Angle(dir1, dir2);
        Vector3 normal = Vector3.Cross(dir1, dir2);
        //      Debug.Log ("normal="+normal);
        //      angle = normal.z > 0 ? angle : (180+(180-angle));
        angle = normal.y > 0 ? angle : (360 - angle);

        //     Debug.Log("         " + angle);
        return angle;
    }

    #endregion

    /// <summary>
    /// 计算目标位置的向量
    /// Calculates the dir.
    /// </summary>
    /// <param name="endAngle">End angle.</param>
    Vector3 calculateDir(float endAngle)
    {
        float radiansX = Mathf.Cos(Mathf.PI * (endAngle) / 180);
        float radiansY = Mathf.Sin(Mathf.PI * (endAngle) / 180);
        return new Vector3(radiansX, 0, radiansY);

    }


    void endMove()
    {

        speed = 0;
        isMoving = false;
        moveState = 0;
        //    Debug.Log("endMove");

        PlayAnimation();


    }
    void EndMove1()
    {

        speed1 = 0;
        isMoving1 = false;
        moveState1 = 0;
        //    Debug.Log("endMove");

    }
    void StartMove()
    {
        if (isMoving)
            return;



        isMoving = true;
        moveState = 1;
    }
    void StartMove1()
    {

        if (isMoving1)
            return;
        isMoving1 = true;
        moveState1 = 1;
    }

}


