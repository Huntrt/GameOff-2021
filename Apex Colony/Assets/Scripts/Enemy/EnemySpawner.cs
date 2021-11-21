using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Start()
	{
		//Spawning an enemy at this point with no rotation
		GameObject e = Instantiate(Manager.i.enemy.EnemySpawn(), transform.position, Quaternion.identity);
		//Group the spawned enemy
		e.transform.parent = Manager.i.map.Egroup;
		//Game started
		Manager.i.StartingGame();
	}
}