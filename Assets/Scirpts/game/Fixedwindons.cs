using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixedwindons : MonoBehaviour {
    private float screenwight=1280f;//宽

    // Use this for initialization
    void Start() {
        float sclecx = 1f;
        if (Screen.width == 1920)
        {
            sclecx = 1.1f;
        }
        else if (Screen.width == 1024)
        {
            sclecx = 1.07f;
        }
        else if (Screen.width==1334)
        {
            if (Screen.height == 750)
            {
                sclecx = 1.11f;
            }
        }


        else if (Screen.width == 1280)
        {
            if (Screen.height == 720)
            {
                sclecx = 1.1f;
            }
        }
        else if (Screen.width == 854)
        {
            sclecx = 1.11f;
        } else if (Screen.width == 2160)
        {
            sclecx = 1.21f;
        }

        this.gameObject.transform.GetComponent<RectTransform>().localScale = new Vector3(sclecx, 1, 1);
	}
	
	// Update is called once per frame

}
