using UnityEngine;
using System.Collections;

public class CrasherControler : EnemyBase {
	public int damage;
	private GameObject player;
	private Vector2 direction;
	// Use this for initialization
	void Start () {
		SetUp ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		if (player != null) {
			direction = (Vector2)(player.transform.position - transform.position);
		}
		targetVelocity = direction.normalized * speed;
		targetAngle = Vector2.Angle (direction, Vector2.right) ;
		if (direction.y < 0) {
			targetAngle = 360 - targetAngle;
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		ShipBase ship = collision.gameObject.GetComponent<ShipBase> ();
		if (ship != null) {
			ship.Damage(damage);
			Destroy (gameObject);
		}
	}

	public override void PlayerKilled () {
		player = null;
	}
}
