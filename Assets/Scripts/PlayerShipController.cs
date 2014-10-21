using UnityEngine;
using System.Collections;

public class PlayerShipController : ShipBase {
	public float timePerShot;
	public GameObject laser;
	public float healthPercent {
		get{return Mathf.Max((float)health /healthMax, 0);}
	}
	private Rect cameraBounds;
	private float fireTime;
	private Camera mainCamera;
	private Vector2 mouseDelta;

	// Use this for initialization
	void Start () {
		SetUp ();
		mainCamera = GameObject.Find ("Main Camera").camera;
		cameraBounds = new Rect (-mainCamera.orthographicSize * mainCamera.aspect, -mainCamera.orthographicSize,
			2 * mainCamera.orthographicSize * mainCamera.aspect, 2 * mainCamera.orthographicSize);
	}

	void Update () {
		mouseDelta = CameraToWorld (Input.mousePosition) - (Vector2)transform.position;
		targetVelocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * speed; 
		targetAngle = mouseDelta.y < 0 ? 360 - Vector2.Angle(mouseDelta, Vector2.right) : Vector2.Angle(mouseDelta, Vector2.right);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
			if (fireTime < 0) {
				fireTime = 0;
			}
		}
		if (Input.GetButton ("Fire1")) {
			if (fireTime == 0) {
				Instantiate(laser, transform.position + (Vector3)mouseDelta.normalized, transform.rotation);
				fireTime = timePerShot;
			}
		}
	}

	Vector2 CameraToWorld(Vector2 point) {
		float x = cameraBounds.xMin + (cameraBounds.width * point.x / mainCamera.pixelWidth);
		float y = cameraBounds.yMin + (cameraBounds.height * point.y / mainCamera.pixelHeight);
		return new Vector2 (x, y);
	}

	void OnDestroy () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			if (enemy != null) {
				EnemyBase ship = enemy.GetComponent<EnemyBase> ();
				if (ship != null) {
					ship.PlayerKilled();
				}
			}
		}
	}

}
