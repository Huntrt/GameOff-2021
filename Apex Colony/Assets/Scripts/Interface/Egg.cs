using UnityEngine;

public class Egg : MonoBehaviour
{
	public Interactable interactable;
    public int cost;
	public GameObject allies;
	EggsPanel panel;

	void Start()
	{
		//Get the eggs panel
		panel = Manager.i.eggsPanel;
		//DIsplay panel upon interact with eggs
		interactable.interact.AddListener(DisplayPanel);
	}

	void DisplayPanel()
	{
		//Stop whole formation when interact
		Manager.i.control.StopFromation();
		//Buy allies when clicked accept on panel
		panel.accept.onClick.AddListener(BuyAllies);
		//Begin closing when panel decline
		panel.decline.onClick.AddListener(Closing);
		//Update the panel info with cost
		panel.UpdateInfo(cost);
		//Active the eggs panel
		panel.gameObject.SetActive(true);
	}

	void BuyAllies()
	{
		//If able to spend food
		if(Foods.i.Spend(cost)) 
		{
			//Open an egg at this egg position with no rotation
			Instantiate(Manager.i.eggs.OpenEgg(), transform.position, Quaternion.identity);		
			//Closing the panel
			Closing();
			//Destroy the gameobject
			Destroy(gameObject);
		}
	}

	void Closing()
	{
		//@ Remove the buying and closing event upon panel close
		panel.accept.onClick.RemoveListener(BuyAllies);
		panel.decline.onClick.RemoveListener(Closing);
		//Deactive the panel
		panel.gameObject.SetActive(false);
	}
}