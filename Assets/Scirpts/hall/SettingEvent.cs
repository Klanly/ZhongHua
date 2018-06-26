using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingEvent : MonoBehaviour
{

    public Slider sliderOne;
    public Slider sliderTwo;
    public Toggle wordToggle;
    public Toggle picToggle;
    GameObject go;
	// Use this for initialization
	void Start ()
    {
        go = GameObject.Find("AudiogaMemager");
        sliderOne.value = PlayerPrefs.GetFloat("sliderOne");
        sliderTwo.value = PlayerPrefs.GetFloat("sliderTwo");
        if (PlayerPrefs.GetString("isShowWord") == "true")
        {
            wordToggle.isOn = true;
            wordToggle.enabled = false;
            
            picToggle.isOn = false;
            num = 0;
        }
        else
        {
            wordToggle.isOn = false;
            picToggle.isOn = true;
            picToggle.enabled = false;
            PlayerPrefs.SetString("isShowWord", "false");
            num = 1;
        }

        sliderOne.onValueChanged.AddListener
            (

                (float Value) =>
                {
                    ChangeSliderOne(Value);
                }
           
            );
        sliderTwo.onValueChanged.AddListener
            (

                (float Value) =>
                {
                    ChangeSliderTwo(Value);
                }

            );
        wordToggle.onValueChanged.AddListener
            (
                (bool Value) =>
                {
                    OnChangeToggle(Value, 0);
                }
            );
        picToggle.onValueChanged.AddListener
            (
                (bool Value) =>
                {
                    OnChangeToggle(Value, 1);
                }
            );

    }
	
	
    //改变音量
	void ChangeSliderOne(float value)
    {
        //go.GetComponent<AudioSource>().volume = value;
        //PlayerPrefs.SetFloat("sliderOne", value);
        NewAudioManger.instance.SetMusicVolume(value);
    }

    //改变音量
    void ChangeSliderTwo(float value)
    {
        //go.transform.GetChild(0).GetComponent<AudioSource>().volume = value;
        //go.transform.GetChild(1).GetComponent<AudioSource>().volume = value;
        //PlayerPrefs.SetFloat("sliderTwo", value);

        NewAudioManger.instance.SetSoundVolume(value);
    }

    int num;
    void OnChangeToggle(bool value,int num)
    {
       
        if (value )
        {
            switch (num)
            {
                case 0:
                    picToggle.enabled = true;
                    picToggle.isOn = false;
                    wordToggle.enabled = false;
                    PlayerPrefs.SetString("isShowWord", "true");
               
                    break;
                case 1:
                    wordToggle.enabled = true;
                    wordToggle.isOn = false;
                    picToggle.enabled = false;
                    PlayerPrefs.SetString("isShowWord", "false");
                   
                    break;
            }
        }
    }
}
