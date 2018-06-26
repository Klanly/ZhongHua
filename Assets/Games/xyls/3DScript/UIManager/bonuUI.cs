using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonuUI : MonoBehaviour
{

    public enum AlignmentStyle
    {
        Left,
        Center,
        Right
    }



    [System.Serializable]
    public class CSpecialWord
    {
        /// <summary>
        /// 限制数
        /// </summary>
        public List<long> m_listNum = new List<long>();
        /// <summary>
        /// 限制单位
        /// </summary>
        public List<long> m_listDWNum = new List<long>();
        public int m_iLenght = 1000;
        public List<string> m_listStrName = new List<string>();
    }
    public CSpecialWord m_cSpecialWord = new CSpecialWord();

    /// <summary>
    /// 是否有正负号
    /// </summary>
    public bool m_bIsSign = false;

    private GameObject timer;
    //暂时存储
    private string StrNum = string.Empty;
    /// <summary>
    /// 倒计时面板
    /// </summary>
    public Transform TimePanel;
    /// <summary>
    /// 对齐方式
    /// </summary>
    private AlignmentStyle _AlignmentStyle;
    public AlignmentStyle m_AlignmentStyle = AlignmentStyle.Left;

    public UIAtlas m_Atls;

    private List<GameObject> m_lGameNumlist = new List<GameObject>();

    private List<GameObject> m_lGameNumlistTime = new List<GameObject>();

    private string m_strTextureName = "Num_Y_";

    public string m_strTextureNameTime = "Num_B_";
    //是否继续
    private bool iscontinue = true;

    /// <summary>
    /// 倒计时开始
    /// </summary>
    //  public bool istime;


    /// <summary>
    /// 每个数字距离
    /// </summary>
    private int _iPerNumWidth = 20;
    private int _iPerNumHeight = 20;
    public int m_iPerNumWidth = 10;
    public int m_iPerNumHeight = 10;

    /// <summary>
    /// 倒计时时间
    /// </summary>
    /// 

    public static bonuUI ins;

    public int time;

    private UISprite temp_gobj;

    private string strname;

    private float _fPerNumDistance = 0;
    public float m_fPerNumDistance = 5;

    private long _iNum = -9999999;
    /// <summary>
    /// 第一个数字的位置
    /// </summary>
    private Vector3 m_vFistPos;

    void Awake()
    {
        ins = this;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(ChangeNumber());
        // SetNum(iscontinue);

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void OnClick()
    {
        ClickEvent.ins.xiaji = "http://47.106.66.89:81/sd-api/trigger-ent?game_id=" + PlayerData.ins.game_id + "&user_id=" + PlayerData.ins.userid + "&room_id=" + PlayerData.ins.room_id;
        StartCoroutine(ClickEvent.ins.RecevedURL(ClickEvent.ins.xiaji, 13));
        //   Application.Quit();
    }
    /// <summary>
    /// 设置左上角的分数
    /// </summary>
    /// <param name="num"></param>
    /// <param name="iscontinue"></param>
    public void SetNum(string str)
    {

        if (str.Length == 4)
        {
            m_vFistPos = new Vector3(-35, 0, 0);
        }
        else if (str.Length == 5)
        {
            m_vFistPos = new Vector3(-45, 0, 0);
        }
        else
        {
            m_vFistPos = new Vector3(0, 0, 0);

        }
        // Debug.Log(StrNum + "               " + str);
        if (StrNum.Length != str.Length)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            m_lGameNumlist.Clear();
            if (str.Length != 0)
            {

                for (int i = 0; i < str.Length; i++)
                {
                    int temp_iLength = 0;
                    temp_gobj = NGUITools.AddSprite(this.gameObject, m_Atls, m_strTextureName + strname);
                    temp_gobj.transform.name = strname + str.Length;
                    temp_gobj.GetComponent<UIWidget>().width = _iPerNumWidth;
                    temp_gobj.GetComponent<UIWidget>().height = _iPerNumHeight;
                    float temp_X = m_vFistPos.x + i * (_fPerNumDistance + _iPerNumWidth);
                    temp_gobj.transform.localPosition = new Vector3(temp_X, 0, 0);
                    temp_gobj.transform.parent = this.transform;
                    m_lGameNumlist.Add(temp_gobj.gameObject);

                }
            }


        }
        if (m_lGameNumlist.Count != 0)
        {
            for (int i = 0; i < m_lGameNumlist.Count; i++)
            {
                m_lGameNumlist[i].GetComponent<UISprite>().spriteName = m_strTextureName + str.Substring(i, 1);

            }


        }




        StrNum = str;



    }

    /// <summary>
    /// 时间计算
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeNumber()
    {
        while (true)
        {

            if (ClickEvent.ins.Game == false)
            {
                if (time <= 0)
                {
                    foreach (Transform item in TimePanel.transform)
                    {
                        if (item.name == "one")
                        {
                            item.gameObject.SetActive(false);
                        }
                        else
                        {
                            item.GetComponent<UISprite>().spriteName = m_strTextureNameTime + "0";
                        }
                    }
                }
                else if (time > 0 && time < 10)
                {


                    foreach (Transform item in TimePanel.transform)
                    {
                        if (item.name == "one")
                        {
                            item.gameObject.SetActive(false);
                        }
                        else
                        {
                            item.GetComponent<UISprite>().spriteName = m_strTextureNameTime + time.ToString().Substring(time.ToString().Length - 1, 1);
                        }
                    }

                }
                else
                {

                    ZPan.ins.SetShow();
                    foreach (Transform item in TimePanel.transform)
                    {

                        if (item.name == "one")
                        {
                            item.GetComponent<UISprite>().spriteName = m_strTextureNameTime + time.ToString().Substring(0, 1);
                        }
                        else
                        {
                            item.GetComponent<UISprite>().spriteName = m_strTextureNameTime + time.ToString().Substring(time.ToString().Length - 1, 1);
                        }

                    }
                }


            }

            yield return null;

            //for (int i = 0; i <PlayerData.ins.handsel_total.Length; i++)
            //{
            //    m_lGameNumlist[i].GetComponent<UISprite>().spriteName = m_strTextureName + PlayerData.ins.handsel_total.Substring(i, 1);
            //}
            // SetNum(Random.RandomRange(4, 6), iscontinue);
            //for (int i = 0; i < this.transform.childCount; i++)
            //{
            //    Destroy(this.transform.GetChild(i).gameObject);
            //    if (i==this.transform.childCount)
            //    {
            //        SetNum(Random.RandomRange(4, 6), iscontinue);
            //    }
            //}

            //  m_lGameNumlist.Clear();

            //  iscontinue = false;

        }
    }

    private void SwitchFistPos()
    {
        switch (_AlignmentStyle)
        {
            case AlignmentStyle.Left:
                {
                    m_vFistPos = new Vector3(-35, 0, 0);
                    //int temp_num = GetNumLength();
                    //m_vFistPos.x += (temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance - _iPerNumWidth * 0.5f);
                    break;
                }
            case AlignmentStyle.Center:
                {
                    m_vFistPos = new Vector3(0, 0, 0);
                    //int temp_num = GetNumLength();
                    //m_vFistPos.x += ((temp_num * m_iPerNumWidth + (temp_num - 1) * _fPerNumDistance) * 0.5f - _iPerNumWidth * 0.5f);
                    break;
                }
            case AlignmentStyle.Right:
                {
                    m_vFistPos = new Vector3(-40, 0, 0);
                    //m_vFistPos.x -= (m_iPerNumWidth * 0.5f);
                    break;
                }
        }

    }
    /// <summary>
    /// 获取字符串长度
    /// </summary>
    /// <returns></returns>
    private int GetNumLength()
    {
        int temp_iLength = 0;
        long temp_num = _iNum;
        if (_iNum < 0) temp_num = (-1) * _iNum;
        if (m_bIsSign)
        {
            temp_iLength += 1;
        }
        if (temp_num == 0) temp_iLength += 1;
        for (int i = 0; i < m_cSpecialWord.m_iLenght; i++)
        {
            if (temp_num >= m_cSpecialWord.m_listNum[i])
            {
                temp_num = temp_num / m_cSpecialWord.m_listDWNum[i];
                temp_iLength += 1;
                break;
            }
        }
        while (temp_num >= 1)
        {
            temp_iLength += 1;
            temp_num = temp_num / 10;
        }
        return temp_iLength;

    }
}
