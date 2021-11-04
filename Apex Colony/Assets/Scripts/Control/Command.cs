using UnityEngine;

public class Command : MonoBehaviour
{
	public Vector2 clickPos, mousePos;
	public GameObject enemySelector;
	//An enum for state of mouse
	enum Mousing {release, click, holding, multi} [SerializeField] Mousing mousing;
	Manager manager; Formator formator;

	//Get the manager and the formator
	void Start() {manager = Manager.i; formator = Manager.i.formator;}

	void Update()
	{
		//Clear the rivals when press key
		//! Same key as set null path in Follows.cs
		if(Input.GetKeyDown(KeyCode.X)) {formator.ClearRivals();}
	}

    void LateUpdate()
    {
		//Get the mouse position
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//If press the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse has click
			mousing = Mousing.click;
			//Clear selected enemy
			formator.ClearSelection();
		}
		//If mouse position has change since mouse click
		if(mousing == Mousing.click && mousePos != clickPos) 
		{
			//Are now holding mouse
			mousing = Mousing.holding;
			//Clear the select box size
			enemySelector.transform.localScale = Vector2.zero;
			//Set the selector position as click position
			enemySelector.transform.position = clickPos;
			//Active the enemy selector
			enemySelector.SetActive(true);
		}
		///Begin select enemy if mouse are holding
		if(mousing == Mousing.holding) {SelectorSCaling();}
		//If release the right mouse button
		if(Input.GetMouseButtonUp(1))
		{
			///If release while click mouse
			if(mousing == Mousing.click)
			{
				//If click on an enemy
				if(enMouse(clickPos) != null)
				{
					//Set the clicked enemy as rival if haven't
					if(HasRival(clickPos)) {formator.rivals.Add(enMouse(clickPos));}
					//Remove the clicked enemy from rival if it already rival
					else {formator.rivals.Remove(enMouse(clickPos));}
				}
				//Generated goal at click position if not click on enemy
				else {manager.goals.GenerateGoals(clickPos);}
				//Mouse release
				mousing = Mousing.release;
			}
			///If release while holding mouse
			if(mousing == Mousing.holding)
			{
				//Mouse released
				mousing = Mousing.release;
				//Rivaling all selected enemy
				formator.RivalEnemy();
				//Deactive the enemy selector
				enemySelector.SetActive(false);
			}
		}
    }

	GameObject enMouse(Vector2 origin) ///The enemy under mouse
	{
		//Cast an ray at origin with no direction and distance on enemy layer
		RaycastHit2D on = Physics2D.Raycast(origin, Vector2.zero, 0 , manager.enemyL);
		//Send the enemy on object the mouse or send null 
		if(on) {return on.collider.gameObject;} return null;
	}

	bool HasRival(Vector2 origin) ///Check if an enemy has been rival
	{
		//Send true if the enemy on mouse are not null and not in the rival list
		if(enMouse(origin) != null && !formator.rivals.Contains(enMouse(origin))) {return true;} return false;
	}

	void SelectorSCaling()
	{
		//Set the enemy selector size as the length from hold to click position
		enemySelector.transform.localScale = mousePos - clickPos;
	}
}