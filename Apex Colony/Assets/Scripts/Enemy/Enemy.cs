using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public string entityName;
	public int foodGain;
	public Heath hp;
	[Tooltip("Attack damage")] public float damage;
	[HideInInspector] public float velocity;
	[Tooltip("Moving speed to go between target")] public float speed;
	[Tooltip("Slowdown percented when enemy in range")] public float approach;
	[Tooltip("Attacking speed")] public float rate; float rateCount;
	[Tooltip("DIstance when the enemy able to detect allies")] public float detection;
	[Tooltip("Attack range")] public float range;
	public Transform detected;
	public combating combat;
	public Patrol patrol;
	public AIDestinationSetter destination;
	public AIPath path;
	[SerializeField] Attacking attacking;
	public EnemyManager enemies;
	public AtkIndicator indicating;

	void Start()
	{
		//Get the enemy manager
		enemies = Manager.i.enemy;
		//If the enemy manger haven't got this enemy object
		if(!enemies.enemiesObj.Contains(gameObject))
		{
			//Add this enemy object into it manager list once when create
			enemies.enemiesObj.Add(gameObject);
			//Add this enemy component into it manager list once when create
			enemies.enemiesComp.Add(this);
		}
		//Enemy has are begining to spawn
		enemies.StartCoroutine("Spawned"); 
		//Set the velocity as moving speed
		velocity = speed;
	}

	void DetectAllies()
	{
		//Create an circel cast to detect enemy
		RaycastHit2D dect = Physics2D.CircleCast
		//Cast at this enemy with range of detection, no direction and distance only on allies layer
		(transform.position, detection, Vector2.zero, 0, Manager.i.layer.allies);
		//If detect an allies
		if(dect) 
		{
			//But there has not detect any enemy
			if(detected == null)
			{
				//Detect the allies
				detected = dect.transform; 
				//The enemy destination are now the detected allies
				destination.target = detected;
				//Search that path and enable auto search
				path.SearchPath(); path.canSearch = true;
			}
			//Begin chasing
			combat = combating.chase;
		}
		//If detect nothing
		else
		{
			//But there are still allies in detection
			if(detected != null)
			{
				//Update the patrol center point
				patrol.center = transform.position;
				//Remove the detected allies
				detected = null;
				//Remove the enemy destination
				destination.target = null;
				//Remove current path and disable auto search
				path.SetPath(null); path.canSearch = false;
			}
			//Reset velocity to speed
			velocity = speed;
			//No longer in combat
			combat = combating.none;
		}
	}

	void Update()
	{
		//Make the attack indicating of this enemy follow it 
		if(indicating != null) {indicating.transform.position = transform.position;}
		//Set the path velocity as enemy velocity
		path.maxSpeed = velocity;
		//Detecting allies
		DetectAllies();
		//If detect an allies
		if(detected != null)
		{
			//The distance between enemy position and detected position
			float distance = Vector2.Distance(transform.position, detected.position);
			//Begin fight when the detected allies are in range
			if(distance <= range) {combat = combating.fight;}
			//Begin chase and set velocity as speed when detect allies out of range
			if(distance > range) {combat = combating.chase; velocity = speed;}
		}
		//If currently fighting
		if(combat == combating.fight)
		{
			//Convert approach to decimal then apply it slowdown to speed
			velocity = (approach/100) * speed;
			//Begin attack rate counter
			rateCount += Time.deltaTime;
			//Attack when rate counter reach how many attack perform in 1 second
			if(rateCount >= 1/rate)
			{
				//Send event with the target allies heath component to attack with damage & range
				attacking.Attack.Invoke(detected.GetComponent<Heath>(), damage, range);
				//Reset the rate count
				rateCount -= rateCount;
			}
		}
	}

	void OnDestroy()
	{
		//End the indcator if the enemy got destroy
		if(indicating != null) {indicating.EndingIndicator(true);} 
	}
}