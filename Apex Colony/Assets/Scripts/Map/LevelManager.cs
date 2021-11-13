using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
	// Map and level count example:
	// [Level 0] Map 1 > 2 > 3 > [Level 1] Map 1 > 2 > 3 > [Loop].
    public int currentMap, currentLevel;
	[Tooltip("The amount of each map per level")] [SerializeField] int completeRequired;
	//Content of each level and set it base on it order in list
	[Serializable] public class Level 
	{
		public string name;
		public Color backgroundColor;
		public GameObject border;
		public List<GameObject> variants; 
		public List<GameObject> specials;
	}
	[Tooltip("List of the all level and their content")]
	public List<Level> levels;
	Maps map;

	void Start()
	{
		//Get the map
		map = Manager.i.map;
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
		if(currentLevel == levels.Count-1) {print("Game Over"); return;}
		//Deactive all the allies object in allies object manager
		foreach (GameObject a in Manager.i.allie.alliesObj) {a.SetActive(false);}
		//Go to the next map
		currentMap++;
		//Go the next level when complete the required amount of map
		if(currentMap > completeRequired) {currentMap = 1; currentLevel++;}
		//Clear the current map variant and specials
		map.useVariants.Clear(); map.useSpecials.Clear();
		//Update the camera background color to be the current level's background color
		Camera.main.backgroundColor = levels[currentLevel].backgroundColor;
		//Set the current map border to be the current level's map border
		map.useBorder = levels[currentLevel].border;
		//Set the current map variant to be the current level's map variant
		map.useVariants = new List<GameObject>(levels[currentLevel].variants);
		//Set the current map specials to be the current level's map specials
		map.useSpecials = new List<GameObject>(levels[currentLevel].specials);
		//Begin map generation
		map.StartGeneration();
	}
}