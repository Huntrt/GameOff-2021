using System.Collections.Generic;
using UnityEngine;

public class AlliesManager : MonoBehaviour
{
    public List<GameObject> alliesObj;
	public List<Allies> alliesComp;
	[SerializeField] GameObject test;

	///For getting the center vector of all allies
	public Vector2 FormationCenter()
	{
		//The total position of all allies
		Vector2 total = Vector2.zero;
		//For each of the allies in allies object
		foreach (GameObject a in Manager.i.allie.alliesObj)
		{
			//Counting the current allies X axis
			total.x += a.transform.position.x;
			//Counting the current allies Y axis
			total.y += a.transform.position.y;
		}
		//Send the center position to be the middle of all allies
		return total / Manager.i.allie.alliesObj.Count;
	}
}
