using UnityEngine;
using System.Collections;

//Used for straight moving projectiles
public class LaserScript : MonoBehaviour {
	//Serializable Fields
	public float speed;
	public int damage;

	// Use this for initialization
	void Start () {
		float angle = Mathf.Deg2Rad * rigidbody2D.rotation;
		rigidbody2D.velocity = speed * new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Called on collision with another object
	void OnCollisionEnter2D (Collision2D collision) {
		ShipBase ship = collision.gameObject.GetComponent<ShipBase> ();
		if (ship != null) {
			ship.Damage(damage);
		}
		if (collision.gameObject.name != "Container") {		//Prevents collisions with the container from registering on this end
			Destroy(gameObject);
		}
	}
}
