using Pathfinding;
using UnityEngine;

public class Allies : MonoBehaviour
{
	public string entityName;
	public Heath hp;
	[Tooltip("Attack damage")] public float damage;
	[HideInInspector] public float velocity;
	[Tooltip("Moving speed to go between target")] public float speed;
	[Tooltip("Slowdown percented when enemy in range")] public float approach;
	[Tooltip("Attacking speed")] public float rate; float rateCount;
	[Tooltip("Attack range")] public float range;
	public combating combat;
	public Rigidbody2D rb;
	[SerializeField] Attacking attacking;
	public AIDestinationSetter destination;
	public AIPath path;
	[HideInInspector] public AlliesManager allie;
	bool isWalking;

	//! Only use disable and enable when begin spawn at the begin of map
	//! But the allies when dead it will need to be destroy and remove from maanger

	void Start()
	{
		//Get the allies manager
		allie = Manager.i.allie;
		//If the allies manger haven;t got this allies object
		if(!allie.alliesObj.Contains(gameObject))
		{
			//Add this allies object into it manager list once when create
			allie.alliesObj.Add(gameObject);
			//Add this allies component into it manager list once when create
			allie.alliesComp.Add(this);
		}
		//Set the velocity as moving speed
		velocity = speed;
	}

	void Update()
	{
		//If currently fighting
		if(combat == combating.fight)
		{
			//Begin attack rate counter
			rateCount += Time.deltaTime;
			//Attack when rate counter reach how many attack perform in 1 second
			if(rateCount >= (1/rate))
			{
				//If the target has destination
				if(destination.target != null)
				//Send event with the target enemy heath component to attack with damage & range
				{attacking.Attack.Invoke(destination.target.GetComponent<Heath>(), damage, range);}
				//Reset the rate count
				rateCount -= rateCount;
			}
		}
	}
}