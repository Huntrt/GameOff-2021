using UnityEngine;

public class MapLoading : MonoBehaviour
{
	//When the map started
	void OnEnable() {Manager.i.stared += LoadingComplete;}
	//Disable the loading panel
	void LoadingComplete() {gameObject.SetActive(false);}
	//Remove the delegate event
	void OnDisable() {Manager.i.stared -= LoadingComplete;}
}
