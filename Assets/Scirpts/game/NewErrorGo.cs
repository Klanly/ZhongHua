using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewErrorGo : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("OnCreate");		
	}
	
	// Update is called once per frame
	
    IEnumerator OnCreate()
    {
        LoginInfo.Instance().mylogindata.isOpenError = false;
        transform.DOScale(Vector3.one, 0.7f);

        yield return new WaitForSeconds(0.7f);
        LoginInfo.Instance().mylogindata.isOpenError = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
