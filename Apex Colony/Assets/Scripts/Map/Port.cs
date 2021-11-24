using UnityEngine;

public class Port : MonoBehaviour
{
	public bool hasData;
	//The slot data of this object
	public ItemData[] datas = new ItemData[3];
	public Interactable interactable;
	[HideInInspector] public PortsPanel panel;

	void Start()
	{
		//Get the ports panel
		panel = Manager.i.portsPanel;
		//Display panel upon interact with ports
		interactable.interact.AddListener(DisplayPanel);
	}

	void DisplayPanel()
	{
		//? Remove left over listener 
		//Remove all listener of each slot buyinhg button
		for(int d = 0; d < panel.slots.Length; d++) {panel.slots[d].buying.onClick.RemoveAllListeners();}
		//REMOVE all the listener of closing
		panel.close.onClick.RemoveAllListeners();
		//Stop whole formation when interact
		Manager.i.control.StopFromation();
		//Begin closing when panel close
		panel.close.onClick.AddListener(Closing);
		///Begin getting the slot data of this port
		//Get new data if haven't has one
		if(!hasData)
		{
			//For each of the data slot need to be fill
			for (int s = 0; s < datas.Length; s++)
			{
				//Getting the item randomly from port
				GameObject item = Manager.i.ports.ItemDrop();
				//Get the pickupable component of item has get
				Pickupable pickup = item.GetComponent<Pickupable>();
				//Set the current data slot item object
				datas[s].item = item;
				//Set the current data slot icon
				datas[s].icon = pickup.data.icon;
				//Set the current data slot description
				datas[s].info = pickup.data.info;
				//Set the current data slot price as item cost
				datas[s].cost = pickup.data.cost;
			}
			//Has get the datas
			hasData = true;
		}
		//Active the port panel
		panel.gameObject.SetActive(true);
		///Begin display the item data to panel's slot
		//For each of the slot need to be display
		for(int d = 0; d < panel.slots.Length; d++)
		{
			//Skip the current data to display if it is null
			if(datas[d] == null) continue;
			//Get the current slot (delegate in loop needed this to work correctly)
			var currentSlot = d;
			//ADD click event to accept button for spawn item of current slot using current data
			panel.slots[d].buying.onClick.AddListener(delegate{SpawnItem(currentSlot);});
			//Display the curret slot price as cost from current item data
			panel.slots[d].price = datas[d].cost;
			//Display the curret icon to display as icon from current item data
			panel.slots[d].iconDisplay = datas[d].icon;
			//Display the curret slot name as name from current item data
			panel.slots[d].itemName.text = datas[d].item.name;
			//Display the curret slot item price as price from current item data
			panel.slots[d].itemPrice.text = datas[d].cost.ToString();
			//Update the description of slot with name info of current item data
			panel.slots[d].UpdateDescription(datas[d].item.name, datas[d].info);
			//Display the slot panel
			panel.slots[d].gameObject.SetActive(true);
		}
	}

	public void SpawnItem(int slot)
	{
		//If has enough food to spawn the item
		if(Foods.i.Spend(datas[slot].cost))
		{
			//Spawn the item object in data slot at this port with no rotation then get it
			GameObject item = Instantiate(datas[slot].item, transform.position, Quaternion.identity);
			//Get the rigidbody2d of item just create
			Rigidbody2D rb = item.GetComponent<Rigidbody2D>(); 
			//Launch the item downward from this port using randomize launch force from port manager
			rb.AddForce(-transform.up *  Manager.i.ports.Force(), ForceMode2D.Impulse);
			//Deactive the slot that buyin the item
			panel.slots[slot].gameObject.SetActive(false);
			//Remove the data slot of item has buy
			datas[slot] = null;
		}
	}

	void Closing()
	{
		//For each of the slot are currently display
		for(int d = 0; d < panel.slots.Length; d++)
		{
			//REMOVE all the click event to accept of current slot
			panel.slots[d].buying.onClick.RemoveAllListeners();
			//Hide the current slot panel
			panel.slots[d].gameObject.SetActive(false);
		}
		//Stop listen to when panel close
		panel.close.onClick.RemoveListener(Closing);
		//Deactive the panel
		panel.gameObject.SetActive(false);
	}
}
