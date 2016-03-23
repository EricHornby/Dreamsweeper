using UnityEngine;
using System.Collections;

public class FlashWhite : MonoBehaviour {

	float flashTime = 0.3f;
	float startTime;
	SpriteRenderer sprite;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		sprite = GetComponent<SpriteRenderer>();
		sprite.material.SetFloat("_FlashAmount",1f);
		Invoke("Unflash",0.3f);
		
		
		Invoke ("FlashRepeating",0.2f);
	}
	
	void FlashRepeating()
	{
		FlashRepeating(1f);
	}
	
	void FlashRepeating(float t)
	{
		InvokeRepeating("Unflash",0.1f,0.2f);
		InvokeRepeating("Flash",0.2f,0.2f);
		Invoke("EndInvocation",t);
	}
	
	void EndInvocation()
	{
		CancelInvoke();
		sprite.material.SetFloat("_FlashAmount",0f);
		Destroy(this);
	}
	
	void Flash()
	{
		sprite.material.SetFloat("_FlashAmount",0.5f);
	}
	
	void Unflash()
	{
		sprite.material.SetFloat("_FlashAmount",0f);
	}
	
	// Update is called once per frame
	void Update () {
	/*
		if (((Time.time - startTime)/flashTime) <= 1f)
		{	
			sprite.material.SetFloat("_FlashAmount",1f -((Time.time - startTime)/flashTime));
		}
		else
		{
			Destroy(this);
		}
		*/
		
		
	}
}
