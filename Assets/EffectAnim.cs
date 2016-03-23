using UnityEngine;
using System.Collections;

public class EffectAnim : MonoBehaviour {

    public bool randomX;

	// Use this for initialization
	void Start () {
	    if (randomX)
        {
            if (Random.Range(0,2) == 1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DeleteSelf()
	{
		Destroy(gameObject);
	}
	
	public void DeleteRoot()
	{
		Destroy(transform.parent.gameObject);
	}
}
