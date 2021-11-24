using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortsPanel : MonoBehaviour
{
	//All the port's slots
	public PortSlot[] slots;
	public GameObject descriptionPanel;
	public TextMeshProUGUI descriptionText;
	public Image descriptionIcon;
	public Button close;
	public spriteState richState, poorState;
}