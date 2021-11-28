using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EggsPanel : MonoBehaviour
{
    public TextMeshProUGUI info;
	public Button accept, close;

	public void UpdateInfo(int cost)
	{
		//Update info with cost receive
		info.text = "Do you want to open this eggs with <b><u>"+cost+"</u></b> food?";
	}
}