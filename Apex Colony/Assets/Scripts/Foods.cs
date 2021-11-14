using UnityEngine;

public class Foods : MonoBehaviour
{
    public int food;
	///Make this into singelton
	public static Foods i; void Awake() {i = this;}

	public bool Spend(int price)
	{
		//Able to spend when price are lower or equal to food then take food away
		if(price <= food) {food -= price; return true;}
		//Not able to spend when can't afford price
		return false;
	}
}
