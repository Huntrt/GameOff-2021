using UnityEngine;

public class Popup : MonoBehaviour
{
	//Deactive the parent (include itself) after an animation
	public void ClosePopup() {transform.parent.gameObject.SetActive(false);}
}