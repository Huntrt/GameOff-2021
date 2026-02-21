using UnityEngine;

public class Pickupable : MonoBehaviour
{
	public ItemData data;
	public delegate void Applying (Allies target);
	public event Applying apply;
	public Rigidbody2D rb;
	public bool isPicking;

	void Awake()
	{
		//Get the rigidybody
		rb = GetComponent<Rigidbody2D>();
		//Update the rigidbody's drag value
		rb.linearDamping = Manager.i.ports.itemDrag;
		//Destroy item when go to the next level
		Manager.i.level.nexting += DestroyItem;
	}

	void LateUpdate()
	{
		//Make the pickupable following the mouse position if this object are pickup
		if(isPicking) {transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);}
	}

	//Destroy the item
	public void DestroyItem() {Destroy(gameObject);}

	public void DropItem(Vector2 dropPos)
	{
		//No longer pick this item
		isPicking = false;
		//Create an raycast to cast at drop position that only detect the allies
		RaycastHit2D drop = Physics2D.Raycast(dropPos, Vector2.zero, 0 ,Manager.i.layer.allies);
		//Apply the item onto the allies that has dropped on if drop on one then destroy this item
		if(drop) {apply.Invoke(drop.transform.GetComponent<Allies>()); DestroyItem();}
	}

	void OnDisable()
	{
		//Remove delegate event
		Manager.i.level.nexting -= DestroyItem;
	}
}