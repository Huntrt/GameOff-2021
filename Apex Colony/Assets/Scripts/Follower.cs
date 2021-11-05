using UnityEngine;
using Pathfinding;

public class Follower : MonoBehaviour
{
	[SerializeField] Allies allies;
	[SerializeField] AIDestinationSetter destination;
	[SerializeField] AIPath path;
	public int order;
	Manager manager; Formator formator;

	void Start()
	{
		//Get the manager and the formator
		manager = Manager.i; formator = manager.formator;
		//Add this follower as an new follower of formation
		formator.followers.Add(this);
		//Get the order of this follower in formaton upon create
		UpdateOrder();
		//Begin set gooal upon it generation
		manager.goaling.generated += SetGoal;
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
		destination.target = manager.goaling.goals[order].transform;
		//Begin search path and no longer auto search path
		path.SearchPath(); path.repathRate = 1000;
	}

	//FUnction to update this follower order
	public void UpdateOrder() {formator.GetAlliesOrder(this);}
}