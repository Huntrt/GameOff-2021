using UnityEngine;

public class AtkIndicator : MonoBehaviour
{
    [SerializeField] Animator animator;
	bool ending = false;

	void Awake()
	{
		//Add this indicator into list
		Manager.i.indi.attackIndis.Add(this);
	}

	public void EndingIndicator(bool rival = true)
	{
		//Indicator are now ending
		ending = true;
		//Remove this indicator from list
		Manager.i.indi.attackIndis.Remove(this);
		//Play the rival ending animation if rival
		if(rival) {animator.Play("Rivaling");}
		//Play the unrival ending animation if unrival
		else {animator.Play("Unrival");}
	}

	public void ClearIndicator()
	{
		//If the indicator are not currently ending
		if(!ending)
		{
			//Remove this indicator from list
			Manager.i.indi.attackIndis.Remove(this);
			//Destroy the indicator
			DestroyIndicator();
		}
	}

	//Destroy this indicator (use in animator)
	public void DestroyIndicator() {Destroy(gameObject);}
}
