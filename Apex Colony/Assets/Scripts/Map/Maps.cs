using System.Collections.Generic;
using UnityEngine;
using System;

public class Maps : MonoBehaviour
{
	[SerializeField] GameObject frameGroup, sectionGroup;
	[HideInInspector] public Transform Fgroup, Sgroup;
	[HideInInspector] public bool hasGenerated, hasPopulated;
	[HideInInspector] public event Action generated, populated;
	int generatedStatus; bool beginGeneration;
	//Frame's layer
	[HideInInspector] public int layer;
	[Header("[SETTING]")]
	[Tooltip("The min max and raw amount of section to create")]
	public IntMinMax amount;
	[Tooltip("The chance of each side of an frame\nable to create another frame")]
	public float generatedRate;
	[Header("[FRAME]")] public GameObject framePrefab;
	[HideInInspector] public int createdFrame;
	//All the frame has create
	public List<Framer> frames;
	//All the available frame
	List<Framer> availableFrame;
	[Header("[SECTION]")] [Tooltip("All the created section")]
	public List<GameObject> sections;
	[Tooltip("All the section variants")]
	public List<GameObject> variants;
	[Tooltip("All the special section")]
	public List<GameObject> specials;
	public GameObject border;

	void Awake()
	{
		//Get the frame layer
		layer = (1 << (LayerMask.NameToLayer("Frame")));
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
		//If has not generate frame
		if(!hasGenerated)
		{
			//If has not begin generation when stopoed generate frame but generated frame not enough
			if(!beginGeneration && frames.Count == generatedStatus && createdFrame < amount.raw)
			{
				//% Print an error
				Debug.LogError("Fail to generated frame. Restarting...");
				//Has begin generation
				beginGeneration = true;
				//Start map generation again
				StartGeneration();
			}
			//Update generated status
			generatedStatus = frames.Count;
		}
	}

	public void StartGeneration()
	{
		//Reset created frame counter
		createdFrame -= createdFrame;
		//If currently generating
		if(beginGeneration)
		{
			//Destroy the section and frame group in scene
			Destroy(Fgroup.gameObject); Destroy(Sgroup.gameObject);
			//Reset the frames list
			frames.Clear(); frames = new List<Framer>();
			//Reset the sections list
			sections.Clear(); sections = new List<GameObject>();
		}
		//Create the frame group in scene
		Fgroup = Instantiate(frameGroup, transform.position, Quaternion.identity).transform;
		//Create the section group in scene
		Sgroup = Instantiate(sectionGroup, transform.position, Quaternion.identity).transform;
		//Set both group as the children of map
		Fgroup.parent = transform; Sgroup.parent = transform;
		//Create the FIRST frame at the map position with no rotation and group it up
		Instantiate(framePrefab, transform.position, Quaternion.identity).transform.parent = Fgroup;
		//No longer begin generation
		beginGeneration = false;
	}

	public void CompleteGenerated()
	{
		//All frame are now available
		availableFrame = frames;
		//Has complete generation
		hasGenerated = true; generated?.Invoke();
		//Begin fill all the frame with section
		PopulateFrame();
		//Blocking off all the section side that is empty
		foreach (Framer frame in frames) {frame.BlockSection();}
		//Has complete population
		hasPopulated = true; populated?.Invoke();
	}

	public void PopulateFrame()
	{
		///Randomly chose an available frame to has special section
		foreach (GameObject special in specials) {SpecialSections(special);}
		///Fill the rest of the available frame with randomly chose section variant
		VariantSections();
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
		//Block of this frame section
		availableFrame[index].BlockSection();
		//The frame got are now has the special section
		availableFrame[index].section = created;
		//Group the section up
		created.transform.parent = Sgroup.transform;
		//The frame got are no longer available
		availableFrame.RemoveAt(index);
	}
}