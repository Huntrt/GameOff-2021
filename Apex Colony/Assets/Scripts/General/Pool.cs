using System.Collections.Generic; using UnityEngine;

public class Pool : MonoBehaviour
{
	//The pool contain all the object has create
	public List<GameObject> objectsPool;

	//Turn this script into singleton
    public static Pool get; void Awake() {get = this;}

	//Create an clean new list for object pool
	void Start() {objectsPool = new List<GameObject>();}

	//Create the wanted object of whatever call this function with position, rotation and active it
    public GameObject Create(GameObject Need,Vector3 Position,Quaternion Rotation,bool Active = true)
    {
		/// If there is object in pool
        if(objectsPool.Count > 0)
		{
			//Go through all the object in pool in reverse order
			for (int i = objectsPool.Count-1; i >= 0; i--)
			{
				//Remove an null object from bool
				if(objectsPool[i] == null) {objectsPool.RemoveAt(i);}
				//If there is an unactive object in pool with the same name of object need to get
				else if(!objectsPool[i].activeInHierarchy && Need.name+"(Clone)" == objectsPool[i].name)
				{
					//Set it position
					objectsPool[i].transform.position = Position;
					//Set it rotation
					objectsPool[i].transform.rotation = Rotation;
					//Active it depend if need to active
					objectsPool[i].SetActive(Active);
					//Send it to caller and no need to continue code
					return objectsPool[i];
				}
			}
		}
		/// If there is no unactive object in pool
		{
			//Create the needed object witth set position and rotation
			GameObject newObject = Instantiate(Need, Position, Rotation);
			//Set the new object parent as this object for organize
			newObject.transform.SetParent(transform);
			//Add it into pool list
			objectsPool.Add(newObject);
			//Set the new object active state
			newObject.SetActive(Active);
			//Send new object to caller
			return newObject;
		}
    }
}