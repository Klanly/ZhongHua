using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumSprite : MonoBehaviour
{
    public bool isChoose ;
  
    IEnumerator ChooseSprite(float time)
    {
        while (isChoose)
        {

            int randomNum = Random.Range(0, 10);
            transform.GetComponent<Image>().sprite = NumSpriteControl.Instances.num_Sprite[randomNum];
            yield return new WaitForSeconds(time);
           
            if (isChoose == false)
            {                
                break;
            }

        }
    }
    void Update()
    {
        //Debug.Log(isChoose);
    }

}

