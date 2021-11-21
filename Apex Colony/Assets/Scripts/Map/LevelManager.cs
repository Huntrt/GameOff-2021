using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
	// Map and level count example:
	// [Level 0] Map 1 > 2 > 3 > [Level 1] Map 1 > 2 > 3 > [Loop].
    public int currentMap, lv;
	[Tooltip("The amount of each map per level")] [SerializeField] int completeRequired;
	//Content of each level and set it base on it order in list
	[Serializable] public class Level 
	{
		public string name;
		public Color backgroundColor;
		public GameObject border;
		public List<GameObject> variants; 
		public List<GameObject> specials;
		[Tooltip("List of all the enemy spawn on this level")]
		public List<DropData> spawns;
	}
	[Tooltip("List of the all level and their content")]
	public List<Level> levels;
	Maps map; EnemyManager enemy;

	void Start()
	{
		//Get the map
		map = Manager.i.map;
		//Get the enemy manager
		enemy = Manager.i.enemy;
		//Begin the first map
		NextMap();
	}


	void Update()
	{
		//% Go to the next map when click key
		if(Input.GetKeyDown(KeyCode.M)) {NextMap();}
	}

	public void NextMap()
	{
		//Game over when current level reach the total level count
		if(lv == levels.Count-1) {print("Game Over"); return;}
		//Deactive all the allies object in allies object manager
		foreach (GameObject a in Manager.i.allie.alliesObj) {a.SetActive(false);}
		//Go to the next map
		currentMap++;
		//Go the next level when complete the required amount of map
		if(currentMap > completeRequired) {currentMap = 1; lv++;}
		//Update the camera background color to be the current level
		Camera.main.backgroundColor = levels[lv].backgroundColor;
		//Begin map generation
		map.StartGeneration();
	}
}