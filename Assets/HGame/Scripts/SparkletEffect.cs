using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SparkletEffect : MonoBehaviour
{

    float times;
    float speed = 0.1f;

    public Sprite[] sprite;
    private int spriteNum = 0;

    private void Update()
    {
        times += Time.deltaTime;
        if (times >= speed)
        {
            times = 0;
            spriteNum++;
            if (spriteNum >= sprite.Length)
                spriteNum = 0;
            this.GetComponent<Image>().sprite = sprite[spriteNum];
        }
    }
}
