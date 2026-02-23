using System.Collections.Generic;
using System.Collections;
using Game.Settings;
using UnityEngine;

public class Keybind : MonoBehaviour
{
	[System.Serializable] public class Bind
	{
		public string action;
		public KeyCode keyCode;
	}

	/// <summary>Binds store in settings data </summary>
	public Bind[] binds {get {if(SettingsManager.i != null) return SettingsManager.i.Data.binds; return null;}}
	Dictionary<string, int> bindsHash; public Dictionary<string, int> BindsHash {get => bindsHash;}

	[Tooltip("Will replace key label text when binding it")]
	[SerializeField] string waitingMessage, unbindMessage;
	[Tooltip("Press this key when binding will make key to be unbind")]
	[SerializeField] KeyCode unbindKey = KeyCode.Escape;
	[SerializeField] Keybind_Button currentBindButton;
	public bool areBinding {get => currentBindButton != null;}
	
	#region Set this class to singleton
	static Keybind _i; public static Keybind i 
	{
		get
		{
			//If this class is not static
			if(_i == null)
			{
				//Find object with this class to make it static
				_i = GameObject.FindFirstObjectByType<Keybind>();
				//Refresh the hash of save when it become singleton
				_i.RefreshHash();
			}
			return _i;
		}
	}
	#endregion

	void OnValidate()
	{
		RefreshHash();
	}

	public void RefreshHash()
	{
		//Create an new hash
		bindsHash = new Dictionary<string, int>();
		//Hash by attach each action their list index for fast access
		for (int i = 0; i < binds.Length; i++) bindsHash.Add(binds[i].action, i);
	}

	///<summary>Get the binded keycode of action (Make sure the action exist)</summary>
	public KeyCode GetKey(string actionName)
	{
		//Get the keybind of given action using dictionary
		return GetBind(actionName).keyCode;
	}

	///<summary>Get the bind of given action in dictionary (Make sure the action exist)</summary>
	public Bind GetBind(string actionName)
	{
		//! MAKE SURE ACTION NAME EXIST IN binds[]
		return binds[BindsHash[actionName]];
	}
	
	///<summary>Start binding for the requested button</summary>
	public void BeginBind(Keybind_Button binder)
	{
		if(areBinding) return;
		//Save the button that requested to bind
		currentBindButton = binder;
		//Start the key binding process
		StartCoroutine(Binding());
	}

	IEnumerator Binding()
	{
		//If currently binding
		while(areBinding)
		{
			//Display the watitng message on button keycode text
			currentBindButton.RefreshKeyCodeDisplay(waitingMessage);
            //Go though ALL the key to check if there is currently any input (PERFORMANCE HEAVY)
			foreach(KeyCode pressedKey in System.Enum.GetValues(typeof(KeyCode)))
			{
				//If any key got press
				if(Input.GetKey(pressedKey))
				{
					//Set unbind if pressed key are the unbind key
					if(pressedKey == unbindKey) SetUnbind(false);
					//If press any key beside unbind key
					if(pressedKey != unbindKey)
					{
						///Set the keycode of current binder to be press key
						GetBind(currentBindButton.BindedAction).keyCode = pressedKey;
						//Refresh the binder keycode display
						currentBindButton.RefreshKeyCodeDisplay();
						//? Delay to prevent re-bind after binding left mouse
						if(pressedKey == KeyCode.Mouse0) yield return new WaitForSecondsRealtime(0.55f);
					}
					//Stop assigning
					currentBindButton = null;
				}
			}
			yield return null;
		}
	}

	///<summary>Cancel any binding in process</summary>
	public void SetUnbind(bool cancelBinding)
	{
		if(!areBinding) return;
		///Set the keycode of binder to be none
		GetBind(currentBindButton.BindedAction).keyCode = KeyCode.None;
		//Display the unbind message on button keycode text
		currentBindButton.RefreshKeyCodeDisplay(unbindMessage);
		//Cancel binding process if requested (mainly use if unbind by button)
		if(cancelBinding)
		{
			//No longer bind any button
			currentBindButton = null;
			//Hard stop the key binding process
			StopCoroutine("Binding");
		}
	}
}