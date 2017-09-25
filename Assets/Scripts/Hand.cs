using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour{
	public int MaxHandSize = 11;

	public int Actions = 1;
	public int Supplies = 1;
	public int DamageMelee = 0;
	public int DamageRanged = 0;
	public int DamageMagic = 0;

	public int DamagetempMelee = 0;
	public int DamagetempRanged = 0;
	public int DamagetempMagic = 0;
	public int Mana = 0;
	public int TempMana = 0;

	public List<Card> cards = new List<Card>();

	public Card DrawCard(){
		if (GM.instance.deck.cards.Count > 0) {
			Card cardToDraw = GM.instance.deck.DrawCard ();
			if (GM.instance.deck.cards.Count != 0) {
				GM.instance.deck.cards [GM.instance.deck.cards.Count - 1].shouldrender = true;
				GM.instance.deck.cards [GM.instance.deck.cards.Count - 1].Render ();
			}
			cards.Add (cardToDraw);
			cardToDraw.hand = this;
			cardToDraw.Discarded = false;
			cardToDraw.shouldrender = true;
			cardToDraw.Render ();
			//Debug.Log ("Added " + cardToDraw.Name);
			SetPositions ();
			if (cards.Count > MaxHandSize) {
				cardToDraw.MoveToTrash ();
			}
			//UpdateStats ();
			return cardToDraw;
		} else {
			
			Debug.Log ("No more cards in deck, shuffling");
			GM.instance.deck.ShuffleAll ();
			Card cardToDraw = null;
			if (GM.instance.deck.cards.Count != 0) {

				cardToDraw = DrawCard ();
				//GM.instance.state.NewTurnStarts ();

			}
			return cardToDraw;
		}
	}

	public void DrawHand(){
		for (int i = 0; i < 5; i++) {
			DrawCard ();
		}
		CheckForNextPhase ();
		UpdateStats ();
	}
		
	public void CheckForNextPhase(){
		//Debug.Log ("checking next phase");
		bool anyactioncards = false;
		foreach (var item in cards) {
			if (item.GetActivationState () == GM.instance.State) {
				//Debug.Log ("card " + item.Name + " can be used this turn");
				anyactioncards = true;
			}
		}
		//Debug.Log ("any action cards: " + anyactioncards + " but state is " + GM.instance.State);
		if (!anyactioncards || Actions <= 0) {
			if (GM.instance.State == "Default") {
				GM.instance.State = "Attacking";
				GM.instance.StateChanged ();
			}
		}
	}
	public void DiscardHand(){
		foreach (var item in cards) {
			Mana += item.Mana;
		}
		while (cards.Count > 0){
			cards[0].MoveToTrash ();
		}
		//GM.instance.deck.DebugTrash ();
		Actions = Supplies;
		Supplies = 1;
		DamageMelee = 0;
		TempMana = 0;
		SetPositions ();
		NewTurn ();
		UpdateStats ();
	}

	public void SetPositions(){
		
		foreach (var item in cards) {
			item.SetPosition (cards.IndexOf (item), cards.Count);
		}

	}

	public void NewTurn(){
		DamagetempMelee = 0;
		DamagetempRanged = 0;
		DamagetempMagic = 0;
	}

	public void UpdateStats(){
		GM.instance.StateChanged ();
		DamageMelee = DamagetempMelee;
		DamageRanged = DamagetempRanged;
		DamageMagic = DamagetempMagic;
		foreach (var item in cards) {
			if (item.attacktype == Attacktype.Melee)
				DamageMelee += item.Damage;

			if (item.attacktype == Attacktype.Ranged)
				DamageRanged += item.Damage;

			if (item.attacktype == Attacktype.Magic)
				DamageMagic += item.Damage;
		}


		GM.instance.DamageTextMelee.text = DamageMelee.ToString();
		GM.instance.DamageTextRanged.text = DamageRanged.ToString();
		GM.instance.DamageTextMagic.text = DamageMagic.ToString();

		//GM.instance.ManaText.text = Mana + " Mana " + TempMana + " Temporary Mana";
		if (Actions != 1) {
			GM.instance.MovesText.text = Actions + " Actions" ;
		} else {
			GM.instance.MovesText.text = "1 Action" ;
		}
		if (Supplies != 1){
			GM.instance.MovesText.text += " | "+ Supplies + " Supplies";
		}else{
			GM.instance.MovesText.text += " | " +Supplies + " Supply";
		}
	}
}
