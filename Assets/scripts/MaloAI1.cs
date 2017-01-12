using UnityEngine;
using System.Collections;

using X_UniTMX;

public class MaloAI1 : MonoBehaviour {

	Transform mypos = null;
	Rigidbody2D body;
	Animator anim;
	TouchDamage damager;
	SpriteRenderer ren;
	Collider2D mycollider;
	TileLayer main_map_layer = null;

	public string debugstr = "";


	bool flipped = false;
	public Room theroom;

	bool aturdido;
	float fin_aturdido;

	float next_decision = 0;

	float ai_joystick_x,ai_joystick_y;

	Transform targetpos;
	public float speed = 3;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();	
		mypos = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		damager = GetComponent<TouchDamage> ();
		ren = GetComponent<SpriteRenderer> ();

		mycollider = GetComponent<Collider2D> ();

		theroom = GameObject.FindGameObjectWithTag ("room").GetComponent<Room>();
		theroom.register ();

		GameObject mapGO = GameObject.FindGameObjectWithTag ("map");
		TiledMapComponent mapcomp = mapGO.GetComponent<TiledMapComponent>();
		Map themap = mapcomp.TiledMap;
		foreach (Layer l in themap.Layers) {
			if (l.Name == "main") {
				main_map_layer = (TileLayer)l;
				// main_map_layer.Tiles [2, 2]  es null  = vacio
				// main_map_layer.Tiles [2, 2] .CurrentID   tileid
			}
		}

		//Debug.Log("p: "+transform.position + " themap: "+mapGO.transform.position+ " fromparent: "+transform.localPosition );
		main_map_layer.SetTile (transform.localPosition.x, transform.localPosition.y,4);
		aturdido = false;

		ai_joystick_x = 0.0f;
		ai_joystick_y = 0.0f;



	}
	
	// Update is called once per frame
	void Update () {
		if (targetpos == null) {
			if (Control.theplayer != null) {
				targetpos = Control.theplayer.GetComponent<Transform> ();
			}
		}
		theroom.updateMalo (transform);

		int search_depth = 5;
		float decision_dt = 0.2f;

		float tnow = Time.time;
		if (tnow > next_decision) {
			next_decision += decision_dt;


			float util_left = expectedUtilityOfMove (search_depth,0.0f, -.5f, 0);
			float util_right = expectedUtilityOfMove (search_depth,0.0f, +.5f, 0);
			float util_up = expectedUtilityOfMove (search_depth,0.0f, 0, -.5f);
			float util_down = expectedUtilityOfMove (search_depth,0.0f, 0, +.5f);

			//Debug.Log ("l "+util_left+" r "+util_right+" u "+util_up+" d "+util_down);
			if (util_left >= util_right && util_left >= util_up && util_left >= util_down) {
				ai_joystick_x = -1.0f;
				ai_joystick_y = 0.0f;
			} else if (util_right >= util_up && util_right >= util_down) {
				ai_joystick_x = +1.0f;
				ai_joystick_y = 0.0f;
			} else if (util_up >= util_down) {
				ai_joystick_x = 0.0f;
				ai_joystick_y = +1.0f;
			} else {
				ai_joystick_x = 0.0f;
				ai_joystick_y = -1.0f;
			}

			debugstr="["+util_left+","+util_right+","+util_up+","+util_down+"]";
		}
	}

	void FixedUpdate () {
		if (aturdido && Time.time > fin_aturdido) {
			aturdido = false;
			damager.aturdido = false;
			ren.color = new Color (1f, 1f, 1f, 1f);
		}
		if (aturdido) {
			float alfa = (fin_aturdido - Time.time) / 20.0f;
			ren.color = new Color (1.0f*(.8f-alfa), 0f, 0f, .8f);
			return;
		}

		if (targetpos == null)
			return;

//		float dx = targetpos.position.x - mypos.position.x; 
//		float dy = targetpos.position.y - mypos.position.y; 
		float dx = ai_joystick_x;
		float dy = ai_joystick_y;
		float ax = Mathf.Abs (dx);
		float ay = Mathf.Abs (dy);

		bool up = false, down= false, left=false, right=false;

		if (ax > ay) {
			body.velocity = new Vector2 (dx*speed,0);
			if (dx > 0)
				right = true;
			else
				left = true;
		} else if (ay > ax) {
			body.velocity = new Vector2 (0,dy*speed);
			if (dy > 0)
				up = true;
			else
				down = true;
		}

		anim.SetBool ("up",up);
		anim.SetBool ("down",down);
		anim.SetBool ("side",left||right);
		if (left && !flipped) {
			transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
			flipped = true;
		} else if (right && flipped) {
			transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			flipped = false;
		}



	}


	public void hit() {
		if (aturdido)
			return;
		//if (Random.Range (0.0f, 1.0f) < .3f) {
			aturdido = true;
			fin_aturdido = Time.time + Random.Range (4.0f, 10.0f);
			anim.SetBool ("up",false);
			anim.SetBool ("down",false);
			anim.SetBool ("side",false);
			body.velocity = new Vector2 (0f,0f);
			damager.aturdido = true;


		//}
	}

	bool freeToMove(float deltaix, float deltaiy) {
		int ix = (int)Mathf.Floor (transform.localPosition.x + deltaix);
		int iy = (int)Mathf.Floor (-transform.localPosition.y + deltaiy);
//		if (ix < 0 || iy < 0 || ix > 20 || iy > 15) {
//			Debug.Log ("ix= " + ix + " iy= " + iy);
//		}
		Tile t = main_map_layer.Tiles [ix,iy];
		return t == null;
	}


	float expectedUtilityOfMove(int depth, float cost, float dix, float diy) {
		if (!freeToMove (dix, diy)) {
			return -1.0f;
		}
		Vector2 p1 = targetpos.localPosition;
		Vector2 p2 = mypos.localPosition;

		float util_this = Mathf.Sqrt((p2.x + dix - p1.x) * (p2.x + dix - p1.x) + (p2.y - diy - p1.y) * (p2.y - diy - p1.y));
		util_this = 1.0f / util_this;
		if (depth == 0) {
			return util_this-cost;
		}
			
		float auxutil;
		//float util_this = -1.0f;
		auxutil = expectedUtilityOfMove (depth - 1,cost+.1f, dix + .5f, diy);
		util_this = (auxutil > util_this) ? auxutil : util_this;
		auxutil = expectedUtilityOfMove (depth - 1,cost+.1f, dix - .5f, diy);
		util_this = (auxutil > util_this) ? auxutil : util_this;
		auxutil = expectedUtilityOfMove (depth - 1,cost+.1f, dix , diy + .5f);
		util_this = (auxutil > util_this) ? auxutil : util_this;
		auxutil = expectedUtilityOfMove (depth - 1,cost+.1f, dix , diy - .5f);
		util_this = (auxutil > util_this) ? auxutil : util_this;

		return util_this-cost;
	}



//	float expectedUtilityOfMove(int depth, float dx, float dy) {
//		RaycastHit2D[] results = new RaycastHit2D[1];
//		int nhits = mycollider.Cast (new Vector2 (dx, dy), results);
//		if (nhits > 0)  // chocar con el player es bueno !!!
//			return -1.0f;
//		if (depth == 0) {
//			return 3.0; // develover estimacion utilidad distancia al player
//		}
//		// recursion
//
//		float util = -1.0f;
//		float auxutil;
//		auxutil = expectedUtilityOfMove (depth - 1, dx + 1.0f, dy);
//		util = (auxutil > util) ? auxutil : util;
//		auxutil = expectedUtilityOfMove (depth - 1, dx - 1.0f, dy);
//		util = (auxutil > util) ? auxutil : util;
//		auxutil = expectedUtilityOfMove (depth - 1, dx , dy + 1.0f);
//		util = (auxutil > util) ? auxutil : util;
//		auxutil = expectedUtilityOfMove (depth - 1, dx , dy - 1.0f);
//		util = (auxutil > util) ? auxutil : util;
//
//		return util;
//	}
//
}
