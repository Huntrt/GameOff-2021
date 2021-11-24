using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

//Content of each level and it order
[Serializable] public class Level 
	{
		public string name;
		[Header("Map")]
		public Color backgroundColor;
		public GameObject border;
		[Tooltip("The chance of how many content section will spawn (0-100)%")]
		public float contentRate;
		public List<GameObject> empty; 
		public List<GameObject> content;
		public List<GameObject> specials;
		[Header("Enemies")]
		[Tooltip("List of all the enemy spawn on this level")]
		public List<DropData> spawns;
		[Header("Allies")]
		[Tooltip("List of all allies could spawn from egg")]
		public List<DropData> egg;
		[Tooltip("List of all the item could get from port")]
		public List<DropData> item;
	}

public class LevelManager : MonoBehaviour
{
	[Tooltip("The amount of each map per level")] 
    [SerializeField] int mapPerLevel; public int lv;
	int currentMap; public bool completed;
	[Tooltip("How many percent of total enemy needed \nto be kill to progress next level")]
	public float progressRequired;
	public int killCount, killNeeded;
	[Tooltip("List of the all level and their content")] 
	public List<Level> levels;
	[Header("Interface")]
	public TextMeshProUGUI progressCount;
	public Image progressBar;
	Maps map; EnemyManager enemy;

	void Start()
	{
		//Get the map
		map = Manager.i.map;
		//Get the enemy manager
		enemy = Manager.i.enemy;
		//Begin progression after map started
		Manager.i.stared += BeginProgression;
		//Begin the first map
		NextMap();
	}

	void Update()
	{
		//% Go to the next map when click key
		if(Input.GetKeyDown(KeyCode.M)) {NextMap();}
	}

	void BeginProgression()
	{
		//Print error incase of progress required are higher than 100%
		if(progressRequired > 100) {Debug.LogError("progressRequired are now allow to go above 100%");}
		//Get amount of kill needed from total enemy using percent
		killNeeded = (int)((progressRequired * enemy.enemiesObj.Count)/100);
	}

	public void NextMap()
	{
		//Game over when current level reach the total level count
		if(lv == levels.Count-1) {print("Game Over"); return;}
		//Deactive all the allies object in allies object manager
		foreach (GameObject a in Manager.i.allie.alliesObj) {a.SetActive(false);}
		//Go to the next map then clear enemy
		currentMap++; enemy.ClearEnemy();
		//Go the next level when complete the required amount of map
		if(currentMap > mapPerLevel) {currentMap = 1; lv++;}
		//Update the camera background color to be the current level
		Camera.main.backgroundColor = levels[lv].backgroundColor;
		//Begin map generation
		map.StartGeneration();
	}
}