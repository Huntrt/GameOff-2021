using UnityEngine;
using System;

public class Manager : MonoBehaviour
{
	public Formator formator;
	public GoalsCreate goal;
	public Controls control;
	public EnemyManager enemy;
	public Maps map;
	public CameraManager cam;
	public AlliesManager allie;
	public LevelManager lv;
	public AstarPath path;
	///When the level start
	public event Action stared;
	[Serializable] public class Layers {public int enemy, frame;}
	//All the needed layer
	public Layers layer;
	public static Manager i; 

	void Awake()
	{
		//Get the frame layer
		layer.frame = (1 << (LayerMask.NameToLayer("Frame")));
		//Get the enemy layer
		layer.enemy = (1 << (LayerMask.NameToLayer("Enemy")));
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
