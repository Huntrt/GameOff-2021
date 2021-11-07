using System.Collections.Generic; 
using UnityEngine;
using System;

public class GoalsCreate : MonoBehaviour
{
	//Offset between each goal and the size of the formation
	[SerializeField] Vector2 offset, size;
	[SerializeField] GameObject goal; [SerializeField] Transform group;
	public List<GameObject> goals = new List<GameObject>();
	public event Action generated; 

	public void Create(Vector2 clickPosition)
	{
		//The amount of the object has the "Allies" tag are mark as follower
		int followers = GameObject.FindGameObjectsWithTag("Allies").Length;
		//If the goals count are not the same as followers count
		if(goals.Count != followers)
		{
			//Destroy all the goal object inside the goals list
			foreach (GameObject goal in goals) {Destroy(goal);} 
			//Clear and create an new goal list
			goals.Clear(); goals = new List<GameObject>();
			//For each of the follower
			for (int g = 0; g < followers; g++)
			{
				//Create the instance of goal prefab 
				GameObject ins = Instantiate(goal);
				//Add that goal instance to it list and group it
				goals.Add(ins); ins.transform.parent = group;
			}
		}
		//An empty target position 
		Vector2 targetPos = Vector2.zero;
		//The counter and starting X axis
		int counter = -1; float startX = targetPos.x;
		//Get the goal's row amount by using square root with floor
		//? Floor: Get the smallest or equal int of an float (5.7f = 5 / 5.3f = 5 / -5.1 = -6 / -5.8 = 6)
		float row = Mathf.Floor(Mathf.Sqrt(goals.Count));
		//Has get the formation width?
		bool getWidth = false;
		//Go through all the goal in list
		for (int f = 0; f < goals.Count; f++)
		{
			//Increase the counter
			counter++;
			//Offset target position by the X axis IF IT IS NOT THE FIRST OBJECT
			if(f != 0) {targetPos.x += offset.x;}
			//If the counter reached row
			if(counter == row)
			{
				///Set the width size as PREVIOUS target position's X axis once
				if(!getWidth) {size.x = targetPos.x - offset.x; getWidth = true;}
				//Reset the counter and target position's X axis
				counter = 0; targetPos.x = startX;
				//Increase the target position's Y axis by offset
				targetPos.y += offset.y;
			}
			//Set the goal with the current index position to be target position
			goals[f].transform.localPosition = targetPos;
			///Set the height size as the final target position's Y axis
			if(f == goals.Count-1) {size.y = targetPos.y;}
		}
		//Move the group position to the middle of click position using half size
		group.transform.position = clickPosition - (size/2);
		//Send event when complete goal generation
		generated?.Invoke();
		//* An thank for help with alignment: https://www.youtube.com/watch?v=uTBCVgrk0lY
	}
}