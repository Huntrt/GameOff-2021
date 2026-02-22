"BGM_Manager" are "Don't Destroy On Load" so just place it in all scene

To play an audio clip as bgm:
	> Add "BGM_Player" to an object.
	> Add an entry transitions[]
	> Add the audio clip want play to transition.clip.
	> Config the transition.exit/enter duration to your liking.
	> To play transition run SelectTransition(clip added in transition).