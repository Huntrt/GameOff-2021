using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[HideInInspector] public int layer;
	public List<Transform> enemies;

	void Awake()
	{
		//Get the enemy layer
		layer = (1 << (LayerMask.NameToLayer("Enemy")));
	}
}
