using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class EnemyList{
	public Enemy Template;
	public static EnemyList list;
	public EnemyTemplate[] enemies;
	public void GetEnemies(){


		string path = File.ReadAllText(Application.dataPath + "/Resources/Enemies.json");
		JsonData enemyData = JsonMapper.ToObject(path);

		enemies = new EnemyTemplate[enemyData.Count];
		//Debug.Log (enemyData.Count);
		for (int i = 0; i < enemies.Length; i++) {
			enemies [i] = JsonMapper.ToObject<EnemyTemplate> (enemyData[i].ToJson());
		}

		list = this;
		GameObject template= (GameObject)Resources.Load ("Enemy");
		Template = template.GetComponent<Enemy> ();
	}
}
