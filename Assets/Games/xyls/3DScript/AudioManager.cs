using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource Win_1;
    public UISlider MusicSlider;
    public UISlider YinXiaoSlider;
	public GameObject[] objs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeMusicValue() {

        Win_1.volume = MusicSlider.value;
    }

    public void ChangeYinXiaoValue() {
		for (int i = 0; i < objs.Length; i++) {
			objs[i].transform.GetComponent<UIPlaySound>().volume=YinXiaoSlider.value;
		}
        
    }
}
