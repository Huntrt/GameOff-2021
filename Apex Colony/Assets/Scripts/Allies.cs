using UnityEngine;

public class Allies : MonoBehaviour
{
	[Tooltip("Max heath and current heath")] public float maxHeath, heath;
	[Tooltip("Attack damage")] public float damage;
	[HideInInspector] public float velocity;
	[Tooltip("Moving speed to go between target")] public float speed;
	[Tooltip("Slowdown percented when enemy in range")] public float approach;
	[Tooltip("Attacking speed")] public float rate; float rateCount;
	[Tooltip("Attack range")] public float range;
	public combating combat;
	public Rigidbody2D rb;
	public AlliesManager allie;

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
		//Reset current heath
		heath = maxHeath;
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
			//If rate counter reach attack rate
			if(rateCount >= rate)
			{
				//Begin attack
				Attack();
			}
		}
	}

	void Attack()
	{
		
	}
}