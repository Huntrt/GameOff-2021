using UnityEngine; using UnityEngine.Events;

public class Attacking : MonoBehaviour
{
	//Event to send heath and healing or damaging it
	public class HpEvent : UnityEvent <Heath, float, float> {} 
	//Create an new event variable
	public HpEvent Attack = new HpEvent();
}
