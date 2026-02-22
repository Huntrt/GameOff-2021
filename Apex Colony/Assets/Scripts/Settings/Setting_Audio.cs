using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Settings
{
public class Setting_Audio : Setting
{
	[SerializeField] Adjuster[] adjusters;
	[SerializeField] Dictionary<AudioMixerGroup, Adjuster> adjustersDict = new Dictionary<AudioMixerGroup, Adjuster>();

	[System.Serializable] public class Adjuster
	{
		[SerializeField] string exposedParameter; public string ExposedParameter {get => exposedParameter;}
		[SerializeField] AudioMixerGroup mixerGroup; public AudioMixerGroup MixerGroup {get => mixerGroup;}
		[HideInInspector] public int dataIndex;
		[HideInInspector] public Setting_Audio setting;
		[Header("UI")]
		public string labelDisplay;
		public TMPro.TextMeshProUGUI label;
		[SerializeField] Slider slider; public Slider Slider {get => slider;}
		//Volume are stored in settings data at saved index
		public int volume {get => SettingsManager.i.Data.volumes[dataIndex];}

		public void RefreshUI()
		{
			slider.minValue = 0; slider.maxValue = 100;
			slider.SetValueWithoutNotify(volume);
			SetVolume(volume);
		}

		public void SetVolume(float value)
		{
			///Set the mixer group exposed volume paramter
			mixerGroup.audioMixer.SetFloat(exposedParameter, PercentageToDecibel(value/100));
			///Save to data (with 0.100 rounding)
			SettingsManager.i.Data.volumes[dataIndex] = Mathf.RoundToInt(value);
			DisplayVolume();
			//Call this setting category has changed
			setting.onSettingChange?.Invoke(Category.audio);
		}

		public void DisplayVolume()
		{
			//Display the label as "34"
			label.text = Mathf.RoundToInt(volume).ToString();
		}
	}
	
	public override void BuildUI()
	{
		base.BuildUI();
		adjustersDict.Clear();
		//Prevent forgeting to set data volumes array in inspector
		if(SettingsManager.i.Data.volumes.Length != adjusters.Length) 
		{
			SettingsManager.i.Data.volumes = new int[adjusters.Length]; 
			print("Resize the data volumes array to match with setting (resize to "+ adjusters.Length + ")");
		}
		for (int i = 0; i < adjusters.Length; i++)
		{
			Adjuster adjuster = adjusters[i];
			//Add this adjuster to dictionary
			adjustersDict.Add(adjuster.MixerGroup, adjuster);
			//Make adjuster change volume when slider change
			adjuster.Slider.onValueChanged.AddListener(adjuster.SetVolume);
			adjuster.setting = this;
			adjuster.dataIndex = i;
			adjuster.RefreshUI();
		}
	}

	public override void ShowDataInUI()
	{
		base.ShowDataInUI();
		foreach (Adjuster adjuster in adjusters)
		{
			adjuster.RefreshUI();	
		}
	}

	public Adjuster GetAdjuster(AudioMixerGroup mixerGroup) {return adjustersDict[mixerGroup];}
	public AudioMixerGroup GetMixerGroup(string exposedParameter) 
	{
		foreach (Adjuster adjuster in adjusters)
		{
			if(adjuster.ExposedParameter == exposedParameter) return adjuster.MixerGroup;
		}
		Debug.LogError("There are no mixer group for the '" + exposedParameter + "' exposed parameter");
		return null;
	}

	/// <summary>Convert 0 -> 1 base to decibal logarithmic -80 -> 0</summary>
	public static float PercentageToDecibel(float percentage) {return Mathf.Clamp(Mathf.Log10(percentage) * 20, -80, 0);}

	void OnDisable()
	{
		foreach (Adjuster adjuster in adjusters) 
		{
			adjuster.Slider.onValueChanged.RemoveListener(adjuster.SetVolume);
		}
	}
}
}