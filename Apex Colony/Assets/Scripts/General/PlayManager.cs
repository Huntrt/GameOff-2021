using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
	public GameObject blockInput;
    public Slider zoomSlider, cameraSlider;
    public TMPro.TextMeshProUGUI zoomAmount, cameraAmount;

	//Turn this object into singleton
	public static PlayManager i; void Awake() {i = this;}

	void Start()
	{
		//@ Get the zoom and camera speed's slider amount from game manager value
		zoomSlider.value = GameManager.i.zoomSpeed;
		cameraSlider.value = GameManager.i.cameraSpeed;
	}

	void Update()
	{
		//@ Changing the game manager's zoom and camera speed using slider
		GameManager.i.zoomSpeed = zoomSlider.value;
		GameManager.i.cameraSpeed = cameraSlider.value;
		//@ Update the zoom and camera speed's display amount from game manager value
		zoomAmount.text = (int)(GameManager.i.zoomSpeed) + "";
		cameraAmount.text = (int)(GameManager.i.cameraSpeed) + "";
	}

	//Enter the game scene
	public void Playing() {SceneManager.LoadScene("Game");}
	//Enter the menu scene (continue to prevent exit when paused)
	public void Menuing() {Continuing(); SceneManager.LoadScene("Menu");}
	//Save the latest time then pause the game
	public void Pausing() {latestTime = Time.timeScale; Time.timeScale = 0;}
	//Contiue back to the latest time
	float latestTime; public void Continuing() {Time.timeScale = latestTime;}
}