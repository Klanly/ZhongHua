using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能：播放器(旧版[之后仅作为旧版声音文件储存])
/// </summary>
public class Audiomanger : MonoBehaviour {

    public static Audiomanger _instenc;

    public AudioClip backgroundmusic;               //原本的背景音乐

    public AudioClip LobbyBG;
    public AudioClip GameBG;

    public AudioClip winmusic;
    
    public AudioClip clickbuttom;

    public AudioClip dangshuangBG;                  //单双使用的背景音乐
    public AudioClip newBG;
    public AudioClip kaiJiang;
    public List<AudioClip> pokerColor;
    public List<AudioClip> pokerNum;
    public List<AudioClip> playZhuangXian;
    public List<AudioClip> playTip;
    public List<AudioClip> playDanShuang;
    public List<AudioClip> playElb;
    public List<AudioClip> playTianDi;

    public AudioClip ZhuangZeng;
    public AudioClip XianZeng;
    public AudioClip ZhuangP;
    public AudioClip XianP;

    public AudioClip [] zhuangnumber;
    public AudioClip [] xiannumber;

    //输赢音效
    public AudioClip[] Xwy_Win;

    private void Awake()
    {
        _instenc = this;
    }

    public void ChangeBGMusic_KaiJiang()
    {
        AudioSource temp = this.transform.GetComponent<AudioSource>();
        temp.clip = kaiJiang;
        temp.Play();
    }

    public void ChangeBGMusic()
    {
        AudioSource temp = this.transform.GetComponent<AudioSource>();
        temp.clip = newBG;
        temp.Play();
    }

    // Use this for initialization
   public void playwinmusic()
    {
        AudioSource temp = this.transform.GetChild(0).GetComponent<AudioSource>();
        temp.clip = winmusic;

        temp.Play();
    }

  

    //播放对应物体上的播放插件
    public void playfromGo(GameObject musicGo)
    {
        if (musicGo != null)
        {
            musicGo.GetComponent<AudioSource>().Play();
        }
    }

    public void clickvoice()
    {
        AudioSource temp = this.transform.GetChild(0).GetComponent<AudioSource>();
        temp.clip = clickbuttom;

        temp.Play();
    }
    public void OnClick(string value)
    {
        if (value == "")
        {

        }
    }
    public void PlayDanTiaoWin( int num)
    {
        StartCoroutine(PlayDanTiao(num));

    }
    IEnumerator PlayDanTiao(int pokerNum_)
    {
        int color = pokerNum_ / 13;
        int num = pokerNum_ % 13;
        if (color != 4)
        {
            AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
            colorAs.clip = pokerColor[color];
            colorAs.Play();
            yield return new WaitForSeconds(colorAs.clip.length);
            colorAs.clip = pokerNum[num];
            colorAs.Play();
        }
        else
        {
            if (num == 0)
            {
                AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
                colorAs.clip = pokerColor[13];
                colorAs.Play();
            }
            else
            {
                AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
                colorAs.clip = pokerColor[14];
                colorAs.Play();
            }
        }
       
    }
    public void PlayTip(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = playTip[num];
        colorAs.Play();
    }
    public void PlayDanShuangWin(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = playDanShuang[num];
        colorAs.Play();
    }
    public void PlayElbWin(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = playElb[num];
        colorAs.Play();
    }

    public void PlayZhuangXianWin(int num)
    {
        //StartCoroutine(PlayZhuangXian(num));
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = playZhuangXian[num];
        colorAs.Play();

    }
    //IEnumerator PlayZhuangXian(int num)
    //{
    //    AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
    //    colorAs.clip = playZhuangXian[num];
    //    colorAs.Play();
    //    //yield return colorAs.isPlaying;
    //    yield return new WaitForSeconds(colorAs.clip.length);
    //    colorAs.clip = playZhuangXian[3];
    //    colorAs.Play();
    //}
    public void PlayTianDiWin(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = playTianDi[num];
        colorAs.Play();
    }


    public void PlaySound(AudioClip clip)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = clip;
        colorAs.Play();
    }

    public void playZhuangnumber(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = zhuangnumber[num];
        colorAs.Play();
    }

    public void playXiannumber(int num)
    {
        AudioSource colorAs = transform.GetChild(1).GetComponent<AudioSource>();
        colorAs.clip = xiannumber[num];
        colorAs.Play();
    }
}
