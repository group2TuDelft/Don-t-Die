using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 1000f;

	Vector3 movement;
	Animator anim;
	CharacterController controller;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

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
	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);

		Turn ();

	}

	void Move(float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.forward + movement);
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
		if (Input.GetKey ("up")) {
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
