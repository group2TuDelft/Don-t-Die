using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 10f;
	public float smoothing = 5f;

	Vector3 movement;
	Animator anim;
	CharacterController controller;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;
	float h;
	float v;
	bool direction;
	 

	void Awake()
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
		controller = GetComponent <CharacterController>();
	}

	void Update()
	{
		SetAnimation ();
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
	}
		
	void FixedUpdate()
	{

		Move ();

		Turn ();

	}

	void Move()
	{
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)){
			direction = true;
		}
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)){
			direction = false;
		}
		if (direction == true) {
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
	void SetAnimation () {
		if (Input.GetKey (KeyCode.W)) {
			if (Input.GetKey (KeyCode.LeftControl)) {
				anim.SetBool ("AnimRun", true);
			} else {
				anim.SetBool ("AnimWalk", true);
				anim.SetBool ("AnimRun", false);
			} 
		} else { 
			anim.SetBool ("AnimWalk", false);
			anim.SetBool ("AnimRun", false);
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			if (anim.GetBool ("AnimHasGun") == true) {
				anim.SetBool ("AnimHasGun", false);
			} else {
				anim.SetBool ("AnimHasGun", true);
			}
		}
		if (Input.GetKey (KeyCode.Mouse0)) {
			anim.SetBool ("AnimShooting", true);
		} else {
			anim.SetBool ("AnimShooting", false);
		}
	}

}
