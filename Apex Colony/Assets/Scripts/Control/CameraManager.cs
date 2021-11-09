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
		//Reset camera back to formation when map started
		cam.position = Manager.i.allie.FormationCenter();
	}

	void Update() 
	{
		//Reset the camera zoom when press key
		if (Input.GetKey(KeyCode.T)) {Camera.main.orthographicSize = defaultZoom;}
		//Move the camera position back to the center of formation
		if (Input.GetKey(KeyCode.R)) {cam.position = Manager.i.allie.FormationCenter();}
		//Zoom camera
		ZoomCamera();
		//Camera are not moving
		cameraMove = false;
		//@ Stop camera is option are not free
		if(option != CameraOption.free) {return;}
		//@ Move camera toward key input using speed and camera are moving
		if (Input.GetKey(KeyCode.UpArrow)) {MoveCamera(Vector2.up, keySpeed);cameraMove = true;}
		if (Input.GetKey(KeyCode.DownArrow)) {MoveCamera(Vector2.down, keySpeed);cameraMove = true;}
		if (Input.GetKey(KeyCode.LeftArrow)) {MoveCamera(Vector2.left, keySpeed);cameraMove = true;}
		if (Input.GetKey(KeyCode.RightArrow)) {MoveCamera(Vector2.right, keySpeed);cameraMove = true;}
	}
	
	void LateUpdate()
	{
		//@ Stop camera is option are not free
		if(option != CameraOption.free || lockDrag) {return;}
		//If the mouse has go out of bounds on Y axis
		if (Input.mousePosition.y > height - bound || Input.mousePosition.y < 0 + bound 
		//or the mouse has go out of bounds on X axis
		|| Input.mousePosition.x < 0 + bound || Input.mousePosition.x > width - bound)
		{
			//Camera moving
			cameraMove = true;
			//Get the direction from camera center to mouse
			Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - cam.position).normalized;
			//Move camera toward mouse using speed
			MoveCamera((Vector2)dir, dragSpeed);
		}
	}

	void MoveCamera(Vector2 direction, float speed)
	{
		//Move camera with speed toward the direction
		cam.position = (Vector2)cam.position + (Vector2)(direction * (speed * Time.deltaTime));
		//Reset the camera's Z axis
		cam.position = new Vector3(cam.position.x, cam.position.y, -10);
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
