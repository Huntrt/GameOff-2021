using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Heath : MonoBehaviour
{
	public float maxHeath;
    [SerializeField] float _curHeath; public float curHeath {get => _curHeath;}
	[SerializeField] Transform heathbar;
	[Tooltip("How long to stop combat since the last time take damage")]
	[SerializeField] float coolOff;
	[HideInInspector] public bool inCombat;
	[SerializeField] float flashDuration;
	[SerializeField] Color hurtColor, healColor;
	public ParticleSystem deathEffect;	List<SpriteRenderer> renders = new List<SpriteRenderer>();
	List<Color> defaultColors = new List<Color>();
	Manager manager;

	void Start()
	{
		//Get the manger
		manager = Manager.i;
		//Reset heath
		_curHeath = maxHeath;
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
		_curHeath -= damage;
		//Create the hurt popup at this object with no rotation and no auto activation
		GameObject popup = Pool.get.Create(manager.hurtPop, transform.position, Quaternion.identity, false);
		//Update the popup text display as the damage has taken
		popup.GetComponentInChildren<TextMeshProUGUI>().text = "-" + damage;
		//Add this popup into the world canvas and active the gameobject
		popup.transform.SetParent(manager.worldCanvas.transform); popup.SetActive(true);
		//Flash the hurt color
		Flashing(hurtColor);
		//If heath are zero
		if(_curHeath <= 0) 
		{
			//Play the death effect
			deathEffect.transform.parent = null; deathEffect.Play();
			//If this eneity are enemy
			if(gameObject.CompareTag("Enemy"))
			{
				//Gain the food of enemy
				Foods.i.Gain(GetComponent<Enemy>().foodGain);
				//Count this enemy has been kill
				manager.level.killCount++;
			}
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
		_curHeath += heal; 
		//Create the heal popup at this object with no rotation and no auto activation
		GameObject popup = Pool.get.Create(manager.healPop, transform.position, Quaternion.identity, false);
		//Update the popup text display as the healing has get
		popup.GetComponentInChildren<TextMeshProUGUI>().text = "+" + heal;
		//Add this popup into the world canvas and active the gameobject
		popup.transform.SetParent(manager.worldCanvas.transform); popup.SetActive(true);
		//Flash the geal color
		Flashing(healColor);
		//Claming current heath from max heath
		_curHeath = Mathf.Clamp(_curHeath, 0, maxHeath);
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
		heathbar.transform.localScale = new Vector3(_curHeath/maxHeath,1,1);
		//Enable the heathbar parent
		heathbar.parent.gameObject.SetActive(true);
	}

	//Hide the heathbar by disabling it parent
	public void HideHP() {heathbar.parent.gameObject.SetActive(false);}
}
