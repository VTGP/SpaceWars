using UnityEngine;
using System.Collections;

public class ShipBase : MonoBehaviour {
	public int healthMax;
	public float speed;
	public float acceleration;
	public float rotationalCap;
	protected Vector2 targetVelocity;
	protected float targetAngle;
	protected int health;
	private float force;


	// CALL THIS IN START OF SUBCLASSES
	public virtual void SetUp () {
		health = healthMax;
		force = acceleration * rigidbody2D.mass;
	}

	public void Damage (int amount) {
		health -= amount;
		if (health <= 0) {
			Destroy(gameObject);
		}
	}

	//If a derived class overrides FixedUpdate, this must be called
	void FixedUpdate () {
		rotate (targetAngle);
		rigidbody2D.AddForce((targetVelocity - rigidbody2D.velocity).normalized * force);
	}
	/*protected void UpdateTransform (Vector2 targetVelocity, float targetAngle) {
		velocity.x = accelerate (velocity.x, targetVelocity.x);
		velocity.y = accelerate (velocity.y, targetVelocity.y);
		transform.position += velocity * Time.deltaTime;
		transform.rotation = Quaternion.Euler (0, 0, 
			rotate(transform.rotation.eulerAngles.z, targetAngle));
		if (isBounded) {
			Vector3 location = transform.position;
			if (location.x > bounds.xMax) {
				location.x = bounds.xMax;
				velocity.x = 0;
			}
			if (location.x < bounds.xMin) {
				location.x = bounds.xMin;
				velocity.x = 0;
			}
			if (location.y > bounds.yMax) {
				location.y = bounds.yMax;
				velocity.y = 0;
			}
			if (location.y < bounds.yMin) {
				location.y = bounds.yMin;
				velocity.y = 0;
			}
			transform.position = location;
		}
	}

	float accelerate (float value, float goal) {
		if (value == goal) {
			return value;
		}
		if (value > goal) {
			return Mathf.Max (value - acceleration * Time.deltaTime, goal);
		} else {
			return Mathf.Min (value + acceleration * Time.deltaTime, goal);
		}
	}*/

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
			rigidbody2D.rotation += Time.deltaTime * rotationalCap * sign;
		}
	}
}
