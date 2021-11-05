using UnityEngine;

public class EnemySelector : MonoBehaviour
{
	//Get the formator
	Formator formator; void Start() {formator = Manager.i.formator;}

    private void OnTriggerEnter2D(Collider2D other)
	{
		///SELECT the enemy that enter trigger
		if(other.CompareTag("Enemy")) {formator.selectings.Add(other.gameObject);}
	}

    private void OnTriggerExit2D(Collider2D other)
	{
		///DESELECT the enemy that exit trigger
		if(other.CompareTag("Enemy")) {formator.selectings.Remove(other.gameObject);}
	}
}
