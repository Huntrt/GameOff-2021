using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Start()
	{
		//Get the enemy manager
		EnemyManager enemy = Manager.i.enemy;
		//Counting spawner has spawn
		enemy.spawnerCount++;
		//Beginning spawning enemy
		if(enemy.spawned) {enemy.spawned = false;}
		//Spawning an enemy at this point with no rotation
		GameObject e = Instantiate(enemy.EnemySpawn(), transform.position, Quaternion.identity);
		//Group the spawned enemy
		e.transform.parent = Manager.i.map.Egroup;
	}
}