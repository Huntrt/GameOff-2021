using System.Collections.Generic;
using UnityEngine;

public class Projectiled : MonoBehaviour
{
    [HideInInspector] public float damage, range;
	[SerializeField] float speed, piercing; float _piercing;
	public string belong;
	float travelled; Vector2 prevPos;
	public TrailRenderer trail;
	[SerializeField] Rigidbody2D rb;
	List<Collider2D> hitted = new List<Collider2D>();
	public Collider2D col;

	void OnEnable()
	{
		//Clear the collider got hit
		hitted.Clear(); hitted = new List<Collider2D>();
		//No longer ignore all the collider has hit
		foreach (Collider2D hit in hitted) {Physics2D.IgnoreCollision(col, hit, false);}
		//If there is trial
		if(trail != null)
		{
			//Clear the trail
			trail.Clear();
			//Reassign this trial to be projectile child
			trail.transform.parent = transform;
			//Reset the trail position
			trail.transform.localPosition = Vector2.zero;
			//Clear the trail
			trail.Clear();
		}
		//Update the previous position
		prevPos = transform.position;
		//Reset piercing
		_piercing= piercing;
		//Reset the travelld distance
		travelled = range;
	}

    void Update()
    {
		//If the previous position has change
		if((Vector2)transform.position != prevPos)
		//Decrease travelled using the distance between current position and previous position
		{travelled -= Vector2.Distance(transform.position, prevPos);}
		//Update the previous position
		prevPos = transform.position;
		//Deactive the projectile when travel all the range
		if(travelled <= 0) {DeactiveProjectile();}
		//Update the bullet velocity as the speed stats at transfrom right
		rb.linearVelocity = transform.up * speed;
    }

	void OnCollisionEnter2D(Collision2D other) 
	{
		//If this object belong to allies and hit an enemy
		if(belong == "Allies" && other.collider.CompareTag("Enemy"))
		{
			//Deal damage to the enemy
			other.collider.GetComponent<Heath>().Damaging(damage);
			//Has pierced
			Pierced();
		}
		//If this object belong to enemy and hit an allies
		if(belong == "Enemy" && other.collider.CompareTag("Allies"))
		{
			//Deal damage to the allies
			other.collider.GetComponent<Heath>().Damaging(damage);
			//Has pierced
			Pierced();
		}
		//Deactive the projectile if collide with wall
		if(other.collider.CompareTag("Wall")) {DeactiveProjectile();}
		//Ignore colliision of the object it collide with
		Physics2D.IgnoreCollision(col, other.collider); hitted.Add(other.collider);
	}

	//Lost an piercing and deactive object when out of piercing
	void Pierced() {_piercing--; if(_piercing <= 0) {gameObject.SetActive(false);}}

	//Deactive the propjectile and remove the trial parent
	void DeactiveProjectile() {gameObject.SetActive(false); if(trail != null) trail.transform.parent = null;}
}
