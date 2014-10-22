using UnityEngine;
using System.Collections;

/// <summary>
/// Deletes objects that go too far off the screen
/// </summary>
public class ContainerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Called when an object leaves the collider
	//Because collider is a trigger it does not interact through physics
	void OnTriggerExit2D (Collider2D other) {
		Destroy (other.gameObject);;
	}
}
