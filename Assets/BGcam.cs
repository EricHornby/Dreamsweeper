using UnityEngine;
using System.Collections;

public class BGcam : MonoBehaviour {


	public float speed = 5.0f;
	
	float anchorX;
	
	public int type;
	
	public GameObject baseItem;
	
	private Vector3 dragOrigin;

	// Use this for initialization
	void Start () {
		anchorX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + speed * Time.deltaTime,transform.position.y,transform.position.z);
		
		if ((transform.position.x - anchorX) > baseItem.GetComponent<Renderer>().bounds.size.x)
		{
			transform.position = new Vector3(anchorX, transform.position.y);
		}
	}
}
