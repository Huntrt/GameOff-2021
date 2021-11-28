using UnityEngine.EventSystems;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public Vector2 clickPos, mousePos; Vector2 prevMouse;
	public GameObject enemySelector;
	//An enum for state of key input
	enum Inputing {release, press, hold} [SerializeField] Inputing input;
	Manager manager; Formator formator; IndicatorManager indi;

	//Get the manager, the formator and the indicator
	void Start() {manager = Manager.i; formator = manager.formator; indi = manager.indi;}

	void Update()
	{
		//Get the mouse position
		mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//When press stop key to stop movement 
		if(Input.GetKeyDown(KeyCode.X)) {StopFromation();}
		///Selecting rival when click right mouse
		Selecting();
		///Create new move indicator when pressed the move key
		if(Input.GetMouseButtonDown(0)) {indi.CreateMoveIndicator(mousePos);}
		///Moving or interact when hold the move key
		if(Input.GetMouseButton(0)) 
		{
			//Make the moving indicator follow mouse position if created it
			if(indi.moveindi != null) {indi.moveindi.transform.position = mousePos;}
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
				///Create formation of goal at mouse for followers move toward (if mouse are updated)
				if(prevMouse != mousePos) {manager.goal.Create(mousePos);}
			}
			//Update the previous mouse position
			prevMouse = mousePos;
		}
		///End the current new move indicator when release the move key
		if(Input.GetMouseButtonUp(0)) {indi.RemoveMoveIndicator();}
	}

	void Selecting()
	{
		//If PRESS attack key
		if(Input.GetMouseButtonDown(1))
		{
			//Get the mouse position upon click
			clickPos = mousePos;
			//Mouse are click
			input = Inputing.press;
		}
		//If RELEASE attack key
		if(Input.GetMouseButtonUp(1))
		{
			///If release while PRESSING attack key
			if(input == Inputing.press)
			{
				//If click on an enemy
				if(clickEnemey() != null)
				{
					//Get the cliked enemy
					GameObject clicked = clickEnemey();
					//If the enemy click on are already rival
					if(formator.HasRival(clicked)) 
					{
						//Unrival the enemy click on
						formator.UnrivalClicked(clicked);
						//Flash an unrival indicator on the clicked enemy
						indi.FlashAtkIndi(clicked.transform.position, false);
					}
					//If haven't rival the enemy click on
					else 
					{
						//Rivaling the enemt click on
						formator.RivalClicked(clicked);
						//Flash an rival indicator on the clicked enemy
						indi.FlashAtkIndi(clicked.transform.position, true);
					}
				}
			}
			///If release while HOLDING attack key
			if(input == Inputing.hold)
			{
				//Clear all the attack indicator of enemy
				indi.ClearAtkIndi();
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
		if(manager.eggsPanel.gameObject.activeInHierarchy) {manager.eggsPanel.close.onClick.Invoke();}
		//Close the port panel if it currently open
		if(manager.portsPanel.gameObject.activeInHierarchy) {manager.portsPanel.close.onClick.Invoke();}
	}
}