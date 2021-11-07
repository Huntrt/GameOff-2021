using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
	public bool generated;
	public Transform frameGroup, sectionGroup;
	[Header("Frame")]
	//Frame's layer
	[HideInInspector] public int layer;
	[Tooltip("The chance of each side of an frame\nable to create another frame")]
	public float generatedRate;
	public GameObject framePrefab;
	public int createdFrame;
	//All the frame has create
	public List<Framer> frames;
	//All the available frame
	List<Framer> availableFrame;
	[Header("Section")]
	[Tooltip("All the section variants")]
	public List<GameObject> variants;
	[Tooltip("The min max and raw amount of section to create")]
	public IntMinMax amount;
	[Tooltip("All the current section")]
	public List<GameObject> sections;
	public GameObject start, shop;
	public GameObject border;

	void Awake()
	{
		//Get the frame layer
		layer = (1 << (LayerMask.NameToLayer("Frame")));
		//Create the list of frame
		frames = new List<Framer>();
		//The amount of section to create by randomize min and max 
		amount.raw = Random.Range(amount.min, amount.max+1);
	}

	void Start()
	{
		//Start generation when begin
		StartGeneration();
	}

	public void StartGeneration()
	{
		//Create the FIRST frame at the map position with no rotation and group it up
		Instantiate(framePrefab, transform.position, Quaternion.identity).transform.parent = frameGroup;
	}

	public void CompleteGenerated()
	{
		//! For somereason need to check if has generated cause special section will run twitch if not
		//! Casue by the frame count and createdFrame counter?
		//If this the first time generated
		if(!generated)
		{
			//All frame are now available
			availableFrame = frames;
			//Begin spawn special section first
			SpawnSpecialSection();
		}
		//Then begin spawn section
		SpawnSection();
		//Blocking off section side that is empty
		foreach (Framer frame in frames) {frame.BlockSection();}
		//Has complete generation
		generated = true;
	}

	//? Frame that has no section will getting an random one
	void SpawnSection()
	{
		//For each of the frame
		foreach (Framer frame in frames)
		{
			//If the frame has no section
			if(frame.section == null)
			{
				//Random chose an section variant
				GameObject variant = variants[Random.Range(0, variants.Count)];
				//Create the section variant at this frame position with no rotation then group it up
				Instantiate(variant,frame.transform.position,Quaternion.identity).transform.parent = sectionGroup;
				//Set this frame section to be the variant
				frame.section = variant;
			}
		}
	}

	//? Will find an available frame to assign special section
	void SpawnSpecialSection()
	{
		//! NEED TO HAS ENOUGH FRAME FOR SPECIAL SECTION	
		//Save the empty quaternion
		Quaternion q = Quaternion.identity;

		///Create START SECTION
		//The index of start frame that has been randomnly chose
		int startFrame = Random.Range(0, availableFrame.Count);
		//Create the start section at random available frame with no rotation then group it up
		Instantiate(start, availableFrame[startFrame].transform.position, q).transform.parent = sectionGroup;
		//Set that frame section to be start 
		availableFrame[startFrame].section = start;
		//Block this frame selection and this frame are longer available
		availableFrame[startFrame].BlockSection(); availableFrame.RemoveAt(startFrame);

		///Create SHOP SECTION
		//The index of shop frame that has been randomnly chose
		int shopFrame = Random.Range(0, availableFrame.Count);
		//Create the shop section at random available frame with no rotation then group it up
		Instantiate(shop, availableFrame[shopFrame].transform.position, q).transform.parent = sectionGroup;
		//Set that frame section to be shop 
		availableFrame[shopFrame].section = shop;
		//Block this frame selection and this frame are longer available
		availableFrame[shopFrame].BlockSection(); availableFrame.RemoveAt(shopFrame);
	}
}