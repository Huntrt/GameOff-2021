using UnityEngine.UI;
using UnityEngine;

public class GamePause : MonoBehaviour
{
	//Fast way to access pausing state
	public static bool isPausing {get => SettingsManager.i.scriptableObj.pause.isPausing;}
	/// <summary>
	/// Called when this object got disable, mainly use for resume when scene got switch
	/// </summary>
	[SerializeField] bool resumeOnObjDisable = true;
	[SerializeField] Button pauseButton, resumeButton;

	void OnDisable()
	{
		if(resumeOnObjDisable) Pausing(false);
	}

	void Update()
	{
		///Pause hotkey are 'Esc'
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//Toggle to the next pause status
			bool togglePause = !SettingsManager.i.scriptableObj.pause.isPausing;
			//Call the pasue button itself when about to pause
			if(pauseButton != null && togglePause) pauseButton.onClick.Invoke();
			//Call the resume button itself when about to resume
			if(resumeButton != null && !togglePause) resumeButton.onClick.Invoke();
		}
	}

	public void Pausing(bool pause)
	{
		//Skip if manager no longer exist
		if(SettingsManager.i == null) return;

		//Get the pausing data store in settings
		SettingsSO.Pause data = SettingsManager.i.scriptableObj.pause;
		///When PAUSE
		if(pause)
		{
			if(!data.isPausing)
			{
				data.isPausing = true;
				//Save the current time scale
				data.timeScaleBeforePause = Time.timeScale;
				//Time scale now are 0
				Time.timeScale = 0;
			}
		}
		///When RESUME
		if(!pause)
		{
			if(data.isPausing)
			{
				data.isPausing = false;
				//Load the saved time scale before pause
				Time.timeScale = data.timeScaleBeforePause;
				//Null out  saved time scale
				data.timeScaleBeforePause = -1;
			}
		}
	}
}