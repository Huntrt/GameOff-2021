using System.Collections.Generic;
using UnityEngine;

public class Heath : MonoBehaviour
{
	[SerializeField] float maxHeath;
    [SerializeField] float curHeath;
	[SerializeField] Transform heathbar;

	[Tooltip("How long to stop combat since the last time take damage")]
	[SerializeField] float coolOff;
	[HideInInspector] public bool inCombat;
	[SerializeField] float flashDuration;
	[SerializeField] Color hurtColor, healColor;
	public ParticleSystem deathEffect;	List<SpriteRenderer> renders = new List<SpriteRenderer>();
	List<Color> defaultColors = new List<Color>();

	void Start()
	{
		//Reset heath
		curHeath = maxHeath;
		//Go through all the sprite renderer of children
		foreach (SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>())
		{
			//Saving the current render color and component if it not interface
			if(render.sortingLayerName != "Interface") {defaultColors.Add(render.color);renders.Add(render);}
		}
	}

	public void Damaging(float damage)
	{
		//Are now in combat and will keep trying to cooloff to exit combat
		inCombat = true; CancelInvoke("ExitCombat"); Invoke("ExitCombat", coolOff);
		//Decrease current heath
		curHeath -= damage;
		//Flash the hurt color
		Flashing(hurtColor);
		//If heath are zero
		if(curHeath <= 0) 
		{
			//Detach the death effect from this entity
			deathEffect.transform.parent = null;
			//PLay the death effect
			deathEffect.Play();
			//Destroy this entity
			Destroy(gameObject);
		}
		//Show the hp bar
		ShowHP();
	}

	public void Healing(float heal) 
	{
		//Are now in combat and will keep trying to cooloff to exit combat
		inCombat = true; CancelInvoke("ExitCombat"); Invoke("ExitCombat", coolOff);
		//Increase heath when heal
		curHeath += heal; 
		//Flash the geal color
		Flashing(healColor);
		//Claming current heath from max heath
		curHeath = Mathf.Clamp(curHeath, 0, maxHeath);
		//Show the hp bar
		ShowHP();
	}

	public void Flashing(Color flash)
	{
		//Cancel current clear to clear an new one
		CancelInvoke("ClearFlash"); Invoke("ClearFlash", flashDuration);
		//Change all the renders color to be the flash color receive
		for (int c = 0; c < renders.Count; c++) {renders[c].color = flash;}
	}
	public void ClearFlash()
	{
		//Reset all the renders color back to it default color
		for (int c = 0; c < renders.Count; c++) {renders[c].color = defaultColors[c];}
	}

	void ExitCombat() 
	{
		//No longer in combat
		inCombat = false;
		//Hide the hp bar
		HideHP();
	}

	public void ShowHP()
	{
		//Display the percented of current heath from max heath by change heath bar X axis
		heathbar.transform.localScale = new Vector3(curHeath/maxHeath,1,1);
		//Enable the heathbar parent
		heathbar.parent.gameObject.SetActive(true);
	}

	//Hide the heathbar by disabling it parent
	public void HideHP() {heathbar.parent.gameObject.SetActive(false);}
}
