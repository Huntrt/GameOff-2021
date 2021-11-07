using UnityEngine;

public class Controls : MonoBehaviour
{
	public Vector2 clickPos, mousePos;
	public GameObject enemySelector;
	//An enum for state of mouse
	enum Mousing {release, click, holding} [SerializeField] Mousing mousing;
	Manager manager; Formator formator;

	//Get the manager and the formator
	void Start() {manager = Manager.i; formator = Manager.i.formator;}

	void Update()
	{
		//When press key to stop movement 
		if(Input.GetKeyDown(KeyCode.X)) 
		{
			//Clear all rival in formator
			formator.ClearRivals();
			//Stop all path movement of all the followers in formator
			foreach (Follower follower in formator.followers) {follower.StopPath();}
		}
	}

    void LateUpdate()
    {
		//Get the mouse position
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//If PRESS the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse are click
			mousing = Mousing.click;
			//Clear selected enemy
			formator.ClearSelection();
		}
		//If RELEASE the right mouse button
		if(Input.GetMouseButtonUp(1))
		{
			///If release while click mouse
			if(mousing == Mousing.click)
			{
				//If click on an enemy
				if(clickEnemey() != null)
				{
					//Stop rival the click enemy if already rival
					if(formator.HasRival(clickEnemey())) {formator.UnrivalClicked(clickEnemey());}
					//Rival the click enemy if haven't rival
					else {formator.RivalClicked(clickEnemey());}
				}
				//Generated goal at click position if not click on enemy then clear all rivals
				else {formator.ClearRivals(); manager.goaling.GenerateGoals(clickPos);}
			}
			///If release while holding mouse
			if(mousing == Mousing.holding)
			{
				//Rivaling all selected enemy
				formator.RivalSelected();
				//Deactive the enemy selector
				enemySelector.SetActive(false);
			}
			//Releasing mouse and begin targeting rivals
			if(mousing != Mousing.release) {mousing = Mousing.release; formator.TargetRivals();}
		}
		//If mouse position change while still cliking
		if(mousing == Mousing.click && mousePos != clickPos) 
		{
			//Are now holding mouse
			mousing = Mousing.holding;
			//Reset the selector box size
			enemySelector.transform.localScale = Vector2.zero;
			//Set the selector start position at click position
			enemySelector.transform.position = clickPos;
			//Active the enemy selector
			enemySelector.SetActive(true);
		}
		///While holding, expand the selector by using click and mouse position as start & end anchor 
		if(mousing == Mousing.holding) {enemySelector.transform.localScale = mousePos - clickPos;}
    }

	GameObject clickEnemey() ///The enemy go click by mouse
	{
		//Cast an ray at click position with no direction and distance on enemy layer
		RaycastHit2D click = Physics2D.Raycast(clickPos, Vector2.zero, 0 , manager.enemy.layer);
		//Send the enemy on got click if click one else send null 
		if(click) {return click.collider.gameObject;} return null;
	}
}