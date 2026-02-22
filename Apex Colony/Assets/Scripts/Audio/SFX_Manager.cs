using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
	#region Set this class to singleton
	static SFX_Manager _i; public static SFX_Manager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindFirstObjectByType<SFX_Manager>();
				if(_i == null)
				{
					Debug.LogWarning("Missing an gameobject that hold SFX_Manager singleton (Make sure you don't access singleton when it got destroy)");
				}
			}
			return _i;
		}
	}
	#endregion

	[SerializeField] SFX_Library library;
	Dictionary<string, SFX_Library.SFX> sfxDataHash;

	void Update()
	{
		//Counting down sfx cooldown
		foreach (SFX_Library.SFX sfxData in library.Sfxs) if(!sfxData.isReady) sfxData.Timing();
	}

	public static void PlaySFX(string sfxName)
	{
		//Hash the sfx if haven't
		if(i.sfxDataHash == null)
		{
			i.sfxDataHash = new();
			foreach (SFX_Library.SFX sfx in i.library.Sfxs) i.sfxDataHash.Add(sfx.name, sfx);
		}

		SFX_Library.SFX targetSfx = i.sfxDataHash[sfxName];

		if(targetSfx.isReady)
		{
			Audio_Manager.i.Play(targetSfx.clips[Random.Range(0, targetSfx.clips.Length)], "SFXVol");
			targetSfx.ResetCooldown();
		}
	}
}
