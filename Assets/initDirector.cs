using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class initDirector : MonoBehaviour {

	public void goButton() {
		SceneManager.LoadScene ("scene1");
	}

}
