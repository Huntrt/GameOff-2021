using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] GameObject enemy;

    void Start()
	{
		//Create enemy at this point with no rotation
		Instantiate(enemy, transform.position, Quaternion.identity);
	}
}
