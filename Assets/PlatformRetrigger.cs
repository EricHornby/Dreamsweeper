using UnityEngine;
using System.Collections;

public class PlatformRetrigger : MonoBehaviour {

	int count;

	public Tile t;

	void Start () {
		//count = 0;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		count++;
	}
	

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name != "Foot")
        {
            t.Solidify();
            Debug.Log("solidify " + other.gameObject.name);

           // Debug.Log(other.gameObject.name);
        }
			
	}
}
