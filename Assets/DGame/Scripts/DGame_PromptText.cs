using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DGame_PromptText : MonoBehaviour
{

    private float speed = 0.005f;

    public float num = 1;

    private int type = 1;

    public float max = 1.4f;

    public float min = 1;

    private void Update()
    {
        if (type == 1)
        {
            num += speed;
            if (num >= max)
            {
                type = 2;
            }
        }
        else
        {
            num -= speed;
            if (num <= min)
            {
                type = 1;
            }
        }
        GetComponent<RectTransform>().localScale = new Vector3(num, num, 1);
    }
}
