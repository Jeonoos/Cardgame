using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Deck : MonoBehaviour{
	public List<Card> usedCards = new List<Card> ();
	public int TotalHealth;
	public int Health;
	public List<Card> cards = new List<Card>();

	// Use this for initialization
	void Start () {
		//cards.Shuffle ();
		foreach (var item in Overlord.overlord.deck1) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position,Quaternion.Euler (new Vector3(0,0,180))));
			Card cardadded = cards [cards.Count - 1];
			//cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up;
			cardadded.GoToPosition = GM.instance.deckpos.position;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (item);
			cardadded.shouldrender = false;
			//cardadded.transform.parent = GM.instance.DamageBlock;
		}
		TotalHealth = 0;
		foreach (var item in Overlord.overlord.deck1) {
			TotalHealth += item.Armor;
		}
		Health = TotalHealth;
		UpdateHealth ();
		ShuffleAll ();
		GM.instance.hand.DrawHand ();
	}

	/*public void BuildNewDeck(){
		for (int i = 0; i < 4; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			//cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToPosition = GM.instance.deckpos.position;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Hunter.Name);
		}
		for (int i = 0; i < 4; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Marauder);
		}
		for (int i = 0; i < 10; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Sword);
		}
		for (int i = 0; i < 5; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Shield);
		}
		for (int i = 0; i < 4; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Turnaround);
		}
		for (int i = 0; i < 5; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.WarAxe);
		}		
		for (int i = 0; i < 2; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Recovery);
		}
		for (int i = 0; i < 4; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.Advisor);
		}
		for (int i = 0; i < 4; i++) {
			cards.Add(Instantiate(CL.PlainCard, GM.instance.deckpos.position  + Vector3.up * 0.1f * i,Quaternion.identity));
			Card cardadded = cards [cards.Count - 1];
			cardadded.GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cardadded.GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
			cardadded.buildClass (CL.ManaCrystal);
		}
			

		TotalHealth = 0;
		foreach (var item in cards) {
			TotalHealth += item.Armor;
		}
		Health = TotalHealth;
		UpdateHealth ();
		ShuffleAll ();
		//RebuildStack ();
	}*/

	public void UpdateHealth(){
		if (Health > TotalHealth) {
			Health = TotalHealth;
		}
		GM.instance.HealthText.text = Health + "/" + TotalHealth + " Health";
	}
		
	public void RebuildStack(){

	}

	public void TakeDamage(int Damage){
		Health -= Damage;
		if (Health < 1) {
			Debug.Log ("Game Over");
			GM.instance.GameOver (false);
		}
	}

	public void ShuffleAll(){
		cards.AddRange (usedCards);
		cards.Shuffle ();
		usedCards.Clear ();
		RebuildStack ();
		for (int i = 0; i < cards.Count; i++) {
			//cards[i].GoToPosition = GM.instance.deckpos.position + Vector3.up * 0.1f * i;
			cards[i].GoToPosition = GM.instance.deckpos.position;
			cards[i].GoToRotation = Quaternion.Euler (new Vector3(0,0,180));
		}
	}
		
	public Card DrawCard(){
		Card cardToDraw = cards[cards.Count -1];
		cards.RemoveAt (cards.Count-1);
		return cardToDraw;
	}

	public void DebugDeck(){
		string debugstring = "in deck: ";
		foreach (var card in cards) {
			debugstring += "\n " + card.Name;
		}
		Debug.Log (debugstring);
	}
	public void DebugTrash(){
		string debugstring = "in trash: ";
		foreach (var card in usedCards) {
			debugstring += "\n " + card.Name;
		}
		Debug.Log (debugstring);
	}
}

static class MyExtensions
{
	public static void Shuffle<T>(this IList<T> list)  
	{  
		Random rng = new Random();  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0,n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
