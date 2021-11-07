using UnityEngine;

public class Manager : MonoBehaviour
{
	public Formator formator;
	public GoalsCreate goaling;
	public Controls control;
	public EnemyManager enemy;

	//Turn this script into singlton
	public static Manager i; void Awake() {i = this;}
}
