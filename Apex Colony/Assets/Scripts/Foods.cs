using UnityEngine.UI;
using UnityEngine;

public class Foods : MonoBehaviour
{
    public int food;
	///Make this into singelton
	public static Foods i; void Awake() {i = this;}
	public GameObject foodPanel;
	[SerializeField] TMPro.TextMeshProUGUI foodDisplay, foodIO;
	public float displayDuration, IODuration;
	[SerializeField] Color GainColor, SpendColor;
	bool flashFood; public bool showFood; 

	void Update()
	{
		//Display the text
		foodDisplay.text = food.ToString();
		//Diplay food normally if it not flashing
		if(!flashFood) {foodPanel.SetActive(showFood);}
	}

	public bool Spend(int price)
	{
		//Display the output text when spending
		InputOutput(SpendColor, "-" + price);
		//Able to spend when price are lower or equal to food then take food away
		if(price <= food) {food -= price; return true;}
		//Not able to spend when can't afford price
		return false;
	}

	public void Gain(int amount)
	{
		//@ Only gain when there is panel (mostly to prevent error when exit playmod)
		if(foodPanel == null) {return;}
		//Display the input text when gain more than zero
		if(amount > 0) {InputOutput(GainColor, "+" + amount);}
		//Show the food panel
		foodPanel.SetActive(true);
		//Increase the food with amount gain
		food += amount;
		//HIde the food panel after set time
		CancelInvoke("HideDisplay"); Invoke("HideDisplay", displayDuration);
		//Begin flash food panel
		flashFood = true;
	}

	public void InputOutput(Color outlineColor, string text)
	{
		//Show the IO outline color as either gain or spend color
		foodIO.color = outlineColor;
		//Update the IO text
		foodIO.text = text;
		//Hide the IO display after an duartion
		CancelInvoke("HideIO"); Invoke("HideIO", IODuration);
	}

	//Hide the display when not currently showing it and food panel stopped flash
	public void HideDisplay() {if(!showFood) {foodPanel.SetActive(false);} flashFood = false;}
	//Clear the the IO text
	public void HideIO() {foodIO.text = "";}
}