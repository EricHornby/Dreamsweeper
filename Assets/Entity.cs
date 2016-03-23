using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public bool footIsTouchingGround;
	public bool bodyIsClippingSomething;
	public string otherColTag;

	public bool grounded;
	
	public bool isMoving;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Land()
	{
	}
	
	public virtual void Attacked(int force)
	{
		
	}
}
