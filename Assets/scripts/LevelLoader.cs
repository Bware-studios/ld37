using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	void Start () {
		Game.thegame.LoadMap ();
	}

}
