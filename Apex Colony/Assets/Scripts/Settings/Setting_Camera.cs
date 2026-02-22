using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Settings
{

public class Setting_Camera : Setting
{
	[SerializeField] TMP_Text camSpeedAmount;
	[SerializeField] Slider camSpeedSlider;
	[SerializeField] TMP_Text zoomSpeedAmount;
	[SerializeField] Slider zoomSpeedSlider;

	public override void BuildUI()
	{
		base.BuildUI();
		camSpeedSlider.onValueChanged.AddListener(SetCameraSpeed);
		zoomSpeedSlider.onValueChanged.AddListener(SetZoomSpeed);
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Set camera and zoom speed slider to what in data
		camSpeedSlider.SetValueWithoutNotify(SettingsManager.i.Data.cameraMoveSpeed);
		zoomSpeedSlider.SetValueWithoutNotify(SettingsManager.i.Data.zoomSpeed);
	}
	
	public override void ApplyData()
	{
		base.ApplyData();
		//Clamp the quality data from 0 to the amount of quality available
		SetCameraSpeed(SettingsManager.i.Data.cameraMoveSpeed);
		SetZoomSpeed(SettingsManager.i.Data.zoomSpeed);
	}

	public void SetCameraSpeed(float amount)
	{
		//Save to data
		SettingsManager.i.Data.cameraMoveSpeed = amount;
		camSpeedAmount.text = Mathf.RoundToInt(amount).ToString();
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.camera);
	}

	public void SetZoomSpeed(float amount)
	{
		//Save to data
		SettingsManager.i.Data.zoomSpeed = amount;
		zoomSpeedAmount.text = Mathf.RoundToInt(amount).ToString();
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.camera);
	}

	void OnDisable()
	{
		camSpeedSlider.onValueChanged.RemoveAllListeners();
		zoomSpeedSlider.onValueChanged.RemoveAllListeners();
	}
}

}