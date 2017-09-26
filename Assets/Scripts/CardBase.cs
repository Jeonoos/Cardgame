using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class CardBase : MonoBehaviour {
	public int CardID;
	public CardTemplate template;
	public Texture[] textures;
	public bool shouldrender = false;
	public TextMesh renderName;
	public TextMesh renderMain;
	public TextMesh renderLabel;
	public TextMesh renderDamage;
	public GameObject typeDisplay;
	public GameObject artDisplay;
	public bool Discarded = true;
	public bool Selected = false;
	Effects effects;
	public bool Used = false;
	public Hand hand;

	public bool CostAction = false;
	public bool Command = false;
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
	public string Text = "";
	public string Name = " ";
	string Label = "-";
	public JsonData cardData;


	public Vector3 GoToPosition = Vector3.zero;
	public Quaternion GoToRotation = Quaternion.identity;

	public void buildClass(CardTemplate reference){
		cardData = reference.cardData;
		CardID = reference.cardID;
		CostAction = reference.CostAction;
		attacktype = reference.attacktype;
		Damage = reference.Damage;
		Supplies = reference.Supplies;
		Armor = reference.Armor;
		Mana = reference.Mana;
		Heal = reference.Heal;
		Moves = reference.Moves;
		CardDraw = reference.CardDraw;
		ManaCost = reference.ManaCost;
		Name = reference.Name;
		template = reference;
		/*string path = File.ReadAllText (Application.dataPath + "/Resources/Cards.json");
		JsonData cardData = JsonMapper.ToObject(path);

		CostAction = (bool) cardData[cardname]["CostAction"];
		string typename = (string)cardData [cardname] ["attacktype"];
		attacktype = (Attacktype)System.Enum.Parse (typeof (Attacktype), typename);

		Damage = (int) cardData[cardname]["Damage"];
		Supplies = (int )cardData[cardname]["Supplies"];
		Armor = (int)cardData[cardname]["Armor"];
		Mana = (int )cardData[cardname]["Mana"];
		Heal = (int)cardData[cardname]["Heal"];
		Moves = (int)cardData[cardname]["Moves"];
		CardDraw = (int)cardData[cardname]["CardDraw"];
		ManaCost = (int)cardData[cardname]["ManaCost"];
		Effect = (string)cardData[cardname]["Effect"];
		EffectText = (string)cardData[cardname]["EffectText"];
		Name = (string)cardData[cardname]["Name"];*/
	}
	void Start(){
		Refresh ();
	}

	public void Refresh(){
		effects = new Effects ();
		Text = "";
		Label = "-";

		if (cardData [CardID].Keys.Contains ("Prepare")) {
			Text += "<size=65>Prepare:</size> " + (string)cardData [CardID] ["Prepare"] ["EffectText"] +" # ";
			Label += "Strategist-";
		} else if (CostAction) {
			//Label += "Action-";
			Label += "Provisioner-";
			Text += "<size=65>Prepare:</size> ";
		}
		if (cardData [CardID].Keys.Contains ("Prepare"))
			CostAction = true;
		if (cardData [CardID].Keys.Contains ("Command"))
			Command = true;
		if (ManaCost > 0)	{
			CostMana = true;
			Text += "Requires " + ManaCost + " mana. # ";
		}


			//Label += "Damage-";
			//Text += "+" + Damage + " Damage. \n";
			renderDamage.text = Damage.ToString();


		if (Supplies > 0) {
			//Label += "Supply-";
			Text += "+" + Supplies + " Supplies. # \n";
		}
		if (Mana > 0) {
			//Label += "Mana-";
			Text += "+" + Mana + " Mana. # \n";
		}
		if (Heal > 0) {
			//Label += "Heal-";
			Text += "+" + Heal + " Heal. # \n";
		}
		if (Armor > 0) {
			//Label += "Defense-";
			//Text += "+" + Armor + " Defense. # \n";
		}
		if (Moves > 0) {
			if (Moves > 1)
				Text += "+" +Moves + " Actions. # \n";
			else
				Text += "+" +Moves + " Action. # \n";
		}
		if (CardDraw > 0) {
			if (CardDraw > 1)
				Text += "Draw " + CardDraw + " cards. # \n";
			else
				Text += "Draw " + CardDraw + " card. # \n";

		}
		if (cardData [CardID].Keys.Contains ("Command")) {
			Text += "<size=65>Command:</size> " + (string)cardData [CardID] ["Command"] ["EffectText"] + " # ";
			Label += "Commander-";
		}
		switch (attacktype) {
		case Attacktype.Melee:
			//Label += "Melee-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [0];
			break;
		case Attacktype.Ranged:
			//Label += "Ranged-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [1];
			break;
		case Attacktype.Magic:
			//Label += "Magic-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [2];
			break;
		case Attacktype.Defense:
			//Label += "Defense-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [3];
			break;
		default:
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [4];
			break;
		}
		Label = (Label == "-") ? "" : Label;
		artDisplay.GetComponent<Renderer> ().material.mainTexture = ((Texture)Resources.Load ("CardArt/" + Name)!= null)? (Texture)Resources.Load ("CardArt/" + Name): (Texture)Resources.Load ("CardArt/Default");
		name = Name;
		renderName.text = Name;
		renderMain.text = Text;
		renderLabel.text = Label;
		renderMain.GetComponent<TextWrap> ().updateText ();
	}
	public void UpdateStats(){
		Start ();
		GM.instance.hand.UpdateStats ();
	}
}
