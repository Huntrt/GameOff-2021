using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSFXPlayer : MonoBehaviour
{	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0) && EventSystem.current.IsPointerOverGameObject())
		{
			SFX_Manager.PlaySFX("Click");
		}
	}
}
