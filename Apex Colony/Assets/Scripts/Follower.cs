using UnityEngine;
using Pathfinding;

public class Follower : MonoBehaviour
{
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
		formator.goalGenerator.generated += SetGoal;
	}

	void SetGoal()
	{
		//Set the target destination as the goal that has same index as follower order
		destination.target = formator.goalGenerator.goals[order].transform;
		//Begin search path
		path.SearchPath();
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
