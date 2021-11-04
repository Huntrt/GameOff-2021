using UnityEngine;

public class Manager : MonoBehaviour
{
	public Formator formator;
	public GoalsCreate goals;
	public MouseAction mouse;
	[HideInInspector] public int enemyL;

	//Turn this script into singlton
	public static Manager i; void Awake() {i = this;}

	void Start()
	{
		//Get the enemy layer
		enemyL = (1 << (LayerMask.NameToLayer("Enemy")));
	}
}
