using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	//List of followers function
	public List<Follower> followers;
	//List of enemy has been mark as rival
	public List<GameObject> rivals;
	//List of enemy currently select
	public List<GameObject> selectings;

	#region Allies
	public void GetFollowerOrder(Follower follower)
	{
		//Go through all of the followers in list
		for (int f = 0; f < followers.Count; f++) 
		{
			//Give the of requested follower it order
			if(followers[f] == follower) {follower.order = f;}
		}
	}
	#endregion

	#region  Rivaling
	///Check if an enemy has been rival
	public bool HasRival(GameObject enemy) {if(!rivals.Contains(enemy)) {return false;} return true;}
	
	///Rival the clicked enemy if haven't then target rival
	public void RivalClicked(GameObject enemy) {if(!rivals.Contains(enemy)) {rivals.Add(enemy);}}

	///Stop rivalling the clicked enemy if is it then target rival
	public void UnrivalClicked(GameObject enemy) {if(rivals.Contains(enemy)) {rivals.Remove(enemy);}}

	///Turn all current select enemy to rival if haven't
	public void RivalSelected() 
	{
		//Each of the enemy in selectings will be add to rivals if haven't 
		foreach (GameObject enemy in selectings) {if(!rivals.Contains(enemy)) {rivals.Add(enemy);}}
	}

	public void TargetRivals()
	{
		//Remove all the empty rival in it list
		rivals.RemoveAll(GameObject => GameObject == null);
		//If there is follower and rival in their own list
		if(followers.Count > 0 && rivals.Count > 0)
		{
			//The time follwer has target rival
			int assigned = 0;
			//Go through all the followers
			for (int f = 0; f < followers.Count; f++)
			{
				//If current follower are not null
				if(followers[f] != null)
				{
					//Assign the follower target to be assigning rival
					followers[f].SetRival(rivals[assigned]);
					//Has complete 1 assign
					assigned++;
					//Reset ther assign count if out of rival to assign
					if(assigned >= rivals.Count) {assigned = 0;}
				}
			}
		}
	}

	///Clear all the enemy currently rival
	public void ClearRivals() {rivals.Clear(); rivals = new List<GameObject>();}
	#endregion
	
	//Clear all the enemy currentlu been selected
	public void ClearSelection() {selectings.Clear(); selectings = new List<GameObject>();}

}