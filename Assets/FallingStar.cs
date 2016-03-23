using UnityEngine;
using System.Collections;

public class FallingStar : MonoBehaviour {

	public SpriteRenderer sprite;
	
	public Color col1;
	public Color col2;
	public Color col3;
	
	public float fallspeed;
	
	float startTime;
	public float lifetime;
	public Animator anim;
	
	public float horiz;
	
	public bool randomY;
	public bool noAnim;
	
	public bool randomX = true;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		Invoke("DeleteSelf",lifetime);
		
		if (randomX)
		{
			horiz = Random.Range(-20f,20f);
		}
		
		
		int r = Random.Range(0,3);
		if (r == 0)
		{
			sprite.color = col1;
		}
		else if (r== 1)
		{
			sprite.color = col2;
		}
		else
		{
			sprite.color = col3;
		}
		
		if (randomY)
		{
			r = Random.Range(0,3);
			if (r == 0)
			{
				
			}
			else if (r == 1)
			{
				fallspeed = 0;
			}
			else if (r == 2)
			{
				fallspeed = -fallspeed;
			}
		}
		if (!noAnim)
		{
			//anim.Play("SingleStar_",-1,Random.Range(0,1f));
		}
		else
		{
			anim.StopPlayback();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate()
	{	
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
	
	void DeleteSelf()
	{
		Destroy(gameObject);
	}
}
