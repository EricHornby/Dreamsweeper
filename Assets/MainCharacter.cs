using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MainCharacter : Entity {

	public string spriteName;
	public Sprite[] runSprites = new Sprite[4];
	public Sprite[] idleSprites = new Sprite[4];
	public Sprite[] fallSprites = new Sprite[4];
	public Sprite[] jumpSprites = new Sprite[4];
	public Sprite[] attackSprites = new Sprite[4];
	public Sprite[] dashSprites = new Sprite[4];
	public Sprite[] jumpAttackSprites = new Sprite[4];
	
	public Sprite[] chargeRunSprites = new Sprite[4];
	public Sprite[] chargeIdleSprites = new Sprite[4];
	public Sprite[] chargeFallSprites = new Sprite[4];
	public Sprite[] chargeJumpSprites = new Sprite[4];
	public Sprite[] chargeAttackSprites = new Sprite[4];
	public Sprite[] chargeDashSprites = new Sprite[4];
	public Sprite[] chargeJumpAttackSprites = new Sprite[4];
	
	float frameTime = 0.2f;
	
	public SpriteRenderer sprite;
	Rigidbody2D rigid;
	
	int spriteNum = 0;
	
	public CharacterState charState = CharacterState.Idle;
	
	public bool faceRight;
	
	public float speed;

    public bool hovering;
	
	public BoxCollider2D footSpot;
	public BoxCollider2D clipper;
	public Attack meleeSwipe;
	
	public float vertVelocity;
	
	public float jumpStrength = 40f;
	public float fallMax = 40f;
	
	public float jumpDecel = 1f;
	public float fallAcel = 1f;
	
	public float horizontalPressure = 0f;
	public float verticalPressure = 0f;
	
	int airJumps = 0;
	
	bool lastJumpButtonState;
	
	bool fastLand;
	bool fastFalling;
	bool superJump;
	
	bool attacking;
	bool jumpAttacking;
	bool queueAttack;
	
	float pastV;

	public bool charged;
	bool justUncharged;
	
	bool flinching;
	bool cantMoveFromHit;

	float timeOfLastRelease = 0f;
    float timeOfLastReleaseV = 0f;
	float groundTimer = 0f;
	
	
	float lastH;
	float tapH;
	bool running;

    float lastV;
    float tapV;
	
	float landTime = 0f;
			
			
	public bool enemy_was_hit_recently;
	public GameMaster GM;
	
	
	public bool pass_through;	
	
	
	public static MainCharacter instance;

    public Foot foot;
    public bool onPassablePlatform;

    public Vector3 originalSpritePosition;

    public float fastFallStartTime;
			
	public enum CharacterState{
		Idle,
		Jumping,
		Running,
		Falling,
	}

	// Use this for initialization
	void Start () {
       
        foot = GetComponentInChildren<Foot>();
		instance = this;
		GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
		Sprite[] sprites = Resources.LoadAll<Sprite>(spriteName);
		
		Sprite[] chargedSprites = Resources.LoadAll<Sprite>("TinyWitchCharged");
		rigid = GetComponent<Rigidbody2D>();
		
		for (int x = 0; x < 4; x++)
		{
			runSprites[x] = sprites[x];
			chargeRunSprites[x] = chargedSprites[x];
		}
		
		for (int x = 4; x < 8; x++)
		{
			idleSprites[x-4] = sprites[x];
			chargeIdleSprites[x-4] = chargedSprites[x];
		}
		
		for (int x = 8; x < 12; x++)
		{
			jumpSprites[x-8] = sprites[x];
			chargeJumpSprites[x-8] = chargedSprites[x];
		}
		
		for (int x = 12; x < 16; x++)
		{
			fallSprites[x-12] = sprites[x];
			chargeFallSprites[x-12]  = chargedSprites[x];
		}
		
		for (int x = 16; x < 20; x++)
		{
			dashSprites[x-16] = sprites[x];
			chargeDashSprites[x-16]  = chargedSprites[x];
		}
		
		for (int x = 20; x < 24; x++)
		{
			attackSprites[x-20] = sprites[x];
			chargeAttackSprites[x-20]  = chargedSprites[x];
		}
		
		for (int x = 24; x < 28; x++)
		{
			jumpAttackSprites[x-24] = sprites[x];
			chargeJumpAttackSprites[x-24]  = chargedSprites[x];
		}

        originalSpritePosition = sprite.transform.localPosition;

       Animate();	
	}
	
	void Animate()
	{
		Sprite[] sprites = idleSprites;
		if (charState == CharacterState.Idle)
		{
			sprites = idleSprites;	
			if (charged || justUncharged)
			{
				sprites = chargeIdleSprites;
			}
		}
		if (charState == CharacterState.Running)
		{
			if (!running)
			{
				sprites = runSprites;
				if (charged || justUncharged)
				{
					sprites = chargeRunSprites;
				}	
			}
			else
			{
				sprites = dashSprites;
				if (charged || justUncharged)
				{
					sprites = chargeDashSprites;
				}	
			}
			
		}
		if (charState == CharacterState.Falling)
		{
			sprites = fallSprites;	
			if (charged || justUncharged)
			{
				sprites = chargeFallSprites;
			}
		}
		if (charState == CharacterState.Jumping)
		{
			sprites = jumpSprites;	
			if (charged || justUncharged)
			{
				sprites = chargeJumpSprites;
			}
		}
		
		spriteNum++;
		
		if (attacking)
		{
				meleeSwipe.col.enabled = true;				

			
			if (spriteNum == 2)
			{
				if (charged && !enemy_was_hit_recently)
				{
					FireBigStar();
					charged = false;
					justUncharged = true;
				}
			}
			
			sprites = attackSprites;
			if (jumpAttacking)
			{
				sprites = jumpAttackSprites;
			}
			if (charged || justUncharged)
			{
				sprites = chargeAttackSprites;
				if (jumpAttacking)
				{
					sprites = chargeJumpAttackSprites;
				}
			}
		}
		
	
		
		if (spriteNum == sprites.Length-1)
		{
			if (attacking)
			{
				justUncharged = false;
				attacking = false;
				jumpAttacking = false;
				meleeSwipe.col.enabled = false;
			}
		}
		
		if (spriteNum > sprites.Length-1)
		{
				spriteNum = 0;
				justUncharged = false;
		}
		
		sprite.sprite = sprites[spriteNum];
		
		float nextFrame = frameTime;
		
		
		if (running)
		{
			nextFrame = frameTime * 0.6f;
		}
		
		if (attacking)
		{
			nextFrame = frameTime * 0.4f;
			
		}
		
		if (jumpAttacking)
		{
		//	nextFrame = frameTime * 0.4f;
			
		}
		
		
		
		Invoke("Animate",nextFrame);
		
		
	}
	
	
	void Attack()
	{
		if (CrossPlatformInputManager.GetButtonDown("Attack") && !cantMoveFromHit)
		{
			meleeSwipe.col.enabled = true;	
			
			if (!attacking)
			{
				enemy_was_hit_recently = false;
				spriteNum = 0;
				
				if (grounded)
				{
					sprite.sprite = attackSprites[0];
					if (charged)
					{
						sprite.sprite = chargeAttackSprites[0];
					}
				}
				else
				{
					sprite.sprite = jumpAttackSprites[0];
					if (charged)
					{
						sprite.sprite = chargeJumpSprites[0];
					}
					jumpAttacking = true;
				}
				
			}
			
			if (attacking)
			{
				queueAttack = true;
			}
			
			attacking = true;
			
			
			
			//if (spriteNum >= 1) meleeSwipe.col.enabled = true;
		}
	}
	
	void ResetFastLand()
	{
		fastLand = false;
		landTime = Time.time;
	}
	
	void Move(float h, float v)
	{
		if (h < 0 && faceRight)
		{
			FlipHoriz();	
		}
		if (h > 0 && !faceRight)
		{
			FlipHoriz();
		}
		
		float h_mag = Math.Abs(h);

        float h_raw = Input.GetAxisRaw("Horizontal");

        //if (h == 0 && (lastH!= 0))
        if (h_raw == 0 && (lastH != 0))
        {
			timeOfLastRelease = Time.time;
			tapH = lastH;
			running = false;
		}

        //if (Input.get)
		
		if ((h_raw > 0 || h_raw < 0) && Time.time-timeOfLastRelease < 0.15f && grounded && !running)
		{
			if ((tapH > 0 && h_raw > 0) || (tapH < 0 && h_raw < 0))
			{
				Debug.Log("Activate Run!");
				running = true;	
				RunEffects();
			}
		}
		
		
		
		if (grounded && attacking)
		{
			h = h/2f;
			running = false;
		}
			
		if (!grounded)
		{
			
			if (vertVelocity > 0)
			{
				ChangeState(CharacterState.Jumping);
				
			}
			else
			{
				if (charState != CharacterState.Falling)
				{
					ChangeState(CharacterState.Falling);
				}
				
			}
		}
		else
		{
			airJumps = 0;
			if (h_mag > 0)
			{
				ChangeState(CharacterState.Running);
			}
			else
			{
				ChangeState(CharacterState.Idle);
			}
		}
		
		
		
		
		if (CrossPlatformInputManager.GetButton("Jump"))
		{
				//Jump();
		}
		else
		{
			if (vertVelocity > 0 && !attacking)
			{
				vertVelocity = vertVelocity/2f;
			}
			lastJumpButtonState = false;
		}
		
		float h_pressure_localized = horizontalPressure;
		
		if (!faceRight)
		{
			h_pressure_localized = horizontalPressure * -1f;
		}
		
		float tempSpeed = speed;
		
		if (running)
		{
			tempSpeed = speed*1.5f;
		}

        if (fastFalling)
        {
            vertVelocity *= 1.5f;
            if (charState == CharacterState.Falling && !attacking)
            {

            }
            else
            {
                Debug.Log("Fast Fall interrupted " + charState + " " + attacking);
                fastFalling = false;
            }
        }

        rigid.MovePosition(new Vector2(transform.position.x + (h + h_pressure_localized) * tempSpeed * Time.deltaTime, transform.position.y + verticalPressure + vertVelocity * Time.deltaTime));
		
		if (Math.Abs(h) > 0 || Math.Abs(vertVelocity) > 0)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}
		//rigid.transform.position = new Vector2(transform.position.x + (h + h_pressure_localized) * speed * Time.deltaTime, transform.position.y + verticalPressure + vertVelocity * Time.deltaTime); 
		
		if (v< -0.5f && pastV > -0.5f)
		{
            //activate FastFall

            BeginFastFall();

            /*

            if (!fastFalling)
            {
                fastFallStartTime = Time.time;
            }
			fastFalling = true;*/

		}

        float raw_v = CrossPlatformInputManager.GetAxisRaw("Vertical");

        /*
        if (raw_v < -0.5f && charState == CharacterState.Falling)
        {
            if (!fastFalling)
            {
                fastFallStartTime = Time.time;
            }
            fastFalling = true;
        }*/
		

       
		
		
		pastV = v;
		lastH = h;
	}
	
	void RunEffects()
	{
		if (grounded)
			CreateEffect("SpeedUpCloud");
		if (running)
		{
			Invoke("RunEffects",0.1f);
		}
	}
	
	void Fall(float v)
	{
		if (!grounded)
		{
			if (vertVelocity > 0)
			{
				vertVelocity = vertVelocity *0.95f;
				//vertVelocity -= 0.02f;
				vertVelocity -= jumpDecel;
				if (vertVelocity < 0)
				{
					vertVelocity = 0;
				}
			}
			else
			{
				//vertVelocity = vertVelocity * 1.05f;
				//vertVelocity -= 0.02f;
				vertVelocity -= fallAcel;
			}
			
			if (vertVelocity > jumpStrength)
			{
				//vertVelocity = jumpStrength;
			}
			if (vertVelocity*-1f > fallMax)
			{
				if (v >= 0)
				{
					vertVelocity = -fallMax;
				}

                if (fastFalling)
                {
                    vertVelocity = -fallMax * 3;
                }

            }

            
			
			if (charState == CharacterState.Falling)
			{
				if (!attacking && (CrossPlatformInputManager.GetButton("Jump")))
				{
                    fastFalling = false;
                    hovering = true;
					if (vertVelocity*-1f > fallMax/3)
					{
						//Hover
						vertVelocity = -fallMax/3;
					}
				}
                else
                {
                    hovering = false;
                }
				
			}
		}
		else
		{
			vertVelocity = 0;
		}
	}
	
	void Jump()
	{
		if (!cantMoveFromHit)
		{
			if (grounded && !attacking)
			{
				ChangeState(CharacterState.Jumping);
				if (!fastLand)
				{
					
					vertVelocity = jumpStrength;
				}
				else
				{
					//super jump for fast land -> jump
//					Debug.Log("super jump!");
					superJump = true;
					JumpStars();
					vertVelocity = jumpStrength * 1.75f;
                }
				
				
			}	
			else if (!lastJumpButtonState)
			{
				if (airJumps < 1)
				{
					CreateEffect("JumpEffect2");
					vertVelocity = jumpStrength;
					airJumps++;
				}
			}
			
			
			
			lastJumpButtonState = true;
		}	
		
	}
	
	void ChangeState(CharacterState cState)
	{
		if (!(cState == CharacterState.Falling && grounded))
		{
			if (cState != charState)
			{
//				Debug.Log("changing state to :" + cState);
				charState = cState;
				if (!attacking)
				{
					spriteNum = 0;
				}
			}
		}
		
		
	}
	
	void FireBigStar()
	{
		GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + "BigStar") as GameObject) as GameObject;
		jumpEffect.transform.parent = transform;
		jumpEffect.transform.localPosition = new Vector2(0f,-5f);
		jumpEffect.transform.parent = null;
		
		BigStar star = jumpEffect.GetComponent<BigStar>();
		star.source = this;
	
		
		if (!faceRight)
		{
			jumpEffect.transform.localScale = new Vector3(-1f,1f,1f);
			star.horizVelocity = -star.horizVelocity;
		}
	}
	
	
	void CreateEffect(string effectName)
	{
        CreateEffect(effectName, new Vector2(0f, 0f));
	}

    void CreateEffect(string effectName, Vector2 sourcePos)
    {
        GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + effectName) as GameObject) as GameObject;
        jumpEffect.transform.parent = transform;
        jumpEffect.transform.localPosition = new Vector2(0f, 0f);
        jumpEffect.transform.parent = null;
        if (transform.localScale.x < 0)
        {
            jumpEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void CreateEffectFollow(string effectName)
    {
        CreateEffectFollow(effectName, new Vector2(0f, 0f));
    }

    void CreateEffectFollow(string effectName, Vector2 sourcePos)
    {
        GameObject jumpEffect = Instantiate(Resources.Load("Prefabs/" + effectName) as GameObject) as GameObject;
        jumpEffect.transform.parent = transform;
        jumpEffect.transform.localPosition = new Vector2(0f, 0f);
        if (transform.localScale.x < 0)
        {
            jumpEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void JumpStars()
	{
		if (charState == CharacterState.Jumping && superJump)
		{
			CreateEffect("SuperJumpStar");
			Invoke("JumpStars",0.01f);
		}
	}

    void FallStars()
    {
        if (charState == CharacterState.Falling && fastFalling)
        {
            // CreateEffect("FastFallStar");
            // CreateEffect("DropShine");
            CreateEffect("JumpEffect2");
            Invoke("FallStars", 0.02f);
        }
    }
	
	void FlipHoriz()
	{
		if (!(CrossPlatformInputManager.GetButton("Jump") && charState == CharacterState.Falling) && CrossPlatformInputManager.GetAxis("Vertical") <= 0.25f && !attacking)
		{
			faceRight = !faceRight;
			
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
		
	}
	
	public override void Land()
	{
		grounded = true;
		horizontalPressure = 0;
		cantMoveFromHit = false;
		if (fastFalling)
		{
			fastLand = true;
			Invoke("ResetFastLand",0.25f);
			
			GameObject landingSplash = Instantiate(Resources.Load("Prefabs/JumpEffect1") as GameObject) as GameObject;
			landingSplash.transform.parent = transform;
			landingSplash.transform.localPosition = new Vector2(-1.27f,-3.92f);
			landingSplash.transform.parent = null;
		}
		
		if (charState == CharacterState.Falling)
		{
			ChangeState(CharacterState.Idle);
		}
		
		
	}

    public void SpriteOffsetTo(Vector3 pos)
    {
        sprite.gameObject.transform.position = pos;
    }

    public void SpriteReset()
    {
        sprite.gameObject.transform.localPosition = originalSpritePosition;
    }

    public void SpriteOffset(Vector3 offset)
    {
        sprite.gameObject.transform.position = sprite.gameObject.transform.position + offset;
    }
	
	void FixedUpdate()
	{

        

		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		
		if (cantMoveFromHit)
		{
			h = 0;
			v = 0;
		}
		
		
		
		if (grounded)
		{
			if (v < -0.5)
			{
				//groundTimer+=Time.deltaTime;
			}
			else
			{
				groundTimer = 0;
			}
		}
		
		if (charState == CharacterState.Jumping)
		{
			groundTimer = 0;
		}

		if (charState == CharacterState.Falling)
		{
			groundTimer = 0;
		}
				
        /*
		if (groundTimer > 0.4f)
		{
			pass_through = true;
			GetComponent<Rigidbody2D>().AddForce(new Vector3(0,-0.001f));
            foot.footPass = true;

		}
		else
		{
			pass_through = false;
            foot.footPass = false;
        }*/
        
        /*
        if (!onPassablePlatform)
        {
            pass_through = false;
            foot.footPass = false;
        }	
        */
        
        
        float v_raw = CrossPlatformInputManager.GetAxisRaw("Vertical");

        /*
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            pass_through = true;
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -0.01f));
            foot.footPass = true;
            Debug.Log("Pass Through!");
        }*/
		
		
		
	
		
		if (grounded || charState == CharacterState.Falling)
		{
			//superJump = false;
		}
		
		if (footIsTouchingGround && charState != CharacterState.Jumping)
		{
			if (!grounded)
			{
				Land();
			}
			
			grounded = true;
			
		}
		else
		{
				grounded = false;
			
		}
		
		Fall(v);
		Move(h, v);
		
		if (bodyIsClippingSomething)
		{
			if (otherColTag == "Enemy")
			{
				Attacked(1);
			}
		}
		
        
	}
	
	public override void Attacked(int force)
	{
		if (!flinching)
		{
			flinching = true;
			//gameObject.AddComponent<FlashWhite>();
			GM.bar.Damage(force);
			
			horizontalPressure = -0.5f;
			
			verticalPressure = 2f;
			if (grounded)
			{
				
			}
			else
			{
				vertVelocity = 0f;
			}
			EndInvocation();
			Invoke("FlinchCancel",0.2f);
			Invoke("EndFlinch",1.5f);
			cantMoveFromHit = true;
			ChangeState(CharacterState.Falling);
			FlashRepeating();
		}
		
		
		
	}
	
	void FlashRepeating()
	{
		float f_speed = 0.25f;
		InvokeRepeating("Flash",0f,f_speed*2);
		InvokeRepeating("Unflash",f_speed,f_speed*2);
		
	}
	
	void EndInvocation()
	{
		CancelInvoke("Flash");
		CancelInvoke("Unflash");
		sprite.material.SetFloat("_FlashAmount",0f);
		sprite.color = new Color(1f, 1f, 1f, 1f);
	}
	
	void Flash()
	{
		//sprite.material.SetFloat("_FlashAmount",0.7f);
		sprite.material.SetFloat("_FlashAmount",0f);
		sprite.color = new Color(1f, 1f, 1f, 0.5f);
	}
	
	void Unflash()
	{
		//sprite.material.SetFloat("_FlashAmount",0f);
		sprite.material.SetFloat("_FlashAmount",0.25f);
		sprite.color = new Color(1f, 1f, 1f, 1f);
	}
	
	void EndFlinch()
	{
		flinching = false;
		bodyIsClippingSomething = false;
		EndInvocation();
	}
	
	void FlinchCancel()
	{
		cantMoveFromHit = true;
		//vertVelocity = -50f;
		verticalPressure = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        /*
            float h = CrossPlatformInputManager.GetAxis("Horizontal");

            Move (h, v);
            Attack();
            */

        //most previously active
        

		float v = CrossPlatformInputManager.GetAxis("Vertical");

        float v_raw = CrossPlatformInputManager.GetAxisRaw("Vertical");

        if (CrossPlatformInputManager.GetButtonDown("Jump") && (!CrossPlatformInputManager.GetButton("Down") || fastLand || !onPassablePlatform) && ((v >= -0.6f && v_raw >= -0.6f) || fastLand || !onPassablePlatform))
		{
			Jump();
            if (onPassablePlatform)
            {
                Debug.Log("Jumped from Passable Platform! " + fastLand + " " + v_raw);
            }
		}
        else if ((v_raw < -0.6 || CrossPlatformInputManager.GetButton("Down")) && CrossPlatformInputManager.GetButtonDown("Jump") && onPassablePlatform)
        {
            /*
            pass_through = true;
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -0.01f));
            foot.footPass = true;
            Debug.Log("Pass Through In Update!");*/
        }

        
        PassDown();

        Attack();

      
        if (CrossPlatformInputManager.GetButton("Down") && charState == CharacterState.Falling)
        {
            BeginFastFall();
        }

    }

    void BeginFastFall()
    {
        if (!fastFalling && !grounded && !fastLand && !hovering && !pass_through)
        {
            fastFalling = true;
            // CreateEffect("DropShine", new Vector2(0f, -20f));
            FallStars();


           // CreateEffect("FastFallStar");
        }
       
        //CreateEffect("FastFallStar");
        //CreateEffect("FastFallStar");
    }
	
	void PassDown()
    {
        float v_raw = Input.GetAxisRaw("Vertical");

        if (CrossPlatformInputManager.GetButton("Down"))
        {
            v_raw = -1f;
        }

        //if (h == 0 && (lastH!= 0))
        if (v_raw == 0 && (lastV != 0))
        {
            timeOfLastReleaseV = Time.time;
            tapV = lastV;
            pass_through = false;
        }

        //if (Input.get)

        if ((v_raw > 0 || v_raw < 0) && Time.time - timeOfLastReleaseV < 0.15f && grounded)
        {
            if ((tapV > 0 && v_raw > 0) || (tapV < 0 && v_raw < 0))
            {
                Debug.Log("Activate PassDown!");
                pass_through = true;
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -0.01f));
                foot.footPass = true;
                Invoke("UndoPassDown", 0.25f);
            }
        }

        lastV = v_raw;
    }

    void UndoPassDown()
    {
        pass_through = false;
        foot.footPass = false;
    }


	
}
