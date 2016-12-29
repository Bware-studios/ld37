using UnityEngine;
using System.Collections;
using X_UniTMX;

[ExecuteInEditMode]
public class Depth : MonoBehaviour {
	public int order;

	public TiledMapComponent mapa;
	public int w,h;

	void Start() {
		TiledMapComponent m = null;
		GameObject mapparent = null;

		if (transform.parent) {
			if (transform.parent.parent.parent) { 
				mapparent = transform.parent.parent.parent.gameObject;
			}
		}
		if (mapparent) {
			m = mapparent.GetComponent<TiledMapComponent> ();
		}
		if (m) {
			mapa = m;
		}
		if (mapa) {
			Map map = mapa.TiledMap;
			if (map != null) {
				w = map.Width;
				h = map.Height;
			}
		}
	}


	void FixedUpdate () {
		setDepth ();
	}



	void setDepth() {
		Vector3 p = transform.localPosition;
		float y = p.y;

		transform.localPosition = new Vector3(p.x,p.y,y);

		//int order;
		order = w*(int)(-y);

		gameObject.GetComponent<SpriteRenderer> ().sortingOrder = order;
	}

}
