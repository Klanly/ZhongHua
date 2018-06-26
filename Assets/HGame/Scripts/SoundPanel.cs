using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    public Image gameMusicButton;
    public Image natureMusicButton;

    public Sprite[] sprites;


    private void Update()
    {
        //if (HGameData.Instance.GameMusic == 0) { gameMusicButton.sprite = sprites[0]; } else { gameMusicButton.sprite = sprites[1]; }
        //if (HGameData.Instance.NatureMusic == 0) { natureMusicButton.sprite = sprites[0]; } else { natureMusicButton.sprite = sprites[1]; }
    }


    /// <summary>
    /// 游戏音乐按钮
    /// </summary>
    public void GameMusicButton()
    {
        //HGameData.Instance.GameMusic += 1;
        //if (HGameData.Instance.GameMusic > 1)
        //    HGameData.Instance.GameMusic = 0;
        HGameData.Instance.SaveData();
    }

    /// <summary>
    /// 背景音乐按钮
    /// </summary>
    public void NatureMusicButton()
    {
        //HGameData.Instance.NatureMusic += 1;
        //if (HGameData.Instance.NatureMusic > 1)
        //    HGameData.Instance.NatureMusic = 0;
        //switch (HGameData.Instance.NatureMusic)
        //{
        //    case 0:
        //        PlayerAudioClip.instance.OnPlay();
        //        break;
        //    case 1:
        //        PlayerAudioClip.instance.OnStopAudio();
        //        break;
        //}
        HGameData.Instance.SaveData();
    }

}
