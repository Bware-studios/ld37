using UnityEngine;
using System.Collections;

public class TouchDamage : MonoBehaviour {

	public LayerMask damagetolayers;
	public bool aturdido;

	public float damage_interval = .3f;
	public float damage_hit = .5f;

	float last_time_damage;


	void OnCollisionStay2D (Collision2D c) {
		if (aturdido)
			return;
		int layerbin = (1 << c.gameObject.layer);
		bool damage = (0 != (layerbin & damagetolayers.value));

		if (damage) {
			if (Time.time > last_time_damage + damage_interval) {
				last_time_damage = Time.time;
				Damageable dreceiver = c.gameObject.GetComponent<Damageable> ();
				dreceiver.damage (damage_hit);
			}
		}

	}
}
