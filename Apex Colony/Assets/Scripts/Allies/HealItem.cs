using UnityEngine;

public class HealItem : MonoBehaviour
{
	Pickupable picker;
    [Tooltip("How big is the healing radius")] public float radius;
	[Tooltip("How many heath it will heal allies")] public float heal;
	[SerializeField] ParticleSystem effect;

	void Start()
	{
		//Get the pickupable component
		picker = GetComponent<Pickupable>();
		//Begin heal the allies in radius
		picker.apply += Heal;
	}

	void Heal(Allies allie)
	{
		//Move the effect to the allies click on
		effect.transform.position = allie.transform.position;
		//Set the effect size as the radius
		effect.transform.localScale = new Vector2(radius, radius);
		//Play the heal effect
		effect.transform.parent = null; effect.Play();
		//Get all the allies got hit using circlecast
		RaycastHit2D[] hits = Physics2D.CircleCastAll
		//The circle cast are on the allies click with size of radius and only on allies layer
		(allie.transform.position, radius, Vector2.zero, 0, Manager.i.layer.allies);
		//Heal all the allies got hit by the circle cast
		foreach (RaycastHit2D hit in hits) {hit.transform.GetComponent<Heath>().Healing(heal);}

		SFX_Manager.PlaySFX("Consume Item");
	}
}