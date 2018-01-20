using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class AnalyticsManager : MonoBehaviour {
	public GameObject Inventory;
	public GameObject Player;


	float deathTimer = 0.0f;
	float timer;
	PlayerHealth playerHealth;
	WeaponSelection WeaponSelection;
	List<Weapon> data;


	void Start () {
		WeaponSelection = Inventory.GetComponent<WeaponSelection> ();
		data = new List<Weapon> ();
		playerHealth = Player.GetComponent<PlayerHealth> ();
		timer = 0.0f;
	}
		
	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= 1.0f) {
			UpdateWeapons ();
		}
		if (playerHealth.getCurrentHealth() >= 0) {
			deathTimer += Time.deltaTime;
		}
	}


	// Update is called once per frame
	void UpdateWeapons () {
		string weaponName = WeaponSelection.GetWeaponName() ;
		int weaponId = WeaponSelection.GetWeaponID();
		bool found = false;
		if( weaponId != -1){
			for (int i = 0; i < data.Count ; i++) {
				if (data [i].getName() == weaponName) {
					data [i].UpdateTimer (timer);
					found = true;
				}
			}
			if (found == false) {
				Weapon w = new Weapon (weaponId, weaponName, 0.0f);
				data.Add (w);
			}
		}

		if (playerHealth.getCurrentHealth () <= 0) {
			reportDeath ();
		}

		timer = 0.0f;
	}

	void reportDeath(){

		string path = "Assets/Prototypes/Sidi/Analytics/Death.txt";
		string path1 = "Assets/Prototypes/Sidi/Analytics/Killer.txt";
		string path2 = "Assets/Prototypes/Sidi/Analytics/Score.txt";
		string killer = playerHealth.killer;
		//Report Death time
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine(string.Format("{0:N3}", deathTimer) + ",");
		writer.Close();

		//Report The killer 
		StreamWriter writer1 = new StreamWriter(path1, true);
		writer1.WriteLine(killer + ",");
		writer1.Close();

		//Report The Score
		StreamWriter writer2 = new StreamWriter(path2, true);
		writer2.WriteLine(ScoreManager.score + ",");
		writer2.Close();
		this.enabled = false;

		// Report the weapon statistics on weapon use

		WeaponReport ();

	}

	void WeaponReport(){
		string path = "Assets/Prototypes/Sidi/Analytics/WeaponReport.txt" ;
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine (" ");
		for (int i = 0; i < data.Count; i++) {
			writer.WriteLine (" ");
			writer.Write (data [i].getId ()+ ", ");
			writer.Write (data [i].getName ()+ ", ");
			writer.Write (Math.Round((double)data [i].getTime ()/deathTimer ,2)+ ", ");
		}

		writer.Close ();
	}
}
