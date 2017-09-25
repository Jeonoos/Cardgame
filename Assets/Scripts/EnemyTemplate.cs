using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTemplate{
	public int Damage;
	public int Health;
	public List<string> Traits;
	public int Strength;
	public string Name;
	public EnemyTemplate(){
		
	}
	public EnemyTemplate (int Damage_, int Health, List<string> Traits_, int Strength_, string Name_){
		Debug.Log ("starting enemytemplate");
		Damage = Damage_;
		Health = Health;
		Traits = Traits_;
		Strength = Strength_;
		Name = Name_;
	}
}
