using UnityEngine.UI;
using UnityEngine;

namespace Game.Settings
{
public class Keybind_Button : MonoBehaviour
{
	[SerializeField] string bindedAction; public string BindedAction {get {return bindedAction;} set {bindedAction = value; Setup();}}
	[SerializeField] TMPro.TMP_Text actionLabel, keycodeLabel;
	[SerializeField] Button bindingButton;
	[SerializeField] bool prettyKeyCode, specialCharacter, autoSetup;
	Keybind manager;

	void OnValidate()
	{
		#if UNITY_EDTIOR
		//Auto setup if this object is not an prefab when needed
		if(PrefabUtility.GetCorrespondingObjectFromSource(gameObject) != null && autoSetup) Setup();
		#endif
	}

	void Setup()
	{
		//Get the manager
		manager = Keybind.i;
		//Auto naming this button stuff base it action
		gameObject.name = bindedAction + " Bind";
		actionLabel.gameObject.name = bindedAction + " Action Label";
		actionLabel.text = bindedAction;
		keycodeLabel.gameObject.name = bindedAction + " KeyCode Label";
		RefreshKeyCodeDisplay();
	}

	void OnEnable()
	{
		manager = Keybind.i;
		bindingButton.onClick.AddListener(BindButtonClick);
		RefreshKeyCodeDisplay();
	}

	void OnDisable()
	{
		bindingButton.onClick.RemoveListener(BindButtonClick);
	}

	void BindButtonClick()
	{
		//Begin bind when button click
		manager.BeginBind(this);
	}

	public void RefreshKeyCodeDisplay(string customText = "")
	{
		//Auto set the keycode text base on this action in dictionary
		if(customText == "") {keycodeLabel.text = KeyCodeFormatting(manager.GetKey(bindedAction).ToString());}
		//Set the keycode text as custom if needed
		else {keycodeLabel.text = customText;}
	}

	string KeyCodeFormatting(string keycodeString)
	{
		string formmatted = keycodeString;
		///If wanted to display special character as themselve instead of it name
		if(specialCharacter)
		{
			string s = formmatted;
			if (s.Contains("Hash")) s = s.Replace("Hash", "#");
			if (s.Contains("Dollar")) s = s.Replace("Dollar", "$");
			if (s.Contains("Percent")) s = s.Replace("Percent", "%");
			if (s.Contains("Ampersand")) s = s.Replace("Ampersand", "&");
			if (s.Contains("Asterisk")) s = s.Replace("Asterisk", "*");
			if (s.Contains("LeftParen")) s = s.Replace("LeftParen", "(");
			if (s.Contains("RightParen")) s = s.Replace("RightParen", ")");
			if (s.Contains("Minus")) s = s.Replace("Minus", "-");
			if (s.Contains("Equals")) s = s.Replace("Equals", "=");
			if (s.Contains("LeftBracket")) s = s.Replace("LeftBracket", "[");
			if (s.Contains("RightBracket")) s = s.Replace("RightBracket", "]");
			if (s.Contains("Backslash")) s = s.Replace("Backslash", "\\");
			if (s.Contains("Semicolon")) s = s.Replace("Semicolon", ";");
			if (s.Contains("BackQuote")) s = s.Replace("BackQuote", "~");
			if (s.Contains("Quote")) s = s.Replace("Quote", "'");
			if (s.Contains("Comma")) s = s.Replace("Comma", ",");
			if (s.Contains("Period")) s = s.Replace("Period", ".");
			if (s.Contains("Slash")) s = s.Replace("Slash", "/");
			formmatted = s;
		}
		///If need to make keycode name better
		if(prettyKeyCode)
		{
			string s = formmatted;
			if(s.Contains("Alpha")) s = s.Remove(0,5); //Aplha3 -> 3
			if(s.Contains("Arrow")) s = s.Replace("Arrow", ""); //UpArrow -> Up
			//Go through the whole keycode name
			char[] chars = s.ToCharArray(); for (int i = 0; i < chars.Length; i++)
			{
				//Skip if the keycode start with capital F (F1,F2,F3,...)
				if(chars[i] == 'F') break;
				//Adding space behind capital text or number (LeftShift -> Left Shift / Keypad3 -> Keypad 0)
				if((char.IsUpper(chars[i]) || char.IsDigit(chars[i])) && i > 0) {s = s.Insert(i, " ");}
			}
			if(s.Contains("Mouse 0")) s = s.Replace("Mouse 0", "Left Mouse"); // Mouse0 -> Left Mouse
			if(s.Contains("Mouse 1")) s = s.Replace("Mouse 1", "Right Mouse"); // Mouse1 -> Right Mouse
			if(s.Contains("Mouse 2")) s = s.Replace("Mouse 2", "Middle Mouse"); // Mouse0 2 -> Middle Mouse
			formmatted = s;
		}
		return formmatted;
	}
}
}