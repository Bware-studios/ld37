using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	Rigidbody2D body ;

	float last_fire_time = 0.0f;
	public float fire_time_interval = 0.2f;

	public float speed = 6.0f;

	int dir_x=0 ,dir_y=0;


	public static GameObject theplayer = null; 

	public GameObject bullet = null;

	public bool active = true;
	bool flipped = false;

	Animator anim;

	// Use this for initialization
	void Start () {
		theplayer = this.gameObject;
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		GameObject camobj =  GameObject.FindGameObjectWithTag ("MainCamera");
		Follow cam = camobj.GetComponent<Follow> ();
		cam.target_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (!active)
			return;
		
		float dx = Input.GetAxis ("Horizontal");
		float dy = Input.GetAxis ("Vertical");

		bool up = false, down = false, right=false, left=false;
		if (dx != 0) {
			dir_x = (int)Mathf.Sign (dx);
			dir_y = 0;
			if (dir_x > 0)
				right = true;
			if (dir_x < 0)
				left = true;
		} else if (dy != 0) {
			dir_x = 0;
			dir_y = (int)Mathf.Sign (dy);
			if (dir_y > 0)
				up = true;
			if (dir_y < 0)
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


		bool fire = Input.GetButton ("Jump");

		Vector2 v = new Vector2 (speed*dx,speed*dy);
		body.velocity = v;

		if (fire && Time.time > last_fire_time + fire_time_interval) {
			last_fire_time = Time.time;
			Fire (dir_x,dir_y);
		}
	}



	void Fire(int dx,int dy) {
		//Debug.Log ("v ("+dx+","+dy+")");
		if (bullet == null)
			return;
		if (dx == 0 && dy == 0)
			dy = -1;
		Vector2 mypos = transform.position;
		Vector3 npos = new Vector3 (mypos.x + .5f*dx, mypos.y + .5f*dy +.5f, 0.0f);
		GameObject nbullet = (GameObject)Instantiate (bullet, npos, Quaternion.identity, null);
		nbullet.GetComponent<Bullet>().launch (dx,dy);
	
	}

	public void die() {
		active = false;
		anim.SetTrigger ("die");
	//	Debug.Log ("muerto");
	}


		
}
