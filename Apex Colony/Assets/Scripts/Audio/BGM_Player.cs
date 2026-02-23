using System.Collections.Generic;
using UnityEngine;

public class BGM_Player : MonoBehaviour
{
	public int defaultTransition = -1;
	[SerializeField] List<Transition> transitions = new();
	[SerializeField] AudioClip[] levelsBGM;
	[SerializeField] float levelBGMTransitionSpeed;
	bool populatedTransitionsWithLevelBGM;

	[System.Serializable] public class Transition
	{
		[SerializeField] AudioClip clip; public AudioClip Clip {get => clip; set => clip = value;}
		public float exitDuration, enterDuration;
		[Tooltip("When this exit it will continue whatever enter transition left off")]
		[SerializeField] bool continueWhenExit; public bool ContinueWhenExit {get => continueWhenExit;} 
	}
	
	void Start()
	{
		if(defaultTransition >= 0) SelectTransition(transitions[defaultTransition].Clip);
	}

	public void SelectTransition(AudioClip audioClip)
	{
		//Find the transition that hae the same audio clip as selection
		foreach (Transition transition in transitions) if(transition.Clip == audioClip)
		{
			//Play the transtion along with it data to bgm
			BGM_Manager.i.PlayBGM(transition.Clip, transition.exitDuration, transition.enterDuration, transition.ContinueWhenExit);
			return;
		}
		Debug.LogWarning("There are no transition data for '" + audioClip.name + "' audio clip");
	}

	public void PlayLevelBGM()
	{
		//Auto populate transition data with level bgm since it generic
		if(!populatedTransitionsWithLevelBGM)
		{
			foreach (AudioClip bgm in levelsBGM)
			{
				Transition transition = new();
				transition.Clip = bgm;
				transition.exitDuration = levelBGMTransitionSpeed;
				transition.enterDuration = levelBGMTransitionSpeed;
				transitions.Add(transition);
			}

			populatedTransitionsWithLevelBGM = true;
		}
	}
}