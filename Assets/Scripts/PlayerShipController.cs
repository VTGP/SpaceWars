using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Player
/// </summary>
public class PlayerShipController : ShipBase {
	//Serialized Fields
	public float timePerShot;		//Seconds per shot
	public GameObject laser;		//The projectile to use

	//Public Access to health
	public float healthPercent {
		get{return Mathf.Max((float)health /healthMax, 0);}
	}

	private Rect cameraBounds;			//Camera point to world point convertion...thing
	private float fireTime;				//Timer to when it can fire again (0 when ready)
	private Camera mainCamera;			
	private Vector2 mouseDelta;			//Where the mouse is relative to the player in the world

	// Use this for initialization
	void Start () {
		SetUp ();
		mainCamera = GameObject.Find ("Main Camera").camera;		//Sets up the camera point to world point method
		cameraBounds = new Rect (-mainCamera.orthographicSize * mainCamera.aspect, -mainCamera.orthographicSize,
			2 * mainCamera.orthographicSize * mainCamera.aspect, 2 * mainCamera.orthographicSize);
	}

	//Called each frame
	void Update () {
		mouseDelta = CameraToWorld (Input.mousePosition) - (Vector2)transform.position;
		targetVelocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * speed; 		//Retrieves input from the axis variables
		targetAngle = mouseDelta.y < 0 ? 360 - Vector2.Angle(mouseDelta, Vector2.right) : Vector2.Angle(mouseDelta, Vector2.right);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
		}
		if (Input.GetButton ("Fire1")) {				//Is Fire down?
			if (fireTime <= 0) {
				Instantiate(laser, transform.position + (Vector3)mouseDelta.normalized, transform.rotation);
				fireTime = timePerShot;
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

}
