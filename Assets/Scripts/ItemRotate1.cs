using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 转灯管理脚本
/// </summary>
public class ItemRotate1 : MonoBehaviour
{
    //是否已经结果
    public static bool IsWin;

    //灯集合
    public GameObject Items;

    //亮着的灯编号集合
    public Queue<int> Nums = new Queue<int>();

    //0		12		[0]
    //1	3	5	7	9	11	13	15	17	19	21	23[2]
    //2	4	6	8	10	14	16	18	20	22		[8]

    bool IsMove;
    bool IsRun;
    
    bool IsDown;
    bool IsStop;
    bool IsLightning;

    int winnings;

    //开始点
    int StartPos;

    //当前点
    int NowPos;

    //减速点
    int DownPos;

    //结束点
    int EndPos;

    //计数用
    int Num;

    public float gametime;
    float mathftime;
    float Lightningtime;

    public float movetime = 0;
    public float runtime = 0;
    
    public float interval = 0.05f;
    public float overinsterval = 0.2f;

    void Start()
    {
        runtime = 0;
        interval = 0.05f;
        overinsterval = 0.25f;
    }


    private void FixedUpdate()
    {
        if (IsMove)
        {
            runtime += Time.deltaTime;
            if (runtime >= interval)
            {
                runtime = 0;
                NowPos++;
                if (NowPos == 24)
                {
                    NowPos = 0;
                }
                Nums.Enqueue(NowPos);
                while (Nums.Count > 4)
                {
                    Nums.Dequeue();
                }
                TurnOffAll();
                for (int i = 0; i < Nums.Count; i++)
                {
                    Items.transform.GetChild(Nums.ToArray()[i]).GetChild(0).gameObject.SetActive(true);
                }
            }
        }



        if (IsRun)//跑灯
        {
            gametime += Time.deltaTime;
            runtime += Time.deltaTime;


            if (gametime>= 2f&& NowPos==DownPos)
            {
                //Debug.Log("激活减速");
                IsDown = false;
                IsStop = true;
            }


            if (IsStop)//准备停止开始减速
            {
                interval = Mathf.MoveTowards(interval, overinsterval, 0.002f);
                if (interval == overinsterval)
                {
                    //Debug.Log("进过"+Num);
                    if (NowPos == EndPos)
                    {
                        //准备停止
                        if (runtime >= interval)
                        {
                            runtime = 0;
                            if (Nums.Count > 0)
                            {
                                Nums.Dequeue();
                                TurnOffAll();
                                for (int i = 0; i < Nums.Count; i++)
                                {
                                    Items.transform.GetChild(Nums.ToArray()[i]).GetChild(0).gameObject.SetActive(true);
                                }
                            }
                            else
                            {
                                IsWin = true;
                                IsRun = false;
                                IsDown = false;
                                IsStop = false;
                                TurnOffAll();
                                IsLightning = true;
                                Items.transform.GetChild(EndPos).GetChild(0).gameObject.SetActive(true);
                                Audiomanger._instenc.PlayElbWin(winnings);

                                //想办法调用一次获取结果
                                //Debug.Log("停下了");
                            }
                        }
                    }
                    else
                    {
                        //寻找停止点
                        if (runtime >= interval)
                        {
                          
                            runtime = 0;
                            NowPos++;
                            if (NowPos == 24)
                            {
                                NowPos = 0;
                            }
                            Nums.Enqueue(NowPos);
                            while (Nums.Count > 4)
                            {
                                Nums.Dequeue();
                            }
                            TurnOffAll();
                            for (int i = 0; i < Nums.Count; i++)
                            {
                                Items.transform.GetChild(Nums.ToArray()[i]).GetChild(0).gameObject.SetActive(true);
                            }
                            
                        }
                    }
                }
                else
                {
                    //减速中
                    if (runtime >= interval)
                    {
                        runtime = 0;
                        Num++;
                        NowPos++;
                        if (NowPos == 24)
                        {
                            NowPos = 0;
                        }
                        Nums.Enqueue(NowPos);
                        while (Nums.Count > 4)
                        {
                            Nums.Dequeue();
                        }
                        TurnOffAll();
                        for (int i = 0; i < Nums.Count; i++)
                        {
                            Items.transform.GetChild(Nums.ToArray()[i]).GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                //跑灯中
                if (runtime >= interval)
                {
                    runtime = 0;
                    NowPos++;
                    if (NowPos == 24)
                    {
                        NowPos = 0;
                    }
                    Nums.Enqueue(NowPos);
                    while (Nums.Count > 4)
                    {
                        Nums.Dequeue();
                    }
                    TurnOffAll();
                    for (int i = 0; i < Nums.Count; i++)
                    {
                        Items.transform.GetChild(Nums.ToArray()[i]).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }


        if (IsLightning)
        {
            Lightningtime += Time.deltaTime;
            if (Lightningtime > 0.2f)
            {
                Lightningtime = 0;
                if (Items.transform.GetChild(EndPos).GetChild(0).gameObject.activeSelf)
                {
                    Items.transform.GetChild(EndPos).GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    Items.transform.GetChild(EndPos).GetChild(0).gameObject.SetActive(true);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
          
        }
    }

    public void MoveStart()
    {
        IsMove = true;
    }

    /// <summary>
    /// 设置点信息
    /// </summary>
    public void SetPos(int x,int y,int win)
    {
        StartPos = x;
        EndPos = y;
        winnings = win;

        NowPos = StartPos;

        DownPos = EndPos + 16;
        if (DownPos>24)
        {
            DownPos -= 24;
        }

        IsMove = false;
        IsRun = true;
        IsDown = false;
        IsStop = false;
        IsLightning = false;
    }

    //中途进入设置起始点
    public void GetEnd(int pos)
    {
        EndPos = pos;
    }

    public void ResetItem()
    {
        IsWin = false;

        runtime = 0;
        gametime = 0;

        interval = 0.05f;
        overinsterval = 0.25f;

        NowPos = EndPos;
        //Debug.Log(NowPos);
        IsMove = false;
        IsRun = false;
        IsDown = false;
        IsStop = false;
        IsLightning = false;

        Items.transform.GetChild(EndPos).GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// 关掉全部灯
    /// </summary>
    public void TurnOffAll()
    {
        for (int i = 0; i < Items.transform.childCount; i++)
        {
            Items.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
}

    
