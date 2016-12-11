using UnityEngine;
using System.Collections;

public class RoomShape : MonoBehaviour {
	public Room theroom;

	// Use this for initialization
	void Start () {
		theroom = GameObject.FindGameObjectWithTag ("room").GetComponent<Room> ();
		GameObject colobj = GameObject.Find ("objects1_room");
		colobj.transform.parent = transform;
		theroom.setBounds(colobj.GetComponent<BoxCollider2D>());
	}
	

}
