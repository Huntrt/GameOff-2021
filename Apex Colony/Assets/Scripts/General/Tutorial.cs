using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Game.Settings;

public class Tutorial : MonoBehaviour
{
    [System.Serializable] class Tutorialing 
	{
		public Sprite image;
		[TextArea(10,100)] public string info;
	}
	[SerializeField] Tutorialing[] tutorials;
	[SerializeField] Image displayImage;
	[SerializeField] TextMeshProUGUI displayInfo;
	[SerializeField] int currentDisplay;
	[SerializeField] TMP_Text pageCounter;

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
		pageCounter.text = currentDisplay+1 + "/" + tutorials.Length;

		//Get the current tutorial info
		string info = tutorials[currentDisplay].info;

		//Collect all the word have "$user_name" pattern
		MatchCollection matches = Regex.Matches(info, @"\$\w+");

		//Replace what in the pattern with that keybind keycode and hightlight them
		foreach (Match match in matches)
		{
			//Prettify the keycvode will be show in info
			string keycodeDisplay = Keybind.i.GetBind(match.Value.Replace("$","").Replace("_", " ")).keyCode.ToString();
			keycodeDisplay = "<color=#ffff00><b>" + Keybind_Button.KeyCodeFormatting(keycodeDisplay) + "</color></b>";

			info = info.Replace(match.Value, keycodeDisplay);
		}

		//Display the image of current turtorial
		displayImage.sprite = tutorials[currentDisplay].image;
		//Display the info has modify
		displayInfo.text = info;
	}
}
