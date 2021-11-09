using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] bool lockDrag, lockKey;
	[Tooltip("The size of the section that won't drag camera")]
	[SerializeField] float bound;
	[SerializeField] float dragSpeed, keySpeed, zoomSpeed, defaultZoom;
	[SerializeField] FloatMinMax zoomLimit;
	float width, height;
	Transform cam;

	//Get the screen width an height and the camera transform
	void Start()  {width = Screen.width; height = Screen.height; cam = Camera.main.transform;}

	void Update() 
	{
		//Reset the camera zoom when press key
		if (Input.GetKey(KeyCode.C)) {Camera.main.orthographicSize = defaultZoom;}
		//Zoom camera
		ZoomCam();
		//@ Stop camera key control if it lock
		if(lockKey) {return;}
		//@ Move camera toward key input using speed
		if (Input.GetKey(KeyCode.UpArrow)) {MoveCamera(Vector2.up, keySpeed);}
		if (Input.GetKey(KeyCode.DownArrow)) {MoveCamera(Vector2.down, keySpeed);}
		if (Input.GetKey(KeyCode.LeftArrow)) {MoveCamera(Vector2.left, keySpeed);}
		if (Input.GetKey(KeyCode.RightArrow)) {MoveCamera(Vector2.right, keySpeed);}
	}
	
	void LateUpdate()
	{
		//@ Stop drag camera movement if it lock
		if(lockDrag) {return;}
		//If the mouse has go out of bounds
		if (Input.mousePosition.y > height - bound || Input.mousePosition.y < 0 + bound || Input.mousePosition.x < 0 + bound || Input.mousePosition.x > width - bound)
		{
			//Move camera toward mouse direction using speed
			MoveCamera((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - cam.position).normalized, dragSpeed);
		}
	}

	void MoveCamera(Vector2 direction, float speed)
	{
		//Move camera with speed toward the direction
		cam.position = (Vector2)cam.position + (Vector2)(direction * (speed * Time.deltaTime));
		//Reset the camera's Z axis
		cam.position = new Vector3(cam.position.x, cam.position.y, -10);
	}

	bool zommed; void ZoomCam()
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
