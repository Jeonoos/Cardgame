using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public static class CL{
	public static CardTemplate[] Cardlist;
	//public static CL list;
	public static Card PlainCard;
	/*
	public static CardTemplate Marauder;
	public static CardTemplate Hunter;
	public static CardTemplate Sword;
	public static CardTemplate WarAxe;
	public static CardTemplate Shield;
	public static CardTemplate Turnaround;
	public static CardTemplate Recovery;
	public static CardTemplate Advisor;
	public static CardTemplate ManaCrystal;
	public static CardTemplate Swap;
	public static CardTemplate Sacrifice;
	public static CardTemplate Flurry;
	public static CardTemplate Fortify;
	public static CardTemplate Restart;
	public static CardTemplate ArmorUp;
	public static CardTemplate Scavenger;
	*/
	public static string[] cards;
	public static void GetList(){
		string path = File.ReadAllText (Application.dataPath + "/Resources/Cards.json");
		JsonData cardData = JsonMapper.ToObject(path);

		GameObject temp = (GameObject)Resources.Load("Cards/PrefabCard");
		PlainCard = temp.GetComponent<Card> ();

		/*Hunter = new CardTemplate (true, Attacktype.Ranged, 1, 0, 0,0, 0, 2, 0, 1,"","", "Hunter");
		Marauder = new CardTemplate (true, Attacktype.Melee, 2, 0, 0, 0, 0, 1, 0, 2,"","", "Marauder");
		Sword = new CardTemplate(false, Attacktype.Melee,3,0,0, 0,0,0,0, 0,"","","Footman");
		WarAxe = new CardTemplate(true, Attacktype.Melee,5,0,0,0,0,0,0,0,"","","Knight");
		Shield = new CardTemplate (false, Attacktype.Defense,1,4,0,0,0,0,0,0,"","","Sentry tower");
		Turnaround = new CardTemplate(true, Attacktype.Melee,1,0,0,0,1,0,0,0,"turnaround","Discard up to 3 cards, then draw that many cards", "Turnaround");
		Recovery = new CardTemplate(true, Attacktype.Melee,1,0,0,3,0,0,0,1,"","", "Combat medic");
		Advisor = new CardTemplate (true, Attacktype.Ranged,1,0,0,0,1,0, 0,0,"advisor", "Discard up to 5 cards, then gain that many actions", "Advisor");
		ManaCrystal = new CardTemplate (false, Attacktype.None,1,0,2,0,0,0,0,0,"", "", "Mana Crystal");
		Swap = new CardTemplate (true, Attacktype.Magic,1,0,0,0,0,2,0,0,"swap", "Discard 3 cards", "Swap");
		Sacrifice = new CardTemplate (true, Attacktype.Melee,9,0,0,0,0,0,0,0,"scavenger", "Discard 2 cards", "Berseker");
		Flurry = new CardTemplate (true, Attacktype.Ranged,1,0,0,0,0,0,0,0,"flurry", "Remove all actions. Draw until you draw an action card", "Last Call");
		Fortify = new CardTemplate (true, Attacktype.Ranged,1,0,0,0,0,0,0,0,"fortify", "Gain health equal to the amount of armor in your hand", "Repair");
		Restart = new CardTemplate (true, Attacktype.None,1,0,0,0,0,0,0,1,"restart", "Draw 2 actions cards", "Inspire");
		ArmorUp = new CardTemplate (true, Attacktype.Melee,1,0,0,0,0,0,0,0,"armorup", "Draw 5 defense cards", "To the walls");
		Scavenger = new CardTemplate (true, Attacktype.Ranged,1,0,0,0,0,0,0,3,"scavenger", "discard 2 cards", "Scavenger");
		Cardlist = new CardTemplate[]{Hunter, Marauder, Sword, WarAxe, Shield, Turnaround, Recovery, Advisor, ManaCrystal, Swap, Sacrifice, Flurry, Fortify, Restart, ArmorUp, Scavenger};
		*/
		Cardlist = new CardTemplate[cardData.Count];
		for (int i = 0; i < Cardlist.Length; i++) {
			Cardlist [i] = new CardTemplate ();
			Cardlist [i].Build (i);
		}
	}

	static Card getCard(string Name){
		Card toreturn;
		GameObject ob = (GameObject)Resources.Load ("Cards/" + Name);
		toreturn = ob.GetComponent<Card> ();
		return toreturn;
	}
		
}
