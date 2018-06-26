using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能：新版音效管理脚本
/// </summary>
public class NewAudioManger : MonoBehaviour {

    //单例模式
    public static NewAudioManger instance;

    //音量记录静态变量
    public static float MusicVolume;
    public static float SoundVolume;


    //初始化
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        Debug.logger.logEnabled = false;
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            instance = this;
            Music = this.GetComponent<AudioSource>();
            //Effect = this.transform.Find("Effect").GetComponent<AudioSource>();
            //Sound = this.transform.Find("Sound").GetComponent<AudioSource>();

            Effect = this.transform.GetChild(0).GetComponent<AudioSource>();
            Sound = this.transform.GetChild(1).GetComponent<AudioSource>();

            if (PlayerPrefs.HasKey("sliderOne"))
            {
                MusicVolume = PlayerPrefs.GetFloat("sliderOne");
            }
            else
            {
                MusicVolume = 1;
            }

            if (PlayerPrefs.HasKey("sliderOne"))
            {
                SoundVolume = PlayerPrefs.GetFloat("sliderTwo");
            }
            else
            {
                SoundVolume = 1;
            }
            //校准音量
            Music.volume = MusicVolume;
            Effect.volume = SoundVolume;
            Sound.volume = SoundVolume;
        }
    }

    //播放组件
    private AudioSource Music;
    private AudioSource Sound;
    private AudioSource Effect;





    //登录界面背景音乐
    public AudioClip LoginBG;
    //大厅界面背景音乐
    public AudioClip LobbyBG;


    //游戏场景默认背景音乐【需要额外音效，请通过自行获取改变】
    public AudioClip GameBG;

    /// <summary>
    /// 播放默认登录背景音乐
    /// </summary>
    public void PlayLoginBG()
    {
        Music.clip = LoginBG;
        Music.Play();
    }

    /// <summary>
    /// 播放默认大厅背景音乐
    /// </summary>
    public void PlayLobbyBG()
    {
        Music.clip = LoginBG;
        Music.Play();
    }

    /// <summary>
    /// 播放默认游戏背景音乐
    /// </summary>
    public void PlayGameBG()
    {
        Music.clip = GameBG;
        Music.Play();
    }


    /// <summary>
    /// 通用播放器[背景]
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        Music.clip = clip;
        Music.Play();
    }

    /// <summary>
    /// 通用播放器[音效] 主要为按钮点击一类的短暂音效
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        Sound.clip = clip;
        Sound.Play();
    }

    /// <summary>
    /// 通用播放器[效果]  主要为播放时间较长的音效
    /// </summary>
    public void PlayEffect(AudioClip clip)
    {
        Effect.clip = clip;
        Effect.Play();
    }



    /// <summary>
    /// 音量调节[音乐]
    /// </summary>
    public void SetMusicVolume(float value)
    {
        Music.volume = value;
        MusicVolume = value;
        PlayerPrefs.SetFloat("sliderOne", MusicVolume);
       
    }


    /// <summary>
    /// 音量调节[音效]
    /// </summary>
    public void SetSoundVolume(float value)
    {
        Sound.volume = value;
        Effect.volume = value;
        SoundVolume = value;
        PlayerPrefs.SetFloat("sliderTwo", SoundVolume);
    }
}
