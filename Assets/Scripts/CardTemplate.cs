using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class CardTemplate{
	public int cardID;
	public bool CostAction = false;
	public Attacktype attacktype = Attacktype.Melee;
	public int Damage = 0;
	public int Supplies = 0;
	public int Armor = 0;
	public int Mana = 0;
	public int Heal = 0;
	public int Moves = 0;
	public int CardDraw = 0;
	public int ManaCost = 0;
	public string Name = "";
	public JsonData cardData;
	public void Build(int i){
		cardID = i;
		string path = File.ReadAllText (Application.dataPath + "/Resources/Cards.json");
		cardData = JsonMapper.ToObject(path);

		CostAction = (cardData[i].Keys.Contains("CostAction"))?(bool) cardData[i]["CostAction"]: false;
		if (cardData [i].Keys.Contains ("Prepare"))
			CostAction = true;
		string typename = (cardData[i].Keys.Contains("attacktype"))?(string) cardData[i]["attacktype"]: "None";
		attacktype = (Attacktype)System.Enum.Parse (typeof (Attacktype), typename);
		Damage = (cardData[i].Keys.Contains("Damage"))?(int) cardData[i]["Damage"]: 0;
		Supplies = (cardData[i].Keys.Contains("Supplies"))?(int) cardData[i]["Supplies"]: 0;
		//Armor = (cardData[i].Keys.Contains("Armor"))?(int) cardData[i]["Armor"]: 0;
		if (attacktype == Attacktype.Defense)
			Armor = (cardData[i].Keys.Contains("Damage"))?(int) cardData[i]["Damage"]: 0;
		Mana = (cardData[i].Keys.Contains("Mana"))?(int) cardData[i]["Mana"]: 0;
		Heal = (cardData[i].Keys.Contains("Heal"))?(int) cardData[i]["Heal"]: 0;
		Moves = (cardData[i].Keys.Contains("Moves"))?(int) cardData[i]["Moves"]: 0;
		CardDraw = (cardData[i].Keys.Contains("CardDraw"))?(int) cardData[i]["CardDraw"]: 0;
		ManaCost = (cardData[i].Keys.Contains("ManaCost"))?(int) cardData[i]["ManaCost"]: 0;
		Name = (cardData[i].Keys.Contains("Name"))?(string) cardData[i]["Name"]: "NoName";

	}
}
