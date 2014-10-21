using UnityEngine;
using System.Collections;

public abstract class EnemyBase : ShipBase {
	public int expYield;
	public int enemyCapCount;

	public override void SetUp () {
		base.SetUp ();
		GameObject manager = GameObject.FindWithTag ("GameController");
		if (manager != null) {
			manager.GetComponent<Manager> ().Spawned (this);
		}
	}
	public virtual void PlayerKilled () {
		
	}

	void OnDestroy () {
		GameObject manager = GameObject.FindWithTag ("GameController");
		if (manager != null && this.health <= 0) {
			manager.GetComponent<Manager> ().Destroyed (this);
		}
	}
}
