using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [System.Serializable] class Tutorialing 
	{
		public Sprite image;
		public string key;
		[TextArea(10,100)] public string info;
	}
	[SerializeField] Tutorialing[] tutorials;
	[SerializeField] Image displayImage;
	[SerializeField] TextMeshProUGUI displayInfo;
	[SerializeField] int currentDisplay;

	//Display the first turtorial
	void Start() {DisplayTurtorial();}

	public void NextTutorial()
	{
		//Go to the next turtorial display
		currentDisplay++;
		//Reset back to the first tutorial if has go through all of them
		if(currentDisplay >= tutorials.Length) {currentDisplay = 0;}
		//Display the tutorial
		DisplayTurtorial();
	}

	public void PrevTutorial()
	{
		//Go to the previous turtorial display
		currentDisplay--;
		//Reset back to the final tutorial if has go pass all of them
		if(currentDisplay < 0) {currentDisplay = tutorials.Length-1;}
		//Display the tutorial
		DisplayTurtorial();
	}

	public void DisplayTurtorial()
	{
		//Get the current tutorial info
		string info = tutorials[currentDisplay].info;
		//The key need to display
		string key = tutorials[currentDisplay].key;
		//If need to replace an key in info
		if(key != "")
		{
			//Get the hotkey has the same name as key needed to display without tag
			string keyDisplay = Keybind.i.GetBind(key.Replace("$", "")).keyCode.ToString();
			//Replace the key needed to display to it keycode in info
			info = info.Replace(key, "[" + keyDisplay + "]");
		}
		//Display the image of current turtorial
		displayImage.sprite = tutorials[currentDisplay].image;
		//Display the info has modify
		displayInfo.text = info;
	}
}
