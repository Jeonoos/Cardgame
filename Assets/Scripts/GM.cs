using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour {
	public Button turnbutton;
	public Light Sun;
	public AttackButton attackingbutton;
	public string State = "Default";
	public Text DamageTextMelee;
	public Text DamageTextRanged;
	public Text DamageTextMagic;
	public Transform DamageBlock;
	public Text MovesText;
	public TextMesh InstructionsText;
	public Text HealthText;
	//public static CL list;
	public static GM instance;
	public static Effects effects;
	public Deck deck;
	public Hand hand;
	public Transform deckpos;
	public Transform handpos;
	public Transform trashpos;
	public Transform armypos;
	public static EnemyList enemies;
	public Army army;
	public MainCanvasScript Canvas;
	Vector3 BlockrelativePos;
	// Use this for initialization
	void Start () {
		attackingbutton = new AttackButton();
		BlockrelativePos = DamageBlock.position - Camera.main.transform.position;
		//state = new GameState ();
		effects = new Effects ();
		hand = new Hand();
		//list = new CL();
		CL.GetList ();
		instance = this;
		enemies = new EnemyList ();
		enemies.GetEnemies ();
		//deck.BuildNewDeck ();
		army.Spawn ();
		//hand.DrawHand ();
		//StateChanged();
	}
	string defaultstate = "Nothing";
	Quaternion sunrotation = Quaternion.identity;
	void Update(){
		//if (Input.GetKeyUp (KeyCode.Space)) {
		//	hand.DrawCard ();
		//}
		//if (Input.GetKeyUp (KeyCode.A)) {
		//	hand.DiscardHand ();
		//}
		//if (Input.GetKeyUp (KeyCode.Q)) {
		//	deck.RebuildStack ();
		//}
		if (State != defaultstate){
			defaultstate = State;
			StateChanged ();
		}

		Sun.transform.rotation = Quaternion.Lerp (Sun.transform.rotation, sunrotation, 0.04f);

		DamageBlock.position = Vector3.Lerp (DamageBlock.position, Camera.main.transform.position + BlockrelativePos,0.1f);
	}

	public void BackToMenu(){
		SceneManager.LoadScene ("Deckbuild");
	}

	public void GameOver(bool won){
		Canvas.SetGameOverScreen (won);
	}
	public void StateChanged(){
		army.UpdateArmy ();
		//hand.CheckForNextPhase ();
		foreach (var item in hand.cards) {
			item.StateChanged ();
		}
		switch (State) {
		case "Default":
			turnbutton.transform.Find("Text").gameObject.GetComponent<Text>().text = "Attack";
			sunrotation = Quaternion.Euler (new Vector3 (100, 0, 0));
			//attackingbutton.light.enabled = false;
			//Torch.enabled = false;
			break;
		case "Attacking":
			turnbutton.transform.Find("Text").gameObject.GetComponent<Text>().text = "End Turn";
			sunrotation = Quaternion.Euler (new Vector3 (250, 0, 0));
			//attackingbutton.light.enabled = true;
			switch (attackingbutton.attacktype) {
			case Attacktype.Melee:
				//Torch.color = new Color();
				break;
			case Attacktype.Ranged:
				//Torch.color = Color.yellow;
				break;
			case Attacktype.Magic:
				//Torch.color = Color.cyan;
				break;
			default:
				break;
			}
			//Torch.enabled = true;
			break;
		case "Night":
			sunrotation = Quaternion.Euler (new Vector3 (300, 0, 0));
			//attackingbutton.light.enabled = false;
			//Torch.enabled = true;
			break;
		default:
			sunrotation = Quaternion.Euler(new Vector3(100,0,0));
			//attackingbutton.light.enabled = false;
			//.enabled = false;
			break;
		}
		Sun.transform.rotation = Quaternion.Lerp (Sun.transform.rotation, sunrotation, 0.04f);
	}

	public void ButtonClicked(Button button){
		if (button.name == "EndTurn") {
			if (State != "Discarding" && State != "Night") {
				if (State == "Attacking") {
					GM.instance.State = "Default";
					hand.DiscardHand ();
					army.EndTurn ();
					State = "Night";
				}else{
					GM.instance.State = "Attacking";
				}
			}
		}
		if (button.name == "Accept") {
			effects.Accept ();
		}
	}

	public void SetAttack(Slider slider){
		switch ((int)slider.value) {
		case 0:
			attackingbutton.attacktype = Attacktype.Melee;
			break;
		case 1:
			attackingbutton.attacktype = Attacktype.Ranged;
			break;
		case 2:
			attackingbutton.attacktype = Attacktype.Magic;
			break;
		default:
			break;
		}
		//attackingbutton.light.enabled = false;
		//attackingbutton = this;
		//if (State == "Attacking")
			//light.enabled = true;
		army.UpdateArmy ();
	}
}
