using UnityEngine;
using Pathfinding;

public class Follower : MonoBehaviour
{
	[SerializeField] Allies allies;
	[SerializeField] AIDestinationSetter destination;
	[SerializeField] AIPath path;
	public int order;
	public float distance, velocity;
	public bool moving;
	bool hasVelocity; //? to handle velocity delay during set goal
	Manager manager; Formator formator;

	void Start()
	{
		//Get the manager and the formator
		manager = Manager.i; formator = manager.formator;
		//Add this follower as an new follower of formation upon it create
		if(!formator.followers.Contains(this)) {formator.followers.Add(this);}
		//Get the order of this follower in formaton upon create
		UpdateOrder();
		//Begin set goal upon it creation
		manager.goal.created += SetGoal;
	}

	void Update()
	{
		//Set the path speed as velocity stats
		path.maxSpeed = allies.velocity;
		//Get the current path velocity
		velocity = path.velocity.magnitude;
		//If allies are not currently combat
		if(allies.combat == combating.none)
		{
			//The player are NOT moving when path velocity are zero and has velocity	
			if(velocity == 0 && hasVelocity) {moving = false;}
			//The player are NOW moving and has get velocity when path velocity are not zero
			if(velocity > 0) {moving = true; hasVelocity = true;}
		}
		//If there is no target destination
		if(destination.target == null) 
		{
			//Stop path and no longer move
			StopPath(); path.maxSpeed = 0.1f;
			//Will get new rival if lost rival while chasing or fighting
			if(allies.combat != combating.none) {formator.TargetRivals();}
			//Allies are no longer combat
			allies.combat = combating.none;
		}
		///If the target destination do exist OR is it not an enemy
		if(destination.target == null || !destination.target.CompareTag("Enemy"))
		{
			//If not moving while has velocity
			if(!moving && hasVelocity)
			{
				//Cast an circle cast to detect eneny
				RaycastHit2D detect = Physics2D.CircleCast
				//The circle are at this object with radius of allies range stat and only on enemy layer
				(transform.position, allies.range, Vector2.zero, 0, manager.layer.enemy);
				//Set the enemt got detected as rival
				if(detect) {SetRival(detect.transform.gameObject);}
			}
		}
		///If the target destination exist and it is an enemy
		if(destination.target != null && destination.target.CompareTag("Enemy"))
		{
			//Enable auto search path
			path.autoRepath.maximumInterval = 0f;
			//Get the distance between this follower the target enemy
			distance = Vector2.Distance(transform.position, destination.target.position);
			//If the enemy are in the allies range
			if(distance <= allies.range)
			{
				//Conver approach to decimal then apply approach slowdown to speed
				allies.velocity = (allies.approach/100) * allies.speed;
				//Change allies state state to fight
				allies.combat = combating.fight;
			}
		}
	}

	void SetGoal()
	{
		//Set the target destination as the goal that has same index as follower order
		destination.target = manager.goal.goals[order].transform;
		//Searching for path then disable auto search path
		path.SearchPath(); path.autoRepath.maximumInterval = 1000;
		//Update allies velocity to moving speed then set the path speed as velocity
		allies.velocity = allies.speed; path.maxSpeed = allies.velocity;
		//Don't has velocity yet and allies no longer combat
		hasVelocity = false; allies.combat = combating.none;
	}

	//Set this follower's target destination to the rival
	public void SetRival(GameObject rival) 
	{
		//If there is no target destination or the rival are an new enemny
		if(destination.target == null || rival != destination.target.gameObject) 
		{
			//Set the allies velocity as the movement speed
			allies.velocity = allies.speed;
			//Set destination target as rival receive then searching path
			destination.target = rival.transform; path.SearchPath();
			//Begin chasing the enemy
			allies.combat = combating.chase;
		}
	}

	//Function to update this follower order
	public void UpdateOrder() {formator.GetFollowerOrder(this);}
	//Function to stop follower movment and remove it target
	public void StopPath()
	{
		//Remove current path and remove target destination
		path.SetPath(null); destination.target = null;
		//No longer auto search path
		path.autoRepath.maximumInterval = 1000;
	}
}