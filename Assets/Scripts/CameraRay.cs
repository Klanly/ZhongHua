using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour {

    private Ray ray;
	private RaycastHit hit;

	
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if(Physics.Raycast(ray,out hit,200,9))
			{
				Debug.Log (hit.collider.gameObject.name);
			}

		}
	}
}
