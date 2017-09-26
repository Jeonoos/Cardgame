using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Card : CardBase{
	bool hasarrived = false;
	bool isdragging = false;

	Vector3 inspectingvector = new Vector3 (2.5f, 25.45f, -16.45f);
	Quaternion inspectingquat = Quaternion.Euler(new Vector3(-23.74f,0,0));
	Quaternion defaultquat = Quaternion.Euler (-17, 0, 0);
	Quaternion discardingquat = Quaternion.Euler (-30, 0, 0);
	public void SetPosition(int pos, int total){
		float maxsize = Mathf.Pow(hand.cards.Count, 0.35f) * 2;
		if (maxsize > 8)
			maxsize = 8;
		GoToPosition = (new Vector3 ((10 * pos/(hand.cards.Count / maxsize) - 5 * (total-1)/(hand.cards.Count / maxsize)),0.1f * pos,0) + GM.instance.handpos.position);
		GoToRotation = defaultquat;
	}

	public void Update(){
		if (isdragging) {
			presstimer += Time.deltaTime;
		}
//		if (hasarrived && isdragging) {
//			Ray temppos = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			Physics.Raycast (temppos,out hit);
//			Vector3 pos = hit.point;
//			pos.y = transform.position.z;
//			pos.y = -pos.z;
//
//			transform.position = Vector3.Lerp (transform.position, pos, 5 * Time.deltaTime);
//		} else {
		if (Vector3.Distance (transform.position, GoToPosition) < 0.01) {
				if (!hasarrived) {
					hasarrived = true;
					Render ();
				}
			} else {
			transform.position = Vector3.Lerp (transform.position, GoToPosition, 5 * Time.deltaTime);
			}
			transform.rotation = Quaternion.Lerp (transform.rotation, GoToRotation, 5 * Time.deltaTime);

	}
	bool isInspecting = false;
	void RightMouseDown(){
		if (!Discarded) {
			hand.SetPositions ();
			if (!isInspecting) {
				GoToPosition = inspectingvector;
				GoToRotation = inspectingquat;
				isInspecting = true;
			} else
				isInspecting = false;
		}
	}

	public string GetActivationState(){
		//Debug.Log ("Getting state");
		if (cardData [CardID].Keys.Contains ("Prepare") || CostAction) {
			return "Default";
		} else if (cardData [CardID].Keys.Contains ("Command")) {
			return "Attacking";
		}
		return null;
	}

	public void Activate(){
		if (GM.instance.State == "Default") {
			bool canActivate = false;
			if (CostAction) {
				canActivate = true;
				if (hand.Actions <= 0)
				//not enough actions
				canActivate = false;
			}
			if (ManaCost > hand.Mana + hand.TempMana) {
				//not enough mana
				canActivate = false;
			}
			if (!CostAction && Mana > 0) {
				hand.TempMana += Mana;
				hand.UpdateStats ();
				MoveToTrash ();
			}
			if (canActivate) {
				hand.Mana -= ManaCost;
				hand.Actions -= 1;
				hand.Actions += Moves;
				hand.Supplies += Supplies;
				GM.instance.deck.Health += Heal;
				GM.instance.deck.UpdateHealth ();
				hand.Mana += Mana;
				for (int i = 0; i < CardDraw; i++) {
					hand.DrawCard ();
				}
				if (cardData [CardID].Keys.Contains ("Prepare")) {
					if (cardData [CardID] ["Prepare"].Keys.Contains ("EffectScript"))
						GM.effects.StartEffect ((string)cardData [CardID] ["Prepare"] ["EffectScript"], this);
					else
						GM.effects.UseEffect ((string)cardData [CardID] ["Prepare"] ["Effect"], this);
				}else
					MoveToTrash ();
			}
		} else if (GM.instance.State == "Discarding" && !Discarded) {
			bool isunique = GM.effects.Discarded (this);
			if (isunique) {
				GoToRotation = discardingquat;
				Selected = true;
			} else {
				GoToRotation = defaultquat;
				Selected = false;
			}
		} else if (GM.instance.State == "Attacking") {

			if (cardData [CardID].Keys.Contains ("Command")){
				if (hand.Actions > 0) {
					hand.Actions--;
					if (cardData [CardID] ["Command"].Keys.Contains ("EffectScript"))
						GM.effects.StartEffect ((string)cardData [CardID] ["Command"] ["EffectScript"], this);
					else
						GM.effects.UseEffect ((string)cardData [CardID] ["Command"] ["Effect"], this);
					hand.UpdateStats ();
					GM.instance.deck.UpdateHealth ();
				}
			}
		}
		hand.UpdateStats ();
		GM.instance.hand.CheckForNextPhase ();
	}
	float presstimer = 0;
	void OnMouseDown(){
		if (!Discarded) {
		Debug.Log ("Clicked On " + Name);
		isdragging = true;
		//if (!Discarded) {
		//	Activate ();
		//}
		}
	}

	void OnMouseUp(){
		if (!Discarded) {
			isdragging = false;
			if (presstimer < 0.2f) {
				Activate ();
			}
			presstimer = 0;
		}
	}

	public void StateChanged(){

		gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor",  Color.black);
		if (!Discarded) {
			if (cardData [CardID].Keys.Contains ("Command") && GM.instance.State == "Attacking" && hand.Actions > 0) {
				gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", gameObject.GetComponent<Renderer> ().material.color * 0.12f);
			}
			if ((cardData [CardID].Keys.Contains ("Prepare") || CostAction) && (GM.instance.State == "Default" || GM.instance.State == "Discarding") && hand.Actions > 0) {
				gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", gameObject.GetComponent<Renderer> ().material.color * 0.12f);
			}
		}
	}

	public void MoveToTrash(){
		buildClass (template);
		Refresh ();
		hasarrived = false;
		foreach (var item in GM.instance.deck.usedCards) {
			if (GM.instance.deck.usedCards.IndexOf (item) != GM.instance.deck.usedCards.Count - 1) {
				item.shouldrender = false;
				item.Render ();
			}else{
				item.GoToPosition = item.GoToPosition - Vector3.up * 0.2f;
			}
		}
		hand.SetPositions ();
		Selected = false;
		Discarded = true;
		GM.instance.deck.usedCards.Add (this);
		//GoToPosition = GM.instance.trashpos.position + Vector3.up * GM.instance.deck.usedCards.Count * 0.1f;
		GoToRotation = Quaternion.Euler(0,0,0);
		GoToPosition = GM.instance.trashpos.position;
		hand.cards.Remove (this);
		hand.UpdateStats ();
		hand.SetPositions ();
		StateChanged ();
	}
		

	void OnMouseEnter(){
		//if (!Discarded) {
		//	GoToPosition += Vector3.up;
		//}
	}

	void OnMouseExit(){
		//if (!Discarded) {
		//	GoToPosition -= Vector3.up;
		//}
	}

	public void Render(){
		if (shouldrender) {
			gameObject.GetComponent<MeshRenderer> ().enabled = true;
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.GetComponent<MeshRenderer> ().enabled = true;
			}
		} else {
			gameObject.GetComponent<MeshRenderer> ().enabled = false;
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}
}
[System.Serializable]
public class Stats
{
	public bool CostAction = false;
	public bool CostMana = false;
	public Attacktype attacktype = Attacktype.Melee;
	public int Damage = 0;
	public int Supplies = 0;
	public int Armor = 0;
	public int Mana = 0;
	public int Heal = 0;
	public int Moves = 0;
	public int CardDraw = 0;
	public int ManaCost = 0;
	string Effect = " ";
	string EffectText = " ";
	string Text = "";
	public string Name = " ";
	string Label = "-";
}

public enum Attacktype 
{
	Melee,Ranged,Magic,Defense,None
}

public enum Triggertype
{
	Prepare, Command, Loot
}
