using UnityEngine;
using System.Collections;

public class PlayerContainer : MonoBehaviour {
	public Rect bounds;
	// Use this for initialization
	void Start () {
		EdgeCollider2D edgeCollider = this.GetComponent <EdgeCollider2D> ();
		Vector2[] points = new Vector2[5];
		points [0] = new Vector2 (bounds.xMin, bounds.yMin);
		points [1] = new Vector2 (bounds.xMax, bounds.yMin);
		points [2] = new Vector2 (bounds.xMax, bounds.yMax);
		points [3] = new Vector2 (bounds.xMin, bounds.yMax);
		points [4] = new Vector2 (bounds.xMin, bounds.yMin);
		edgeCollider.points = points;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
