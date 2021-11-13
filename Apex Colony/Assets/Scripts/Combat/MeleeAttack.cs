using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    void Start()
    {
		//Begin melee attack when event called
        GetComponent<Attacking>().Attack.AddListener(Meleeing);
    }

	//Dealing damage to any heath in range
    void Meleeing(Heath inRange, float damage) {inRange.Damaging(damage);}
}