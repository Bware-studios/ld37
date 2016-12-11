using UnityEngine;
using System.Collections;

public class MaloAI1 : MonoBehaviour {

	Transform mypos = null;
	Rigidbody2D body;
	Animator anim;
	bool flipped = false;
	public Room theroom;


	Transform targetpos;
	public float speed = 3;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();	
		mypos = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		theroom = GameObject.FindGameObjectWithTag ("room").GetComponent<Room>();
		theroom.register ();
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





}
