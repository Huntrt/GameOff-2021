using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public bool cameraMove;
	enum CameraOption {free, locked, follow} [SerializeField] CameraOption option;
	//% Locking drag control
	public bool lockDrag;
	[Tooltip("The size of the section that won't drag camera")]
	[SerializeField] float bound;
	[SerializeField] float dragSpeed, keySpeed, zoomSpeed, defaultZoom;
	[SerializeField] FloatMinMax zoomLimit;
	float width, height;
	Transform cam;

	void Start()  
	{
		//Get the screen width an height and the camera transform
		width = Screen.width; height = Screen.height; cam = Camera.main.transform;
		//Reset cmaera when map started
		Manager.i.stared += ResetCamera;
	}

	void ResetCamera()
	{
		//Reset camera back to formation when map started
		cam.position = Manager.i.allie.FormationCenter();
		//Reset the camera's Z axis
		cam.position = new Vector3(cam.position.x, cam.position.y, -10);
	}

	void Update() 
	{
		//Camera are not moving
		cameraMove = false;
		//Zoom camera
		ZoomCamera();
		//@ Camera are no longer follow or control by key/arrow
		if(option == CameraOption.locked) {return;}
		//If cmaeraoption is follow
		if(option == CameraOption.follow)
		{	
			//Get the formation center
			Vector2 center = Manager.i.allie.FormationCenter();
			//Camera follow the formation ceneter
			cam.position = center;
			//Reset the camera's Z axis
			cam.position = new Vector3(cam.position.x, cam.position.y, -10);
		}
		//Only able to move camera if option are free
		if(option == CameraOption.free)
		{	
			//Reset the camera zoom when press key
			if (Input.GetKey(KeyCode.T)) {Camera.main.orthographicSize = defaultZoom;}
			//Move the camera position back to the center of formation
			if (Input.GetKey(KeyCode.R)) {cam.position = Manager.i.allie.FormationCenter();}
			//The camera moving direction
			Vector2 moveDirection = Vector3.zero;
			//@ Changing the camera moving direction base on key input
			if (Input.GetKey(KeyCode.UpArrow)) {moveDirection.y = 1;}
			if (Input.GetKey(KeyCode.DownArrow)) {moveDirection.y = -1;}
			if (Input.GetKey(KeyCode.LeftArrow)) {moveDirection.x = -1;}
			if (Input.GetKey(KeyCode.RightArrow)) {moveDirection.x = 1;}
			//Moving the camera with move direction with key speed
			MoveCamera(moveDirection.normalized, keySpeed);
		}
	}
	
	void LateUpdate()
	{
		//Only able to drag camera if camera is in free mode an drag is not lock
		if(option == CameraOption.free && !lockDrag) {DragCamera();}
		//Set the camera position
		cam.position = new Vector3
		(
			//Restrict the camera X position in the map's min and max X 
			Mathf.Clamp(cam.position.x, Manager.i.map.mapMin.x, Manager.i.map.mapMax.x),
			//Restrict the camera Y position in the map's min and max Y 
			Mathf.Clamp(cam.position.y, Manager.i.map.mapMin.y, Manager.i.map.mapMax.y),
			//Reset the camera Z
			-10
		);
	}

	void DragCamera()
	{
		//If the mouse has go out of bounds on Y axis
		if (Input.mousePosition.y > height - bound || Input.mousePosition.y < 0 + bound 
		//or the mouse has go out of bounds on X axis
		|| Input.mousePosition.x < 0 + bound || Input.mousePosition.x > width - bound)
		{
			//Get the direction from camera center to mouse
			Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - cam.position).normalized;
			//Move camera toward mouse using speed
			MoveCamera((Vector2)dir, dragSpeed);
		}
	}

	void MoveCamera(Vector2 direction, float speed)
	{
		//If camera are not moving
		if(!cameraMove)
		{
			//Move camera with speed toward the direction
			cam.position = (Vector2)cam.position + (Vector2)(direction * (speed * Time.deltaTime));
		}
		//Camera are now moving
		cameraMove = true;
	}

	void ZoomCamera()
	{
		//If the mouse ae scrolling
		if(Input.mouseScrollDelta.y != 0)
		{
			//Increase or decrease the orthographic size with zoom amount and speed
			Camera.main.orthographicSize -= Input.mouseScrollDelta.y * (zoomSpeed * Time.deltaTime);
			//Prevent the camera from zoom too far or too close
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomLimit.min, zoomLimit.max);
		}
	}
}
