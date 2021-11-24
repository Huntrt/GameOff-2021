using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PortSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button buying;
	[HideInInspector]public SpriteRenderer iconDisplay;
	public TextMeshProUGUI itemName, itemPrice;
	[HideInInspector] public int price;
	[TextArea(3,20)][SerializeField] string description;
	PortsPanel panel;

	//Get the port panel
	void Start() {panel = Manager.i.portsPanel;}

	public void UpdateDescription(string name, string info)
	{
		//@ Update the description with color codded name, price and info
		description = 
		"<b><color=#d9a52e>" + name + "</color></b>\n\n" +
		"Cost <b><color=#4a5ccf>" + price + "</color></b> food\n\n- "+ info;
	}

	void Update()
	{
		//Get the color block of buying button
		ColorBlock colors = buying.colors;
		//If there enough food to buy this slot
		if(price <= Foods.i.food)
		{
			//@ Update the buying button's color state to be rich color state
			colors.normalColor = panel.richState.normal;
			colors.highlightedColor = panel.richState.highlight;
			colors.pressedColor = panel.richState.press;
		}
		//If don't has enough food to buy this slot
		else
		{
			//@ Update the buying button's color state to be poor color state
			colors.normalColor = panel.poorState.normal;
			colors.highlightedColor = panel.poorState.highlight;
			colors.pressedColor = panel.poorState.press;
		}
		//Update the buying button colors
		buying.colors = colors;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		//Change the description icon as this slot icon
		panel.descriptionIcon.sprite = iconDisplay.sprite;
		//Change the description display's text as this slot description
        panel.descriptionText.text = description;
		//Show the description panel display
        panel.descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		//Hide the description panel display
        panel.descriptionPanel.SetActive(false);
    }
}
