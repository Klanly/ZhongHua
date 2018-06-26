using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using LitJson;

public class Notice : MonoBehaviour
{
    int noticeCount;
    public Text noticeText;
    Vector3 vecFir;
    private Vector3 startPoint;
    private Vector3 endPoint;

    // Use this for initialization
    void Start()
    {
        vecFir = noticeText.rectTransform.localPosition += new Vector3(noticeText.preferredWidth, 0, 0);
        StartCoroutine(NoticeAnimation());

		if(LoginInfo.Instance().mylogindata.choosegame!=0)
		{
			StartCoroutine(GetAnnouncementInfo(LoginInfo.Instance().mylogindata.URL + "notice?" + "user_id=" + LoginInfo.Instance().mylogindata.user_id + "&game_id=" + LoginInfo.Instance().mylogindata.choosegame));

		}
       



    }


	//添加大厅公告
	public void AddNotice()
	{
		StartCoroutine(GetAnnouncementInfo2(LoginInfo.Instance().mylogindata.URL + "notice?" + "user_id="+LoginInfo.Instance().mylogindata.user_id +"&game_id="+ 0));
	}


    private IEnumerator NoticeAnimation()
    {
        while (true)
        {

            noticeText.rectTransform.localPosition -= new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.05f);

            if (noticeText.rectTransform.localPosition.x <= (vecFir.x - noticeText.preferredWidth / 2f - 719f - 5))
            {
                noticeText.rectTransform.localPosition = (vecFir + new Vector3(noticeText.preferredWidth / 2f + 10, 0, 0));
                noticeText.transform.localScale = Vector3.one;

                noticeCount++;
                //yield return new WaitForSeconds(0.05f);

            }

        }
    }

    IEnumerator GetAnnouncementInfo(string URL)
    {
		Debug.Log (URL);
        while (true)
        {
            UnityWebRequest temp = UnityWebRequest.Get(URL);

            yield return temp.Send();
            if (temp.error==null)
            {
                JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);
                if (jd["code"].ToString() == "200")
                {
                    ///将读取到的公告信息进行一个循环并进行调整
                    this.gameObject.SetActive(true);
                    if (jd["List"].Count > 0)
                    {
                        if (noticeCount >= jd["List"].Count)
                        {
                            noticeCount = 0;
                        }
                        if (noticeText.text != "" && !noticeText.text.Equals(jd["List"][noticeCount]["site"].ToString()))
                        {
                            noticeText.text = jd["List"][noticeCount]["site"].ToString();
                            noticeText.rectTransform.localPosition = (vecFir + new Vector3(noticeText.preferredWidth / 2f, 0, 0));
                            noticeText.transform.localScale = Vector3.one;
                        }
                        else
                        {
                            //Debug.Log("公告不需要更新");
                        }
                    }
                    else
                    {
                        noticeText.text = "暂无公告";
                        this.gameObject.SetActive(false);
                    }


                }
                else
                {
                    noticeText.text = "暂无公告";
                    this.gameObject.SetActive(false);
                    //gonggao.rectTransform.localPosition = (vecFir + new Vector3(gonggao.preferredWidth / 2f, 0, 0));
                    //gonggao.transform.localScale = Vector3.one;
                }
            }
            yield return new WaitForSeconds(2);
        }
       
    }

	IEnumerator GetAnnouncementInfo2(string URL)
	{
		Debug.Log (URL);
		while (true)
		{
			UnityWebRequest temp = UnityWebRequest.Get(URL);

			yield return temp.Send();
			JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);
			if (jd["code"].ToString() == "200")
			{
				///将读取到的公告信息进行一个循环并进行调整
				this.gameObject.SetActive (true);
				if(jd["List"].Count>0)
				{
					if (noticeCount >= jd["List"].Count)
					{
						noticeCount = 0;
					}
					if (jd["List"][noticeCount]["status"].ToString()=="0")
					{
						noticeText.text = jd["List"][noticeCount]["site"].ToString();
						noticeText.rectTransform.localPosition = (vecFir + new Vector3(noticeText.preferredWidth / 2f, 0, 0));
						noticeText.transform.localScale = Vector3.one;
					}
					else
					{
//						Debug.Log("公告不需显示");
						this.gameObject.SetActive (false);
					}
				}else
				{
					noticeText.text = "暂无公告";
					this.gameObject.SetActive (false);
				}


			}
			else
			{
				noticeText.text = "暂无公告";
				this.gameObject.SetActive (false);
				//gonggao.rectTransform.localPosition = (vecFir + new Vector3(gonggao.preferredWidth / 2f, 0, 0));
				//gonggao.transform.localScale = Vector3.one;
			}
			yield return new WaitForSeconds(2);
		}

	}
}
