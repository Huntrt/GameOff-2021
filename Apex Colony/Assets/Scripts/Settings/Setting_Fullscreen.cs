using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Settings
{

public class SettingsUI_Fullscreen : Setting
{
	[Header("Both are optinal")]
	[Tooltip("Simple toggle between 'Window' mode and 'FullScreenWindow(Borderless)' for shorter game")]
	[SerializeField] Toggle fullscreenToggle;
	[Tooltip("Dropdown for all main window option \nShould only use in window build since it might need 'ExllusiveFullscreen'")]
	[SerializeField] TMPro.TMP_Dropdown fullscreenDropdown;
	List<FullScreenMode> fullScreensOption = new List<FullScreenMode>();
	[SerializeField] bool showWindowMaximizeOption;

	bool isWindowed {get => SettingsManager.i.Data.fullScreenMode == FullScreenMode.Windowed;}

	public override void BuildUI()
	{
		base.BuildUI();
		BuildDropdown();
		if(fullscreenToggle != null) fullscreenToggle.onValueChanged.AddListener(ToggleChoose);
		if(fullscreenDropdown != null) fullscreenDropdown.onValueChanged.AddListener(DropdownChoose);
	}

	void BuildDropdown()
	{
		if(fullscreenDropdown == null) return;
		fullscreenDropdown.ClearOptions();
		List<string> optionsDisplay = new List<string>();

		//Add the 'FullScreenWindow' window mode option (Borderless)
		fullScreensOption.Add(FullScreenMode.FullScreenWindow); optionsDisplay.Add("Borderless");
		//If game running on window add the 'ExclusiveFullScreen' window mode option
		if(Application.platform == RuntimePlatform.WindowsPlayer)
		{fullScreensOption.Add(FullScreenMode.ExclusiveFullScreen); optionsDisplay.Add("Exclusive FullScreen");}
		//If game running on window or mac add the 'MaximizedWindow' window mode option
		if(showWindowMaximizeOption && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer))
		{fullScreensOption.Add(FullScreenMode.MaximizedWindow); optionsDisplay.Add("Maximized Window");}
		//Add the 'Windowed' window mode option
		fullScreensOption.Add(FullScreenMode.Windowed); optionsDisplay.Add("Windowed");

		//Apply those option to dropdown
		fullscreenDropdown.AddOptions(optionsDisplay);
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Refresh toggle to be fullscreen current data
		if(fullscreenToggle != null) fullscreenToggle.SetIsOnWithoutNotify(!isWindowed);
		//Go through to check all dropdown fullscreen option
		if(fullscreenDropdown != null) for (int i = 0; i < fullScreensOption.Count; i++)
		{
			//Select the option match with current fullscreen mode
			if(SettingsManager.i.Data.fullScreenMode == fullScreensOption[i]) fullscreenDropdown.SetValueWithoutNotify(i);
		}
	}
	
	public override void ApplyData()
	{
		base.ApplyData();
		SetFullscreenMode(SettingsManager.i.Data.fullScreenMode);
	}

	void ToggleChoose(bool state)
	{
		//Dropdown only set 2 state of fullscreen (windowed and borderless) 
		SetFullscreenMode(state ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
	}

	void DropdownChoose(int choose)
	{
		SetFullscreenMode(fullScreensOption[choose]);
	}

	public void SetFullscreenMode(FullScreenMode setFullScreenMode)
	{
		///Set the game fullscreen to what setted
		Screen.fullScreenMode = setFullScreenMode;
		///Save to data
		SettingsManager.i.Data.fullScreenMode = setFullScreenMode;
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.graphic_fullscreen);
	}

	void OnDisable()
	{
		if(fullscreenToggle != null) fullscreenToggle.onValueChanged.RemoveListener(ToggleChoose);
		if(fullscreenDropdown != null) fullscreenDropdown.onValueChanged.RemoveListener(DropdownChoose);
	}
}

}