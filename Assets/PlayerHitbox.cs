using UnityEngine;
using System.Collections;

public class PlayerHitbox : MonoBehaviour {

	public Entity home;
	
	
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.name != "dead")
		{
			home.bodyIsClippingSomething = true;
			home.otherColTag = other.tag;
	//		Debug.Log(other.gameObject.name);
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		home.bodyIsClippingSomething = false;
		home.otherColTag = null;
	}
}
