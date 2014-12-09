using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Player
/// </summary>
public class PlayerShipController : ShipBase {
	//Serialized Fields
	public float timePerShot;		//Seconds per shot
	public float timePerCharge;
	public float chargeDuration;
	public GameObject laser;		//The projectile to use
	public float rapidFireRate;

	public enum LaserPowerUp {
		Default,
		RapidFire,
		Multishot,
		Beam
	}

	//Public Access to health
	public float healthPercent {
		get{return Mathf.Max((float)health /healthMax, 0);}
	}

	private Rect cameraBounds;			//Camera point to world point convertion...thing
	private float fireTime;				//Timer to when it can fire again (0 when ready)
	private float chargeTime;
	private bool isInvulnerable = false;
	private Camera mainCamera;			
	private Manager manager;
	private LaserPowerUp laserPowerUp = LaserPowerUp.RapidFire;
	// Use this for initialization
	void Start () {
		SetUp ();
		mainCamera = GameObject.Find ("Main Camera").camera;		//Sets up the camera point to world point method
		cameraBounds = new Rect (-mainCamera.orthographicSize * mainCamera.aspect, -mainCamera.orthographicSize,
			2 * mainCamera.orthographicSize * mainCamera.aspect, 2 * mainCamera.orthographicSize);
		manager = GameObject.Find ("Manager").GetComponent <Manager> ();
	}

	//Called each frame
	void Update () {
		Vector2 mouseDelta = (CameraToWorld (Input.mousePosition) - (Vector2)transform.position).normalized;		//Normalized
		targetVelocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * speed; 		//Retrieves input from the axis variables
		targetAngle = mouseDelta.y < 0 ? 360 - Vector2.Angle(mouseDelta, Vector2.right) : Vector2.Angle(mouseDelta, Vector2.right);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
		}
		if (chargeTime > 0) {
			if (isInvulnerable && chargeTime < timePerCharge - chargeDuration) {
				isInvulnerable = false;
			}
			chargeTime -= Time.deltaTime;
		}
		if (Input.GetButton ("Fire1")) {				//Is Fire down?
			if (fireTime <= 0) {
				switch (laserPowerUp) {
				case LaserPowerUp.Default:
					Instantiate(laser, transform.position + (Vector3)mouseDelta, transform.rotation);
					fireTime = timePerShot;
					break;
				case LaserPowerUp.Multishot:
					Instantiate(laser, transform.position + new Vector3(mouseDelta.x * .866f - mouseDelta.y * .5f,
					    mouseDelta.y * .866f + mouseDelta.x * .5f, 0), transform.rotation * new Quaternion(0,0,.2588f, .9659f));
					Instantiate(laser, transform.position + new Vector3(mouseDelta.x * .866f + mouseDelta.y * .5f,
						mouseDelta.y * .866f - mouseDelta.x * .5f, 0), transform.rotation * new Quaternion(0,0,-.2588f, .9659f));
					Instantiate(laser, transform.position + (Vector3)mouseDelta, transform.rotation);
					fireTime = timePerShot;
					break;
				case LaserPowerUp.RapidFire:
					Instantiate(laser, transform.position + (Vector3)mouseDelta, transform.rotation);
					fireTime = timePerShot / rapidFireRate;
					break;
				}
			}
		}
		if (Input.GetButton ("Charge")) {
			if (chargeTime <= 0) {
				rigidbody2D.velocity = new Vector2 (50 * Mathf.Cos (Mathf.Deg2Rad * this.targetAngle), 50 * Mathf.Sin (Mathf.Deg2Rad * this.targetAngle));
				chargeTime = timePerCharge;
				isInvulnerable = true;
			}
		}
	}

	//Helper method for turning a camera point to a world point
	Vector2 CameraToWorld(Vector2 point) {
		return new Vector2 (cameraBounds.xMin + (cameraBounds.width * point.x / mainCamera.pixelWidth),
   			cameraBounds.yMin + (cameraBounds.height * point.y / mainCamera.pixelHeight));
	}

	//Called when the object is destroyed
	void OnDestroy () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");		
		foreach (GameObject enemy in enemies) {
			if (enemy != null) {
				EnemyBase ship = enemy.GetComponent<EnemyBase> ();
				if (ship != null) {
					ship.PlayerKilled();		//Tells all enemies that the player no longer exists
				}
			}
		}
	}

	public override void Damage (int amount) {
		if (!isInvulnerable && !manager.RemoveExp (amount)) {
			Destroy(this.gameObject);
		}
	}
}
