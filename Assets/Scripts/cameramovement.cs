using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramovement : MonoBehaviour {

	float dy;
	// Use this for initialization
	void Start () {
		dy = Camera.main.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			dy = Input.GetAxis ("Mouse ScrollWheel");
		}else{
			dy = dy * 0.95f;
		}
		float camY = Camera.main.transform.position.z;
		camY += dy * 800 * Time.deltaTime;
		camY = Mathf.Clamp (camY, -26, GM.instance.army.y * 15);
		Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,Camera.main.transform.position.y,camY);
	}
}
