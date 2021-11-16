using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
	public GameObject projectile;
	public Transform firepoint;

    void Start()
    {
		//Begin melee projectiling when attack event called
        GetComponent<Attacking>().Attack.AddListener(Projectiling);
    }

    void Projectiling(Heath inRange, float damage, float range)
    {
		//Create projectile at firepoint with this object rotation without active it
        GameObject ins = Pool.get.Create(projectile, firepoint.position, transform.rotation, false);
		//Get the projectile create's stats
		Projectiled stat = ins.GetComponent<Projectiled>();
		//Set the stat damage and range (speed are set in prefab it self)
		stat.damage = damage; stat.range = range;
		//The projectile are now belong to object it create
		stat.belong = transform.tag;
		//Active the projectile
		ins.SetActive(true);
    }
}