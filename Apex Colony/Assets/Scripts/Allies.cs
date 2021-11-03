using UnityEngine;

public class Allies : MonoBehaviour
{
	[Tooltip("Max heath and current heath")] public float maxHeath, heath;
	[Tooltip("Movement speed")] public float speed;
	[Tooltip("Attack Damage")] public float damage;
	[Tooltip("Attack speed or attack rate")] public float rate;
	[Tooltip("Attack range")] public float range;
	[Tooltip("Detection range")] public float detection;
	public bool inCombat = false;

	void OnEnable()
	{
		//Reset current heath
		heath = maxHeath;
	}

	void Update()
	{
		
	}
}
