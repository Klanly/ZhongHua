using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptText : MonoBehaviour
{
    private float speed = 0.005f;

    private float num = 1;

    private int type = 1;

    private void Update()
    {
        if (type == 1)
        {
            num += speed;
            if (num >= 1.4f)
            {
                type = 2;
            }
        }
        else
        {
            num -= speed;
            if (num <= 1f)
            {
                type = 1;
            }
        }
        GetComponent<RectTransform>().localScale = new Vector3(num, num, 1);
    }

}
