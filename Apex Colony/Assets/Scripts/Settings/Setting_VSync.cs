using UnityEngine;
using UnityEngine.UI;

namespace Game.Settings
{

public class Setting_VSync : Setting
{
	[SerializeField] Toggle vsyncToggle;

	public override void BuildUI()
	{
		base.BuildUI();
		vsyncToggle.onValueChanged.AddListener(SetVSync);
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Set the toggle to be vsync current data
		vsyncToggle.SetIsOnWithoutNotify(SettingsManager.i.Data.vSync);
	}

	public override void ApplyData()
	{
		base.ApplyData();
		SetVSync(SettingsManager.i.Data.vSync);
	}

	public void SetVSync(bool setVSync)
	{
		///Set vSync when toggle where 1 is on and 0 is off
		QualitySettings.vSyncCount = setVSync ? 1 : 0;
		///Save to data
		SettingsManager.i.Data.vSync = setVSync;
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.graphic_vSync);
	}

	void OnDisable()
	{
		vsyncToggle.onValueChanged.RemoveListener(SetVSync);
	}
}

}