using UnityEngine;
using System.Collections;

/// <summary>
/// All ships should extend this, or a derived class of this
/// </summary>
public class ShipBase : MonoBehaviour {
	//Serialized Variables
	public int healthMax;				//Max and starting health
	public float speed;					//Max speed in units / second
	public float acceleration;			//Max acceleration in units / second ^2
	public float rotationalCap;			//Max rotational speed in degrees / second

	//Accessable in derived classes
	protected Vector2 targetVelocity;	//The intended velocity vector, set this in derived class's OnUpdate method
	protected float targetAngle;		//The intended angle in degrees from the x axis, set this in derived class's OnUpdate method
	protected int health;				//Current health

	private float force;				//Used to move the object


	// CALL THIS IN START OF SUBCLASSES
	public virtual void SetUp () {
		health = healthMax;
		force = acceleration * rigidbody2D.mass;
	}

	//Call to damage the player
	public void Damage (int amount) {
		health -= amount;
		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}

	//If a derived class needs to call FixedUpdate, this won't be called
	//Called each physics frame
	void FixedUpdate () {
		rotate (targetAngle);
		rigidbody2D.AddForce((targetVelocity - rigidbody2D.velocity).normalized * force);
	}

	//Helper method for moving toward the goal angle
	void rotate (float goal) {
		float diff = Mathf.Abs(rigidbody2D.rotation - goal);
		if (diff == 0) {
			return;
		}
		if (diff > 180) {
			diff = 360 - diff;
		}
		if (diff < rotationalCap * Time.deltaTime) {
			rigidbody2D.rotation = goal;
		} else {
			float sign = (Mathf.Abs(goal - rigidbody2D.rotation) == diff) ? Mathf.Sign(goal - rigidbody2D.rotation) : Mathf.Sign(rigidbody2D.rotation - goal);
			rigidbody2D.rotation += Time.fixedDeltaTime * rotationalCap * sign;
		}
	}
}
