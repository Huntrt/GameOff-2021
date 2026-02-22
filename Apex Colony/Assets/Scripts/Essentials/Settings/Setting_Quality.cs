using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.Settings
{

public class Setting_Quality : Setting
{
	[SerializeField] TMP_Dropdown qualityDropdown;

	public override void BuildUI()
	{
		base.BuildUI();
		//Adding all quality setting in project settings to dropdown
		qualityDropdown.ClearOptions();
		List<string> optionsDisplay = new List<string>();
		for (int i = 0; i < QualitySettings.count; i++)
		{
			optionsDisplay.Add(QualitySettings.names[i]);
		}
		qualityDropdown.AddOptions(optionsDisplay);

		qualityDropdown.onValueChanged.AddListener(SetQuality);

	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Set select dropdown ooption to what data is
		qualityDropdown.SetValueWithoutNotify(SettingsManager.i.Data.graphicQuality);
	}
	
	public override void ApplyData()
	{
		base.ApplyData();
		//Clamp the quality data from 0 to the amount of quality available
		SetQuality(Mathf.Clamp(SettingsManager.i.Data.graphicQuality, 0, QualitySettings.count-1));
	}

	public void SetQuality(int setQuality)
	{
		///Set the game graphic level 
		QualitySettings.SetQualityLevel(setQuality);
		//Save to data
		SettingsManager.i.Data.graphicQuality = setQuality;
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.graphic_quality);
	}

	void OnDisable()
	{
		qualityDropdown.onValueChanged.RemoveListener(SetQuality);
	}
}

}