using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Healthbar : MonoBehaviour {


	public int maxHP;
	public int hp;
	
	public MainCharacter target;
	
	public Stack<HealthPip> fullPips = new Stack<HealthPip>();
	public Stack<HealthPip> emptyPips = new Stack<HealthPip>();
	
	public List<HealthPip> allPips = new List<HealthPip>();
	
	// Use this for initialization
	void Start () {
		target = GameObject.Find("TinyWitch").GetComponent<MainCharacter>();
		
		for (int x = 0; x < maxHP/3; x++)
		{
			GameObject pipObj = Instantiate(Resources.Load("Prefabs/HealthPip") as GameObject) as GameObject;
			pipObj.transform.parent = transform;
			pipObj.transform.localPosition = new Vector3(x *10f, 0f);
			fullPips.Push(pipObj.GetComponent<HealthPip>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate()
	{
		int pipHP = 0;
		if (hp > maxHP)
		{
			hp = maxHP;
		}
		
		foreach(HealthPip h in fullPips)
		{
			pipHP += h.hp;
		}
		
		
		
		if (hp != 0)
		{
			if (pipHP < hp)
			{
				HealthPip topPip = fullPips.Peek();
				if (topPip.hp < 3)
				{	
					topPip.hp++;
				}
				else
				{
					HealthPip newTopPip = emptyPips.Pop();
					newTopPip.hp++;
					fullPips.Push(newTopPip);
				}	
			}
			
			if (pipHP > hp)
			{
				HealthPip topPip = fullPips.Peek();
				if (topPip.hp > 0)
				{
					topPip.hp--;
				}
				else
				{
					emptyPips.Push(fullPips.Pop());
				}
			}
		}
		
		
	}
	
	public void Damage(int amt)
	{
		hp -= amt;
	}
}
