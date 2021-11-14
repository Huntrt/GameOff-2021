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
	public LevelManager lv;
	public AstarPath path;
	public EggsPanel eggsPanel;
	public EggsManager eggs;
	///When the level start
	public event Action stared;
	//All the needed layer
	[Serializable] public class Layers {public int allies, enemy, frame, inter;} public Layers layer;
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
		//Make this script into singleton
		i = this;
	}

	//When allies has spawned
	public void SpawnedAllies() 
	{
		//Gams started
		stared?.Invoke();
		//Hide the generation loading when game start
		map.generationLoading.SetActive(false);
	}
}
