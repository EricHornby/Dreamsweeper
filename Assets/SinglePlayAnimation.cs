using UnityEngine;
using System.Collections;

public class SinglePlayAnimation : MonoBehaviour {

    SpriteRenderer sprite;

    public Sprite[] sprites;

    public float frameSpeed;

    float lastFrameTime;

    int frame = 0;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[0];
        lastFrameTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time - lastFrameTime > frameSpeed)
        {
            frame++;
            lastFrameTime = Time.time;
            

            if (frame >= sprites.Length)
            {
                Destroy(gameObject);
                
            }
            else
            {
                sprite.sprite = sprites[frame];
            }
        }
	}
}
