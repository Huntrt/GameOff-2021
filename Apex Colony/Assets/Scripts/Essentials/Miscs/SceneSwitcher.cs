using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
	public void Switch(int targetScene)
	{
		//Use special method if target scene are negative
		if(targetScene < 0) 
		{
			//-1 mean enter game scene with menu outro
			if(targetScene == -1) {GetComponent<Animation>().Play("Menu Canvas Outro"); return;}
			//-2 mean quit the game
			if(targetScene == -2) {Application.Quit(); return;}
			Debug.LogError("There no special method for neagtive scene index " + targetScene); return;
		}
		SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
	}
}