using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] GameObject enemy;

    void Start()
	{
		//Create enemy at this point with no rotation
		GameObject e = Instantiate(enemy, transform.position, Quaternion.identity);
		//Group the spawned enemy
		e.transform.parent = Manager.i.map.Egroup;
		//Game started
		Manager.i.StartingGame();
	}
}
