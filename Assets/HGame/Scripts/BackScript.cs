using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackScript : MonoBehaviour
{
    public GameObject BackBG;
    public Image Back;
    private float backTime;
    private int backNum = 0;
    public Sprite[] back;

    void Update()
    {
        if (BackBG.activeSelf)
        {
            backTime += Time.deltaTime;
            if (backTime >= 0.05f)
            {
                backTime = 0;
                backNum++;
                if (backNum >= back.Length)
                {
                    backNum = 0;
                }
                Back.sprite = back[backNum];

            }
        }
    }
}
