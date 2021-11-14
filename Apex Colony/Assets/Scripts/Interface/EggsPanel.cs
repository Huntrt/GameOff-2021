using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EggsPanel : MonoBehaviour
{
    public TextMeshProUGUI info;
	public Button accept, decline;

	public void UpdateInfo(int cost)
	{
		//Update info with cost receive
		info.text = "Do you want to open this eggs with <color=#4a5ccf>"+cost+"</color> food?";
	}
}
