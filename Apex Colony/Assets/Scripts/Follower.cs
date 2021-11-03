using UnityEngine;
using Pathfinding;

public class Follower : MonoBehaviour
{
	[SerializeField] Allies allies;
	[SerializeField] AIDestinationSetter destination;
	[SerializeField] AIPath path;
	public int order;
	Formator formator;

	void Start()
	{
		//Get the formator
		formator = Formator.i;
		//Add this follower as an new follower of formation
		formator.follows.Add(gameObject);
		//Get the order of this follower in formaton
		GetFollowerOrder();
		//Begin set gooal upon it generation
		formator.goalCre.generated += SetGoal;
	}

	void Update()
	{
		//Set the path speed
		path.maxSpeed = allies.speed;
	}

	void SetGoal()
	{
		//Set the target destination as the goal that has same index as follower order
		destination.target = formator.goalCre.goals[order].transform;
		//Begin search path and no longer auto search path
		path.SearchPath(); path.repathRate = 1000;
	}

	void GetFollowerOrder()
	{
		//Go through all the follows count 
		for (int f = 0; f < formator.follows.Count; f++)
		{
			//Get the order of this follower inside the follow list
			if(formator.follows[f] == gameObject) {order = f;}
		}
	}
}
