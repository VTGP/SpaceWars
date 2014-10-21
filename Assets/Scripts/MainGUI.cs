using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	public Texture barTexture;
	public Rect normalizedExpBar;
	public Rect normalizedHealthBar;
	private Rect pixelExpBar;
	private Rect pixelHealthBar;
	private PlayerShipController player;
	private Manager manager;
	private float expPercent;
	private float healthPercent;
	// Use this for initialization
	void Start () {
		pixelExpBar = new Rect (normalizedExpBar.x * camera.pixelWidth, normalizedExpBar.y * camera.pixelHeight, 
			normalizedExpBar.width * camera.pixelWidth, normalizedExpBar.height * camera.pixelHeight);
		pixelHealthBar = new Rect (normalizedHealthBar.x * camera.pixelWidth, normalizedHealthBar.y * camera.pixelHeight,
		    normalizedHealthBar.width * camera.pixelWidth, normalizedHealthBar.height * camera.pixelHeight);
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShipController> ();
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Manager> ();
	}
	
	void OnGUI () {
		expPercent = manager.expPercent;
		healthPercent = player.healthPercent;

		GUI.DrawTextureWithTexCoords (pixelExpBar , barTexture, new Rect (0, 0.75F, 1, 0.25F));
		GUI.DrawTextureWithTexCoords (RectSqueeseLeft(pixelExpBar, expPercent), barTexture, new Rect (0, 0.5F, expPercent, 0.25F));
		GUI.DrawTextureWithTexCoords (RectSqueeseRight(pixelExpBar, expPercent), barTexture, new Rect (expPercent, 0.0F, 1 - expPercent, 0.25F));

		GUI.DrawTextureWithTexCoords (pixelHealthBar, barTexture, new Rect (0, 0.75F, 1, 0.25F));
		GUI.DrawTextureWithTexCoords (RectSqueeseLeft (pixelHealthBar, healthPercent), barTexture, new Rect (0, 0.5F, healthPercent, 0.25F));
		GUI.DrawTextureWithTexCoords (RectSqueeseRight (pixelHealthBar, healthPercent), barTexture, new Rect (healthPercent, 0.25F, 1 - healthPercent, 0.25F));
	}

	Rect RectSqueeseLeft(Rect rect, float percent) {
		return new Rect (rect.x, rect.y, rect.width * percent, rect.height);
	}

	Rect RectSqueeseRight(Rect rect, float percent) {
		return new Rect (rect.x + percent * rect.width, rect.y, rect.width * (1 - percent), rect.height);
	}
}
