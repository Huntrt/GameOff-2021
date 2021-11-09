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
	[SerializeField] SpriteRenderer render;
	public Rigidbody2D rb;
	public AlliesManager allie;
	Color baseColor; //% base color of allies

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
		//% Get the basic color
		baseColor = render.color;
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
				//% Attack function (temporaray change color)
				Invoke("Attack", 0.2f); render.color = Color.white;
			}
		}
	}

	void Attack()
	{
		//% Reset color after complete attack
		render.color = baseColor;
		//% Reset rate count
		rateCount -= rateCount;
	}
}