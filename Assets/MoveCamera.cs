using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public float speedX = 0.5f;
	public float speedY = 0.5f;
	
	float anchorX;
	float anchorY;
	
	public int type;
	
	public SpriteRenderer baseItem;
	
	private Vector3 dragOrigin;
	
	public Transform target;
	public JSONMapReader map;
	
	public Vector3 groundPos;
	
	public float anchorStartX;
	
	public float staticAdjustY;
	
	public float inherentVelocityX;
	
	float displaceX;
	
	void Start()
	{
		anchorX = transform.position.x;
		anchorY = transform.position.y;
		dragOrigin = transform.position;
		target = GameObject.Find("Main Camera").transform;
		map = GameObject.Find("Map").GetComponent<JSONMapReader>();
		anchorStartX = target.transform.position.x;
	
		GameObject rightCopy = Instantiate(baseItem.gameObject) as GameObject;
		rightCopy.transform.position = new Vector3(baseItem.gameObject.transform.position.x + baseItem.bounds.size.x, baseItem.gameObject.transform.position.y);
		
		GameObject rightCopy2 = Instantiate(baseItem.gameObject) as GameObject;
		rightCopy2.transform.position = new Vector3(baseItem.gameObject.transform.position.x + 2*baseItem.bounds.size.x, baseItem.gameObject.transform.position.y);
		
		GameObject leftCopy = Instantiate(baseItem.gameObject) as GameObject;
		leftCopy.transform.position = new Vector3(baseItem.gameObject.transform.position.x - baseItem.bounds.size.x, baseItem.gameObject.transform.position.y);
		
		GameObject leftCopy2 = Instantiate(baseItem.gameObject) as GameObject;
		leftCopy2.transform.position = new Vector3(baseItem.gameObject.transform.position.x - 2*baseItem.bounds.size.x, baseItem.gameObject.transform.position.y);
		
		leftCopy.transform.parent = baseItem.transform.parent;
		rightCopy.transform.parent = baseItem.transform.parent;
		
		leftCopy2.transform.parent = baseItem.transform.parent;
		rightCopy2.transform.parent = baseItem.transform.parent;
	}
	
	void Update()
	{	
		groundPos = new Vector3(0f,-map.mapY*20f);
		Vector3 diff = target.position - groundPos;
		diff = new Vector3(diff.x * speedX, diff.y * speedY, diff.z);
		
		Vector3 newPos = dragOrigin + diff;
		newPos = new Vector3(newPos.x+displaceX,newPos.y + staticAdjustY,newPos.z);
		while (newPos.x - anchorX > baseItem.bounds.size.x)
		{
			newPos = new Vector3(newPos.x - baseItem.bounds.size.x, newPos.y, newPos.z);
		} 
		
		
		transform.position = newPos;
	}
	
	void FixedUpdate()
	{
		displaceX += inherentVelocityX * speedX;
		
		if (displaceX > baseItem.bounds.size.x)
		{
			displaceX -= baseItem.bounds.size.x;
		}
	}
}
