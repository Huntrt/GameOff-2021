using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Settings
{

public class Setting_FPSCap : Setting
{
	[SerializeField] Slider fpsCapSlider;
	[SerializeField] TMPro.TMP_Text fpsCapSliderLabel;
	[SerializeField] int minSlider = 0, maxSlider = 480;


	public override void BuildUI()
	{
		base.BuildUI();
		//Set min max of slider
		fpsCapSlider.minValue = minSlider;
		fpsCapSlider.maxValue = maxSlider;
		//Display the default lavel
		SetLabel(Mathf.RoundToInt(fpsCapSlider.value));
		fpsCapSlider.onValueChanged.AddListener(Choosing);
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		//Set slider and label to what data have
		fpsCapSlider.SetValueWithoutNotify(SettingsManager.i.Data.fpsCap);
		SetLabel(SettingsManager.i.Data.fpsCap);
	}
	
	public override void ApplyData()
	{
		base.ApplyData();
		SetFPSCap(SettingsManager.i.Data.fpsCap);
	}

	void Choosing(float choosed)
	{
		//Choosing value are round to nearest 10 (121 = 120)
		int rounded = Mathf.RoundToInt(choosed /= 10) * 10;
		SetFPSCap(rounded);
		SetLabel(rounded);
	}

	void SetLabel(int value)
	{
		fpsCapSliderLabel.text = value > 0 ? value.ToString() : "INF";
	}

	public void SetFPSCap(int setFpsCap)
	{
		///Set game FPS cap to set value
		Application.targetFrameRate = setFpsCap;
		//Save to data
		SettingsManager.i.Data.fpsCap = setFpsCap;
		//Call this setting category has changed
		onSettingChange?.Invoke(Category.graphic_fpsCap);
	}

	void OnDisable()
	{
		fpsCapSlider.onValueChanged.RemoveListener(Choosing);
	}

}

}