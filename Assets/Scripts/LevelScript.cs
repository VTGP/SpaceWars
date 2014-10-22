using UnityEngine;
using System.Collections;

/// <summary>
/// Used to store spawning and upgrade information for the level
/// </summary>
public class LevelScript : MonoBehaviour {
	//Serialized Fields
	//All arrays should be the same size as the Manager's enemies[], and enemies are in the same order
	public int level;						//The Number of this level (Zero Index)
	public float[] spawnRates;				//The spawn rates for each enemy should add to 1
	public int[] startCount;				//The amount of each enemy to start the wave with
	public float timeBetweenSpawns;			//Time between spawns
	public int maxEnemies;					//Maximum enemies in the map (some may count for more than one or 0)
	public int expRequired;					//Exp required to advance

	private float[] spawnChances;			//A converted spawn rate array (cumulative sums)
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

	/// <summary>
	/// Selects an enemy by index (return) 
	/// and a location to spawn them at (out)
	/// </summary>
	/// <returns>The interger index of the enemy.</returns>
	/// <param name="location">The location to spawn at (out)</param>
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

	/// <summary>
	/// Generates a random location to spawn at.
	/// </summary>
	/// <returns>The location to spawn at.</returns>
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
