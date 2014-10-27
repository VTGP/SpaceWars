using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for the Blinker Enemy
/// </summary>
public class BlinkerController : EnemyBase {
	//Serialized Fields
	public float timeBetweenBlinks;
	public float normalizedTimeOfBlink;
	public float blinkRange;
	public float attackRange;
	public float timeBetweenAttacks;
	public float attackTime;
	public int damage;
	
	private Vector2 direction;
	private float blinkCooldown;
	private Animator animator;
	private int blinkHash;
	private bool blink;
	private Vector2 blinkLocation;
	private GameObject player;
	private LineRenderer lineRenderer;
	private float attackCooldown;
	private float detectionRange;

	// Use this for initialization
	void Start () {
		SetUp ();
		blinkCooldown = 0;
		animator = gameObject.GetComponent<Animator> ();
		blinkHash = Animator.StringToHash ("Blink");
		player = GameObject.FindGameObjectWithTag ("Player");
		lineRenderer = this.GetComponent<LineRenderer> ();
		lineRenderer.enabled = false;
		detectionRange = this.GetComponent <CircleCollider2D> ().radius;
	}
	
	// Update is called once per frame
	void Update () {
		//Cools down the weapon
		if (attackCooldown > 0) {
			attackCooldown -= Time.deltaTime;
			if (attackCooldown < timeBetweenAttacks - attackTime) {
				lineRenderer.enabled = false;
			}
		}
		//Cools down blink
		if (blinkCooldown > 0) {
			blinkCooldown -= Time.deltaTime;
		}
		//Checks animation for proper time to blink
		if (blink && animator.GetCurrentAnimatorStateInfo (0).IsName("BlinkerBlinkAnim")
		    	&& animator.GetCurrentAnimatorStateInfo (0).normalizedTime > normalizedTimeOfBlink) {
			blink = false;
			Blink(blinkLocation);
		}
		//Handles attacking and player related data
		if (player != null) {			//Confirms that player is in the world
			direction = (Vector2)(player.transform.position - transform.position);
			if (attackCooldown <= 0 && !blink && direction.magnitude <= attackRange) {
				lineRenderer.enabled = true;
				lineRenderer.SetPosition(1, direction.magnitude * Vector2.right);
				player.GetComponent<PlayerShipController> ().Damage(damage);
				attackCooldown = timeBetweenAttacks;
			}
		}
		//Maintains a distance within attack range and outside of the detection range from player
		if (direction.magnitude > attackRange) {
			targetVelocity = direction.normalized * speed;
		} else if (direction.magnitude < detectionRange) {
			targetVelocity = -direction.normalized * speed;
		} else {
			targetVelocity = Vector2.zero;
		}
		//Faces player
		targetAngle = Vector2.Angle (direction, Vector2.right) ;		//
		if (direction.y < 0) {											// Gets rotation from a vector
			targetAngle = 360 - targetAngle;							//
		}
	}

	//Called when something enters the trigger collider
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "PlayerProjectile" && blinkCooldown <= 0) {
			BlinkAway(coll.gameObject.rigidbody2D);
		}
	}

	/// <summary>
	/// Blinks away from the specified rigidbody
	/// </summary>
	/// <param name="r">Rigidbody to avoid</param>
	// More Work needed on strategicly dodging
	void BlinkAway (Rigidbody2D r) {
		animator.SetTrigger (blinkHash);
		blink = true;
		blinkLocation = rigidbody2D.position + blinkRange * Random.insideUnitCircle;
	}

	/// <summary>
	/// Blinks to the specified location.
	/// </summary>
	/// <param name="location"> The location to blink to in World</param>
	void Blink (Vector2 location) {
		transform.position = location;
		blinkCooldown = timeBetweenBlinks;
	}
}