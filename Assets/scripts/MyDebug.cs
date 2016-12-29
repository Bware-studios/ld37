using UnityEngine;
using System.Collections;

public class MyDebug : MonoBehaviour {

	public static MyDebug dbg;

	public int level;
	public string lastmsg = "";

	void Start() {
		dbg = this;
		level = Game.thegame.level;
	}

	void OnGUI() {
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;

		GUI.Label (new Rect (0, 0, 100, 30), "level: "+level, style);
		GUI.Label (new Rect (0, 30, 200, 30), "m: "+lastmsg, style);


		if (GUI.Button (new Rect(0,300,100,50),"win")) {
			Game.thegame.event_win ();
		}
		if (GUI.Button (new Rect(0,350,100,50),"loss")) {
			Game.thegame.event_loss ();
		}
	}
}
