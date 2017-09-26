using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Overlord : MonoBehaviour {
	public List<CardTemplate> deck1;
	public static Overlord overlord;
	void Awake(){
		if (overlord != null) {
			Destroy (this.gameObject);
		}else{
			overlord = this;
		}
		deck1 = new List<CardTemplate> ();
	}

	void Start(){
		DontDestroyOnLoad (this);
	}
	
	public void StartGame(){
		Deckbuilder.deckbuilder.SaveToFile ();
		if (Deckbuilder.deckbuilder.Armor > 0) {
			deck1 = Deckbuilder.deckbuilder.deck;
			foreach (var item in deck1) {
				//Debug.Log ("we got " + item.Name);
			}
			SceneManager.LoadScene ("Main");
		} else {
			Deckbuilder.deckbuilder.armortext.text = "You need atleast 1 defense card";
		}
	}

	void Update(){
		if (Input.GetMouseButtonDown (1)) {
			Ray temppos = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(GM.instance != null)
				if (GM.instance.State != "Discarding")
					GM.instance.hand.SetPositions ();
			RaycastHit hit;
			Physics.Raycast (temppos,out hit);
			hit.collider.gameObject.SendMessage ("RightMouseDown",SendMessageOptions.DontRequireReceiver);
		}
	}
}
