using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class KeyInfo : MonoBehaviour
{
	[HideInInspector] public string Name; Button button; Hotkeys key;
	[Tooltip("The index of child that has text component\nIf using child mode location")] 
	[SerializeField] int childIndex;
	[Tooltip("The gameobject that handle display key code\nIf using custom object location")]
    public TextMeshProUGUI display;

	void Start()
	{
		
		//Getting this object's name and the hotkey singleton
		Name = gameObject.name; key = Hotkeys.s;
		//! If this gameobject name don't match an keycode variable
		if(key.GetType().GetField(Name) == null)
		{
			//Print error
			Debug.LogError("There are no keycode variable matching the " + Name + "'s name");
			Debug.LogError("The "+gameObject+" object need to match an keycode variable name in order to work");
			///No longer re the below code
			return;
		}
		//Getting the text component from children base on the index has set
		display = transform.GetChild(childIndex).GetComponent<TextMeshProUGUI>();
		//Display the keycode variable's that has the same name as this name when create
		display.text = key.GetType().GetField(Name).GetValue(key).ToString();
		//Get the button component
		button = GetComponent<Button>();
		//Add the hotkey's assign event with key info parameter as this component to button
		button.onClick.AddListener(delegate {key.StartAssign(this);});
	}
}