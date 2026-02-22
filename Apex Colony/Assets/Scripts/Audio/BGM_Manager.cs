using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
	#region Set this class to singleton
	static BGM_Manager _i; public static BGM_Manager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindFirstObjectByType<BGM_Manager>();
			}
			return _i;
		}
	}
	#endregion

	///ONLY ONE BGM TRACK ARE ALLOW TO PLAY

	public bool isDDOL = false;
	public AudioMixerGroup bgmMixerGroup;
	[HideInInspector] public Audio_Manager.Speaker exitSpeaker, enterSpeaker;
	bool hasExit, hasEnter;
	float exitDuration, exitTimer, enterDuration, enterTimer;
	bool willContinue;
	public Dictionary<AudioClip, float> bgmTimestamps = new Dictionary<AudioClip, float>(); 

	void Start()
	{
		//Destroy self if it already dont destroy on load since game started
		if(Time.time > 0) {Destroy(gameObject); return;}
		//DDOL need to be root object
		gameObject.transform.SetParent(null);
		//This object is now dont destroy on load
		DontDestroyOnLoad(gameObject);
		isDDOL = true;
	}

	public void PlayBGM(AudioClip clip, float exitDuration, float enterDuration, bool continueWhenExit)
	{
		//If has enter
		if(hasEnter)
		{
			//Overdrive exit speaker if new exit needed (overlapping)
			if(hasExit) exitSpeaker.DestroySource();
			hasExit = true;
			//Current enter become exit
			exitSpeaker = enterSpeaker;
			//Save the exit speaker time stamp
			SaveTimestamp(exitSpeaker);
			//Instant mute if have no duration to exit
			exitSpeaker.SetSourceVolume((exitDuration <= 0) ? 0 : exitSpeaker.Source.volume);
		}
		hasEnter = true;
		///New speaker for given BGM to transition
		enterSpeaker = new Audio_Manager.Speaker(clip, bgmMixerGroup, transform);
		//Setup the speaker
		enterSpeaker.Setup();
		enterSpeaker.Source.clip = clip;
		enterSpeaker.Source.loop = true;
		//If needed to continue th enter bgm
		if(willContinue && bgmTimestamps.ContainsKey(enterSpeaker.Clip))
		{
			//Set the enter speaker clip time as it time saved
			enterSpeaker.Source.time = bgmTimestamps[enterSpeaker.Clip];
			//Consume continue of previous transition
			willContinue = false;
		}
		//Play the BGM audio
		enterSpeaker.Source.Play();
		//Default are mute but will max volume if have no duration
		enterSpeaker.SetSourceVolume((enterDuration <= 0) ? 1 : 0);
		//Reseting to given enter and exit duration
		this.enterDuration = enterDuration;
		this.exitDuration = exitDuration;
		enterTimer = enterDuration; exitTimer = exitDuration;
		//Will the next transition be continue?
		willContinue = continueWhenExit;
	}

	void SaveTimestamp(Audio_Manager.Speaker speaker)
	{
		//Added new time stamp for speaker if needed
		if(!bgmTimestamps.ContainsKey(speaker.Clip)) {bgmTimestamps.Add(speaker.Clip, speaker.Source.time);}
		//Save the time stamp of clip current playing on speaker
		bgmTimestamps[speaker.Clip] = speaker.Source.time;
	}

	void Update()
	{
		//Timing for exit if has
		if(exitTimer > 0 && hasExit)
		{
			exitTimer -= Time.deltaTime;
			//Slowly decrease exit bgm volume 
			exitSpeaker.SetSourceVolume(Mathf.Lerp(0, 1, exitTimer / exitDuration));
			return;
		}
		//Timing for enter
		if(enterTimer > 0)
		{
			//Stop exit speaker source
			if(hasExit) exitSpeaker.Source.Stop();
			enterTimer -= Time.deltaTime;
			//Slowing increase the enter bgm volume
			enterSpeaker.SetSourceVolume(Mathf.Lerp(0, 1, (enterDuration - enterTimer) / enterDuration));
		}
	}
}