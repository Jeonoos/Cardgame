using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckbuildCard : CardBase {
	
	public TextMesh renderNumber;
	/*public CardTemplate template;
	public Texture[] textures;
	public TextMesh renderName;
	public TextMesh renderMain;
	public TextMesh renderLabel;
	public TextMesh renderDamage;
	public GameObject typeDisplay;
	Effects effects;
	public bool Used = false;
	public Hand hand;
	#region variables
	public bool CostAction = false;
	public bool CostMana = false;
	public int Damage = 0;
	public Attacktype attacktype = Attacktype.Melee;
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
	#endregion

	public void buildClass(CardTemplate reference){
		template = reference;
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
		Effect = reference.Effect;
		EffectText = reference.Text;
		Name = reference.Name;
	}
	void Start () {
		effects = new Effects ();
		Label = "-";
		if (ManaCost > 0)	{
			CostMana = true;
			Text += "Requires " + ManaCost + " mana. \n";
		}
		if (CostAction) {
			Label += "Action-";
		}
		if (Damage > 0) {
			//Label += "Damage-";
			//Text += "+" + Damage + " Damage. \n";
			renderDamage.text = Damage.ToString();
		}
		if (Supplies > 0) {
			//Label += "Supply-";
			Text += "+" + Supplies + " Supplies. \n";
		}
		if (Mana > 0) {
			//Label += "Mana-";
			Text += "+" + Mana + " Mana. \n";
		}
		if (Heal > 0) {
			//Label += "Heal-";
			Text += "+" + Heal + " Heal. \n   ";
		}
		if (Armor > 0) {
			//Label += "Defense-";
			Text += "+" + Armor + " Defense. \n";
		}
		if (Moves > 0) {
			if (Moves > 1)
				Text += "+" +Moves + " Actions. \n";
			else
				Text += "+" +Moves + " Action. \n";
		}
		if (CardDraw > 0) {
			if (CardDraw > 1)
				Text += "Draw " + CardDraw + " cards. \n";
			else
				Text += "Draw " + CardDraw + " card. \n";

		}
		if (Effect != null) {
			if (Text ==	"") {
				Text = EffectText;
			} else {
				Text += "\n" + EffectText;
			}
		}

		switch (attacktype) {
		case Attacktype.Melee:
			Label += "Melee-";
			Debug.Log ("is melee");
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [0];
			break;
		case Attacktype.Ranged:
			Label += "Ranged-";
			Debug.Log ("is ranged");
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [1];
			break;
		case Attacktype.Magic:
			Label += "Magic-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [2];
			break;
		case Attacktype.Defense:
			Label += "Defense-";
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [3];
			break;
		default:
			Debug.Log ("default");
			typeDisplay.GetComponent<Renderer> ().material.mainTexture = textures [4];
			break;
		}

		name = Name;
		renderName.text = Name;
		renderMain.text = Text;
		renderLabel.text = Label;


	}*/

	void OnMouseDown(){
		Deckbuilder.deckbuilder.AddCard (template, this);
	}

	void RightMouseDown(){
		Deckbuilder.deckbuilder.AddCard (template, this);
	}

}
