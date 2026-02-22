using System.Data.Common;
using Game.Settings;
using UnityEngine;

/// <summary>
/// Handle the process of set the settings data to the game
/// Also handle set data base on the game 
/// </summary>
public class SettingsManager : MonoBehaviour
{
	#region Set this class to singleton
	static SettingsManager _i; public static SettingsManager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindFirstObjectByType<SettingsManager>();
			}
			return _i;
		}
	}
	#endregion
	
	[SerializeField] SettingsSO settingScriptableObj;
	public SettingsSO scriptableObj {get => settingScriptableObj;}
	public SettingsData Data {get => settingScriptableObj.data;}
	[Tooltip("Most of the 'Graphic' setting are auto save by unity across session, enable this use will make data use that")]
	[SerializeField] bool applyGraphicAutosaveToData;
	Setting[] settings;

	void Awake()
	{
		//Find all the object use base 'Setting' script
		settings = FindObjectsByType<Setting>(FindObjectsInactive.Include, FindObjectsSortMode.None);
	}

	void Start()
	{
		//Set the unity auto save grpahic to data
		if(applyGraphicAutosaveToData) GraphicAutosaveToData();
		RebuildAllSettingUI();
	}

	public void RebuildAllSettingUI()
	{
		//Build the UI and show data in those UI of settings then apply those data
		foreach (Setting setting in settings)
		{
			setting.BuildUI();
			setting.ShowDataInUI();
			setting.ApplyData();
		}
	}

	void GraphicAutosaveToData()
	{
		settingScriptableObj.data.vSync = (QualitySettings.vSyncCount >= 1) ? true : false;
		settingScriptableObj.data.fullScreenMode = Screen.fullScreenMode;
		settingScriptableObj.data.resolution = Setting_Resolution.ScreenSize;
		settingScriptableObj.data.fpsCap = Application.targetFrameRate;
		settingScriptableObj.data.graphicQuality = QualitySettings.GetQualityLevel();
	}

	public void SetData(SettingsSO data)
	{
		this.settingScriptableObj = data; 
	}
}