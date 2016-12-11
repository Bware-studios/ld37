using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Room : MonoBehaviour {
	public int total=0;
	public int inroom=0;
	BoxCollider2D box;
	public Text marcador;

	public void register() {
		total += 1;
	}

	public void Update() {
		//Debug.Log (inroom);
		if (inroom == total) {
			//Debug.Log ("todos");
			//if (Game.thegame)
			//	Game.thegame.win ();
		}
		marcador.text = "in: " + inroom + "/" + total;
		inroom = 0;
	}
		
	public void setBounds (BoxCollider2D b) {
		box = b;
		Debug.Log ("bounds: " + box);
	}

	public void updateMalo(Transform pos) {
		//Debug.Log (pos.position);
		if (box.OverlapPoint(pos.position)) {
			inroom+=1;
		}
	}


}
