using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
	[SerializeField] ParticleSystem effect;
	
	enum AttackSFX {Light, Standard, Heavy} [SerializeField] AttackSFX attackSFX;

    void Start()
    {
		//Begin melee attack when attack event called
        GetComponent<Attacking>().Attack.AddListener(Meleeing);
    }

    void Meleeing(Heath inRange, float damage, float range) 
	{
		//Dealing damage to heath of the enemy in range
		if(inRange != null) {inRange.Damaging(damage);}
		//Play the melee particle effect
		effect.Play();

		SFX_Manager.PlaySFX("Attack " + attackSFX.ToString());
	}
}