using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	//Declare these in your class
	int frameCounter = 0;
	float lastestFramerate;
	public float refreshRate = 0.5f;
	float refreshCounter;
	[SerializeField] TMPro.TMP_Text counter;
	[SerializeField] int decimalRounding;

	void Update()
	{
		if(refreshCounter < refreshRate)
		{
			refreshCounter += Time.deltaTime;
			frameCounter++;
		}
		else
		{
			//This code will break if you set your 'refreshTime' to 0, which makes no sense.
			lastestFramerate = (float)frameCounter/refreshCounter;
			frameCounter = 0;
			refreshCounter = 0.0f;
		}
		counter.text = "FPS " + System.Math.Round(lastestFramerate, decimalRounding);
	}
}