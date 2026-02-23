using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public bool cameraMove;
	enum CameraOption {free, locked, follow} [SerializeField] CameraOption option;
	//% Locking drag control
	public bool lockDrag;
	[Tooltip("The size of the section that won't drag camera")]
	[SerializeField] float bound;
	[SerializeField] float defaultZoom;
	[SerializeField] FloatMinMax zoomLimit;
	public TMPro.TextMeshProUGUI optionDisplay, zoomAmount;
	float width, height;
	Transform cam;

	void Start()  
	{
		//Get the screen width an height and the camera transform
		width = Screen.width; height = Screen.height; cam = Camera.main.transform;
	}

	public void ResetCamera()
	{
		//Reset camera back to formation when map started
		cam.position = Manager.i.allie.FormationCenter();
		//Reset the camera's Z axis
		cam.position = new Vector3(cam.position.x, cam.position.y, -10);
	}

	void Update() 
	{
		//Display the zoom amnount
		zoomAmount.text = (int)Camera.main.orthographicSize + "";
		//Change camera option when press the camera option key
		if(Input.GetKeyDown(Keybind.i.GetKey("Switch Camera Mode"))) {ChangeOption();}
		//Camera are not moving
		cameraMove = false;
		//Zoom camera
		ZoomCamera();
		//@ Camera are no longer follow or control by key/arrow
		if(option == CameraOption.locked) {return;}
		//If cmaeraoption is follow
		if(option == CameraOption.follow)
		{
			//If there is allies
			if(Manager.i.allie.alliesComp.Count > 0)
			{
				//Get the formation center
				Vector2 center = Manager.i.allie.FormationCenter();
				//Camera follow the formation ceneter 
				cam.position = center;
				//Reset the camera's Z axis
				cam.position = new Vector3(cam.position.x, cam.position.y, -10);
			}
		}
		//Only able to move camera if option are free
		if(option == CameraOption.free)
		{	
			//Only able to drag camera if is not lock
			if(!lockDrag) {DragCamera();}
			//Reset the camera zoom when press reset zoom key
			if (Input.GetKey(Keybind.i.GetKey("Zoom Reset"))) {ResetZoom();}
			//Move the camera position back to the center of formation when press reset camera key
			if (Input.GetKey(Keybind.i.GetKey("Camera Center Formation"))) {cam.position = Manager.i.allie.FormationCenter();}
			//The camera moving direction
			Vector2 moveDirection = Vector3.zero;
			//@ Changing the camera moving direction base on it setting input
			if (Input.GetKey(Keybind.i.GetKey("Camera Up"))) {moveDirection.y = 1;}
			if (Input.GetKey(Keybind.i.GetKey("Camera Down"))) {moveDirection.y = -1;}
			if (Input.GetKey(Keybind.i.GetKey("Camera Left"))) {moveDirection.x = -1;}
			if (Input.GetKey(Keybind.i.GetKey("Camera Right"))) {moveDirection.x = 1;}
			//Moving the camera with move direction with camera speed
			MoveCamera(moveDirection.normalized, SettingsManager.i.Data.cameraMoveSpeed);
		}
	}
	
	void LateUpdate()
	{
		//Restrict the camera position when reached the border
		float redundant = Manager.i.map.sectionSize/2;

		cam.position = new Vector3
		(
			//Restrict the camera X position in the map's min and max X 
			Mathf.Clamp(cam.position.x, Manager.i.map.mapMin.x + redundant, Manager.i.map.mapMax.x + redundant),
			//Restrict the camera Y position in the map's min and max Y 
			Mathf.Clamp(cam.position.y, Manager.i.map.mapMin.y + redundant, Manager.i.map.mapMax.y + redundant),
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
			//Move camera toward mouse camera speed
			MoveCamera((Vector2)dir, SettingsManager.i.Data.cameraMoveSpeed);
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
			//Increase or decrease the orthographic size with zoom amount and it speed
			Camera.main.orthographicSize -= Input.mouseScrollDelta.y * (SettingsManager.i.Data.zoomSpeed * Time.deltaTime);
			//Prevent the camera from zoom too far or too close
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomLimit.min, zoomLimit.max);
		}
	}

	public void ChangeOption()
	{
		//Cycle through all the option
		option += 1; if((int)option > 2) {option = 0;}
		//Update the option display when after cycle
		optionDisplay.text = option.ToString();
	}

	//Reset camera size back to the default zoom
	public void ResetZoom() {Camera.main.orthographicSize = defaultZoom;}
}