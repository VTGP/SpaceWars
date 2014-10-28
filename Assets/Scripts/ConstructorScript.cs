using UnityEngine;
using System.Collections;

/// <summary>
/// Runs the constructor enemy
/// </summary>
public class ConstructorScript : EnemyBase {
	public GameObject spawn;		//Enemy to spawn
	public float circleRadius;		//Radius of the path to make around origin

	private Animator animator;
	private bool shouldSpawn;
	private int idleNameHash;
	private Transform spawnPoint;
	private int animName;
	// Use this for initialization
	void Start () {
		SetUp ();
		animator = this.GetComponent <Animator> ();
		idleNameHash = Animator.StringToHash ("Base Layer.ConstructorIdleAnim");
		spawnPoint = this.GetComponentInChildren <Transform> ();
		animator.SetBool ("Construct", true);
	}
	
	// Update is called once per frame
	void Update () {
		animName = animator.GetCurrentAnimatorStateInfo (0).nameHash;
		if (shouldSpawn &&  animName == idleNameHash) {
			Instantiate (spawn, spawnPoint.position, spawnPoint.rotation);
			shouldSpawn = false;
		} else if (!(shouldSpawn || animName == idleNameHash)) {
			shouldSpawn = true;
		}
		targetAngle = Vector2.Angle (Vector2.right, rigidbody2D.position);
		if (rigidbody2D.position.y < 0) {
			targetAngle = 360 - targetAngle;
		}
		targetAngle -= 90;
		if (rigidbody2D.position.sqrMagnitude > circleRadius * circleRadius) {
			targetVelocity = rigidbody2D.position.normalized * -speed;
		} else {
			targetVelocity = (new Vector2(rigidbody2D.position.y, -rigidbody2D.position.x)).normalized * speed;
		}
	}
}
