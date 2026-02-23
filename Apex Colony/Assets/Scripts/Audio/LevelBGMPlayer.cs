using UnityEngine;

public class LevelBGMPlayer : MonoBehaviour
{
	[SerializeField] AudioClip[] musics;
	[SerializeField] float transitionSpeed;
	[SerializeField] BGM_Player bgm_player;
	bool populatedTransitions;

	void OnEnable()
	{
		Manager.i.map.generated += PlayBGMRandomly;
	}

	void PlayBGMRandomly()
	{
		//Populate bgm player data with level music 
		if(!populatedTransitions)
		{
			foreach (AudioClip music in musics)
			{
				bgm_player.AddTransition(music, transitionSpeed, transitionSpeed);
			}
			populatedTransitions = true;
		}

		//Chose an random track for level
		BGM_Manager.i.PlayBGM(musics[Random.Range(0, musics.Length)], transitionSpeed, transitionSpeed, false);
	}

	void OnDisable()
	{
		if(Manager.i != null) Manager.i.map.generated -= PlayBGMRandomly;
	}
}
