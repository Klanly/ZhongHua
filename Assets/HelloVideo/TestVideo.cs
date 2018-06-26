using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVideo : MonoBehaviour
{
    
    static HelloUnityVideo app = null;
	// Use this for initialization
	void Start ()
    {
        if (!ReferenceEquals(app, null))
        {
            app = new HelloUnityVideo();
            app.loadEngine();
        }
        app.join("BaiJiaLe");
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnOpenVideo()
    {
       
    }

}
