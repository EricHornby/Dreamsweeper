using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour {

	public Collider2D col;
	public Entity source;
	
	bool notTouching;
	float lastTouch;
	float count = 0;
	float timeSinceLastTouch;

    public bool footPass;
	
	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D>();
		count = 0;
	}


    void FixedUpdate()
    {
        
        if (col.IsTouchingLayers() && !footPass)
        {
            //source.footIsTouchingGround = true;
           // source.grounded = true;
        }
        else
        {
            source.footIsTouchingGround = false;
            //source.grounded = false;
           // source.footIsTouchingGround = false;
        }
	

            /*
		if (count == 0)
		{
			source.footIsTouchingGround = false;
		}
		
		timeSinceLastTouch = Time.time - lastTouch;
				
		if (timeSinceLastTouch > 0.25f && source.grounded && source.isMoving)
		{
			count = 0;
		}	*/
	}
	
    
	void OnTriggerEnter2D(Collider2D other)
	{		
	
	
		
		if (other.gameObject.layer != 18)
		{
			count++;
			lastTouch = Time.time;
			source.footIsTouchingGround = true;
			if (!source.grounded)
			{
				//Debug.Log("landing!");
				source.Land();
			}
		}
		
		
	}
	
	void OnTriggerStay2D(Collider2D other)
	{

		if (other.gameObject.layer != 18)
		{
			lastTouch = Time.time;
			source.footIsTouchingGround = true;	
		}
		
		if (count == 0)
		{
			count++;
		}
		
			
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer != 18)
		{
			count--;
		}	
		
		//source.footIsTouchingGround = false;
	}
	

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.contacts[0].normal.y > 0)
		{
			lastTouch = Time.time;
			Debug.Log("landing!");
			source.footIsTouchingGround = true;
			source.Land();
		}
	
	}
	
	void OnCollisionStay2D(Collision2D other)
	{
		
		
		if (other.contacts[0].normal.y > 0)
		{
			source.footIsTouchingGround = true;
			lastTouch = Time.time;
		}
		
	}
	
	void OnCollisionExit2D(Collision2D other)
	{
		source.footIsTouchingGround = false;
	}
    
    
}
