using UnityEngine;
using System.Collections;

public class BigStar : Attack {

	public float horizVelocity;
	
	
	void Start()
	{
		Invoke ("DeleteSelf",10f);
	}
	
	void FixedUpdate()
	{
		transform.position = new Vector3(transform.position.x + horizVelocity * Time.deltaTime , transform.position.y, transform.position.z);
		
		CreateEffect("BigStarTrail");
		
	}
	
	void CreateEffect(string effectName)
	{
		GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + effectName) as GameObject) as GameObject;
		jumpEffect.transform.parent = transform;
		jumpEffect.transform.localPosition = new Vector2(0f,0f);
		jumpEffect.transform.parent = null;
	}
	
	public void DeleteSelf()
	{
		Destroy(gameObject);
	}
}
