using UnityEngine;
using Pathfinding;

public class Follower : MonoBehaviour
{
	[SerializeField] Allies allies;
	[SerializeField] AIDestinationSetter destination;
	[SerializeField] AIPath path;
	public int order;
	Manager manager;

	void Start()
	{
		//Get the manager
		manager = Manager.i;
		//Add this follower as an new follower of formation
		manager.formator.follows.Add(gameObject);
		//Get the order of this follower in formaton
		GetFollowerOrder();
		//Begin set gooal upon it generation
		manager.goals.generated += SetGoal;
	}

	void Update()
	{
		//Set the path speed
		path.maxSpeed = allies.speed;
		//Clear the current path when press key
		//! Same key as clear trivals in Command.cs
		if(Input.GetKeyDown(KeyCode.X)) {path.SetPath(null);}
	}

	void SetGoal()
	{
		//Set the target destination as the goal that has same index as follower order
		destination.target = manager.goals.goals[order].transform;
		//Begin search path and no longer auto search path
		path.SearchPath(); path.repathRate = 1000;
	}

	void GetFollowerOrder()
	{
		//Go through all the follows count in formator
		for (int f = 0; f < manager.formator.follows.Count; f++)
		{
			//Get the order of this follower inside the follow list
			if(manager.formator.follows[f] == gameObject) {order = f;}
		}
	}
}