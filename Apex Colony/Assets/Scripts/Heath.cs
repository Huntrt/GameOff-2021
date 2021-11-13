using UnityEngine;

public class Heath : MonoBehaviour
{
	[SerializeField] float maxHeath;
    [SerializeField] float curHeath;

	void Start()
	{
		//Reset heath
		curHeath = maxHeath;
	}

	public void Damaging(float damage)
	{
		//Decrease current heath
		curHeath -= damage;
		//Destroy gameobject current heath are zero
		if(curHeath <= 0) {Destroy(gameObject);}
	}

	public void Healing(float heal) 
	{
		//Increase heath when heal
		curHeath += heal; 
		//Claming current heath from max heath
		curHeath = Mathf.Clamp(curHeath, 0, maxHeath);
	}
}
