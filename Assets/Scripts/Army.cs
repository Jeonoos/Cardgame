using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {
	public Enemy[,] army;
	int TotalDamage = 0;
	public int x;
	public int y;
	public int TotalBudget;
	public int frontrow = 0;
	public Army (int budget){

	}

	// Use this for initialization
	public void Spawn () {
		int tempbudget = TotalBudget;
		army = new Enemy[x, y];
		for (int y = 1; y < army.GetLength(1); y++) {
			for (int x = 0; x < army.GetLength(0); x++) {
				if (tempbudget >= 10) {
					Enemy tobuild = Instantiate (GM.enemies.Template, GM.instance.deckpos.position, Quaternion.identity);
					army [x, y] = tobuild;
					EnemyTemplate enemytoBuild;
					do {
						enemytoBuild = GM.enemies.enemies [Mathf.FloorToInt (Random.value * (GM.enemies.enemies.Length))];
						//Debug.Log(string.Format("it's {0} which costs {1} while allowed budget is {2}. Row budget is {3}", enemytoBuild.Name, enemytoBuild.Strength, tempbudget, y * TotalBudget/40));
					} while (enemytoBuild.Strength > tempbudget || enemytoBuild.Strength > y * TotalBudget/40);
					tempbudget -= enemytoBuild.Strength;
					army [x, y].BuildEnemy (enemytoBuild);
					//Debug.Log (army [x, y].Name);
					army [x, y].SetPosition (x, y);
					//Debug.Log (string.Format ("Setting {0} at x={1} y={2}", army [x, y].Name, x, y));
				}
			}
			UpdateArmy ();
		}
	}
	
	public void EnemyDied(int eX, int eY){
		army [eX, eY] = null;
		bool anyalive = false;
		for (int x = 0; x < army.GetLength(0); x++) {
			for (int y = 0; y < army.GetLength(1); y++) {
				if (army [x, y] != null) {
					army [x, y].ComradeDied ();
					if (x != eX || y != eY) {
						anyalive = true;
					}
				}
			}
		}
		if (!anyalive) {
			Debug.Log ("you win!");
			GM.instance.GameOver (true);
		}
		UpdateArmy ();
	}

	public void EndTurn(){
		for (int x = 0; x < army.GetLength(0); x++) {
			if (army [x, 0] != null) {
				TotalDamage += army [x, 0].Damage;
				army [x, 0].Hover ();
			}
		}
		Debug.Log ("Took " + TotalDamage + " Damage");

		GM.instance.deck.TakeDamage (TotalDamage);
		TotalDamage = 0;
		GM.instance.deck.UpdateHealth ();
		Invoke ("SlideDown", 1);
	}

	void SlideDown(){
		for (int x = 0; x < army.GetLength(0); x++) {
			for (int y = 0; y < army.GetLength(1); y++) {
				if (army [x, y] != null)
					army [x, y].SlideDown ();
					//army [x, y].ComradeDied ();
//				if (army [x, y - 1] == null) {
//					if (army [x, y] != null) {
//						army [x, y].SetPosition (x, y - 1);
//						army [x, y - 1] = army [x, y];
//						//Debug.Log ("at x=" + x + " y = " + (y-1)+ ": "+army[x,y-1] );
//						army [x, y] = null;
//					}
//				}
			}
		}
		for (int x = 0; x < army.GetLength(0); x++) {
			for (int y = 0; y < army.GetLength(1); y++) {
				if (army [x, y] != null) {
					army [x, y].SetPosition (x, y);
				}
			}
		}
		GM.instance.State = "Default";
		GM.instance.hand.DrawHand ();
	}

	public void UpdateArmy(){
		//Debug.Log ("updating army");

		frontrow = -1;
		foreach (var item in army) {
			if (item != null) {
				item.ShowAvailability ();
				//item.ComradeDied ();
				if (item.y < frontrow || frontrow == -1) {
					frontrow = item.y;
					//Debug.Log (item.y);
				}
			}
		}
	}
}
