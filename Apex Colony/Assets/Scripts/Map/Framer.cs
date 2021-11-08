using UnityEngine;

public class Framer : MonoBehaviour
{
	[Tooltip("Go by the order UP | DOWN | LEFT | RIGHT")]
    public bool[] sides = {false, false, false, false};
	[Tooltip("The chance to create another side frame")]
    public float[] chance = {0, 0, 0, 0};
	[Tooltip("The size of an frame")]
	public float size;
	[Tooltip("the section on the object")]
	public GameObject section;
	Maps map;

	void Start()
	{
		//Get the map
		map = Manager.i.map;
		//Add this frame into map upon create
		map.frames.Add(this);
		//Generate more frame
		Generate();
		//Complete generation when this is the final frame (need +1)
		if(map.frames.Count == map.amount.raw+1) {map.CompleteGenerated();}
	}

	public void Generate()
	{
		//Scanning to check if there is frame nearby
		ScanFrame();
		//Create the up and down side frame
		CreateFrame(0, Vector2.up); CreateFrame(1, Vector2.down); 
		//Create the left and right side frame
		CreateFrame(2, Vector2.left); CreateFrame(3, Vector2.right);
	}

	void ScanFrame()
	{
		//Check the up and down side to see if there is any frame
		sides[0] = CheckFrame(Vector2.up); sides[1] = CheckFrame(Vector2.down);
		//Check the left and down right to see if there is any frame
		sides[2] = CheckFrame(Vector2.left); sides[3] = CheckFrame(Vector2.right);
	}

	bool CheckFrame(Vector2 direction)
	{
		//Cast an ray at this frame with set direction with size as length and only on frame layer
		RaycastHit2D[] checks = Physics2D.RaycastAll(transform.position, direction, size, map.layer);
		//If there is some frame got check by raycast
		if(checks.Length > 0) {foreach (RaycastHit2D check in checks)
		//If the checked frame are not this then this side already has frame
		{if(check.collider.gameObject != gameObject) {return true;}}}
		//There is no side frame is hit nothing
		return false;
	}

	//The chance to create another
	float GetChance() {return Random.Range(0, 100);}

	void CreateFrame(int side, Vector2 direction)
	{
		//If this side has no frame and still able to create more frame
		if(map.createdFrame < map.amount.raw && sides[side] == false)
		{
			//The chance to repeat by randomly chose
			float rate = Random.Range(0, 100);
			//Create another frame on if has an chance
			if(rate <= map.generatedRate)
			{	
				//The frame on this side has been create
				sides[side] = true;
				//Get this side frame create position by increase this side direction with size
				Vector2 create = (Vector2)transform.position + (direction * size);
				//Create an new frame at create position with no rotation
				GameObject sideFrame = Instantiate(map.framePrefab, create, Quaternion.identity);
				//Group the side frame up
				sideFrame.transform.parent = map.Fgroup.transform;
				//Has create an frame
				map.createdFrame++;
				//% Debug line to display connection from this frame to new side frame
				Debug.DrawLine(transform.position, sideFrame.transform.position, Color.black, 1000);
			}
		}
	}

	public void BlockSection()
	{
		//Scanning again to check if there is frame nearby
		ScanFrame();
		//If there no frame upward
		if(!sides[0]) 
		{
			//Get this side frame empty position by increase this side upward with half size
			Vector2 emptySide = (Vector2)transform.position + (Vector2.up * size/2);
			//Create the border upward that rotate 0 degree
			Instantiate(map.border, emptySide, Quaternion.Euler(0,0,0));
		}
		//If there no frame downward
		if(!sides[1]) 
		{
			//Get this side frame empty position by increase this side downward with half size
			Vector2 emptySide = (Vector2)transform.position + (Vector2.down * size/2);
			//Create the border downward that rotate 180 degree
			Instantiate(map.border, emptySide, Quaternion.Euler(0,0,180));
		}
		//If there no frame leftward
		if(!sides[2]) 
		{
			//Get this side frame empty position by increase this side leftward with half size
			Vector2 emptySide = (Vector2)transform.position + (Vector2.left * size/2);
			//Create the border leftward that rotate 90 degree
			Instantiate(map.border, emptySide, Quaternion.Euler(0,0,90));
		}
		//If there no frame rightward
		if(!sides[3])
		{
			//Get this side frame empty position by increase this side rightward with half size
			Vector2 emptySide = (Vector2)transform.position + (Vector2.right * size/2);
			//Create the border rightward that rotate -90 degree
			Instantiate(map.border, emptySide, Quaternion.Euler(0,0,-90));
		}
	}
}
