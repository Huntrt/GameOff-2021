using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	//List of allies followers function
	public List<Follower> followers;
	//List of enemy has been mark as rival
	public List<GameObject> rivals;
	//List of enemy currently select
	public List<GameObject> selectings;

	public void GetAlliesOrder(Follower follower)
	{
		//Go through all of the followers to get the order of the follower requested
		for (int f = 0; f < followers.Count; f++) {if(followers[f] == follower) {follower.order = f;}}
	}

	#region  Rivaling

	///Check if an enemy has been rival
	public bool HasRival(GameObject enemy) {if(!rivals.Contains(enemy)) {return false;} return true;}
	
	///Rival the clicked enemy if haven't 
	public void RivalClicked(GameObject enemy) {if(!rivals.Contains(enemy)) {rivals.Add(enemy);}}

	///Stop rivalling the clicked enemy if is it rival
	public void UnrivalClicked(GameObject enemy) {if(rivals.Contains(enemy)) {rivals.Remove(enemy);}}

	///Turn all current select enemy to rival if haven't
	public void RivalSelected() 
	{
		//Each of the enemy in selecting will be add to rival if haven't
		foreach (GameObject enemy in selectings) {if(!rivals.Contains(enemy)) {rivals.Add(enemy);}}
	}

	///Clear all the enemy currently rival
	public void ClearRivals() {rivals.Clear(); rivals = new List<GameObject>();}

	#endregion
	
	//Clear all the enemy currentlu been selected
	public void ClearSelection() {selectings.Clear(); selectings = new List<GameObject>();}

}