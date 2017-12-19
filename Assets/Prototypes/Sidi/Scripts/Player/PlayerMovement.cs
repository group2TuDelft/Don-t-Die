using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 8f;
	public float smoothing = 5f;

	[SerializeField] Camera mainCamera;
	[SerializeField] Camera FPC;
	Vector3 movement;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;
	float h;
	float v;
	float mouse_x;
	float mouse_y;
	bool direction;
	 

	void Awake()
	{
		floorMask = LayerMask.GetMask ("Floor");
		playerRigidbody = GetComponent<Rigidbody>();


	}

	void Update()
	{
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		mouse_x = Input.GetAxisRaw ("Mouse X");
		mouse_y = Input.GetAxisRaw ("Mouse Y");
	}
		
	void FixedUpdate()
	{
		if (Input.GetKey (KeyCode.X)) {
			mainCamera.enabled = false;
			FPC.enabled = true;
			Vector3 newRotation = transform.localEulerAngles;
			newRotation.y = transform.localEulerAngles.y + mouse_x;
			transform.localEulerAngles = newRotation;
			Move ();
		} else {
			mainCamera.enabled = true;
			FPC.enabled = false;
			Turn ();
			Move ();
		}
	}

	void Move()
	{	
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)){
			direction = true;
			speed = 8;
		}
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)){
			direction = false;
			speed = 6;
		}
		if (direction == true) {
			if (v == 1 && Input.GetKey (KeyCode.LeftControl)) {
				speed = 17;
			} else {
				speed = 8;
			}
			movement = transform.forward * v * speed * Time.deltaTime;
			playerRigidbody.MovePosition (playerRigidbody.position + movement);
		} else {
			movement.Set(h,0f,0f);
			movement *= speed * Time.deltaTime;
			playerRigidbody.transform.Translate (movement);
		}
	}

	void Turn()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidbody.MoveRotation (newRotation);
		}
	}
}
