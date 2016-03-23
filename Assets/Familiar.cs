using UnityEngine;
using System.Collections;

public class Familiar : MonoBehaviour {

	public Sprite[] hoverSprites = new Sprite[4];
	SpriteRenderer sprite;
	Rigidbody2D rigid;
	public MainCharacter friend;
	
	int spriteNum;
	float frameTime = 0.2f;
	
	Vector3 targetLocation = new Vector2(0f,0f);
	public float speed;
	
	// Use this for initialization
	void Start () {
		Sprite[] sprites = Resources.LoadAll<Sprite>("familiar");
		sprite = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		for (int x = 0; x < 4; x++)
		{
			hoverSprites[x] = sprites[x];
		}
		Animate();
		Move();
	}
	
	void Animate()
	{
		Sprite[] sprites = hoverSprites;
		spriteNum++;
		
		
		if (spriteNum > sprites.Length-1)
		{
			spriteNum = 0;
		}
		
		sprite.sprite = sprites[spriteNum];
		
		float nextFrame = frameTime;
		
		
		Invoke("Animate",nextFrame);
		
	}
	
	void FixedUpdate()
	{
	
		if (transform.position.x < friend.transform.position.x)
		{
			transform.localScale = new Vector3(-1f,1f,1f);
		}	
		else
		{
			transform.localScale = new Vector3(1f,1f,1f);
		}
		Move ();
	}
	
	void Move()
	{
		float targetX = friend.transform.position.x + 20;
		float targetY = friend.transform.position.y + 10;
		if (friend.faceRight)
		{
			targetX = friend.transform.position.x - 20;
		}
		
		
		float dx = targetX- transform.position.x;
		float dy = targetY - transform.position.y;
		
		/*
		if (dx > speed)
		{
			dx = speed;
		}*/
		
		
		rigid.MovePosition(new Vector2(transform.position.x + dx * Time.deltaTime,transform.position.y + dy * Time.deltaTime));
		
	}
}
