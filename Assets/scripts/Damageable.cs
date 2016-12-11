using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damageable : MonoBehaviour {

	public float life = 10.0f;

	Text score1 = null;
	Control playerctl;
	bool dead = false;

	// Use this for initialization
	void Start () {
		playerctl = GetComponent<Control> ();
		if (playerctl) {
			GameObject go = GameObject.FindGameObjectWithTag ("score1");
			score1 = go.GetComponent<Text >();
		}
		updateText ();
	}
	
	public void damage(float d) {
		life -= d;
		if (life <= 0) {
			life = 0;
		
			// morir
			if (playerctl) {
				if (!dead)
					playerctl.die ();
			} else {
				Destroy (gameObject);
			}
			dead = true;
		}
		updateText ();
			
	}

	void updateText() {
		if (score1) {
			score1.text = ""+life;
		}
	}

}
