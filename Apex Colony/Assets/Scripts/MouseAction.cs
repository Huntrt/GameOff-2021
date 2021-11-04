using UnityEngine;

public class MouseAction : MonoBehaviour
{
	public Vector2 clickPos, holdPos;
	//An enum for state of mouse
	enum Mousing {release, click, holding} [SerializeField] Mousing mouse;
	Manager manager;

	void Start()
	{
		//Get the manager
		manager = Manager.i;
	}

    void Update()
    {
		//The mouse position
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//If press the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse has click
			mouse = Mousing.click;
		}
		//Begin holding mouse if mouse position has change from when clicking mouse
		if(mouse == Mousing.click && mousePos != clickPos) {mouse = Mousing.holding;}
		//If mouse are holding
		if(mouse == Mousing.holding)
		{
			//Get the holdig position
			holdPos = mousePos;
		}
		//If release the right mouse button
		if(Input.GetMouseButtonUp(1))
		{
			//If release while click mouse
			if(mouse == Mousing.click)
			{
				//Cast an ray at click position with no direction and distance on enemy layer
				RaycastHit2D click = Physics2D.Raycast(clickPos, Vector2.zero, 0 , manager.enemyL);
				//If there an enemy hit engage
				if(click)
				{
					//* Begin attack
					print("Attack");
				}
				//If there is no enemy to engage
				else
				{
					//Generated goal to move at click position
					manager.goals.GenerateGoals(clickPos);
				}
				//Mouse released
				mouse = Mousing.release;
			}
			//If release while holding mouse
			if(mouse == Mousing.holding)
			{
				//Mouse released
				mouse = Mousing.release;
			}
		}
    }
}
