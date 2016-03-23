using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Monster : Entity {

	public string spriteName;
	public Sprite[] runSprites = new Sprite[4];
	public Sprite[] idleSprites = new Sprite[4];
	public Sprite[] fallSprites = new Sprite[4];
	
	
	float frameTime = 0.2f;
	
	SpriteRenderer sprite;
	Rigidbody2D rigid;
	
	int spriteNum = 0;
	
	bool faceRight;
	
	public float speed;
	
	public BoxCollider2D footSpot;
	
	float vertVelocity;
	float horizVelocity;
	
	public float jumpStrength = 40f;
	public float fallMax = 40f;
	
	public float jumpDecel = 1f;
	public float fallAcel = 1f;
	
	bool dead;

	public bool edgeTurner;

	public bool debugReport;
	
	public Collider2D clipper;

	// Use this for initialization
	void Start () {
		Sprite[] sprites = Resources.LoadAll<Sprite>(spriteName);
		sprite = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		for (int x = 0; x < 4; x++)
		{
			runSprites[x] = sprites[x];
		}
		idleSprites = runSprites;
		fallSprites = runSprites;
		Animate();
		
			
	}
	
	void Animate()
	{
		Sprite[] sprites = idleSprites;
		spriteNum++;
		
		
		if (spriteNum > sprites.Length-1)
		{
			spriteNum = 0;
		}
		
		sprite.sprite = sprites[spriteNum];
		
		float nextFrame = frameTime;
		
		
		Invoke("Animate",nextFrame);
		
	}
	
	void Fall()
	{
		if (!grounded)
		{
			if (vertVelocity > 0)
			{
				vertVelocity = vertVelocity *0.95f;
				vertVelocity -= jumpDecel;
				if (vertVelocity < 0)
				{
					vertVelocity = 0;
				}
			}
			else
			{
				vertVelocity -= fallAcel;
			}

			if (vertVelocity*-1f > fallMax)
			{
				vertVelocity = -fallMax;
				
			}
			
		}
		else
		{
			if (vertVelocity < 0)
			{
				vertVelocity = 0;
			}
		}
	}
	
	void FixedUpdate()
	{
	
		if (footIsTouchingGround)
		{
			if (!grounded)
			{
				//grounded = true;
			}
		}
		else
		{
			//grounded = false;
		}
		
		Fall ();
		if (!dead) {
			Move (speed, 0f);
		}

		if (edgeTurner && LookForEdge ()) {
			FlipHoriz();
		}

	}
	
	void FlipHoriz()
	{
		faceRight = !faceRight;
			
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void Move(float h, float v)
	{
		if (debugReport) {
			Debug.Log("h: " + h + " footground: " + footIsTouchingGround + " horizV: " + horizVelocity);
		}
		if (!footIsTouchingGround) {
			h=0;
		}
		
		float h_mag = Math.Abs(h);
		
		/*
		if (grounded)
		{
			h = 0;
		}*/
		
		h+= horizVelocity;

		if (!faceRight) {
			h = h*-1f;
		}

		if (debugReport) {
			Debug.Log("will attempt move to: " + transform.position.x+h*speed*Time.deltaTime);
		}


		if (footIsTouchingGround && vertVelocity < 0) {
			vertVelocity = 0;
		}


	
		rigid.MovePosition(new Vector2(transform.position.x + h * speed * Time.deltaTime, transform.position.y + vertVelocity * Time.deltaTime));
		if (Math.Abs(h) > 0 || Math.Abs(vertVelocity) > 0)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}
	}
	
	void CreateEffect(string effectName)
	{
		GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + effectName) as GameObject) as GameObject;
		jumpEffect.transform.parent = transform;
		jumpEffect.transform.localPosition = new Vector2(0f,0f);
		jumpEffect.transform.parent = null;
	}
	
	public override void Attacked(int force)
	{
		if (!dead)
		{
			Debug.Log("Lo! I am Hit!");
			gameObject.AddComponent<FlashWhite>();

			horizVelocity = 0f;

			if (grounded)
			{
				//vertVelocity = 50f;
			}
			else
			{
				vertVelocity = 0f;
			}
			horizVelocity = -0f;
			if (grounded)
			{
				Invoke("FlinchCancel",0.1f);
			}
			
			Die();
		}
		
	}
	
	public void Die()
	{
		clipper.enabled = false;
		dead = true;
		name = "dead";
		for (int x = 0; x < 10; x++)
		{
			CreateEffect("DeathStar");
		}	
		CreateEffect("Pop");
		FadeOut();
	}
	
	public void FadeOut()
	{
		Invoke("Fade",0.3f);
	}
	
	void Fade()
	{
		Destroy(gameObject);
	/*
		sprite.material.SetFloat("_FlashAmount",1f);
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.05f);
		if (sprite.color.a >= 0)
		{
			Invoke("Fade",0.1f);
		}
		else
		{
			Destroy(gameObject);
		}*/
	}
	
	void FlinchCancel()
	{
		horizVelocity = 0f;
		vertVelocity = -50f;
	}

	/*
	void LookForFall()
	{
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 (transform.position.x + 10f, transform.position.y), new Vector2 (0f, 1f), 25f, 12);
		if (hit.collider != null) {
			Debug.Log (hit.collider.gameObject.name);
		}

	}*/

	void OnCollisionEnter2D(Collision2D other)
	{
		int contactMid = other.contacts.Length/2;
		//Debug.Log ("bump at all!");
		//Debug.Log ("bump from " + other.contacts[contactMid].normal + " with " + other.gameObject.name + " and point " + other.contacts[contactMid].point + " using contactpoint " + contactMid + " has tag " + other.gameObject.tag);

		if (other.contacts[contactMid].normal.x != 0 && other.contacts[contactMid].normal.y == 0)
		{
		//	Debug.Log ("bump from " + other.contacts[contactMid].normal + " with " + other.gameObject.name + " and point " + other.contacts[contactMid].point + " using contactpoint " + contactMid + " has tag " + other.gameObject.tag);
			
			if (other.collider.transform.position.y > transform.position.y-5) //dirty hotfix, bump sprite up when floor collides sideways
			{
				FlipHoriz();
			}
			else
			{
				//Debug.Log("Flootstuck fix!");
				//transform.position = new Vector2(transform.position.x, transform.position.y+1f);
			}
		}
	}

	void OnCollisionStay2D(Collision2D other)
	{
		Monster otherM = other.gameObject.GetComponent<Monster> ();
		if (otherM != null) {
			if (otherM.faceRight != faceRight) {
				if (faceRight && otherM.transform.position.x > transform.position.x) {
					FlipHoriz ();
					otherM.FlipHoriz ();
				} else if (!faceRight && otherM.transform.position.x < transform.position.x) {
					FlipHoriz ();
					otherM.FlipHoriz ();
				}
			}
		} 
		else if (other.collider.transform.position.y > transform.position.y-5){
			if (faceRight && other.transform.position.x > transform.position.x) {
				FlipHoriz ();
			} else if (!faceRight && other.transform.position.x < transform.position.x) {
				FlipHoriz ();
			}
		}
	}

	bool LookForEdge()
	{
		float forward = -1f;
		if (faceRight) {
			forward = 1f;
		}
		if (footIsTouchingGround) {
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + forward*10.1f, transform.position.y),-Vector2.up,20f);
			if (hit.collider != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		return false;
		

	}
	
}
