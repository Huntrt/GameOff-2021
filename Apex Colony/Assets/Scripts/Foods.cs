using UnityEngine;

public class Foods : MonoBehaviour
{
    public int food;
	///Make this into singelton
	public static Foods i; void Awake() {i = this;}
	public GameObject foodPanel;
	[SerializeField] TMPro.TextMeshProUGUI foodDisplay;
	public float displayDuration;
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
		//Able to spend when price are lower or equal to food then take food away
		if(price <= food) {food -= price; return true;}
		//Not able to spend when can't afford price
		return false;
	}

	public void Gain(int amount)
	{
		//Show the food panel
		foodPanel.SetActive(true);
		//Increase the food with amount gain
		food += amount;
		//HIde the food panel after set time
		CancelInvoke("HideDisplay"); Invoke("HideDisplay", displayDuration);
		//Begin flash food panel
		flashFood = true;
	}

	//Hide the display when not currently showing it and food panel stopped flash
	public void HideDisplay() {if(!showFood) {foodPanel.SetActive(false);} flashFood = false;}
}