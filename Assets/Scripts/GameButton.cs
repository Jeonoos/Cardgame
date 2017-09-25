using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour {
	public string ButtonName;
	public TextMesh DefaultText;

	public void SetText(string text){
		DefaultText.text = text;
	}

	void OnMouseUp(){
		//GM.instance.ButtonClicked (this);
	}
}
