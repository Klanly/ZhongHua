using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationHide : MonoBehaviour
{
    public Sprite[] sprites;


    private float speed;

    private int num = 0;

    private float times;

    private void Update()
    {
        times += Time.deltaTime;
        if (times >= speed)
        {
            times = 0;
            GetComponent<Image>().sprite = sprites[num];
            num++;
            if (num >= sprites.Length)
            {
                num = 0;
            }
        }

    }
}
