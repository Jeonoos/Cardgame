using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects{
	bool accepted;
	int ManaToDrain = 0;
	int mincardsToDiscard = 0;
	int maxcardsToDiscard = 0;
	int cardsdiscarded = 0;
	string instructiontext = "";
	List<Card> discardCards = new List<Card>();
	string effect;
	string nextEffect = "";
	Card callcard;
	public Card drawncard;

	public Dictionary<string, object> variables = new Dictionary<string, object> ();

	public void UseEffect(string effectToCall, Card callingCard){
		//GM.instance.StartCoroutine (this.RunEffect(effectToCall, callingCard));
	}



	void FinishEffect(){

	}

	public void StartEffect(string effect, Card callingCard){
		GM.instance.StartCoroutine (this.ScriptEffect(effect, callingCard));
	}

	IEnumerator ScriptEffect(string effect, Card callingCard){
		callcard = callingCard;
		string CurrentState = GM.instance.State;
		bool donediscarding = false;
		string[] commands = effect.Split (";"[0]);
		for (int i = 0; i < commands.Length-1; i++) {
			yield return GM.instance.StartCoroutine(this.RunLine (commands [i].Split (" "[0])));
		}
		GM.instance.State = CurrentState;
		cardsdiscarded = 0;
		GM.effects = new Effects ();
		GM.instance.InstructionsText.text = "";
		if (GM.instance.State == "Default")
			callingCard.MoveToTrash ();
		GM.instance.hand.CheckForNextPhase ();
	}

	public List<Card> Addcards(List<Card> cardlist, string tag){
		foreach (var item in GM.instance.hand.cards) {
			if (Contains (item, tag))
				cardlist.Add (item);
		}
		return cardlist;
	}

	public List<Enemy> AddEnemy (List<Enemy> enemylist, string tag, int optionalargument){
		foreach (var item in GM.instance.army.army) {
			switch (tag) {
			case "range":
				if (item != null) {
					//Debug.Log (item.y);
					if (item.y < GM.instance.army.frontrow + optionalargument)
						enemylist.Add (item);
					//Debug.Log (enemylist);
				} 
				break;
			default:
				break;
			}
		}
		return enemylist;
	}

	public bool Contains(Card item, string tag){
		switch (tag) {
		case "ranged":
			if (item.attacktype == Attacktype.Ranged)
				return true;
			break;
		case "magic":
			if (item.attacktype == Attacktype.Magic)
				return true;
			break;
		case "melee":
			if (item.attacktype == Attacktype.Melee)
				return true;
			break;
		case "defense":
			if (item.attacktype == Attacktype.Defense)
				return true;
			break;
		case "none":
			if (item.attacktype == Attacktype.None)
				return true;
			break;
		case "action":
			if (item.CostAction)
				return true;
			break;
		default:
			return false;
			break;
		}
		return false;
	}

	IEnumerator RunLine(string[] fragments){
		for (int x = 0; x < fragments.Length; x++) {
			fragments[x] = fragments [x].Trim ();
		}

		Debug.Log (fragments[0]);

		switch (fragments[0]) {
		case "repeat":
			string[] newfragment = new string[fragments.Length - 2];
			for (int x = 2; x < fragments.Length; x++) {
				newfragment [x - 2] = fragments [x];
			}
			for (int i = 0; i < int.Parse(fragments[1]); i++) {
				yield return GM.instance.StartCoroutine (RunLine(newfragment));
			}
			break;
		case "drawuntil":
			Debug.Log (fragments [1]);
			Draw (1);
			while (!Contains (drawncard, fragments [1])) {
				Draw (1);
			}
			break;
		case "modcarddamage":
			List<Card> cards;
			if (fragments [1] == "self")
				cards = new List<Card>{callcard};
			else
				cards = ((List<Card>)(variables [fragments [1]]));
			foreach (var item in cards) {
				switch (fragments[3]) {
				case "add":
					item.Damage += ((int)variables [fragments [2]]); break;
				case "remove":
					item.Damage -= ((int)variables [fragments [2]]);
					break;
				case "set":
					item.Damage = ((int)variables [fragments [2]]);
					break;
				default: break;
				}
				item.UpdateStats ();
			}
			callcard.UpdateStats ();
			GM.instance.hand.UpdateStats ();
			break;
		case "count":
			if (fragments [3] == "value") {
				int counter = 0;
				foreach (var item in (List<Card>)variables[fragments[2]]) {
					switch (fragments [4]) {
					case "armor":
						counter += item.Armor;
						break;
					case "damage":
						counter += item.Damage;
						break;
					default:
						break;
					}
				}
				variables [fragments [1]] = counter;
			} else if (fragments [3] == "number")
				variables[fragments [1]] = ((List<Card>)variables [fragments [2]]).Count;
			break;
		case "newenemylist":
			variables.Add (fragments [1], new List<Enemy> ());
			Debug.Log ((List<Enemy>)variables[fragments[1]]);
			break;
		case "addenemy":
			Debug.Log ((List<Enemy>)variables[fragments[1]]);
			variables[fragments[1]] = AddEnemy (((List<Enemy>)variables[fragments[1]]),fragments[2], int.Parse(fragments[3]));
			break;
		case "killenemy":
			((List<Enemy>)variables [fragments [1]]) [Random.Range (0, ((List<Enemy>)variables [fragments [1]]).Count)].Instakill();
			break;
		case "newcardlist":
			variables.Add (fragments [1], new List<Card> ());
			break;
		case "addcards":
			foreach (var item in variables.Keys) {
				Debug.Log ("Key: " + item);
			}
			//Debug.Log (variables["armorinhand"]);
			variables[fragments[1]] = Addcards (((List<Card>)variables[fragments[1]]), fragments[2]);
			break;
		case "setvar":
			int n;
			int.TryParse (fragments [2], out n);
			if (n != null)
				variables.Add (fragments [1], n);
			else
				variables.Add (fragments [1], fragments [2]);
			Debug.Log ("variable " + fragments[1] +" is " + variables[fragments[1]]);
			break;
		case "discard":
			if (callcard.Discarded == false)
				callcard.Discarded = true;
			Discard (int.Parse(fragments [1]), int.Parse(fragments [2]));
			while (!accepted) {
				yield return null;
			}
			variables [fragments [3]] = cardsdiscarded;
			foreach (var item in discardCards) {
				item.MoveToTrash ();
			}
			break;
		case "draw":
			Draw ((int)variables[fragments[1]]);
			break;
		case "modhandvalue":
			Debug.Log ("adding " + variables [fragments [2]] + " to " + fragments [1]);
			switch (fragments [1]) {
			case "actions": 
				modhandvalue (ref GM.instance.hand.Actions, fragments[3], (int)variables[fragments[2]]);
				break;
			case "supplies": 
				modhandvalue (ref GM.instance.hand.Supplies, fragments[3], (int)variables[fragments[2]]);
				break;
			case "health": 
				modhandvalue (ref GM.instance.deck.Health, fragments[3], (int)variables[fragments[2]]);
				GM.instance.deck.UpdateHealth ();
				break;
			default:
				break;
			}
			GM.instance.hand.UpdateStats ();
			break;
		default:
			break;
		}
	}

	void modhandvalue(ref int value, string op, int amount){
		switch (op) {
		case "add":
			value += amount;
			break;
		case "remove":
			value -= amount;
			break;
		case "set":
			value = amount;
			break;
		default:
			break;
		}
	}

	void Discard(int min, int max){
		mincardsToDiscard = min;
		maxcardsToDiscard = max;
		instructiontext = "Discard " + ((mincardsToDiscard != 0)? (mincardsToDiscard + " to " + maxcardsToDiscard + " cards"): ("up to " + maxcardsToDiscard + " cards"));
		GM.instance.InstructionsText.text = instructiontext; 
		GM.instance.State = "Discarding";
		GM.instance.InstructionsText.GetComponent<TextWrap> ().updateText ();
	}

	void Draw(int amount){
		for (int i = 0; i < amount; i++) {
			drawncard = GM.instance.hand.DrawCard ();
		}
		Accept ();
	}

	public bool Discarded(Card discardedCard){
		bool CanDo = false;
		bool isUnique = true;
		foreach (var item in discardCards) {
			if (item == discardedCard) {
				isUnique = false;
				cardsdiscarded -= 1;
			}
		}
		if (isUnique) {
			if (cardsdiscarded < maxcardsToDiscard || maxcardsToDiscard == 0) {
				discardCards.Add (discardedCard);
				cardsdiscarded += 1;
				CanDo = true;
			}
		} else {
			discardCards.Remove (discardedCard);
		}
		return CanDo;
	}

	public void Accept(){
		if (cardsdiscarded >= mincardsToDiscard && cardsdiscarded <= maxcardsToDiscard)
			accepted = true;
	}


}
