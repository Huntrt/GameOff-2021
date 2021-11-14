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
	bool hasVelocity = true;
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
			StopMovement(); path.maxSpeed = 0.1f;
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
				RaycastHit2D inRange = Physics2D.CircleCast
				//The circle are at this object with radius of allies range stat and only on enemy layer
				(transform.position, allies.range, Vector2.zero, 0, manager.layer.enemy);
				//Set the enemt got in range as rival
				if(inRange) {SetRival(inRange.transform.gameObject);}
			}
		}
		///If the target destination exist
		if(destination.target != null)
		{
			//Enable auto search path
			path.canSearch = true;
			//Get the distance between this follower the target enemy or interactable
			distance = Vector2.Distance(transform.position, destination.target.position);
			//If the enemy or interactble are in the allies range
			if(distance <= allies.range)
			{
				//Rotate toward the destination target
				transform.up = destination.target.position - transform.position;
				//If the destination target are an ENEMY
				if(destination.target.CompareTag("Enemy"))
				{
					//Convert approach to decimal then apply it slowdown to speed
					allies.velocity = (allies.approach/100) * allies.speed;
					//Change allies state state to fight
					allies.combat = combating.fight;
				}
				//If the destination target are an INTERACTABLE
				if(destination.target.CompareTag("Interactable"))
				{
					//Call the interact event of the interactable target when in range
					destination.target.GetComponent<Interactable>().interact.Invoke();
				}
			}
		}
	}

	///Set the target destination as the goal that has same index as follower order then begin moving
	void SetGoal() {destination.target = manager.goal.goals[order].transform; Moving();}
	///Set the target destination as interactable receive then begin moving
	public void SetInteract(Transform react) {destination.target = react; Moving();}
	///Moving when receving goal or interactable
	void Moving()
	{
		//Enable auto search path for searching path then disable auto search path
		path.canSearch = true; path.SearchPath(); path.canSearch = false;
		//Update allies velocity to moving speed then set the path speed as velocity
		allies.velocity = allies.speed; path.maxSpeed = allies.velocity;
		//Don't has velocity yet and allies no longer combat
		hasVelocity = false; allies.combat = combating.none;
	}

	///Set this follower's target destination to the rival
	public void SetRival(GameObject rival) 
	{
		//If there is no target destination or the rival are an new enemny
		if(destination.target == null || rival != destination.target.gameObject) 
		{
			//Set the allies velocity as the movement speed
			allies.velocity = allies.speed;
			//Set destination target as rival receive
			destination.target = rival.transform;
			//Enable auto search path and begin search
			path.canSearch = true; path.SearchPath();
			//Begin chasing the enemy
			allies.combat = combating.chase;
		}
	}

	//Function to update this follower order
	public void UpdateOrder() {formator.GetFollowerOrder(this);}
	//Function to stop follower movment and remove it target
	public void StopMovement()
	{
		//Remove current path and remove target destination
		path.SetPath(null); destination.target = null;
		//No longer auto search path
		path.canSearch = false;
	}
	
	void OnDestroy()
	{
		//No longer listen to set goal upon destroy
		manager.goal.created -= SetGoal;
		//Remove this follower from fromation
		formator.followers.Remove(this);
		//Upate order of all the follower in list
		foreach (Follower f in formator.followers) {f.UpdateOrder();}
	}
}