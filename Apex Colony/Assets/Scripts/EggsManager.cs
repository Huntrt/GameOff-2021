using System.Collections.Generic;
using UnityEngine;

public class EggsManager : MonoBehaviour
{
	//Egg drop info, the allies and it ratio to drop
    [System.Serializable] public class EggDrop {public GameObject allies; public float ratio;}
	public List<EggDrop> eggs;
	//The total ratio value of all egg drop
	float totalRatio;

	public GameObject OpenEgg()
	{
		//Reset the total ratio
		totalRatio -= totalRatio;
		//Get the total ratio of all egg drop
		foreach (EggDrop e in eggs) {totalRatio += e.ratio;}
		//The chance randomly got from zero to total ratio
		float chance = Random.Range(0, totalRatio);
		//Go throught all the egg drop in list
		for (int d = eggs.Count - 1; d >= 0 ; d--)
		{
			//Send the allies in egg when it ratio decrease with chance are lower or equal to zero
			if((chance - eggs[d].ratio) <= 0) {return eggs[d].allies;}
			//Decrease the chance if the egg ratio are higher than it
			else {chance -= eggs[d].ratio;}
		}
		//Not important
		return null;
	}
}