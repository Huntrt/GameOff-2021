using UnityEngine.EventSystems;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public Vector2 clickPos, mousePos;
	public GameObject enemySelector;
	//An enum for state of key input
	enum Inputing {release, press, hold} [SerializeField] Inputing input;
	Manager manager; Formator formator;

	//Get the manager and the formator
	void Start() {manager = Manager.i; formator = Manager.i.formator;}

	void Update()
	{
		//Get the mouse position
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//When press their key to stop movement 
		if(Input.GetKeyDown(KeyCode.X)) {StopFromation();}
		///Selecting rival when click right mouse
		Selecting();
		///Moving or interact when click their key
		if(Input.GetMouseButton(0)) 
		{
			//Create an raycast to cast at current position that only detect the interactable layer
			RaycastHit2D react = Physics2D.Raycast(mousePos, Vector2.zero, 0 ,manager.layer.inter);
			//If click on any interactable
			if(react) 
			{
				//Clear all the rival and close all the panel
				formator.ClearRivals(); ClosePanel();
				///Make fromation go toward the clicked interactable
				foreach (Follower f in formator.followers) {f.SetInteract(react.transform);}
			}
			///If not click on any interactable and UI
			if(!react && !EventSystem.current.IsPointerOverGameObject())
			{
				//Clear all the rival and close all the panel
				formator.ClearRivals(); ClosePanel();
				///Create formation of goal at mouse for followers move toward
				manager.goal.Create(mousePos);
			}
		}
	}

	void Selecting()
	{
		//If PRESS their key
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse are click
			input = Inputing.press;
		}
		//If RELEASE their key
		if(Input.GetMouseButtonUp(1))
		{
			///If release while PRESSING key
			if(input == Inputing.press)
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
			///If release while HOLDING key
			if(input == Inputing.hold)
			{
				//Rivaling all selected enemy
				formator.RivalSelected();
				//Deactive the enemy selector
				enemySelector.SetActive(false);
			}
			//Releasing mouse and begin targeting rivals
			if(input != Inputing.release) {input = Inputing.release; formator.TargetRivals();}
			//Clear selected enemy when release mouse
			formator.ClearSelection();
		}
		///If mouse position are change while still PRESSING key
		if(input == Inputing.press && mousePos != clickPos) 
		{
			///Are now holding key
			input = Inputing.hold;
			//Reset the selector box size
			enemySelector.transform.localScale = Vector2.zero;
			//Set the selector start position at click position
			enemySelector.transform.position = clickPos;
			//Active the enemy selector
			enemySelector.SetActive(true);
		}
		///While holding, expand the selector by using click and mouse position as start & end anchor 
		if(input == Inputing.hold) {enemySelector.transform.localScale = mousePos - clickPos;}
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

	public void ClosePanel()
	{
		//Close the egg panel if it currently open
		if(manager.eggsPanel.gameObject.activeInHierarchy) {manager.eggsPanel.decline.onClick.Invoke();}
		//Close the port panel if it currently open
		if(manager.portsPanel.gameObject.activeInHierarchy) {manager.portsPanel.close.onClick.Invoke();}
	}
}