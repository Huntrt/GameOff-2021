using UnityEngine;

public class BeginPoint : MonoBehaviour
{
    void Awake()
    {
		//Spawn formation upon populated frame with section
        Manager.i.map.populated += SpawFormation;
    }

	void SpawFormation()
	{
		//For each of the allies object in the allies object manager
		foreach (GameObject allie in Manager.i.allie.alliesObj)
		//Set those allie position as begin point then active those allie
		{allie.transform.position = transform.position; allie.SetActive(true);}
		//Create goal at this begin positoin
		Manager.i.goal.Create(transform.position);
		//! Focus camera to ceneter point of formation
	}

	void OnDisable()
	{
		//Remove delegate event
        Manager.i.map.populated -= SpawFormation;
	}
}
