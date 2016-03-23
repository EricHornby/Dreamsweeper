using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	
	public Transform target;
	public float offset;
	public JSONMapReader map;
	
	float x_extend;
	float y_extend;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = new Vector3(target.position.x, target.position.y + offset, transform.position.z);
		
		x_extend = Screen.width/4f;
		y_extend = Screen.height/4f;
		
		
		
		transform.position = new Vector3(Mathf.Clamp(newPos.x,x_extend-10f,(map.mapX)*20f-x_extend-10f), Mathf.Clamp(newPos.y, (-map.mapY)*20f+y_extend+10f,-y_extend+10f), newPos.z);
		//transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
		
	}
}
