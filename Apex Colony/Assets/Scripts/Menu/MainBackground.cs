using UnityEngine;
using UnityEngine.UI;

public class MainBackground : MonoBehaviour
{
	public Sprite[] images;
	public Image background;
    
	void Awake()
	{
		//Get an radom image as background
		background.sprite = images[Random.Range(0,images.Length)];
	}
}