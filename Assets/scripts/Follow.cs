using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target_transform;


	void Update () {
		if (target_transform) {
			Vector3 tpos = target_transform.position;
			transform.position = new Vector3(tpos.x,tpos.y,-10.0f);
		}
	}
}
