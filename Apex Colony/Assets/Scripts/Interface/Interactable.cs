using UnityEngine; using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
	public UnityEvent interact;

	//Display food panel upon interaction
	void Start() {interact.AddListener(DisplayFood);}

	// Showing food panel
	void DisplayFood() {Foods.i.showFood = true;}
}