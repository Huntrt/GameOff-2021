using UnityEngine;

public class PickupManager : MonoBehaviour
{
	public Pickupable picked;

	void Update()
	{
		///When click the pickup key
		if(Input.GetMouseButtonDown(1)) 
		{
			//Get the mouse position
			Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
			///Drop the pickup item at mouse position if currently pick one
			if(picked != null) {picked.DropItem(mousePos); picked = null; return;}
			//Create an raycast to cast at current position that only detect the pickupable layer
			RaycastHit2D pick = Physics2D.Raycast(mousePos, Vector2.zero, 0 ,Manager.i.layer.pick);
			///Get the component and picking up the pickupable that got detected from raycast
			if(pick) {picked = pick.collider.GetComponent<Pickupable>(); picked.isPicking = true;}
		}
	}
}