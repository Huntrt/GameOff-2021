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
				if(clickEnemey() != null)
				{
					///Stop rival the click enemy if already rival
					if(formator.HasRival(clickEnemey())) {formator.UnrivalClicked(clickEnemey());}
					///Rival the click enemy if haven't rival
					else {formator.RivalClicked(clickEnemey());}
				}
				//Generated goal at click position if not click on enemy
				else {manager.goaling.GenerateGoals(clickPos);}
				//Mouse release
				mousing = Mousing.release;
			}
			///If release while holding mouse
			if(mousing == Mousing.holding)
			{
				//Mouse released
				mousing = Mousing.release;
				//Rivaling all selected enemy
				formator.RivalSelected();
				//Deactive the enemy selector
				enemySelector.SetActive(false);
			}
		}
    }

	GameObject clickEnemey() ///The enemy click by mouse
	{
		//Cast an ray at click position with no direction and distance on enemy layer
		RaycastHit2D click = Physics2D.Raycast(clickPos, Vector2.zero, 0 , manager.enemyL);
		//Send the enemy on got click if click one else send null 
		if(click) {return click.collider.gameObject;} return null;
	}

	void SelectorSCaling()
	{
		//Set the enemy selector size as the length from hold to click position
		enemySelector.transform.localScale = mousePos - clickPos;
	}
}