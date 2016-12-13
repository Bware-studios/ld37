using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Damageable : MonoBehaviour {

	public float life = 10.0f;

	Text score1 = null;
	Control playerctl;
	MaloAI1 maloctl;
	bool dead = false;

	public AudioSource hurtsound;


	// Use this for initialization
	void Start () {
		playerctl = GetComponent<Control> ();
		maloctl = GetComponent<MaloAI1> ();

		if (playerctl) {
			GameObject go = GameObject.FindGameObjectWithTag ("score1");
			score1 = go.GetComponent<Text >();
		}
		updateText ();
	}
	
	public void damage(float d) {
		if (hurtsound)
			hurtsound.Play ();
		life -= d;
		if (life <= 0) {
			life = 0;
		
			// morir
			if (playerctl) {
				if (!dead) {
					playerctl.die ();
					waitAndExit (3);
				}
			} else if (maloctl) {
				maloctl.hit ();
			}else {
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

	void waitAndExit(float seconds) {
		//yield return new WaitForSeconds (seconds);
		SceneManager.LoadScene ("perder");
	}


}
