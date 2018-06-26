using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class playformess : MonoBehaviour {

   public AudioSource winplay;

    private void OnEnable()
    {
        Audiomanger._instenc.playfromGo(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
