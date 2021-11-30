using UnityEngine;
using TMPro;

public class Foods : MonoBehaviour
{
    public int food;
	///Make this into singelton
	public static Foods i; void Awake() {i = this;}
	public GameObject foodPanel;
	[SerializeField] TextMeshProUGUI[] foodCounter;
	[SerializeField] TextMeshProUGUI foodIO;
	public float displayDuration;
	[SerializeField] Color GainColor, SpendColor;

	void Update()
	{
		//Display all the food counter text as the current food amount
		foreach (TextMeshProUGUI display in foodCounter) {display.text = food.ToString();}
	}

	public bool Spend(int price)
	{
		//Show the spend text color
		foodIO.color = SpendColor;
		//If has enough food to spend for price
		if(price <= food) 
		{
			//Update the spend text as the price
			foodIO.text = "-" + price;
			//Clear the food input and output info after set time
			CancelInvoke("HideIO"); Invoke("HideIO", displayDuration);
			//Decrease the food with price
			food -= price; 
			//Are able to afford the price
			return true;
		}
		else
		{
			//Update the spend text as when can't afford price
			foodIO.text = "Not enough food!";
			//Clear the food input and output info after set time
			CancelInvoke("HideIO"); Invoke("HideIO", displayDuration);
		}
		//Not able to spend when can't afford price
		return false;
	}

	public void Gain(int amount)
	{
		//% Only gain when there is panel (mostly to prevent error when exit playmod)
		if(foodPanel == null) {return;}
		//Show the gain text color
		foodIO.color = GainColor;
		//Update the gain text
		foodIO.text = "+" + amount;
		//Clear the food input and output info after set time
		CancelInvoke("HideIO"); Invoke("HideIO", displayDuration);
		//Increase the food with amount gain
		food += amount;
	}

	//Clear the the IO text
	public void HideIO() {foodIO.text = "";}
}