using UnityEngine;

public class Patrol : MonoBehaviour
{
	[Tooltip("The total distance able to move randomly")]
	[SerializeField] float distance;
	[Tooltip("Cooldown to find an new path")]
	[SerializeField] FloatMinMax repeat; 
	float repeatCount;
	public Vector2 center;
	public Enemy enemy;
	[SerializeField] Animator animator;

	void Start()
	{
		//The patrol center are at spawn position
		center = transform.position;
	}

    void Update()
    {
		//Start walking animation
		animator.SetBool("Walk", true);
		//Stop walking animation when the path velocity are zero
		if(enemy.path.velocity.magnitude == 0) {animator.SetBool("Walk", false);}
		//If the enemy are not combating
        if(enemy.combat == combating.none)
		{
			//Counting time to repeat path
			repeatCount += Time.deltaTime;
			//If repeat count has reach repeat rate
			if(repeatCount >= repeat.raw)
			{
				//Get the destination
				GetDestination();
				//Reset the repeat counter
				repeatCount -= repeatCount;
			}
		}
    }

	void GetDestination()
	{
		//Get an new repeat rate
		repeat.raw = Random.Range(repeat.min, repeat.max);
		//Randomly chose the X and Y distance to modify it onto the center point
		center.x += Random.Range(-distance, distance); center.y += Random.Range(-distance, distance);
		//Enable auto search path
		enemy.path.canSearch = true; 
		//Set the destination as the target getted then search path
		enemy.path.destination = center; enemy.path.SearchPath();
		//Disable auto search path
		enemy.path.canSearch = false;
	}
}
