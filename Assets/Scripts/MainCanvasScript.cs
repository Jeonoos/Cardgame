using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasScript : MonoBehaviour {
	public GameObject gameoverscreen;
	public Text defeat;
	public Text win;

	public void SetGameOverScreen(bool won){
		gameoverscreen.gameObject.SetActive (true);
		if (won) {
			win.gameObject.SetActive (true);
		} else {
			defeat.gameObject.SetActive (true);
		}
	}
}
