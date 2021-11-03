using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	public GoalsCreate goalCre;
	[HideInInspector] public int enemyLayer;
	public Vector2 clickPos;
	//List of follows oiject
	public List<GameObject> follows;

	//Turn this script into singlton
	public static Formator i; void Awake() {i = this;}

	void Start()
	{
		//Get the enemy layer
		enemyLayer = (1 << (LayerMask.NameToLayer("Enemy")));
	}

	void Update()
	{
		//If press left mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Cast an ray at click position with no direction and distance on enemy layer
			RaycastHit2D engage = Physics2D.Raycast(clickPos, Vector2.zero, 0 , enemyLayer);
			//If there an enemy hit engage
			if(engage)
			{
				///Begin attack
				print("Attack");
			}
			//If there is no enemy to engage
			else
			{
				//Generated goal to move at click position
				goalCre.GenerateGoals(clickPos);
			}
		}
	}

	public void Replace()
	{

	}

	public void Delete()
	{

	}
}