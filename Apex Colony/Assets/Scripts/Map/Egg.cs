using UnityEngine;

public class Egg : MonoBehaviour
{
	public Interactable interactable;
    public int cost;
	EggsPanel panel;
	bool interacted = false;
	public ParticleSystem effect;

	void Start()
	{
		//Get the eggs panel
		panel = Manager.i.eggsPanel;
		//Display panel upon interact with eggs
		interactable.interact.AddListener(DisplayPanel);
	}

	void DisplayPanel()
	{
		//If this the first time interact
		if(!interacted)
		{
			//Stop whole formation when interact
			Manager.i.control.StopFromation();
			//Buy allies when clicked accept on panel
			panel.accept.onClick.AddListener(BuyAllies);
			//Begin closing when click close button on panel
			panel.close.onClick.AddListener(Closing);
			//Update the panel info with cost
			panel.UpdateInfo(cost);
			//Active the eggs panel
			panel.gameObject.SetActive(true);
			//Has interacted
			interacted = true;

			SFX_Manager.PlaySFX("Interact Map");
		}
	}

	void BuyAllies()
	{
		//If able to spend food
		if(Foods.i.Spend(cost)) 
		{
			//Closing the panel
			Closing();
			//Open an egg at this egg position with no rotation
			Instantiate(Manager.i.eggs.EggDrop(), transform.position, Quaternion.identity);
			//Play the egg open effect
			effect.transform.parent = null; effect.Play();
			//Destroy the gameobject
			Destroy(gameObject);

			SFX_Manager.PlaySFX("Open Egg");
		}
	}

	void Closing()
	{
		//No longer interact
		interacted = false;
		//@ Remove the buying and closing listener upon panel close
		panel.accept.onClick.RemoveListener(BuyAllies);
		panel.close.onClick.RemoveListener(Closing);
		//Deactive the panel
		panel.gameObject.SetActive(false);
	}
}