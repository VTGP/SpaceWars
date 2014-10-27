using UnityEngine;
using System.Collections;

/// <summary>
/// Handles Levels and EXP
/// </summary>
public class Manager : MonoBehaviour {
	//Serializable Fields
	public GameObject[] enemies;				//Array of Enemy Prefabs
	public int levelStart;						//Zero Index Level to Start On
	public float startDelay;					//Time between waves
	public Vector2 spawnBoxCorner;				//The box enemies spawn on

	public float expPercent {
		get{ return (float)exp / maxExp;}
	}

	private float timeToSpawn;					//Time until next spawn
	private int level;							
	private int levelCap;						//Maximum Level, set by number of level objects
	private LevelScript[] levels;				//All Levels, must be attatched to this object to be found, must have all numbers 0-n or there might be an error
	private Vector3 position;					//Position to spawn the enemy at
	private bool initialSpawn = true;			//Used to spawn the initial wave of enemies
	private int maxExp;
	private int maxEnemies;
	private int enemyCount;
	private int exp;

	// Use this for initialization
	//Makes some assumptions about having all levels in order
	void Start () {
		levels = this.GetComponents<LevelScript> ();
		LevelScript[] duplicate = levels;
		for (int c = 0, n = levels.Length; c < n; c++) {
			for (int d = 0; d < n; d++) {
				if (duplicate[d].level == c) {
					duplicate[d] = levels[c];
					continue;
				}
			}
		}
		levelCap = levels.Length;
		level = levelStart;
		Begin ();
	}
	
	// Update is called once per frame
	void Update () {
		timeToSpawn -= Time.deltaTime;
		if (exp >= maxExp) {
			LevelUp();
		}
		if (timeToSpawn <= 0 && enemyCount < maxEnemies) {
			if (initialSpawn) {																			//Initial Spawn
				for (int e = 0, t = enemies.Length; e < t; e++) {
					for (int c = 0, n = levels[level].startCount[e]; c < n; c++) {
						Instantiate(enemies[e], levels[level].RandomLoction(), Quaternion.identity);
					}
				}
				initialSpawn = false;
			} else {																					//All other Spawns
				Instantiate (enemies[levels [level].SpawnRandomly (out position)], position, Quaternion.identity);
				timeToSpawn = levels[level].timeBetweenSpawns;
			}
		}
	}
	
	//Add Level Up screen calls here when implimented
	/// <summary>
	/// Clears map, increments the level, and Starts level.
	/// </summary>
	public void LevelUp () {
		Clear ();
		level++;
		if (level >= levelCap) {
			Win ();
		}
		Begin ();
	}

	/// <summary
	/// Clears the map
	/// </summary>
	public void Clear () {
		ShipBase[] ships = FindObjectsOfType<EnemyBase> ();
		if (ships.Length != 0) {
			foreach (EnemyBase ship in ships) {
				if (ship != null) {
					Destroy (ship.gameObject);
				}
			}
		}
		exp = 0;
	}

	/// <summary>
	/// Starts the level
	/// </summary>
	public void Begin () {
		exp = 0;
		maxExp = levels [level].expRequired;
		maxEnemies = levels [level].maxEnemies;
		timeToSpawn = startDelay;
		initialSpawn = true;
	}

	//Called when you win
	void Win() {
		//YOU WIN, enjoy the null pointer error / array out of index error you are about to get
	}

	/// <summary>
	/// Called as enemies are destroyed
	/// </summary>
	/// <param name="destroyed">
	/// The enemy that was destroyed.
	/// </param>
	public void Destroyed(EnemyBase destroyed) {
		exp += destroyed.expYield;
		enemyCount -= destroyed.enemyCapCount;
	}

	///<summary>
	/// Called as enemies spawn
	/// </summary>
	public void Spawned (EnemyBase spawned) {
		enemyCount += spawned.enemyCapCount;
	}

	///<summary>
	/// Reduces exp, returns false if doing so would result in less than 0 exp
	/// </summary>
	public bool RemoveExp(int amount) {
		if (exp < amount) {
			return false;
		} else {
			exp -= amount;
			return true;
		}
	}
}
