using System.Collections.Generic;
using UnityEngine.Audio;
using Game.Settings;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
	#region Set this class to singleton
	static Audio_Manager _i; public static Audio_Manager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindFirstObjectByType<Audio_Manager>();
			}
			return _i;
		}
	}
	#endregion

	[SerializeField] Setting_Audio setting; //! Not sure accessing mixer group this way is good
	[Tooltip("Each audio clip will have it own speaker")]
	[SerializeField] List<Speaker> speakers = new List<Speaker>(); public List<Speaker> Speakers {get => speakers;}
	[SerializeField] Dictionary<AudioClip,int> speakerClipHash = new Dictionary<AudioClip, int>();
	[SerializeField] Dictionary<string,int> speakerNameHash = new Dictionary<string, int>();

	[System.Serializable] public class Speaker
	{
		[SerializeField] Setting_Audio.Adjuster adjuster; public Setting_Audio.Adjuster Adjuster {get => adjuster;}
		[SerializeField] AudioClip clip; public AudioClip Clip {get => clip;}
		[SerializeField] AudioMixerGroup mixerGroup; public AudioMixerGroup VolumeParameter {get => mixerGroup;}
		public bool loop = false; public bool playOnAwake = false;
		AudioSource source; public AudioSource Source {get => source;}
		GameObject sourceObj;
		[HideInInspector] Transform speakerParent;

		//Create an new speaker for an clip in an mixer group
		public Speaker(AudioClip clip, AudioMixerGroup mixerGroup, Transform speakerParent) 
		{
			this.clip = clip;
			this.mixerGroup = mixerGroup;
			this.speakerParent = speakerParent;
		}

		///<summary>Directly set the volume of speaker's audio source (mainly use for change single audio clip volume)</summary>
		public void SetSourceVolume(float volume) {source.volume = volume;}

		public void Setup()
		{
			//Stop if the source already setup
			if(source != null) return;
			//Get the adjuster of this mixer group
			adjuster = i.setting.GetAdjuster(mixerGroup);

			//Create an new object for speaker with name of it clip (Speaker - Loop/Awake - walk)
			sourceObj = new GameObject("Speaker - " + mixerGroup + (loop ? "Loop":"") + (playOnAwake ? "/Awake":"") + " - " + clip.name);
			sourceObj.transform.SetParent(speakerParent);
			source = sourceObj.AddComponent<AudioSource>();

			//Apply source loop if needed
			source.loop = loop;
			//Make the source use mixer group from adjuster
			source.outputAudioMixerGroup = adjuster.MixerGroup;
		}

		public void DestroySource()
		{
			//Destroy the audio source object if it already setup
			if(sourceObj != null) Destroy(sourceObj);
		}

		public void Play(bool loop = false)
		{
			this.loop = loop;
			//Setup only run once when first time play
			Setup();
			//Play the clip of this speaker
			source.PlayOneShot(clip);
		}
	}

	void OnValidate() 
	{
		SetupHash();
	}

	void Awake() 
	{
		setting.BuildUI();
		SetupHash();
		//Go through all the speaker
		foreach (Speaker speaker in speakers) 
		{
			//Setup the all existing speaker
			speaker.Setup();
			//Auto play the speaker if enable
			if(speaker.playOnAwake) speaker.Play(speaker.loop);
		}
	}
	
	void SetupHash()
	{
		//Clear both hash
		speakerNameHash.Clear();
		speakerClipHash.Clear();
		//? Hash both name and clip itself for PREMADE speaker (when add in inspector)
		for (int i = 0; i < speakers.Count; i++)
		{
			speakerNameHash.Add(speakers[i].Clip.name, i);
			speakerClipHash.Add(speakers[i].Clip, i);
		}
	}

	/// <summary>Will create an speaker to play an audio clip from an volume parameter</summary>
	public void Play(AudioClip clip, string volumeParameter, bool loop = false) 
	{
		//? Dynamicly create an new speaker
		if(!speakerClipHash.ContainsKey(clip)) 
		{
			//Create an new speaker then set it mixer group to the one of given exposed parameter
			speakers.Add(new Speaker(clip, setting.GetMixerGroup(volumeParameter), transform));
			//Hash speaker AudioClip and AudioClipName
			speakerClipHash.Add(clip, speakers.Count-1);
			speakerNameHash.Add(clip.name, speakers.Count-1);
		}
		//Play the speaker with that have given clip
		GetSpeaker(clip).Play(loop);
	}

	/// <summary>Will create an speaker to play an audio clip in given mixer group</summary>
	public void Play(AudioClip clip, AudioMixerGroup mixerGroup, bool loop = false) 
	{
		//? Dynamicly create an new speaker
		if(!speakerClipHash.ContainsKey(clip)) 
		{
			//Create an new speaker then set it mixer group to the one have given exposed parameter
			speakers.Add(new Speaker(clip, mixerGroup, transform));
			//Hash speaker AudioClip and AudioClipName
			speakerClipHash.Add(clip, speakers.Count-1);
			speakerNameHash.Add(clip.name, speakers.Count-1);
		}
		//Play the speaker with that have given clip
		GetSpeaker(clip).Play(loop);
	}

	/// <summary>Play an audio clip by name that already have an speaker</summary>
	public void Play(string clipName, bool loop = false)
	{
		//Send an error if there noi speaker for clip with given name
		if(!speakerNameHash.ContainsKey(clipName))
		{Debug.LogError("The clip '" + clipName + "' don't have an existing speaker"); return;}
		//Play the speaker with that have clip same as given name
		GetSpeaker(clipName).Play(loop);
	}

	public Speaker GetSpeaker(AudioClip clip) {return speakers[speakerClipHash[clip]];}
	public Speaker GetSpeaker(string clipName) {return speakers[speakerNameHash[clipName]];}
	public Speaker GetSpeaker(int speakerIndex) {return speakers[speakerIndex];}
	public List<Speaker> GetSpeakers(string exposedParameter)
	{
		List<Speaker> categorize = new List<Speaker>();
		foreach (Speaker speaker in speakers)
		{
			if(speaker.Adjuster.ExposedParameter == exposedParameter)
			categorize.Add(speaker);
		}
		return categorize;
	}
	
	public bool CheckHashExit(AudioClip clip) {return speakerClipHash.ContainsKey(clip);}
	public bool CheckHashExit(string clipName) {return speakerNameHash.ContainsKey(clipName);}
}