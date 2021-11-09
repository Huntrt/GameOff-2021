using System.Collections.Generic;
using UnityEngine;
using System;

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
	public List<GameObject> sections;
	[Tooltip("All the section variants")]
	public List<GameObject> variants;
	[Tooltip("All the special section")]
	public List<GameObject> specials;
	public GameObject border;
	///
	[Header("[FRAME]-------------------------------------------------")] 
	public GameObject framePrefab;
	[HideInInspector] public int createdFrame;
	//All the frame has create
	public List<Framer> frames;
	//All the available frame
	List<Framer> availableFrame;
	int generateProgress; bool failGenerate;
	[Header("[STATUS]------------------------------------------------")]
	[SerializeField] GameObject groupPrefab;
	public bool hasGenerated, hasPopulated;
	[Tooltip("The map size using the highest axis between all section")]
	public Vector2 mapSize;
	[HideInInspector] public Transform Fgroup, Sgroup, Bgroup;
	[HideInInspector] public event Action generated, populated;

	void Awake()
	{
		//New frams list and section list
		frames = new List<Framer>(); sections = new List<GameObject>();
		//The amount of section to create by randomize min and max if the amount has not set
		if(amount.raw == 0) amount.raw = UnityEngine.Random.Range(amount.min, amount.max+1);
	}

	void Start()
	{
		//Start generation when begin
		StartGeneration();
	}

	void Update()
	{
		//If frame has not generate
		if(!hasGenerated)
		{
			//If haven't FAIL frame generation while generate progress has stopped when generated enough frame
			if(!failGenerate && frames.Count == generateProgress && createdFrame < amount.raw)
			{
				//% Print an error
				//Debug.LogError("Fail to generated frame. Restarting...");
				//Has failed frame generation
				failGenerate = true;
				//Start frame generation again
				StartGeneration();
			}
			//Update generating progress
			generateProgress = frames.Count;
		}
	}

	///Begin frame generation
	public void StartGeneration()
	{
		//Reset created frame counter, map and node size
		createdFrame -= createdFrame; mapSize = Vector2.zero;
		//If has failed generating
		if(failGenerate)
		{
			//Destroy the section group, frame group and border group in scene
			Destroy(Fgroup.gameObject); Destroy(Sgroup.gameObject); Destroy(Bgroup.gameObject);
			//Reset the frames list
			frames.Clear(); frames = new List<Framer>();
			//Reset the sections list
			sections.Clear(); sections = new List<GameObject>();
		}
		//Create an new the frame group in scene
		Fgroup = Instantiate(groupPrefab, transform.position, Quaternion.identity).transform;
		//Create an new the section group in scene
		Sgroup = Instantiate(groupPrefab, transform.position, Quaternion.identity).transform;
		//Create an new the border group in scene
		Bgroup = Instantiate(groupPrefab, transform.position, Quaternion.identity).transform;
		//Set frame, section and border group as the children of map
		Fgroup.parent = transform; Sgroup.parent = transform; Bgroup.parent = transform;
		//Rename frame, section and border group
		Fgroup.name = "Frame Group"; Sgroup.name = "Section Group"; Bgroup.name = "Border Group";
		//Create the FIRST frame at the map position with no rotation and group it up
		Instantiate(framePrefab, transform.position, Quaternion.identity).transform.parent = Fgroup;
		//Has yet to fail frane generation
		failGenerate = false;
	}

	///When frame has completed generation
	public void CompleteGeneration()
	{
		//All frame are now available
		availableFrame = frames;
		//Has complete frame generation
		hasGenerated = true; generated?.Invoke();
		//Begin fill all the frame with section
		PopulateFrame();
		//Blocking off all the section side that is empty
		foreach (Framer frame in frames) {frame.BlockSection();}
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
		///Randomly chose an available frame to has special section
		foreach (GameObject special in specials) {SpecialSections(special);}
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
		//For each of the available frame
		foreach (Framer frame in availableFrame)
		{
			//Randomly chose an section variant
			GameObject variant = variants[UnityEngine.Random.Range(0, variants.Count)];
			//Create chosed section variant at this frame position with no rotation
			GameObject created = Instantiate(variant, frame.transform.position, Quaternion.identity);
			//Group the section up
			created.transform.parent = Sgroup.transform;
			//Add this section to created list
			sections.Add(created);
			//Set this frame section to be the variant
			frame.section = variant;
		}
	}

	void GetMapSize()
	{
		//For each of the section in sections list
		foreach (GameObject section in sections)
		{
			//Turn this section X axis into postive
			float x = Mathf.Abs(section.transform.position.x);
			//Turn this section Y axis into postive
			float y = Mathf.Abs(section.transform.position.y);
			//Map x & y are now the highest axis if it an new record
			if(x > mapSize.x) {mapSize.x = x;} if(y > mapSize.y) {mapSize.y = y;}
		}
		///Fill the entire map with grid graph
		//Get the node amount by get the map size divide it with section size then multiple 
		//with how many node to fill an section
		Vector2 nodes = new Vector2((mapSize.x/sectionSize) * nodeFill, (mapSize.y/sectionSize) * nodeFill);
		//Get the amount of node need create by double? node amount with half of an srction
		Vector2 graph = new Vector2(nodes.x*2 + nodeFill/2, nodes.y*2 + nodeFill/2);
		//Update the grid graph amount and size
		Manager.i.path.data.gridGraph.SetDimensions((int)graph.x, (int)graph.y, nodeSize);
	}
}