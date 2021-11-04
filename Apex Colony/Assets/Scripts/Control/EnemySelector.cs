using UnityEngine;

public class EnemySelector : MonoBehaviour
{
	//Get the formator
	Formator formator; void Start() {formator = Manager.i.formator;}

    private void OnTriggerEnter2D(Collider2D other)
	{
		//If enter trigger with are an new enemy
		if(other.CompareTag("Enemy") && !formator.rivals.Contains(other.gameObject))
		{
			//Add the enemy enter trigger to selected enemy
			formator.selecting.Add(other.gameObject);
		}
	}

    private void OnTriggerExit2D(Collider2D other)
	{
		//If exit trigger with an enemy that been select enemy
		if(other.CompareTag("Enemy"))
		{
			//Add the enemy enter trigger to selected enemy
			formator.selecting.Remove(other.gameObject);
		}
	}
}
