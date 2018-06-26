using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
public class gametext : MonoBehaviour {

     void Awake()
    {
       
    }

    // Use this for initialization
    void Start () {
        Screen.orientation = ScreenOrientation.Landscape;
        StartCoroutine(myloadhall());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator myloadhall()
    {

        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(1);
    }
}
