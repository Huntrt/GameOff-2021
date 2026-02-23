using UnityEngine;

public class BoostItem : MonoBehaviour
{
	Pickupable picker;
    [Tooltip("Increase the maximum heath of allies")] public float maxHeath;
	[Tooltip("How many heath it will heal allies")] public float heal;
	[Tooltip("Increase damage of melee or projectile")] public float damage;
	[Tooltip("Increase speed to go between target")] public float speed;
	[Tooltip("Increase the attacking speed")] public float rate;
	[Tooltip("Increase the attack range")] public float range;

	void Start()
	{
		//Get the pickupable component
		picker = GetComponent<Pickupable>();
		//Begin boost the allies has apply
		picker.apply += Boost;
	}

	void Boost(Allies allie)
	{
		//Increase the allies apply max heath
		allie.hp.maxHeath += maxHeath;
		//Heal the allies apply
		allie.hp.Healing(heal);
		//@ Increase the stats of allies apply
		allie.damage += damage;
		allie.speed += speed;
		allie.rate += rate;
		allie.range += range;
		
		SFX_Manager.PlaySFX("Consume Item");
	}
}