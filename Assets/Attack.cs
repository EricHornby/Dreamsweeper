using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Attack : MonoBehaviour {

	public Collider2D col;
	public MainCharacter source;

	public bool isStar;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D>();
		if (!isStar) {
			col.enabled = false;
		}

	}
	

	void OnTriggerEnter2D(Collider2D other)
	{
		if (isStar) {
			Debug.Log("Star has made contact!");
		}
		if (other.tag != "Foot")
		{
			Debug.Log("hit " + other.gameObject.name);
			other.transform.parent.gameObject.GetComponent<Entity>().Attacked(1);
			
			if (!isStar)
			{
				source.enemy_was_hit_recently = true;
			}
		
			
			
			if (!isStar)
			{
				source.charged = true;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		if (!isStar && !source.grounded)
		{
			source.vertVelocity = 0f;
		}	
	}
	
	void OnTriggerStay(Collider other)
	{

	}
}
