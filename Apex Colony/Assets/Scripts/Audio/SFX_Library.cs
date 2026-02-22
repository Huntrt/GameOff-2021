using UnityEngine;

[CreateAssetMenu(fileName = "SFX_Library", menuName = "SFX_Library", order = 0)]
public class SFX_Library : ScriptableObject
{
	[System.Serializable] public class SFX
	{
		public string name;
		[SerializeField] float cooldown;
		public AudioClip[] clips;
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

	[SerializeField] SFX[] sfxs;

	public SFX[] Sfxs {get => sfxs;}
}
