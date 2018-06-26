using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{

    public float PlaySpeed;

    private float PlayTime;

    public Sprite[] SequenceFrame;

    private int num = 0;


    private void Update()
    {
        PlayTime += Time.deltaTime;
        if (PlayTime >= PlaySpeed)
        {
            PlayTime = 0;
            GetComponent<Image>().sprite = SequenceFrame[num];
            num++;
            if (num >= SequenceFrame.Length)
                num = 0;
        }
    }
}
