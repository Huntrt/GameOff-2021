using System.Collections.Generic;
using UnityEngine;

public class EggsManager : MonoBehaviour
{
	public List<DropData> eggs;
	//The total ratio value of all egg drop
	float totalRatio;

	public GameObject EggDrop()
	{
		//Reset the total ratio
		totalRatio -= totalRatio;
		//Get the total ratio of all egg drop
		foreach (DropData e in eggs) {totalRatio += e.ratio;}
		//The chance randomly got from zero to total ratio
		float chance = Random.Range(0, totalRatio);
		//Go throught all the egg drop in list
		for (int d = eggs.Count - 1; d >= 0 ; d--)
		{
			//Send the allies in egg when it ratio took all the chance
			if((chance - eggs[d].ratio) <= 0) {return eggs[d].obj;}
			//Decrease the chance if the egg ratio are higher than it
			else {chance -= eggs[d].ratio;}
		}
		//Not important
		return null;
	}
}