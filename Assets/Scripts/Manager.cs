using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public GameObject[] enemies;
	public int levelStart;	//Zero Index
	public float startDelay;
	public Vector2 spawnBoxCorner;
	public float expPercent {
		get{ return (float)exp / maxExp;}
	}
	private float timeToSpawn;
	private int level;
	private int levelCap;
	private LevelScript[] levels;
	private Vector3 position;
	private bool initialSpawn = true;
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
			if (initialSpawn) {
				for (int e = 0, t = enemies.Length; e < t; e++) {
					for (int c = 0, n = levels[level].startCount[e]; c < n; c++) {
						Instantiate(enemies[e], levels[level].RandomLoction(), Quaternion.identity);
					}
				}
				initialSpawn = false;
			} else {
				Instantiate (enemies[levels [level].SpawnRandomly (out position)], position, Quaternion.identity);
				timeToSpawn = levels[level].timeBetweenSpawns;
			}
		}
	}

	public void LevelUp () {
		Clear ();
		level++;
		if (level >= levelCap) {
			Win ();
		}
		Begin ();
	}

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

	public void Begin () {
		exp = 0;
		maxExp = levels [level].expRequired;
		maxEnemies = levels [level].maxEnemies;
		timeToSpawn = startDelay;
	}
	
	void Win() {
		//YOU WIN
	}

	public void Destroyed(EnemyBase destroyed) {
		exp += destroyed.expYield;
		enemyCount -= destroyed.enemyCapCount;
	}

	public void Spawned (EnemyBase spawned) {
		enemyCount += spawned.enemyCapCount;
	}
}
