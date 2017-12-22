using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// General idea of how the script is supposed to work:
    /// - The player will have a radius around it wherein NPCs are active.
    /// - If NPCs leave this radius they will deactivate.
    /// - If NPCs renter this radius they will reactivate.
    /// - Waves Will be triggered by Time with some difficulty factor.
    /// </summary>

public class SpawningScript : MonoBehaviour {

    private float radius;
	private float alienspawnradius;
    private float spawnradius;
    private int difficulty;
    private GameObject player;
    public List<GameObject> Enemies = new List<GameObject>();

    // Enemy Prefabs:

    [SerializeField] GameObject AlienWithGun;

    // Use this for initialization
    void Start () {
        radius = 100;
        spawnradius = 4;
		alienspawnradius = 40;
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {

        // Random Spawing:
        if (Time.fixedTime > 1)
        {
            if (Time.fixedTime % 1 == 0)
            {
                Vector3 center = player.transform.position;
				Vector3 pos = RandomCircle(center, alienspawnradius);
				Vector3 pos2 = pos;
				pos2.y = pos2.y + spawnradius;
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
                if (! Physics.CheckSphere(pos2, spawnradius)) { 
                    Enemies.Add(Instantiate(AlienWithGun, pos, rot));
                }

            }
        }

        if (Time.fixedTime % 2 == 0) { 
            for (int i =0; i < Enemies.Count ; i++)
            {
                if ((player.transform.position - Enemies[i].transform.position).magnitude > radius && Enemies[i].activeSelf == true)
                {
                    Enemies[i].SetActive(false);
                }
                if ((player.transform.position - Enemies[i].transform.position).magnitude < radius && Enemies[i].activeSelf == false)
                {
                    Enemies[i].SetActive(true);
                }
            }
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }
}
