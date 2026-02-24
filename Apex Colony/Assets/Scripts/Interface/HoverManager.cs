using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
	[System.Serializable] public class statsDisplay
	{
		public GameObject panel;
		public TextMeshProUGUI name;
		public TextMeshProUGUI heath;
		public TextMeshProUGUI maxHeath;
		public TextMeshProUGUI damage;
		public TextMeshProUGUI rate;
		public TextMeshProUGUI range;
		public TextMeshProUGUI speed;
	}
	public statsDisplay hover;
	bool track = false;
	[SerializeField] GameObject hovering;
	Allies allies; Enemy enemy; Interactable react;

	void Update()
	{
		//Hide the hover panel if not tracking
		if(!track) {hover.panel.SetActive(false);}
		//If hovering over an enemy or allies
		if(hovering != null)
		{
			//Track the entity stas when press tracking stats key
			if(Input.GetKey(Keybind.i.GetKey("Tracking Unit")) && !track) {track = true;}
			//If hover over an enemy
			if(hovering.CompareTag("Enemy"))
			{
				//Display the enemy name
				hover.name.text = enemy.entityName + " (Enemy)";
				//@ Display all the enemy stats onto hover panel
				hover.heath.text = StatFormatter(enemy.hp.curHeath);
				hover.maxHeath.text = StatFormatter(enemy.hp.maxHeath);
				hover.damage.text = StatFormatter(enemy.damage);
				hover.rate.text = StatFormatter(enemy.rate);
				hover.range.text = StatFormatter(enemy.range);
				hover.speed.text = StatFormatter(enemy.speed);
			}
			//If hover over an allies
			if(hovering.CompareTag("Allies"))
			{
				//Display the allies name
				hover.name.text = allies.entityName + " (Allies)";
				//@ Display all the allies stats onto hover panel
				hover.heath.text = StatFormatter(allies.hp.curHeath);
				hover.maxHeath.text = StatFormatter(allies.hp.maxHeath);
				hover.damage.text = StatFormatter(allies.damage);
				hover.rate.text = StatFormatter(allies.rate);
				hover.range.text = StatFormatter(allies.range);
				hover.speed.text = StatFormatter(allies.speed);
			}
			//If hover over an interactable
			if(hovering.CompareTag("Interactable"))
			{
				//Display the interactable name
				hover.name.text = react.interactName;
				//@ N/A all the stats from hover panel
				hover.heath.text = "N/A";
				hover.maxHeath.text = "N/A";
				hover.damage.text = "N/A";
				hover.rate.text = "N/A";
				hover.range.text = "N/A";
				hover.speed.text = "N/A";
			}
			//Showing the hover panel
			hover.panel.SetActive(true);
		}
	}

	string StatFormatter(float stat)
	{
		return System.Math.Round(stat, 1).ToString();
	}

	void LateUpdate()
	{
		//Make this object following the mouse position
		transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); 
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		//If currently not tracking entity
		if(!track)
		{
			//If an enemy enter trigger
			if(other.CompareTag("Enemy"))
			{
				//Getting component and object of enemy enter trigger
				enemy = other.GetComponent<Enemy>(); hovering = other.gameObject;
			}
			//If an allies enter trigger
			if(other.CompareTag("Allies"))
			{
				//Getting component and object of allies enter trigger
				allies = other.GetComponent<Allies>(); hovering = other.gameObject;
			}
			//If an interactable enter trigger
			if(other.CompareTag("Interactable"))
			{
				//Getting component and object of interactable enter trigger
				react = other.GetComponent<Interactable>(); hovering = other.gameObject;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) 
	{
		//Clear hover when exit current hovering object while not tracking
		if(!track && other.gameObject == hovering) {ClearHover();}
	}

	//Stop tracking and clear hover upon press button
	public void StopTracking() {track = false; ClearHover();}
	//Clear all the hover data such as enemy, allies and interactable component
	void ClearHover() {hovering = null; enemy = null; allies = null; react = null;}
}