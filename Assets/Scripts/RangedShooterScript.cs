using UnityEngine;
using System.Collections;

/// <summary>
/// Script of a ranged enemy
/// </summary>
public class RangedShooterScript : EnemyBase {
	//Serialized Fields
	public GameObject projectile;			//The projectile to use (must have a LaserScript or will crash)
	public float maxRange;					//Max range (if player is further than this move toward player)		No effect on projectile range
	public float minRange;					//Min range (if player is closer than this move away from player)	No effect on projecile range

	private GameObject player;						
	private Transform projectileSpawn;		//Empty child GameObject where the projectile spawns
	private Animator animator;				
	private int animFireHash;				//The hash for the Fire field in the animator
	private bool firing;					//Used to instantiate the projectile when charging animation re-begins				
	private float projectileSpeed;			//The speed of the projecile (taken from the projectile prefab)

	// Use this for initialization
	void Start () {
		SetUp ();
		player = GameObject.FindGameObjectWithTag ("Player");
		projectileSpawn = gameObject.transform.GetChild(0);
		animator = this.GetComponent<Animator> ();
		animFireHash = Animator.StringToHash ("Fire");
		projectileSpeed = projectile.GetComponent<LaserScript> ().speed;
	}

	//Called each frame
	void Update () {
		Vector2 direction = Vector2.zero;
		if (player != null) {				//Confirms that player is in the world
			direction = (Vector2)(player.transform.position - transform.position);
			targetAngle = Vector2.Angle (PredictedLocation (direction, player.rigidbody2D.velocity, projectileSpeed), Vector2.right);
		} else {
			direction = direction.normalized * (minRange + maxRange) / 2;
		}
		float distance = direction.magnitude;
		if (distance > maxRange) {
			targetVelocity = direction.normalized * speed;
		} else if (distance < minRange) {
			targetVelocity = direction.normalized * -speed;
		} else {
			targetVelocity = Vector2.zero;
		}
		if (direction.y < 0) {
			targetAngle = 360 - targetAngle;
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("PinkChargedAnim")){				//Is is charged, and is the shot lined up
			animator.SetBool(animFireHash, TargetAquired());
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PinkFiringAnim")) {			//Is it firing
			firing = true;
		}
		if (firing && animator.GetCurrentAnimatorStateInfo(0).IsName("PinkChargeAnim")) {	//Is it charging and was just firing, if so create the projectile
			Instantiate(projectile, projectileSpawn.position, transform.rotation);
			firing = false;
		}
	}
	
	// Tells the enemy to stop looking for the player when they die (prevents null pointer error)
	/// <summary>
	/// Called when the player is killed
	/// </summary>
	public override void PlayerKilled () {
		player = null;
	}

	//Used to stop firing if the shot isn't lined up (broken by aiming change so always returns true)
	bool TargetAquired () {
		return player != null;
		/*RaycastHit2D raycast = Physics2D.Raycast (rigidbody2D.position, (Vector2)projectileSpawn.position - rigidbody2D.position, 50, 1<<11);
		return raycast.collider != null;*/
	}

	//Predicts the player's location when the projectile will hit them
	Vector2 PredictedLocation (Vector2 position, Vector2 velocity, float speed) {
		//Quadratic formula for 
		//speed * time = sqrt((position.x + velocity.x * time)^2 + (position.y + velocity.y)^2)
		//Some algebra required
		float a = speed * speed - velocity.sqrMagnitude;
		float b = -2 * (position.x * velocity.x + position.y * velocity.y);
		float c = -position.sqrMagnitude;
		float minus = (-b - Mathf.Sqrt (b * b - 4 * a * c)) / (2 * a);
		float plus = (-b + Mathf.Sqrt (b * b - 4 * a * c)) / (2 * a);
		return position + velocity * (minus > 0 ? minus : plus);
	}
}
