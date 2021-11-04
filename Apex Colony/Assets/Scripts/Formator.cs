using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	//List of follows object in formation
	public List<GameObject> follows;
	//List of enemy currently select
	public List<GameObject> selecting;
	//List of enemy has been mark as rival
	public List<GameObject> rivals;

	//Function to clear all the enemy has been selected
	public void ClearSelection() {selecting.Clear(); selecting = new List<GameObject>();}
	//Function to clear all the enemy has been selected
	public void ClearRivals() {rivals.Clear(); rivals = new List<GameObject>();}

	public void RivalEnemy()
	{
		//Each of the enemy in selecting will be add to rival if haven't
		foreach (GameObject enemy in selecting) {if(!rivals.Contains(enemy)) {rivals.Add(enemy);}}
	}
}