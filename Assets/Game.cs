using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class Game : MonoBehaviour {

	public static Game thegame = null;
	public int level;
	bool gameended = false;

	public GameObject[] levelprefabs;

	void Awake () {
		if (thegame == null) {
			//DontDestroyOnLoad (gameObject);
			thegame = this;
			level = 1;
		} else {
			if (thegame != this) {
				Destroy (gameObject);
				return;
			} else {
				level += 1;
			}
		}

		if (!gameended) {

			Debug.Log ("Level " + level + " loading");

			if (level <= levelprefabs.Length) {
				Instantiate (levelprefabs[level-1], new Vector3 (-7.5f, 5.0f, 10.0f), Quaternion.identity, null);
			} else {
				//win}
			}
		}

	}

	public void win() {
		if (gameended)
			return;
		gameended = true;
		SceneManager.LoadScene ("won");
	}

	public void button_next() {
		SceneManager.LoadScene ("scene1");
	}


}
