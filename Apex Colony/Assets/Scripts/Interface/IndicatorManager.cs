using UnityEngine;
using System.Collections.Generic;

public class IndicatorManager : MonoBehaviour
{
    public GameObject moveIndiPrefab;
	public MoveIndicator moveindi;
	public GameObject attackIndiPrefab;
	public List<AtkIndicator> attackIndis = new List<AtkIndicator>();

	public void CreateMoveIndicator(Vector2 pos)
	{
		//Create an move indicatior in game then get it component
		moveindi = Instantiate(moveIndiPrefab, pos, Quaternion.identity).GetComponent<MoveIndicator>();
	}

	//Removing the move indicator
	public void RemoveMoveIndicator() {moveindi = null;}

	public void ClearAtkIndi()
	{
		//End all the current attack indicator in list
		for (int i = attackIndis.Count - 1; i >= 0 ; i--) {attackIndis[i].EndingIndicator();}
	}

	public void FlashAtkIndi(Vector2 position, bool areRival)
	{
		//Create an indicator at position has receive
		Instantiate(Manager.i.indi.attackIndiPrefab,position, Quaternion.identity)
		//Ending the indicator by receive rival or unrival end
		.GetComponent<AtkIndicator>().EndingIndicator(areRival);
	}
}
