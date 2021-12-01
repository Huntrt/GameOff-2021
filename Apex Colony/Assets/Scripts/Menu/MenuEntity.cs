using UnityEngine;

public class MenuEntity : MonoBehaviour
{
    
	[Tooltip("The total distance able to move randomly")]
	[SerializeField] float distance;
	[Tooltip("Cooldown to find an new path")]
	[SerializeField] FloatMinMax repeat; 
	float repeatCount;
	public Vector2 center;
	[SerializeField] Animator animator;
	[SerializeField] Vector2 destination;
	[SerializeField] float speed;

	void Start()
	{
		//The patrol center are at spawn position
		center = transform.position;
		//Start walking animation
		animator.SetBool("Walk", true);
		//Get destination upon start
		GetDestination();
	}

    void Update()
    {
		//Look toward the destination
		transform.up = destination - (Vector2)transform.position;
		//Go toward the destination with set speed
		transform.position = Vector3.MoveTowards(transform.position, center, speed * Time.deltaTime);
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

	void GetDestination()
	{
		//Get an new repeat rate
		repeat.raw = Random.Range(repeat.min, repeat.max);
		//Randomly chose the X and Y distance to modify it onto the center point
		center.x += Random.Range(-distance, distance); center.y += Random.Range(-distance, distance);
		//Get the the center has get as destination
		destination = center;
	}
}