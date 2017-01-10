using UnityEngine;
using System.Collections;

public class MaloAI1 : MonoBehaviour {

	Transform mypos = null;
	Rigidbody2D body;
	Animator anim;
	TouchDamage damager;
	SpriteRenderer ren;
	Collider2D mycollider;

	bool flipped = false;
	public Room theroom;

	bool aturdido;
	float fin_aturdido;


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
		aturdido = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetpos == null) {
			if (Control.theplayer != null) {
				targetpos = Control.theplayer.GetComponent<Transform> ();
			}
		}
		theroom.updateMalo (transform);
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

		float dx = targetpos.position.x - mypos.position.x; 
		float dy = targetpos.position.y - mypos.position.y; 
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



	float expectedUtilityOfMove(int depth, float dx, float dy) {
		RaycastHit2D[] results = new RaycastHit2D[1];
		int nhits = mycollider.Cast (new Vector2 (dx, dy), results);
		if (nhits > 0)  // chocar con el player es bueno !!!
			return -1.0f;
		if (depth == 0) {
			return 3.0; // develover estimacion utilidad distancia al player
		}
		// recursion

		float util = -1.0f;
		float auxutil;
		auxutil = expectedUtilityOfMove (depth - 1, dx + 1.0f, dy);
		util = (auxutil > util) ? auxutil : util;
		auxutil = expectedUtilityOfMove (depth - 1, dx - 1.0f, dy);
		util = (auxutil > util) ? auxutil : util;
		auxutil = expectedUtilityOfMove (depth - 1, dx , dy + 1.0f);
		util = (auxutil > util) ? auxutil : util;
		auxutil = expectedUtilityOfMove (depth - 1, dx , dy - 1.0f);
		util = (auxutil > util) ? auxutil : util;

		return util;
	}

}
