using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatMap : MonoBehaviour {
	
	public GameObject heat;
	public GameObject deathHeat;

	public GameObject AnalyticsCamera;
	public int resWidth = 2550; 
	public int resHeight = 3300;


	float interval; 
	Transform transform;
	float timer;
	PlayerHealth playerHealth;
	bool dead = false;
	List<Vector3> positions;
	Camera camera;
	bool done = false;
	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		interval = 20*Time.deltaTime;
		transform = player.GetComponent<Transform> ();
		playerHealth = player.GetComponent<PlayerHealth> ();
		timer = 0.0f;
		positions = new List<Vector3> ();
		camera = AnalyticsCamera.GetComponent<Camera> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		if (timer > interval && done == false) {
			addHeat ();
		}
	}

	void addHeat(){
		timer = 0.0f;
		Debug.Log (playerHealth.getCurrentHealth ());
		if (playerHealth.getCurrentHealth () > 0) {
			positions.Add (player.transform.position);
		} else {
			done = true;
		}
		if (done == true) {
			screenshot();
		}
	}

	public static string ScreenShotName(int width, int height) {
		return string.Format("{0}/Prototypes/Sidi/Analytics/Screenshots/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
		

	void screenshot(){
		// draw the Map

		for(int i = 0; i < positions.Count ; i++){
			Instantiate (heat, positions[i], Quaternion.identity);

		}
		Instantiate (deathHeat, player.transform.position, Quaternion.identity);

		// takes a screenshot of it 
		camera.enabled = true;
		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
		camera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		camera.targetTexture = null;
		RenderTexture.active = null; 
		Destroy(rt);
		byte[] bytes = screenShot.EncodeToPNG();
		string filename = ScreenShotName(resWidth, resHeight);
		System.IO.File.WriteAllBytes(filename, bytes);
		camera.enabled = false;
	}
}
