using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedFog : MonoBehaviour {


	public string fogName;
	Sprite[] FogParts;
	SpriteRenderer[] children;
	float[] offsets;
	float[] speeds;
	
	public float speedMulti = -0.25f;
	float[] baseSpeeds = new float[]{1f,1.25f,1.5f,1.75f};
	
	public bool multiway;
	
	public bool sway;
	public float swayTime = 4f;
	public float swayLengthFactor = 0.5f;
	float originalSpeedMulti;
	float swayPoint;
	float oldSpeed;
	float targetSpeed;	
	bool midSway;
	
	public bool sinusoidal;
	public float radMax = 2f;
	public bool absSinusoidal;
	
	public bool cascade;
	public float cascadeTime = 1f;
	public float cascadeLengthFactor = 0.5f;
	float cascadePoint;
	bool midCascade;
	float[] oldSpeeds;
	float[] targetSpeeds;
	


	// Use this for initialization
	void Start () {
	
		FogParts = Resources.LoadAll<Sprite>(fogName);
		
		children = new SpriteRenderer[FogParts.Length];
		offsets = new float[FogParts.Length];
		speeds = new float[FogParts.Length];
		oldSpeeds = new float[FogParts.Length];
		targetSpeeds = new float[FogParts.Length];
		
		originalSpeedMulti = speedMulti;
		swayPoint = swayTime;
		cascadePoint = cascadeTime;
		
		if (swayTime == 0)
		{
			sway = false;
		}
		
		for (int x = FogParts.Length-1; x>=0; x--)
		{
			CreateChildSprite(x);
			offsets[x] = 0f;
			speeds[x] =baseSpeeds[Random.Range(0,baseSpeeds.Length)];
			if (x < FogParts.Length-1)
			{
				if (speeds[x] == speeds[x+1])
				{
					speeds[x] = baseSpeeds[Random.Range(0,baseSpeeds.Length)];
				}
			}
			
			if (sinusoidal)
			{
				speeds[x] = 1.5f*Mathf.Sin( ( (float)x/(FogParts.Length-1)) * radMax * 3.14f + 0.1f);
				
				if (absSinusoidal)
				{
					speeds[x] = Mathf.Abs(speeds[x]);
				}
			}
			
			if (multiway)
			{
				if (Random.Range(0,2) == 0)
				{
					speeds[x] = -speeds[x];
				}
			}	
		}
	}
	
	void CreateChildSprite(int x)
	{
		GameObject newChild = new GameObject();
		SpriteRenderer render = newChild.AddComponent<SpriteRenderer>();
		render.sprite = FogParts[x];
		newChild.transform.parent = transform;
		
		render.sortingOrder = -2;
		
		children[x] = render;
		
		if (x == FogParts.Length-1)
		{
			newChild.transform.localPosition = new Vector3(0f,0f);
		}
		else
		{
			newChild.transform.position = new Vector3(children[x+1].gameObject.transform.position.x,FogParts[x+1].bounds.size.y + children[x+1].transform.position.y);
		}
		
		
		GameObject tempCopy = Instantiate(newChild) as GameObject;
		
		for (int i = 1; i < 25; i++)
		{
			GameObject newChildCopy = Instantiate(tempCopy) as GameObject;
			newChildCopy.transform.parent = newChild.transform;
			newChildCopy.transform.localPosition = new Vector3(FogParts[x].bounds.size.x*i,0f);
			
			GameObject newChildCopy2 = Instantiate(tempCopy) as GameObject;
			newChildCopy2.transform.parent = newChild.transform;
			newChildCopy2.transform.localPosition = new Vector3(-FogParts[x].bounds.size.x*i,0f);
 		}
		
		Destroy(tempCopy);
		
	}
	
	
	// Update is called once per frame
	void Update () {
		for (int x =0; x<FogParts.Length; x++)
		{
			Movement(x);
		}
		
		if (Time.timeSinceLevelLoad > swayPoint && sway)
		{
			if (!midSway)
			{
				oldSpeed = speedMulti;
				targetSpeed = -speedMulti;
				midSway = true;
			}
			
			speedMulti = Mathf.Lerp(oldSpeed,targetSpeed,  (Time.timeSinceLevelLoad-swayPoint) / (swayTime*swayLengthFactor) );
			
			if (speedMulti == targetSpeed)
			{
				midSway = false;
				swayPoint = Time.timeSinceLevelLoad + swayTime;
			}
		}
		
		if (Time.timeSinceLevelLoad > cascadePoint && cascade)
		{
		
			if (!midCascade)
			{
				for (int x =0; x<FogParts.Length; x++)
				{
					oldSpeeds[x] = speeds[x];
					if (x+1 < FogParts.Length-1)
					{
						targetSpeeds[x] = speeds[x+1];
					}
					else
					{
						targetSpeeds[x] = speeds[0];
					}
					
				}
				
				midCascade = true;
			}
			
			
			for (int x = 0; x < FogParts.Length-1; x++)
			{
				speeds[x] = Mathf.Lerp(oldSpeeds[x],targetSpeeds[x],  (Time.timeSinceLevelLoad-cascadePoint) / (cascadeTime*cascadeLengthFactor) );
			}
			
			if ((Time.timeSinceLevelLoad-cascadePoint) / (cascadeTime*cascadeLengthFactor) >= 1)
			{
				midCascade = false;
				cascadePoint = Time.timeSinceLevelLoad + cascadeTime;
			}
			
			
		}
	}
	
	void Movement(int x)
	{
		children[x].gameObject.transform.position = new Vector3(children[x].gameObject.transform.position.x + speeds[x]*speedMulti,children[x].gameObject.transform.position.y);
		offsets[x]+= speeds[x]*speedMulti;
		
		if (offsets[x] > FogParts[x].bounds.size.x)
		{
			children[x].gameObject.transform.position = new Vector3(children[x].gameObject.transform.position.x - FogParts[x].bounds.size.x,children[x].gameObject.transform.position.y);
			offsets[x] -= FogParts[x].bounds.size.x;
		}
		
		if (offsets[x] < FogParts[x].bounds.size.x)
		{
			children[x].gameObject.transform.position = new Vector3(children[x].gameObject.transform.position.x + FogParts[x].bounds.size.x,children[x].gameObject.transform.position.y);
			offsets[x] += FogParts[x].bounds.size.x;
		}
	}
}
