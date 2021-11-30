using UnityEngine;
using System;

public class Manager : MonoBehaviour
{
	[Header("Component")]
	public Formator formator;
	public GoalsCreate goal;
	public Controls control;
	public EnemyManager enemy;
	public Maps map;
	public CameraManager cam;
	public AlliesManager allie;
	public LevelManager level;
	public AstarPath path;
	public EggsPanel eggsPanel;
	public EggsManager eggs;
	public PortsManager ports;
	public PortsPanel portsPanel;
	public IndicatorManager indi;
	///When the level start
	public event Action starting; public bool started;
	//All the needed layer
	[Serializable] public class Layers {public int allies, enemy, frame, inter, pick;} public Layers layer;
	public static Manager i; 

	void Awake()
	{
		//Get the allies layer
		layer.allies = (1 << (LayerMask.NameToLayer("Allies")));
		//Get the enemy layer
		layer.enemy = (1 << (LayerMask.NameToLayer("Enemy")));
		//Get the frame layer
		layer.frame = (1 << (LayerMask.NameToLayer("Frame")));
		//Get the interactable layer
		layer.inter = (1 << (LayerMask.NameToLayer("Interactable")));
		//Get the pickupable layer
		layer.pick = (1 << (LayerMask.NameToLayer("Pickupable")));
		//Make this script into singleton
		i = this;
	}

	//Game started when spawner has spawn their enemy
	public void StartingGame() 
	{
		//Game started
		starting?.Invoke(); started = true;
		//Reset the camera
		cam.ResetCamera();
		//Begin the current map progression
		level.BeginProgression();
		//Flash food displap
		Foods.i.Gain(0);
		//Hide the generation loading when game start
		map.generationLoading.SetActive(false);
	}
}
