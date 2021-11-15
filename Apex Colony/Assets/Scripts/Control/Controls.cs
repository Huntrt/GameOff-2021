using UnityEngine.EventSystems;
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
		//Get the mouse position
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//When press key to stop movement 
		if(Input.GetKeyDown(KeyCode.X)) {StopFromation();}
		///Selecting rival when click right mouse
		Selecting();
		///Moving or interact when click left mouse
		if(Input.GetMouseButton(0)) 
		{
			//Create an raycast to click at current position only at the interactable layer
			RaycastHit2D react = Physics2D.Raycast(mousePos, Vector2.zero, 0 ,manager.layer.inter);
			//If click on any interactable
			if(react) 
			{
				//Clear all the rival
				formator.ClearRivals();
				///Make fromation go toward the clicked interactable
				foreach (Follower f in formator.followers) {f.SetInteract(react.transform);}
			}
			///If not click on any interactable or UI
			else if(!EventSystem.current.IsPointerOverGameObject())
			{
				//Clear all the rival
				formator.ClearRivals();
				//If the eggs panel are active
				if(manager.eggsPanel.gameObject.activeInHierarchy)
				//Click the declining button of egg panel 
				{manager.eggsPanel.decline.onClick.Invoke();}
				///Create formation of goal at mouse for followers move toward
				manager.goal.Create(mousePos);
			}
		}
	}

	void Selecting()
	{
		//If PRESS the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse are click
			mousing = Mousing.click;
		}
		//If RELEASE the right mouse button
		if(Input.GetMouseButtonUp(1))
		{
			///If release while CLICK mouse
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
			}
			///If release while HOLDING mouse
			if(mousing == Mousing.holding)
			{
				//Rivaling all selected enemy
				formator.RivalSelected();
				//Deactive the enemy selector
				enemySelector.SetActive(false);
			}
			//Releasing mouse and begin targeting rivals
			if(mousing != Mousing.release) {mousing = Mousing.release; formator.TargetRivals();}
			//Clear selected enemy when release mouse
			formator.ClearSelection();
		}
		///If mouse position change while still clicking
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
		RaycastHit2D click = Physics2D.Raycast(clickPos, Vector2.zero, 0 , manager.layer.enemy);
		//Send the enemy on got click if click one else send null 
		if(click) {return click.collider.gameObject;} return null;
	}

	public void StopFromation() ///Stop the whole formation
	{
		//Clear all rival in formator
		formator.ClearRivals();
		//Stop all path movement of all the followers in formator
		foreach (Follower f in formator.followers) {f.StopMovement();}
	}
}