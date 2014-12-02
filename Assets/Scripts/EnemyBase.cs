using UnityEngine;
using System.Collections;

/// <summary>
/// All enemies should extend this
/// </summary>
public abstract class EnemyBase : ShipBase {
	//Serialized Fields
	public int expYield;			//How much EXP this is worth
	public int enemyCapCount;		//How much this counts to the enemy count (to keep from cluttering the map)
	public int collisionDamage;					//The amount of damage dealt upon colliding

	//CALL IN THE START METHOD FOR EACH SUBCLASS
	public override void SetUp () {
		base.SetUp ();
		GameObject manager = GameObject.FindWithTag ("GameController");
		if (manager != null) {
			manager.GetComponent<Manager> ().Spawned (this);
		}
	}

	//Override in subclasses to prevent null pointers when the player dies
	/// <summary>
	/// Called when the player is killed
	/// </summary>
	public virtual void PlayerKilled () {

	}

	//Called upon colliding with another object
	void OnCollisionEnter2D (Collision2D collision) {
		ShipBase ship = collision.gameObject.GetComponent<ShipBase> ();
		if (ship != null) {
			ship.Damage(collisionDamage);
			Destroy (gameObject);
		}
	}

	//Called when the object is destroyed
	void OnDestroy () {
		GameObject manager = GameObject.FindWithTag ("GameController");
		if (manager != null && this.health <= 0) {					//Confirms that the enemy ran out of health rather than being deleted by levelUp, container, ect.
			manager.GetComponent<Manager> ().Destroyed (this);
		}
	}
}
