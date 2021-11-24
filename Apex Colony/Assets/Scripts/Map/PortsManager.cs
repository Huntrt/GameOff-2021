using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class ItemData 
{
	public GameObject item;
	public SpriteRenderer icon;
	public int cost;
	[TextArea(5,100)] public string info;
}

public class PortsManager : MonoBehaviour
{
	public FloatMinMax launchForce;
	public float itemDrag;

	//Getting randomize force
	public float Force() {return Random.Range(launchForce.min, launchForce.max);}

	float totalRatio; public GameObject ItemDrop()
	{
		//Get the level manager then get ity current level's item drop data
		LevelManager lvm = Manager.i.level; List<DropData> items = lvm.levels[lvm.lv].item;
		//Reset the total ratio
		totalRatio -= totalRatio;
		//Get the total ratio of all item drop
		foreach (DropData i in items) {totalRatio += i.ratio;}
		//The chance randomly got from zero to total ratio
		float chance = UnityEngine.Random.Range(0, totalRatio);
		//Go throught all the item drop in list
		for (int d = items.Count - 1; d >= 0 ; d--)
		{
			//Send the current item's object when it ratio took all the chance
			if((chance - items[d].ratio) <= 0) {return items[d].obj;}
			//Decrease the chance if the egg ratio are higher than it
			else {chance -= items[d].ratio;}
		}
		//Not important
		return null;
	}
}

//? How ports work:
// When [Port] got interact it will call to [manager] to generate 3 random [item]
// Each [item] will has pickupable component to contain info about the [item]
// [Port] will save all the data inside pickupable component of [item] it has got for itself
// [Port] will begin to call to display the datas it contain to [port slot]
// [Port] will also assign the [item] to spawn at the [port slot] button
// [Port slot] will handle the process of diplay info it got to text

// [Port] = Storing the item data it got and display them when nedded
// [Port slot] = Displaying the item data from [port] that got click
// [item] = Individual data of item
// [Manager] = Randomly chose item from current level for [port] storing