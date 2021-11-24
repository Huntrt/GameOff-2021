using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
	[Header("[SETTING]-----------------------------------------------")] 
	[Tooltip("The min max and raw amount of section to create")]
	public IntMinMax amount;
	[Tooltip("The chance of each side of an frame\nable to create another frame")]
	public float generatedRate;
	[Tooltip("The size of an single node (recommend 0.4)")]
	public float nodeSize;
	[Tooltip("The amount of node need to fill an section (recommend 13)")]
	public float nodeFill;
	[Tooltip("Size of an section")]
	public float sectionSize;
	[Header("[SECTION]-----------------------------------------------")] 
	[Tooltip("All the created section")]
	public List<GameObject> createdSections;
	///
	[Header("[FRAME]-------------------------------------------------")] 
	public GameObject framePrefab;
	[HideInInspector] public int createdFrame;
	[Tooltip("All the created frame")]
	public List<Framer> createFrames;
	//All the available frame
	List<Framer> availableFrame;
	int generateProgress; bool failGenerate;
	[Header("[STATUS]------------------------------------------------")]
	public GameObject generationLoading;
	[SerializeField] GameObject groupPrefab;
	public bool hasGenerated, hasPopulated;
	[Tooltip("The map size using the highest axis between all section")]
	public Vector2 mapSize;
	///The highest and lowest point of map
	public Vector2 mapMin, mapMax;
	[HideInInspector] public Transform Fgroup, Sgroup, Bgroup, Egroup;
	[HideInInspector] public event System.Action generated, populated;
	[HideInInspector] public LevelManager lvm;

	void Awake()
	{
		//New create frame list 
		createFrames = new List<Framer>(); 
		//New availab frame list 
		availableFrame = new List<Framer>();
		//New create section list 
		createdSections = new List<GameObject>();
		//Get the level manager
		lvm = Manager.i.level;
	}

	void Update()
	{
		//If frame has not generate
		if(!hasGenerated)
		{
			//If haven't FAIL frame loading while generate progress has stopped when generated enough frame
			if(!failGenerate && createFrames.Count == generateProgress && createdFrame < amount.raw)
			{
				//% Print an error
				//% Debug.LogError("Fail to generated frame. Restarting...");
				//Has failed frame loading
				failGenerate = true;
				//Start frame loading again
				StartGeneration();
			}
			//Update generating progress
			generateProgress = createFrames.Count;
		}
	}

	///Begin frame loading
	public void StartGeneration()
	{
		//No longer generated map
		hasGenerated = false;
		//The amount of section to create by randomize min and max if the amount has not set
		amount.raw = UnityEngine.Random.Range(amount.min, amount.max+1);
		//Active the loading ui when start generating
		generationLoading.SetActive(true);
		//Reset created frame counter, map and node size
		createdFrame -= createdFrame; mapSize = Vector2.zero;
		//Grouping frame, section, border and enemy
		#region Grouping
			//If the frame, section. border or enemy group already existing
			if(Fgroup != null || Sgroup != null || Bgroup != null ||Egroup != null)
			{
				//Destroy frame and section group object
				Destroy(Fgroup.gameObject);Destroy(Sgroup.gameObject);
				//Destroy border and enemy group object
				Destroy(Bgroup.gameObject);Destroy(Egroup.gameObject);
			}
			//Create an new frame and section group
			Fgroup = CreateGroup(); Sgroup = CreateGroup();
			//Create an new border and enemy group
			Bgroup = CreateGroup(); Egroup = CreateGroup();
			//Set frame and section group as the children of map
			Fgroup.parent = transform; Sgroup.parent = transform; 
			//Set border and enemy group as the children of map
			Bgroup.parent = transform; Egroup.parent = transform;
			//Rename frame and section group
			Fgroup.name = "Frame Group"; Sgroup.name = "Section Group";
			//Rename border and enemy group
			Bgroup.name = "Border Group"; Egroup.name = "Enemy Group";
		#endregion
		//Reset the CREATED frames list
		createFrames.Clear(); createFrames = new List<Framer>();
		//Reset the AVAILABLE frames list
		availableFrame.Clear(); availableFrame = new List<Framer>();
		//Reset the created section list
		createdSections.Clear(); createdSections = new List<GameObject>();
		//Reset the enemy list
		Manager.i.enemy.ResetList();
		//Create the FIRST frame at the map position with no rotation and group it up
		Instantiate(framePrefab, transform.position, Quaternion.identity).transform.parent = Fgroup;
		//Has yet to fail frane loading
		failGenerate = false;
	}
	//Create group when needed
	Transform CreateGroup() {return Instantiate(groupPrefab,transform.position,Quaternion.identity).transform;}

	///When frame has completed loading
	public void CompleteGeneration()
	{
		//All frame are now available
		availableFrame = new List<Framer>(createFrames);
		//Has complete frame loading
		hasGenerated = true; generated?.Invoke();
		//Begin fill all the frame with section
		PopulateFrame();
		//Blocking off all the section side that is empty
		foreach (Framer frame in createFrames) {frame.BlockSection();}
		//Has complete section population
		hasPopulated = true; populated?.Invoke();
		//Get the size of map
		GetMapSize();
		//Scan all the path after generated frame and populated it with section
		Manager.i.path.Scan();
	}

	///Begin fill frame with section
	public void PopulateFrame()
	{
		///Randomly chose an available frame to has special section in this current level
		foreach (GameObject special in lvm.levels[lvm.lv].specials) {SpecialSections(special);}
		///Fill the rest of the available frame with randomly chose section variant
		VariantSections();
	}

	void SpecialSections(GameObject special)
	{
		//Get index of frame that has been randomnly chose
		int index = UnityEngine.Random.Range(0, availableFrame.Count);
		//Save the empty quaternion
		Quaternion q = Quaternion.identity;
		//Create the special section at the random available frame just got with no rotation
		GameObject created = Instantiate(special, availableFrame[index].transform.position, q);
		//Set that frame section to be spawn 
		availableFrame[index].section = created;
		//Block of this frame special section
		availableFrame[index].BlockSection();
		//The frame got are now has the special section
		availableFrame[index].section = created;
		//Group the section up
		created.transform.parent = Sgroup.transform;
		//The frame got are no longer available
		availableFrame.RemoveAt(index);
	}

	void VariantSections()
	{
		//Get the current levels from manager
		Level lv = lvm.levels[lvm.lv];
		//Get the current level's emtpy section
		List<GameObject> emp = lv.empty;
		//Get the current level's content section
		List<GameObject> cont = lv.content;
		//The list of section that will be use
		List<GameObject> used = new List<GameObject>();
		//For each of the available frame
		foreach (Framer frame in availableFrame)
		{
			//Use the content section if it rate HIGHER than chance, use empty section if not
			if(lv.contentRate >= Random.Range(0f, 100f)){used = cont;} else {used = emp;}
			//Randomly chose an section from the currently used section list
			GameObject section = used[Random.Range(0, used.Count)];
			//Create the chosed section at this frame position with no rotation
			GameObject created = Instantiate(section, frame.transform.position, Quaternion.identity);
			//Group the section up
			created.transform.parent = Sgroup.transform;
			//Add this section to created list
			createdSections.Add(created);
			//Set this frame section to be the variant spawned
			frame.section = section;
		}
	}

	void GetMapSize()
	{
		//For each of the section in sections list
		foreach (GameObject section in createdSections)
		{
			//The x axis of current section
			float x = section.transform.position.x;
			//The y axis of current section
			float y = section.transform.position.y;
			//Get this section position X axis if it higher than the current max axis
			if(section.transform.position.x > mapMax.x) {mapMax.x = x;}
			//Get this section position Y axis if it higher than the current max axis
			if(section.transform.position.y > mapMax.y) {mapMax.y = y;}
			//Get this section position X axis if it lower than the current min axis
			if(section.transform.position.x < mapMin.x) {mapMin.x = x;}
			//Get this section position Y axis if it lower than the current min axis
			if(section.transform.position.y < mapMin.y) {mapMin.y = y;}
			//Convert the current section X and Y to positive value
			float pX = Mathf.Abs(x); float pY = Mathf.Abs(y);
			//Map size x & y are now the highest axis if it an new record
			if(pX > mapSize.x) {mapSize.x = pX;} if(pY > mapSize.y) {mapSize.y = pY;}
		}
		///Fill the entire map with grid graph
		//Get the node amount by get the map size divide it with section size then multiple 
		//with how many node to fill an section
		Vector2 nodes = new Vector2((mapSize.x/sectionSize) * nodeFill, (mapSize.y/sectionSize) * nodeFill);
		//Get the amount of node need create by double? node amount with half of an section
		Vector2 graph = new Vector2(nodes.x*2 + nodeFill/2, nodes.y*2 + nodeFill/2);
		//Update the grid graph amount and size (+1 to prevent out of bounds section)
		Manager.i.path.data.gridGraph.SetDimensions((int)graph.x+10, (int)graph.y+10, nodeSize);
	}
}