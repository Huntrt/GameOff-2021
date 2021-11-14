using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
	[SerializeField] ParticleSystem effect;

    void Start()
    {
		//Begin melee attack when attack event called
        GetComponent<Attacking>().Attack.AddListener(Meleeing);
    }

    void Meleeing(Heath inRange, float damage) 
	{
		//Dealing damage to heath of the enemy in range
		inRange.Damaging(damage);
		//Play the melee particle effect
		effect.Play();
	}
}