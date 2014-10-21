using UnityEngine;
using System.Collections;

/// <summary>
/// Used to store spawning and upgrade information for the level
/// </summary>
public class LevelScript : MonoBehaviour {
	public int level;
	public float[] spawnRates;
	public int[] startCount;
	public float timeBetweenSpawns;
	public int maxEnemies;
	public int expRequired;
	private float[] spawnChances;
	private Vector2 spawnBoxCorner;
	// Use this for initialization
	void Start () {
		spawnBoxCorner = GetComponent<Manager> ().spawnBoxCorner;
		spawnChances = new float[spawnRates.Length];
		float lower = 0;
		for (int c = spawnChances.Length - 1; c >= 0; c--) {
			spawnChances[c] = lower;
			lower = spawnRates[c];
		}
	}

	public int SpawnRandomly(out Vector3 location) {
		location = RandomLoction ();
		float rand = Random.value;
		for (int c = 0, n = spawnChances.Length; c < n; c++) {
			if (rand > spawnChances[c]) {
				return c;
			}
		}
		return 0;
	}

	public Vector3 RandomLoction () {
		float rand = 2 * Mathf.PI * Random.value;
		Vector2 v = new Vector2 (Mathf.Cos (rand), Mathf.Sin (rand));
		if (Mathf.Abs (v.x) > Mathf.Abs (v.y)) {
			v.y /= v.x;
			v.x = Mathf.Sign(v.x);
		} else {
			v.x /= v.y;
			v.y = Mathf.Sign(v.y);
		}
		return new Vector2 (v.x * spawnBoxCorner.x, v.y * spawnBoxCorner.y);
	}
}
