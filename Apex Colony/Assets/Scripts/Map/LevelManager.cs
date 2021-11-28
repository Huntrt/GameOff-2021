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
		[Tooltip("List of all the enemy spawn on this level")]
		public List<DropData> spawns;
		[Tooltip("List of all allies could spawn from egg")]
		public List<DropData> egg;
		[Tooltip("List of all the item could get from port")]
		public List<DropData> item;
	}

public class LevelManager : MonoBehaviour
{
	[Tooltip("The amount of each map per level")] 
    [SerializeField] int mapPerLevel; public int lv;
	int currentMap;
	[Tooltip("How many percent of total enemy needed \nto be kill to progress next level")]
	public float progressRequired; public float processHold; float processCounter;
	public int killCount, killNeeded; int totalEnemy; [HideInInspector] public bool completed;
	[Tooltip("List of the all level and their content")] 
	public List<Level> levels;
	[Header("Interface")]
	public GameObject progressPanel; public GameObject processPanel;
	public TextMeshProUGUI progressCount;
	public Image progressBar, processBar;
	Maps map; EnemyManager enemy; Manager manager;
	public event Action nexting;

	void Start()
	{
		//Get the manager then get map and enemy manager
		manager = Manager.i; map = manager.map; enemy = manager.enemy;
		//Begin the first map
		NextMap();
	}

	void Update()
	{
		//If the game has start
		if(manager.started)
		{
			//Fill the bar from the percent of kill count and needed
			progressBar.fillAmount = (float)killCount/(float)killNeeded;
			//Display the count of enemy has been killed and enemy need to kill
			progressCount.text = killCount + "/" + killNeeded + " kill needed for the next level";
			//If has kill enough or more enemy needed
			if(killCount >= killNeeded)
			{
				//Display new text when kill enough enemy
				progressCount.text = "Hold [Enter] to proceed the next level";
				//Show the process panel when PRESS the process key
				if(Input.GetKeyDown(KeyCode.Return)) {processPanel.SetActive(true);}
				//When HOLDING the process key
				if(Input.GetKey(KeyCode.Return))
				{
					//Increase the process counter
					processCounter += Time.deltaTime;
					//Display the holding process
					processBar.fillAmount = processCounter/processHold;
					//Go to the next map, clear hold process and kill count if has hold long enough
					if(processCounter >= processHold) {killCount = 0; ClearHoldProcess(); NextMap();}
				}
				//Clear the process when RELEASE the process key
				if(Input.GetKeyUp(KeyCode.Return)) {ClearHoldProcess();}
				//Has complete the map
				completed = true;
			}
		}
	}

	void ClearHoldProcess()
	{
		//Hide the process panel 
		processPanel.SetActive(false);
		//Reset the process counter
		processCounter -= processCounter;
		//Reset the process bar
		processBar.fillAmount = 0;
	}

	public void BeginProgression()
	{
		//Print error incase of progress required are higher than 100%
		if(progressRequired > 100) {Debug.LogError("progressRequired are now allow to go above 100%");}
		//Show the progress panel
		progressPanel.SetActive(true);
		//Get the total amount of enemy 
		totalEnemy = enemy.enemiesObj.Count;
		//Get amount of kill needed from total enemy using percent
		killNeeded = (int)((progressRequired * totalEnemy)/100);
	}

	public void NextMap()
	{
		///Game over when current level reach the total level count
		if(lv == levels.Count-1) {print("Game Over"); return;}
		//Deactive all the allies object in allies object manager
		foreach (GameObject a in manager.allie.alliesObj) {a.SetActive(false);}
		//Go to the next map then clear enemy
		currentMap++; enemy.ClearEnemy();
		//Go the next level when complete the required amount of map per level
		if(currentMap > mapPerLevel) {currentMap = 1; lv++;}
		//Update the camera background color to be the current level background color
		Camera.main.backgroundColor = levels[lv].backgroundColor;
		//Game has not start and map has not complete
		manager.started = false; completed = false;
		//Hide the progress panel
		progressPanel.SetActive(false);
		//Hide the food panel
		Foods.i.foodPanel.SetActive(false);
		//Closing the egg panel if it open
		if(manager.eggsPanel.isActiveAndEnabled) {manager.eggsPanel.close.onClick.Invoke();}
		//Closing the port panel if it open
		if(manager.portsPanel.isActiveAndEnabled) {manager.portsPanel.close.onClick.Invoke();}
		//Call the event that are go to the next map
		nexting?.Invoke();
		//Begin map generation
		map.StartGeneration();
	}
}