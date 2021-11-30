using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager i;
	public float cameraSpeed, zoomSpeed;

	void Awake()
	{
		//Move the game manager to don't destroy on load if this is the first one
		if(i == null) {i = this; DontDestroyOnLoad(this);}
		//Destroy gameobject if the object contain game manager are not in don't destroy on load
		if(gameObject.scene.name != "DontDestroyOnLoad") {Destroy(gameObject);}
	}
}