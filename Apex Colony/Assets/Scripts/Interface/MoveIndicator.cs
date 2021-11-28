using UnityEngine;

public class MoveIndicator : MonoBehaviour
{
    [SerializeField] Animator animator;
	bool ending = false;

	void Update()
	{
		//Ending the indicator when it got changed
		if(!ending && Manager.i.indi.moveindi != gameObject) {animator.Play("Moved"); ending = true;}
	}
	//Clear the indicator (use in animator)
	public void ClearIndicator() {Destroy(gameObject);}
}