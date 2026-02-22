using UnityEngine;
using System;

//[CreateAssetMenu(fileName = "Settings Data", menuName = "New Settings Data")]
public class SettingsSO : ScriptableObject
{
	[SerializeField] SettingsData _data; public SettingsData data {get => _data;}

	public void OverwriteData(SettingsData data)
	{
		_data = data;
	}

	[Serializable] public class Pause
	{
		public bool isPausing;
		public float timeScaleBeforePause;
	}

	[SerializeField] Pause _pause; public Pause pause {get => _pause;}
}