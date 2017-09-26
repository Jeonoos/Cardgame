using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	//public int Strength;
	public TextMesh healthText;
	public TextMesh damageText;
	public TextMesh nameText;
	public TextMesh textText;
	public Transform ModelNode;
	public int Health;
	public int HealthMelee;
	public int HealthRanged;
	public int HealthMagic;
	public int Damage;
	public List<string> Traits;
	public string Name;
	public string Text = "";
	public int x;
	public int y;
	public Vector3 goToPos = Vector3.zero;

	bool Contains(string tag){
		foreach (var item in Traits) {
			if (item == tag)
				return true;
		}
		return false;
	}

	bool CheckForLower(){
		bool lowerexists = false;
		if (GM.instance.State != "Attacking")
			return true;
		for (int i = 0; i < GM.instance.army.army.GetLength(0); i++) {
			if (y != 0) {
				for (int dy = 0; dy < y; dy++) {
					if (GM.instance.army.army [i, dy] != null && GM.instance.attackingbutton.attacktype == Attacktype.Melee) {
						lowerexists = true;
					}
				}

				for (int z = 0; z < y; z++) {
					if (GM.instance.army.army [x, z] != null && GM.instance.attackingbutton.attacktype == Attacktype.Ranged) {
						lowerexists = true;
					}
				}

				if (y != 1) {
					for (int dy = 1; dy < y; dy++) {
						if (GM.instance.army.army [i, dy-1] != null && GM.instance.attackingbutton.attacktype == Attacktype.Magic) {
							lowerexists = true;
						}
					}
				}
			}
		}
		return lowerexists;
	}

	void BuildTraits(){
		if (Traits.Contains ("Flying")) {
			Text += string.Format("<color=yellow>{0}</color>", "Flying ");
			//Debug.Log (string.Format("x={0} y={1} Name={2} uses flying", x,y,Name));
			HealthMelee += 2;
			HealthMagic += 2;
		}
		if (Traits.Contains ("Sneaky")) {
			Text += string.Format("<color=red>{0}</color>", "Sneaky ");
			//Debug.Log (string.Format("x={0} y={1} Name={2} uses sneaky", x,y,Name));
			HealthRanged += 2;
			HealthMagic += 2;
		}
		if (Traits.Contains ("Armored")) {
			Text += string.Format("<color=cyan>{0}</color>", "Armored ");
			//Debug.Log (string.Format("x={0} y={1} Name={2} uses armored", x,y,Name));
			HealthMelee += 2;
			HealthRanged += 2;
		}
		if (Traits.Contains ("Rush")) {
			Text += "Rush ";
		}
	}

	bool Kill (){
		Debug.Log ("killing");
		bool succeeded = false;
		bool lowerexists = CheckForLower ();
		if (!lowerexists) {
			if (checkforlethal ()) {
				succeeded = true;
				switch (GM.instance.attackingbutton.attacktype) {
					case Attacktype.Melee: GM.instance.hand.DamagetempMelee -= HealthMelee; break;
					case Attacktype.Ranged: GM.instance.hand.DamagetempRanged -= HealthRanged; break;
					case Attacktype.Magic: GM.instance.hand.DamagetempMagic -= HealthMagic; break;
					default: break;
				}
				Destroy (this.gameObject);
				GM.instance.hand.UpdateStats ();
				GM.instance.army.EnemyDied (x, y);
			}
		}
		return succeeded;
	}

	public void BuildEnemy(EnemyTemplate template){
		Damage = template.Damage;
		Health = template.Health;

		Traits = template.Traits;

		HealthMelee = Health;
		HealthRanged = Health;
		HealthMagic = Health;
		textText.richText = true;
		if (Traits != null)
			BuildTraits ();
		else
			Traits = new List<string> ();

		healthText.text = Health.ToString();
		Name = template.Name;
		int namelength = Name.Length;
		nameText.characterSize = -0.0053f * (float)namelength + 0.1302f;

		GameObject temp = (GameObject)Resources.Load("Enemymodels/" + Name);
		//Debug.Log ("searching for: " + "Enemymodels/" + Name);
		if (temp != null) {
			//GameObject model = (GameObject)Instantiate (temp, ModelNode.position,ModelNode.rotation,ModelNode);
			Debug.Log ("Spawning " + Name);
		}
			

		textText.text = Text;
		damageText.text = Damage.ToString();
		nameText.text = Name;
	}

	public void SetPosition(int x_, int y_){
		//Debug.Log (string.Format ("Setting {0} at x={1} y={2} with health of {3}", this.Name, x_, y, HealthMelee));
		GM.instance.army.army [x_, y_] = this;
		if (x_ != x || y_ != y)
			if (GM.instance.army.army[x,y] == this)
				GM.instance.army.army [x, y] = null;
		x = x_;
		y = y_;
		//if (GM.instance.army.army[x,y] != this)
		goToPos = GM.instance.armypos.position + new Vector3 (15 * x - 7.5f * (GM.instance.army.army.GetLength (0) - 1), 0, 15 * y);
		int[] newplace = CheckMove ();
		if (Traits.Contains ("Rush") && (newplace [0] != x || newplace [1] != y))
			SetPosition (newplace [0], newplace [1]);
	}

	public void ComradeDied(){
		if (Traits.Contains("Rush"))
			SetPosition (CheckMove()[0],CheckMove()[1]);
	}

	public void SlideDown(){
		SetPosition (CheckMove()[0],CheckMove()[1]);
	}

	public void ShowAvailability(){
		
		healthText.text = Health.ToString ();
		if (GM.instance.State == "Attacking") {
			switch (GM.instance.attackingbutton.attacktype) {
			case Attacktype.Melee:
				if (HealthMelee > Health)
					healthText.text = string.Format ("<color=red>{0}</color>", HealthMelee.ToString ());
				break;
			case Attacktype.Ranged:
				if (HealthRanged > Health)
					healthText.text = string.Format ("<color=yellow>{0}</color>", HealthRanged.ToString ());
			//Debug.Log (string.Format("x:{0} y:{1} HealthRanged = {2}", x, y, HealthRanged));
				break;
			case Attacktype.Magic:
				if (HealthMagic > Health)
					healthText.text = string.Format ("<color=cyan>{0}</color>", HealthMagic.ToString ());
				break;
			default:
				break;
			}
		}

		if (!CheckForLower() && checkforlethal()) {
			Color col = Color.white;
			switch (GM.instance.attackingbutton.attacktype) {
			//case Attacktype.Melee: col = Color.red * 0.20f; break;
			//case Attacktype.Ranged: col = Color.yellow * 0.20f; break;
			//case Attacktype.Magic: 	col = Color.cyan * 0.20f; 	break;
			case Attacktype.Melee: col = Color.red; break;
			case Attacktype.Ranged: col = Color.yellow ; break;
			case Attacktype.Magic: 	col = Color.cyan; 	break;
			default:break;
			}
			gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", col * 0.2f);
			//nameText.color = col;
		} else {
			//nameText.color = Color.white;
			gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", gameObject.GetComponent<Renderer> ().material.color * 0);
		}
	}

	int[] CheckMove(){
		if (y != 0) {
			if (GM.instance.army.army [x, y - 1] == null) {
				return new int[]{ x, y - 1 };
			}
		}
		return new int[] {x,y};
	}

	bool checkforlethal(){
		bool succeeded = false;
		if (GM.instance.attackingbutton != null) {
			switch (GM.instance.attackingbutton.attacktype) {
			case Attacktype.Melee:
				if (GM.instance.hand.DamageMelee >= HealthMelee && HealthMelee != 0) {
					succeeded = true;
				}
				break;
			case Attacktype.Ranged:
				if (GM.instance.hand.DamageRanged >= HealthRanged && HealthRanged != 0) {
					succeeded = true;
				}
				break;
			case Attacktype.Magic:
				if (GM.instance.hand.DamageMagic >= HealthMagic && HealthMagic != 0) {
					succeeded = true;
				}
				break;
			default:
				break;
			}
		}
		return succeeded;
	}

	void Update(){
		transform.position = Vector3.Lerp (transform.position,goToPos,5 * Time.deltaTime);
	}

	void OnMouseUp(){
		//Debug.Log ("Up");
		if (GM.instance.State == "Attacking") {
			Kill ();
		}
	}

	public void Hover(){
		goToPos += new Vector3 (0,5,0);
	}

	public void Instakill(){
		Destroy (this.gameObject);
		GM.instance.hand.UpdateStats ();
		GM.instance.army.EnemyDied (x, y);
	}
}
