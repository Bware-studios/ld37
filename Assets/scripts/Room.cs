using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Room : MonoBehaviour {
	public int total=0;
	public int inroom=0;
	int inroomantes;
	BoxCollider2D box;
	public Text marcador;

	public AudioSource entersound;
	public AudioSource exitsound;

	public void register() {
		total += 1;
	}

	public void Update() {
		//Debug.Log (inroom);
		if (inroom == total && total>1) {
			//Debug.Log ("todos");
			//if (Game.thegame)
			//	Game.thegame.win ();
			SceneManager.LoadScene("ganar");
		}
		marcador.text = "in: " + inroom + "/" + total;
		if (inroom > inroomantes) {
			if (entersound)
				entersound.Play ();
		}
		if (inroom < inroomantes) {
			if (exitsound)
				exitsound.Play ();
		}
		inroomantes = inroom;
		inroom = 0;
	}
		
	public void setBounds (BoxCollider2D b) {
		box = b;
	//	Debug.Log ("bounds: " + box);
	}

	public void updateMalo(Transform pos) {
		//Debug.Log (pos.position);
		if (box.OverlapPoint(pos.position)) {
			inroom+=1;
		}
	}


}
