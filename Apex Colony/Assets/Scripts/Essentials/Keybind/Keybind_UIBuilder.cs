using UnityEngine;

namespace Game.Settings
{
public class Keybind_UIBuilder : MonoBehaviour
{
	[SerializeField] bool buildNow;
	[SerializeField] Transform bindButtonGroup;
	/* This group's children will be use to assigning keybind
		The children need to setup (Just use the template prefab):
			Button (+ BindButton.cs)
				> Action Text Label (Assign in BindButton.cs)
				> Keycode Text Label (Assign in BindButton.cs)
	*/

	[SerializeField] Keybind bindManager;

	void OnValidate()
	{
		if(buildNow)
		{
			Building();
			buildNow = false;
		}
	}

	public void Building()
	{
		//Go through all children of bind group
		for (int c = 0; c < bindButtonGroup.childCount; c++)
		{
			Keybind_Button bind = bindButtonGroup.GetChild(c).GetComponent<Keybind_Button>();
			//If there an bind for this child
			if(c < bindManager.binds.Length)
			{
				//Assign the bind action correspond to the it children index 
				bind.BindedAction = bindManager.binds[c].action;	
			}
			//This button is unuse since there no more bind for it
			else
			{
				bind.BindedAction = "UNUSE";
			}
			#if UNITY_EDTIOR
			Undo.RecordObject(bindButtonGroup.GetChild(c), "Building button bind");
			#endif
		}
	}
}
}