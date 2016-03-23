using UnityEngine;
using System.Collections;

public class HealthPip : MonoBehaviour {

	public int hp = 0;
	
	public Sprite[] sprites = new Sprite[4];

	SpriteRenderer sprite;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		sprite.sprite = sprites[hp];
	}
}
