using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class Deckbuilder : MonoBehaviour {
	public List<CardTemplate> deck = new List<CardTemplate> ();
	public static Deckbuilder deckbuilder;
	public Text armortext;
	public int Armor = 0;
	public DeckbuildCard prefabcard;
	public List<DeckbuildCard> buildcardslist = new List<DeckbuildCard> ();
	// Use this for initialization
	void Start () {
		CL.GetList ();
		deckbuilder = this;
		for (int i = 0; i < CL.Cardlist.Length; i++) {
			DeckbuildCard builtcard = Instantiate (prefabcard, new Vector3(i * 10,0,0),Quaternion.identity);
			builtcard.buildClass (CL.Cardlist[i]);
			//Debug.Log ("adding " + builtcard.Name + " to buildcardslist");
			buildcardslist.Add (builtcard);
		}

		StreamReader reader = new StreamReader (Application.dataPath + "/Resources/Deck.txt");
		Overlord.overlord.deck1 = new List<CardTemplate> ();
		string readline = reader.ReadToEnd();
		string[] lines = readline.Split (';');
		foreach (var item in lines) {
			if (item != "")
				AddCard (CL.Cardlist[int.Parse(item)], prefabcard);
		}
		reader.Close ();

		foreach (var item in Overlord.overlord.deck1) {
			Debug.Log ("adding item");
			AddCard (item, prefabcard);
		}
	}

	public void SaveToFile(){
		Debug.Log ("saving");
		StreamWriter textfile = new StreamWriter (Application.dataPath + "/Resources/Deck.txt");
		foreach (var item in deck) {
			//Debug.Log (item.Name);
			textfile.Write (item.cardID.ToString() + ";");
		}
		textfile.Close ();
	}

	// Update is called once per frame
	float dx;
	void Update () {
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			dx = Input.GetAxis ("Mouse ScrollWheel");
		}else{
			dx = dx * 0.95f;
		}
		float camX = Camera.main.transform.position.x;
		camX += dx * 800 * Time.deltaTime;
		if (camX < 10) {
			camX = 10;
		}
		if (camX > 10 * CL.Cardlist.Length) {
			camX = 10 * CL.Cardlist.Length;
		}
		Camera.main.transform.position = new Vector3 (camX,Camera.main.transform.position.y,Camera.main.transform.position.z);
	}

	public void startgame(){
		Overlord.overlord.StartGame ();
	}
	// Update is called once per frame
	public void AddCard(CardTemplate template, DeckbuildCard card){

		Armor = 0;
		int amountindeck = 0;
		foreach (var item in deck) {
			if (item == template) {
				amountindeck += 1;
			}
			Armor += item.Armor;
		}
		if (Input.GetKey (KeyCode.Mouse1)) {
			if (Input.GetKey (KeyCode.LeftControl)) {
				deck.RemoveAll (x => x.cardID == template.cardID);
			} else {
				deck.Remove (template);
			}
		} else {
			if (Input.GetKey (KeyCode.LeftControl)) {
				deck.RemoveAll (x => x.cardID == template.cardID);
				for (int i = 0; i < 10; i++) {
					deck.Add (template);
				}
			} else {
				if (amountindeck < 10) {
					deck.Add (template);
				}
			}
		}
		Armor = 0;
		amountindeck = 0;
		foreach (var item in deck) {
			if (item == template) {
				amountindeck += 1;
			}
			Armor += item.Armor;
		}
		armortext.text = "Total deck health: " + Armor;
		buildcardslist.Find( x => x.Name == template.Name).renderNumber.text= amountindeck.ToString ();
	}
}
