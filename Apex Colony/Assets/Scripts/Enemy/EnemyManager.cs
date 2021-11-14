using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> enemiesObj;
	public List<Enemy> enemiesComp;

	void Awake()
	{

	}

	public void ResetList()
	{
		//Reset the enemy component list
		enemiesComp.Clear(); enemiesComp = new List<Enemy>();
		//Reset the enemy object list
		enemiesObj.Clear(); enemiesObj = new List<GameObject>(); 
	}
}
