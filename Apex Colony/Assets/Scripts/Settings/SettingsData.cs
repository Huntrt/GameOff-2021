using UnityEngine;

[System.Serializable] public class SettingsData
{
	[Tooltip("Enable v-sync")]
	public bool vSync;
	[Tooltip("Full screen option for game\nExclusive Full Screen: Special fullscreen mode only work on 'Window'\nFull Screen Window: Are Borderless (toggle)\nMaximized Window: Windowed got maximized\nWindowed: Normal windowed (toggle)")]
	public FullScreenMode fullScreenMode;
	[Tooltip("The game window resolution")]
	public Vector2Int resolution;
	[Tooltip("The max fps game allow to run\n0 = Unlimited")]
	public int fpsCap;
	[Tooltip("Game quality level (Project quality)")]
	public int graphicQuality;
	[Tooltip("Volumes for audio\nOrder of which audio use are in Setting_Audio.cs")]
	public int[] volumes;
	[Tooltip("Keybinding game use")]
	public Keybind.Bind[] binds;
	[Tooltip("How fast camera will move when press key")]
	public float cameraMoveSpeed;
	[Tooltip("How fast will zoom in and out with scroll wheel")]
	public float zoomSpeed;
}