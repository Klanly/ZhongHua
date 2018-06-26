
using UnityEngine;
using LitJson;
using System.Text;
using UnityEngine.UI;


//控制用户历史记录输出格式脚本
public class outtextstlye : MonoBehaviour {

    public GameObject leftTextListGO;//左边文本对象
    public GameObject rightTextListGO;//右边文本对象
    



    /* 根据客户修改该方法暂时无效
     * 
     * 
    /// <summary>
    /// 设置文本的格式
    /// </summary>
    /// <param name="datatext">服务器用户开奖数据</param>
    /// <param name="isleftorright">数据使用左边还是右边 true:左边 false:右边</param>
    /// <param name="outtext">调整后的字符串</param>
    public static void  settextstyle(string datatext,bool isleftorright,ref string outtext)
    {
        StringBuilder temp = new StringBuilder();

        if (datatext != "")
        {
            JsonData jd = JsonMapper.ToObject(datatext);

            if (jd["code"].ToString().Equals("200"))
            {
                temp.Append("\n");
                for (int i = 0; i < jd["List"].Count; i++)
                {
                    temp.Append("     ");
                    temp.Append(jd["List"][i]["dates"].ToString());


                    if (isleftorright)
                    {
                        if (Screen.height >= 800)
                        {
                            temp.Append("      ");

                        }
                        else if(Screen.height<800)
                        {
                            temp.Append("    ");
                        }

                    temp.Append(setinputtextlist(jd["List"][i]["A"].ToString(),5));
                    temp.Append(" ");
                    temp.Append(setinputtextlist(jd["List"][i]["B"].ToString(),5));
                    temp.Append(" ");
                    temp.Append(setinputtextlist(jd["List"][i]["C"].ToString(),5));
                    temp.Append(" ");
                    temp.Append(setinputtextlist(jd["List"][i]["D"].ToString(),5));
                    temp.Append(" ");
                    temp.Append(setinputtextlist(jd["List"][i]["E"].ToString(),5));

                        if (Screen.height >= 800)
                        {
                            temp.Append("  ");

                        }
                        else if (Screen.height < 800)
                        {
                                temp.Append(" ");
                         }

                    }
                    else
                    {
                        if (Screen.height >= 800)
                        {
                            temp.Append("            ");

                        }
                        else if (Screen.height < 800)
                        {
                            temp.Append("         ");
                        }

                       

                        temp.Append(setinputtextlist(jd["List"][i]["A"].ToString(), 5));
                        temp.Append(" ");
                        temp.Append(setinputtextlist(jd["List"][i]["B"].ToString(), 5));
                        temp.Append(" ");
                        temp.Append(setinputtextlist(jd["List"][i]["C"].ToString(), 5));
                        temp.Append(" ");
                        temp.Append(setinputtextlist(jd["List"][i]["D"].ToString(), 5));

                        if (Screen.height >= 800)
                        {
                            temp.Append("       ");

                        }
                        else if (Screen.height < 800)
                        {
                            temp.Append("    ");
                        }
                    }

                    string[] temp2 = jd["List"][i]["win_total"].ToString().Split(new char[] { '.' });
                    temp.Append(setinputtextlist( temp2[0] , 6));

                    temp.Append("\n");
                }


            }

            outtext = temp.ToString();

        }


    }
    */

     //获取数据并对填充
    void settextInList(string datatext, bool isleftorright,Transform tagetGO)
    {
        if (datatext != "")
        {
            JsonData jd = JsonMapper.ToObject(datatext);

            if (jd["code"].ToString().Equals("200"))
            {
                Transform temp = tagetGO.transform.GetChild(1);

                for (int i = 0; i < jd["List"].Count; i++)
                {
                    temp.GetChild(i).GetChild(0).GetComponent<Text>().text = jd["List"][i]["dates"].ToString();
                    temp.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = jd["List"][i]["A"].ToString();
                    temp.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = jd["List"][i]["B"].ToString();
                    temp.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = jd["List"][i]["C"].ToString();
                    temp.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = jd["List"][i]["D"].ToString();
                    if (isleftorright)
                    {
                     temp.GetChild(i).GetChild(1).GetChild(4).GetComponent<Text>().text = jd["List"][i]["E"].ToString();

                    }
                    string[] temp2 = jd["List"][i]["win_total"].ToString().Split(new char[] { '.' });
                    temp.GetChild(i).GetChild(2).GetComponent<Text>().text =temp2[0].ToString();

                }
            }
        }

    }




    /// <summary>
    /// 将字符串根据长度做处理 不足用空格填补  
    /// </summary>
    /// <param name="text">字符串数据</param>
    /// <param name="lengthcheak">字符串的需要的长度</param>
    /// <returns></returns>
   static string setinputtextlist(string text,int lengthcheak)
    {
        string temp2 = text;

        if (text.Length > lengthcheak)
        {

        }
        else
        {
            for (int i = 0; i < lengthcheak - text.Length; i++)
            {
                if (i % 2==0)
                {
                    temp2 = "  " + temp2;
                }
                else
                {
                    temp2 = temp2 + "  ";
                }
            }
        }

        //Debug.Log("cheak:" + temp2);
        return temp2;
    }

   

    /// <summary>
    /// 获取服务器的用户开奖数据并针对数据进行一定的字符串格式调整
    /// </summary>
    /// <param name="text"></param>
    /// <param name="leftorright"></param>
    public void addtexttoui(string text,bool leftorright)
    {
        if (text != "")
        {
            if (leftorright)
            {

           
            //settextstyle(text, leftorright, ref temp);
                settextInList(text, leftorright,leftTextListGO.transform);
            //leftTextListGO.text = temp;
            }
            else
            {
                settextInList(text, leftorright, rightTextListGO.transform);
            }
        }
    }


}
