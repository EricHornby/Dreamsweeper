using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour {
	
	public Sprite[] sprites;
	public float minFallSpeed;
	public float fallspeed;
	public float lifetime;
	public float horiz;
	
	public bool randomY;
	public bool randomX = true;
	
	int spriteNum;
	
	public float frameTime; 
	
	float startTime;
	
	SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		Invoke("DeleteSelf",lifetime);
		sprite = GetComponent<SpriteRenderer>();
		if (randomX)
		{
			horiz = Random.Range(-20f,20f);
		}
		
		fallspeed = Random.Range(minFallSpeed,fallspeed);
		
		if (randomY)
		{
			fallspeed = Random.Range(-fallspeed,fallspeed);
		}
		
		if (randomX)
		{
			horiz = Random.Range(-horiz,horiz);
		}
		
		spriteNum = Random.Range(0,sprites.Length);
		Animate();
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + horiz * Time.deltaTime , transform.position.y - fallspeed * Time.deltaTime, transform.position.z);
		if (((Time.time - startTime)/lifetime) <= 1f)
		{	
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (1f - (Time.time - startTime)/lifetime) * 2f);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public void Animate()
	{		
		if (spriteNum > sprites.Length-1)
		{
			spriteNum = 0;
		}
		sprite.sprite = sprites[spriteNum];
		spriteNum++;
		Invoke("Animate",frameTime);
	}
	
	void DeleteSelf()
	{
		Destroy(gameObject);
	}
}
