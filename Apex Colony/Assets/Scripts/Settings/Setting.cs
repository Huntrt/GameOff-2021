using UnityEngine;

public class Setting : MonoBehaviour
{
	public enum Category
	{
		graphic_vSync,
		graphic_fullscreen,
		graphic_resolution,
		graphic_fpsCap,
		graphic_quality,
		audio,
		keybind,
		camera
	}

	public delegate void OnSettingChange(Category category); public OnSettingChange onSettingChange;

	public virtual void BuildUI()
	{

	}

	public virtual void ShowDataInUI()
	{

	}

	/// <summary>Apply the setting data to game</summary>
	public virtual void ApplyData()
	{

	}
}