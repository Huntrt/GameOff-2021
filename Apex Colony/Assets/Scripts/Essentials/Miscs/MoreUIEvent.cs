using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class MoreUIEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	bool isHover; public bool IsHover {get => isHover;}
	UnityEvent onHoverEnter = new UnityEvent();
	UnityEvent onHoverExit = new UnityEvent();
	UnityEvent onClick = new UnityEvent();
	public UnityEvent OnHoverEnter {get => onHoverEnter;}
	public UnityEvent OnHoverExit {get => onHoverExit;}
	public UnityEvent OnClick {get => onClick;}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHover = true;
		onHoverEnter.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHover = false;
		onHoverExit.Invoke();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick.Invoke();
	}
}
