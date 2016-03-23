using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tile : MonoBehaviour
{
	public Tile ()
	{
	}
	
	public enum TileType{
		Background,
		Tile,
		Entity,
		Interactable,
		Foreground,
	}
	
	public TileType tileType;
	
	public SpriteRenderer spriteRenderer;
	
	public bool animated;
	
	public bool passdown;
	
	public bool groupsame;
	
	public bool rickety;
	
	public List<Collider2D> ignoredColliders = new List<Collider2D>();
	
	int count;
	
	public Vector3 originalLocation;
	
	bool down;
	
	public GameObject colld;
	
	public string bounceEffect;
	public int effectAmt;
	
	Rigidbody2D rigid;
    float lastDown;

    float offsetTracker;
	
	void Start()
	{
		count = 0;
		originalLocation = transform.position;
		rigid = GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{
		if (count < 0)
		{
			count  = 0;
		}
	}
	
	//List<Collider2D> ignoredColliders = new List<Collider2D>();
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (passdown)
		{

            MainCharacter chara = coll.gameObject.GetComponent<MainCharacter>();
            if (chara != null)
            {
                chara.onPassablePlatform = true;
                if (chara.pass_through)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), coll.collider);
                    ignoredColliders.Add(coll.collider);
                    foreach (Collider2D child in coll.gameObject.GetComponentsInChildren<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), child);
                        ignoredColliders.Add(child);
                    }
                }
            }

            if (coll.contacts[0].normal == new Vector2(0f,1f) || coll.transform.position.y < transform.position.y+20f)
			{
			
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(),coll.collider);
				ignoredColliders.Add(coll.collider);
				foreach (Collider2D child in coll.gameObject.GetComponentsInChildren<Collider2D>())
				{
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(),child);
					ignoredColliders.Add(child);
				}
			}
			else
			{
				if (!down && rickety)
				{
					Down (coll);
				}
				
			}
			//Invoke("Solidify",0.35f);
		}
		else if (!down && rickety)
		{
			Down (coll);
		}
	}


    void NewDown(Collision2D coll)
    {
        
    }	

	void Down(Collision2D coll)
	{
            

		MainCharacter tinyWitch = coll.gameObject.GetComponent<MainCharacter>();
		if (tinyWitch != null && (tinyWitch.charState == MainCharacter.CharacterState.Falling || tinyWitch.charState == MainCharacter.CharacterState.Idle ) && !down)
		{
			Vector2 shiftTo = new Vector2(transform.position.x, transform.position.y - 1f);

            // rigid.MovePosition(shiftTo);

            // coll.gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y - 1f));


            
            
            if (tinyWitch.sprite.transform.localPosition == tinyWitch.originalSpritePosition)
            {
                tinyWitch.SpriteOffset(new Vector2(0f, -1f));
                offsetTracker = -1f;
               // Debug.Log("offset!");
            }
            else
            {
                offsetTracker = 0f;
            }

            spriteRenderer.gameObject.transform.position = (new Vector2(transform.position.x, transform.position.y - 1f));

			down = true;
			
			
			Invoke("UnDown",0.15f);
			colld = coll.gameObject;
			if (bounceEffect != "")
			{
				for (int x =0; x < effectAmt; x++)
				{
					CreateEffect(bounceEffect);
				}
			}
		}
        
		
	}
	
	void UnDown()
	{
        lastDown = Time.time;
		//GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y + 1f));
		if (down)
		{
            //GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y + 1f));
            spriteRenderer.gameObject.transform.position = (new Vector2(transform.position.x, transform.position.y));
            MainCharacter.instance.SpriteOffset(new Vector2(0f, -offsetTracker));
            down = false;
		}
		
	}
	
	void OnCollisionStay2D(Collision2D coll)
	{
		if (passdown)
		{
			MainCharacter chara = coll.gameObject.GetComponent<MainCharacter>();
			if (chara != null)
			{
                chara.onPassablePlatform = true;
				if (chara.pass_through)
				{
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(),coll.collider);
					ignoredColliders.Add(coll.collider);
					foreach (Collider2D child in coll.gameObject.GetComponentsInChildren<Collider2D>())
					{
						Physics2D.IgnoreCollision(GetComponent<Collider2D>(),child);
						ignoredColliders.Add(child);
					}
				}
			}
			
		}
	}
	
	
	void OnTriggerExit2D()
	{
		count--;
        if (passdown)
        {
            //collider2D.isTrigger = false;
            MainCharacter chara = MainCharacter.instance;
            if (chara != null)
            {
                chara.onPassablePlatform = false;
            }
        }
		
	}
	
	void OnTriggerEnter2D()
	{
		count++;
	}
	
	
	public void Solidify()
	{
		foreach(Collider2D col in ignoredColliders)
		{
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(),col,false);
		}
	}
	
	void CreateEffect(string effectName)
	{
		GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + effectName) as GameObject) as GameObject;
		jumpEffect.transform.parent = transform;
		jumpEffect.transform.localPosition = new Vector2( UnityEngine.Random.Range(-10f,10f) ,0f);
		jumpEffect.transform.parent = null;
		if (transform.localScale.x < 0)
		{
			jumpEffect.transform.localScale = new Vector3(-1f,1f,1f);
		}
	}
}


