using UnityEngine;

public class EnemySelector : MonoBehaviour
{
	//Get the formator
	Formator formator; void Start() {formator = Manager.i.formator;}

    private void OnTriggerEnter2D(Collider2D other)
	{
		///When trigger with an enemy 
		if(other.CompareTag("Enemy")) 
		{
			//Select the enemy
			formator.selectings.Add(other.gameObject);
			//Set the enemy component 's indicating
			other.GetComponent<Enemy>().indicating = 
			//Create the attack indicator at this enemy with zero rotation
			Instantiate(Manager.i.indi.attackIndiPrefab, other.transform.position, Quaternion.identity)
			//Get the attack indicator's component
			.GetComponent<AtkIndicator>();
		}
	}

    private void OnTriggerExit2D(Collider2D other)
	{
		///When enemy exit the trigger
		if(other.CompareTag("Enemy")) 
		{
			//Remove this enemy for selecting
			formator.selectings.Remove(other.gameObject);
			//Clear the indicating on the enemy
			other.GetComponent<Enemy>().indicating.ClearIndicator();
		}
	}
}
