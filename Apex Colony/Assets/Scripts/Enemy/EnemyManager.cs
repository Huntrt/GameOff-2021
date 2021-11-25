using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> enemiesObj;
	public List<Enemy> enemiesComp;
	[HideInInspector] public float enemyCount, spawnerCount;
	[HideInInspector] public bool spawned;
	float totalRatio;
	LevelManager lvm;

	//Get the level manager and set enemy spawn status
	void Awake() {lvm = Manager.i.level; spawned = true;}

	public void ClearEnemy()
	{
		//Reset enemy and spawner counter
		spawnerCount = 0; enemyCount = 0;
		//Reset the enemy component list
		enemiesComp.Clear(); enemiesComp = new List<Enemy>();
		//Reset the enemy object list
		enemiesObj.Clear(); enemiesObj = new List<GameObject>(); 
	}

	void Update()
	{
		///START the game when all the spawner has complete spawning their enemy
		if(!spawned) {if(spawnerCount == enemyCount){Manager.i.StartingGame(); spawned = true;}}
	}

	public GameObject EnemySpawn()
	{
		//Get the current level's spawn data
		List<DropData> spawns = lvm.levels[lvm.lv].spawns;
		//Reset the total ratio
		totalRatio -= totalRatio;
		//Get the total ratio of all enemy spawn
		foreach (DropData s in spawns) {totalRatio += s.ratio;}
		//The chance randomly got from zero to total ratio
		float chance = Random.Range(0, totalRatio);
		//Go throught all the enemy spawn in list
		for (int d = spawns.Count - 1; d >= 0 ; d--)
		{
			//Send the enemy to spawn when it ratio took all the chance
			if((chance - spawns[d].ratio) <= 0) {return spawns[d].obj;}
			//Decrease the chance if the spawn ratio are higher than it
			else {chance -= spawns[d].ratio;}
		}
		//Not important
		return null;
	}

	//Begin counting the enemy spawned after 1 frame delay
	IEnumerator Spawned() {yield return null; enemyCount++;}
}
