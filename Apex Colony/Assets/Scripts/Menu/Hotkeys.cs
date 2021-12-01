using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Hotkeys : MonoBehaviour
{
	//The TEXT DISPLAY of key data currently assigning
	TextMeshProUGUI assignDisplay;
	//UI Panel to blocking input during assigning
	[SerializeField] GameObject blockInput;
	[Tooltip("Message when waiting to assign an key")]
	[SerializeField] string waitingMessage;
	//The NAME of key data currently assigning
	[SerializeField] string assignName;
	//Are current assign anything 
	public bool areAssigning;
	//Turn this script into singleton and set assign name to default
	public static Hotkeys s; void Awake() {s = this; assignName = "None";}

	///List of key name
	public KeyCode camUp, camDown, camLeft, camRight, camOption, camReset , zoomReset;
	public KeyCode move, attack, stop, track, procced;

	void Start() 
	{
		//Setup key upon upon game start (useless parameters)
		SetupKeys(SceneManager.GetSceneAt(0),LoadSceneMode.Additive);
		//Setup key upon upon scene change
		SceneManager.sceneLoaded += SetupKeys;
	}

	public void SetupKeys(Scene scene, LoadSceneMode mode)
	{
		//! Refresh the singleton
		s = this;
		//Get the block input object from canvas player manager
		blockInput = PlayManager.i.blockInput;
		// Print an warning if there are no block input panel (can remove if wanted)
		if(blockInput == null) {Debug.LogWarning("There are no UI panel to blocking input for hotkey");}
	}
	
	public void StartAssign(KeyInfo key)
	{
		//The assign display are now display of key call event
		assignDisplay = key.display;
		//The assign name are now name of key call event
		assignName = key.Name;
		//Begin assigning
		areAssigning = true;
		//Begin blocking input when assign if has one
		if(blockInput != null) {blockInput.SetActive(true);}
	}
	
	void Update()
	{
		//If currently assigning
        if(areAssigning)
        {
			//Change the assign display to waititng message
			assignDisplay.text = waitingMessage;
            //Go though all the key to check if it there is currently any input
            foreach(KeyCode pressedKey in System.Enum.GetValues(typeof(KeyCode)))
            //If there is an input from any keycode and there is assign name (prevent error two input at once)
            if(Input.GetKey(pressedKey) && this.GetType().GetField(assignName) != null)
            {
				//Change keycode variable has the same name as assign name to pressed keycode type
				this.GetType().GetField(assignName).SetValue(this, pressedKey);
				//Change the assign key's display text as pressed keycode type
				assignDisplay.text = pressedKey.ToString();
				//No longer assign and clear current assign key name
				areAssigning = false; assignName = "None";
				//Stop blocking input when no longer assign if has one
				if(blockInput != null) {blockInput.SetActive(false);}
            }
        }
	}

	
	//Remove delegate event
	void OnDisable() {SceneManager.sceneLoaded -= SetupKeys;}
}