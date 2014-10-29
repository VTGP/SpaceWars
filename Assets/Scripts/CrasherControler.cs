using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for enemies that just run into you
/// </summary>
public class CrasherControler : EnemyBase {
	//Serialized Fields
	public int damage;					//The amount of damage dealt upon colliding

	private GameObject player;

	// Use this for initialization
	void Start () {
		SetUp ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	//Called each frame
	void Update () {
		Vector2 direction = Vector2.zero;
		if (player != null) {			//Confirms that player is in the world
			direction = (Vector2)(player.transform.position - transform.position);
		}
		targetVelocity = direction.normalized * speed;
		targetAngle = Vector2.Angle (direction, Vector2.right) ;		//
		if (direction.y < 0) {											// Gets rotation from a vector
			targetAngle = 360 - targetAngle;							//
		}																//
	}

	//Called upon colliding with another object
	void OnCollisionEnter2D (Collision2D collision) {
		ShipBase ship = collision.gameObject.GetComponent<ShipBase> ();
		if (ship != null) {
			ship.Damage(damage);
			Destroy (gameObject);
		}
	}
	
	/// <summary>
	/// Tells the enemy to stop looking for the player when they die
	/// </summary>
	public override void PlayerKilled () {
		player = null;
	}
}
