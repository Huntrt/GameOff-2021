using System.Collections.Generic;
using UnityEngine;

public class Formator : MonoBehaviour
{
	public GoalGenerator goalGenerator;
	//List of follows oiject
	public List<GameObject> follows;

	//Turn this script into singlton
	public static Formator i; void Awake() {i = this;}

	public void Replace()
	{

	}

	public void Delete()
	{

	}
}