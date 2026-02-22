using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Game.Settings
{
	
public class Setting_Resolution : Setting
{
	[SerializeField] TMP_Dropdown resDropdown;
	List<ResolutionOption> resOptions = new List<ResolutionOption>();
	[SerializeField] bool showHz, onlyLowestHz, onlyCommonResolution;
	[SerializeField] List<Vector2Int> commonResolutions = new List<Vector2Int>
	{
		new Vector2Int(800, 600),
		new Vector2Int(1024, 768),
		new Vector2Int(1280, 800),
		new Vector2Int(1280, 1024),
		new Vector2Int(1366, 768),
		new Vector2Int(1440, 900),
		new Vector2Int(1600, 900),
		new Vector2Int(1680, 1050),
		new Vector2Int(1920, 1080),
		new Vector2Int(1920, 1200),
		new Vector2Int(2048, 1080),
		new Vector2Int(2560, 1440),
		new Vector2Int(2560, 1600),
		new Vector2Int(2880, 1800),
		new Vector2Int(3200, 1800),
		new Vector2Int(3440, 1440),
		new Vector2Int(3840, 1600),
		new Vector2Int(3840, 2160),
		new Vector2Int(5120, 2880),
		new Vector2Int(7680, 4320)

	};

	public static Vector2Int ResolutionToVector(Resolution res) {return new Vector2Int(res.width, res.height);}
	public static Vector2Int ScreenSize {get => new Vector2Int(Screen.width, Screen.height);}

	[System.Serializable] class ResolutionOption
	{
		public Vector2Int dimensions;
		public RefreshRate refreshRate;
		public ResolutionOption(Vector2Int dimensions, RefreshRate refreshRate) {this.dimensions = dimensions; this.refreshRate = refreshRate;}
	}

	public override void BuildUI()
	{
		base.BuildUI();
		resOptions.Clear();
		//Go through ALL unity provide resolution
		for (int r = 0; r < Screen.resolutions.Length; r++)
		{
			//This resolution about to be filter
			Vector2Int resVector = ResolutionToVector(Screen.resolutions[r]);
			bool isAnOption = true;
			///It might not be an option if it the higher HZ
			if(onlyLowestHz)
			{
				//Check if previous resolution are the same but with lower Hz
				if(r>0 && resVector == ResolutionToVector(Screen.resolutions[r-1])) isAnOption = false;
			}
			///It might not be an option if it uncommon
			if(onlyCommonResolution && !commonResolutions.Contains(resVector)) 
			{
				isAnOption = false;
			}
			///After 2 filter the resolution are now an dropdown option
			if(isAnOption)
			{
				//Add this screen size to option
				resOptions.Add(new ResolutionOption(resVector, Screen.resolutions[r].refreshRateRatio));
			}
		}

		//Adding resolution option to dropdown UI
		resDropdown.ClearOptions();
		List<string> displayRes = new List<string>();
		for (int o = 0; o < resOptions.Count; o++)
		{
			ResolutionOption option = resOptions[o];
			//Format option string
			displayRes.Add
			(
				//Display the resolution as "1920x1080"
				option.dimensions.x + "x" + option.dimensions.y +
				//Show the refesh rate as "1920x1080 60Hz" if needed to show multiple Hz option
				(showHz ? " " + System.Math.Round(option.refreshRate.value) + "Hz" : "") +
				//Mark the last resolution (best supported by desktop) as native "1920x1080 60Hz (Native)"
				((o >= resOptions.Count-1) ? " (Native)" : "")
			);
		}
		resDropdown.AddOptions(displayRes);
		
		resDropdown.onValueChanged.AddListener(Choosing);
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Go through all option of this dropdown
		for (int r = 0; r < resOptions.Count; r++)
		{
			//Set the dropdown selected to be the option match with current screen dimension
			if(resOptions[r].dimensions == ScreenSize) {resDropdown.SetValueWithoutNotify(r); return;}
		}
		//? Dropdown are auto select if using custom resolution
	}

	public override void ApplyData()
	{
		base.ApplyData();
		//Automaticly set any negative (invalid) resolution to native resolution value
		Vector2Int dataRes = SettingsManager.i.Data.resolution;
		if(dataRes.x < 0) dataRes.x = resOptions[resOptions.Count-1].dimensions.x;
		if(dataRes.y < 0) dataRes.y = resOptions[resOptions.Count-1].dimensions.y;
		SetResolution(dataRes);
	}

	void Choosing(int choosed)
	{
		//Set resolution using dropdown choose
		SetResolution(resOptions[choosed].dimensions);
	}

	public void SetResolution(Vector2Int setResolution)
	{
		///Set game resolution to given resolution
		Screen.SetResolution(setResolution.x, setResolution.y, SettingsManager.i.Data.fullScreenMode);
		///Save to the data
		SettingsManager.i.Data.resolution = setResolution;
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.graphic_resolution);
	}

	void Update()
	{
		DisplayCustomResolution();
	}

	Vector2Int prevScreenSize;
	bool customOptionExisted;
	
	void DisplayCustomResolution()
	{
		//Dont care for display if dropdown hidden
		if(!resDropdown.isActiveAndEnabled) return;
		//If the game windows get resized
		if(ScreenSize != prevScreenSize)
		{
			//Go through to check if new screen size already an dropdown option
			bool alreadyOption = false;
			for (int i = 0; i < resOptions.Count; i++)
			{
				alreadyOption = resOptions[i].dimensions == ScreenSize;
				if(alreadyOption) break;
			}
			//If the screen size not an option (customized)
			if(!alreadyOption)
			{
				//Add an new option for custom dropdown
				if(!customOptionExisted)
				{
					resOptions.Insert(0, new ResolutionOption(ScreenSize, Screen.currentResolution.refreshRateRatio));
					resDropdown.options.Insert(0, new TMP_Dropdown.OptionData());
					customOptionExisted = true;
				}
				//Set the custom resolution option data and UI
				resOptions[0].dimensions = ScreenSize; 
				//Mark the first resolution when using custom resolution as "1009 x 754 60Hz (Custom)"
				resDropdown.options[0].text = ScreenSize.x + "x" + ScreenSize.y + " (Custom)";

				resDropdown.RefreshShownValue();
				resDropdown.SetValueWithoutNotify(0);
			}
		}
		prevScreenSize = ScreenSize;
	}

	void OnDisable()
	{
		resDropdown.onValueChanged.RemoveListener(Choosing);
	}
}

}