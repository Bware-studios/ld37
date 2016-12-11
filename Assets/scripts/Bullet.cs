using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public LayerMask destroybylayers;
	public LayerMask damagetolayers;

	public Sprite[] skins;

	public float speed = 10.0f;

	Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();	
		int iskin = Random.RandomRange (0, skins.Length);
		GetComponent<SpriteRenderer> ().sprite = skins [iskin];
		GetComponent<Rigidbody2D> ().rotation = Random.RandomRange (0, 360);
	}


	public void launch(int dx,int dy) {
		GetComponent<Rigidbody2D>().velocity = new Vector2 (speed*dx,speed*dy);
	}


	void OnCollisionEnter2D (Collision2D c) {
		int layerbin = (1 << c.gameObject.layer);
		bool damage = (0 != (layerbin & damagetolayers.value));
		bool destroyed = (0 != (layerbin & destroybylayers.value));
		if (damage) {
			Damageable dreceiver = c.gameObject.GetComponent<Damageable> ();
			dreceiver.damage (6.0f);
		}
		if (destroyed) {
			Destroy (gameObject);
		}
	}


}
