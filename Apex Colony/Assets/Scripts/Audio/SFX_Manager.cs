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

	[System.Serializable] class SfxData
	{
		public string sfxName;
		public AudioClip[] clips;
		[SerializeField] float cooldown;
		float cooldownTimer;
		public bool isReady {get => cooldownTimer <= 0;}

		public void Timing()
		{
			cooldownTimer -= Time.deltaTime;
		}

		public void ResetCooldown()
		{
			cooldownTimer = cooldown;
		}
	}

	[SerializeField] SfxData[] sfxDatas;
	Dictionary<string, SfxData> sfxDataHash;

	void Update()
	{
		//Counting down sfx cooldown
		foreach (SfxData sfxData in sfxDatas) if(!sfxData.isReady) sfxData.Timing();
	}

	public static void PlaySFX(string sfxName)
	{
		//Hash the sfx if haven't
		if(i.sfxDataHash == null)
		{
			i.sfxDataHash = new();
			foreach (SfxData sfxData in i.sfxDatas) i.sfxDataHash.Add(sfxData.sfxName, sfxData);
		}

		SfxData sfx = i.sfxDataHash[sfxName];

		if(sfx.isReady)
		{
			Audio_Manager.i.Play(sfx.clips[Random.Range(0, sfx.clips.Length)], "SFXVol");
			sfx.ResetCooldown();
		}
	}
}
