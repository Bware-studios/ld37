using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class Game : MonoBehaviour {

	public static Game thegame = null;
	public int level = 1;

	public GameObject[] levelprefabs;

	void Awake () {
		if (thegame == null) {
			//Debug.Log ("Game created");
			DontDestroyOnLoad (gameObject);
			thegame = this;
		} else if (thegame != this) {
			Destroy (gameObject);
			return;
		}
			

	}

	public void LoadMap() {
		int imap = (level - 1) % levelprefabs.Length;
		Instantiate (levelprefabs[imap], new Vector3 (-7.5f, 5.0f, 10.0f), Quaternion.identity, null);
	}



	public void event_win() {
		level += 1;
		SceneManager.LoadScene ("ganar");
	}


	public void event_loss() {
		level = 1;
		//Destroy (gameObject);
		SceneManager.LoadScene ("perder");
	}


	public void event_next() {
		//Debug.Log ("l"+level+" - next");
		SceneManager.LoadScene ("scene1");
	}
		
}
