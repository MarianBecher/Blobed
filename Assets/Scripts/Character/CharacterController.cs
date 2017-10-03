using UnityEngine;
using System.Collections.Generic;

[System.Flags]
public enum CharColor
{
    White = (1 << 0),
    Blue = (1 << 1),
    Green = (1 << 2),
    Red = (1 << 3),
}


public class CharacterController : MonoBehaviour
{
    private InputController input;


    [Header("Character")]
    [SerializeField]
    private CharacterType type = CharacterType.Small;
    [SerializeField]
#if UNITY_EDITOR
    [EnumFlag]
#endif
    private CharColor characterColor;
    public bool active = false;

    [Header("Physics")]
    [SerializeField]
    private LayerMask GROUND_LAYERS;
    [SerializeField]
    private LayerMask WALL_LAYERS;
    [SerializeField]
    private LayerMask DEATH_LAYERS;

    public CharColor CharacterColor { get { return characterColor; } set { characterColor = value; setColor(characterColor); } }
    public CharacterType Type { get { return type; } set { type = value; loadStats(); } }

    //Current Stats
    private float moveSpeed = 0;
    private float lookDirection = 1;
    private float jumpPower = 0;
    private const float wallJumpPowerModifier = 0.5f;
    private bool canWallJump = false;
    private bool grounded = false;
    private bool onWall = false;
    private int wallJumpDirection = 0;
    private bool lastFrameJumpedPressed = false;
    private const float wallJumpPause = 0.2f; //Disable DoubleJump for this time in s to prevent same wall jump
    private float lastWallJumpTs = 0;
    private bool isCurrentlyWalljumping = false;


    //Combine & Split
    private const float combineTime = 0.25f;
    private CharacterController combineTarget;
    private float tsStartWalkingInCombineTarget;
    private bool lastFrameWalkedInCombineTarget;

    //Components
    private Rigidbody2D rb;
    private CharacterContainer charContainer;
    private GameObject pointer;
    private Animator animator;
    private ReversableContainer reversableContainer;

    
	void Awake ()
    {
        setColor(characterColor);
        loadStats();
        animator = this.GetComponent<Animator>();
	    input = InputController.getController();
        charContainer = GameObject.Find("LevelManager").GetComponent<CharacterContainer>();
        reversableContainer = GameObject.Find("LevelManager").GetComponent<ReversableContainer>();

    }

    void Start()
    {
        charContainer.addCharacter(this);
        if (this.active)
        {
            charContainer.setNewActivePlayer(this);
        }
    }
	
    void Update()
    {
        HashSet<MoveInstructions> moveInstructions = this.input.getMoveInstructions();
        
        checkGrounded();
        checkDeath();
        onWall = false;
        
        if (!active || reversableContainer.IsReversing)
            moveInstructions.Clear(); //Nothing to do bro

        if (!grounded || isCurrentlyWalljumping)
        {
            checkOnWall(1);
            if(!onWall)
                checkOnWall(-1);
        }

        if (moveInstructions.Contains(MoveInstructions.Left))
        {
            if(!onWall)
                this.move(-1);
        }

        if (moveInstructions.Contains(MoveInstructions.Right))
        {
            if (!onWall)
                this.move(1);
        }

        if (!(moveInstructions.Contains(MoveInstructions.Right) || moveInstructions.Contains(MoveInstructions.Left)))
            this.stopMoving();

        if (moveInstructions.Contains(MoveInstructions.Jump))
            jump();
        else
            lastFrameJumpedPressed = false;

        if (input.isClickedOnCharacter(this) && active)
            handleSplit();

        updateAnimation();
	}

    private void updateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("active", active);
            animator.SetInteger("movement", (int)rb.velocity.x);
            animator.SetBool("onGround", grounded);
        }
    }

    private void setColor(CharColor c)
    {
        int colorCounter = 0;
        Color newColor = new Color(0,0,0);


        if((int) c >= (int) CharColor.Red)
        {
            newColor.r += 1;
            colorCounter++;
            c -= (int)CharColor.Red;
        }

        if ((int)c >= (int)CharColor.Green)
        {
            newColor.g += 1;
            colorCounter++;
            c -= (int)CharColor.Green;
        }

        if ((int)c >= (int)CharColor.Blue)
        {
            newColor.b += 1;
            colorCounter++;
            c -= (int)CharColor.Blue;
        }

        if ((int)c >= (int)CharColor.White)
        {
            if(colorCounter == 0)
            {
                newColor.r = 1f;
                newColor.g = 1f;
                newColor.b = 1f;
            }
            else if(colorCounter == 3)
            {
                newColor.r = 0.5f;
                newColor.g = 0.5f;
                newColor.b = 0.5f;
            }
            else
            {
                newColor.r += 0.5f;
                newColor.g += 0.5f;
                newColor.b += 0.5f;
            }
        }
        newColor.a = 1;

        for(int i = 0; i < 4; i++)
        {
            this.transform.Find("color-container").GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    public void loadStats()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        CharacterSettings settings = null;
        switch (this.Type)
        {
            case CharacterType.Small:
                settings = new SmallCharacterSettings();
                break;
            case CharacterType.Medium:
                settings = new MediumCharacterSettings();
                break;
            case CharacterType.Large:
                settings = new LargeCharacterSettings();
                break;
        }

        this.moveSpeed = settings.getMoveSpeed();
        this.jumpPower = settings.getJumpPower();
        this.canWallJump = settings.canWallJump();
        this.transform.localScale = settings.getScale();
        this.rb.mass = settings.getMass();
    }

    private void checkGrounded()
    {
        Vector2 collisionCheckSize = new Vector2(0.4f * this.transform.localScale.x, -0.05f * this.transform.localScale.y);
        Vector2 collisionCheckOffset = new Vector2(0f * this.transform.localScale.x, -0.31f * this.transform.localScale.y);
        Vector2 topLeft = (Vector2)this.transform.position + collisionCheckOffset - 0.5f * collisionCheckSize;
        Vector2 bottomRight = (Vector2)this.transform.position + collisionCheckOffset + 0.5f * collisionCheckSize;
        Collider2D[] collisions = Physics2D.OverlapAreaAll(topLeft, bottomRight, GROUND_LAYERS);

        grounded = false;
        foreach(Collider2D c in collisions)
        {
            if (c.gameObject != this.gameObject)
            {
                grounded = true;
                isCurrentlyWalljumping = false;
                break;
            }
        }
    }

    private void checkDeath()
    {
        Vector2 collisionCheckSize = new Vector2(0.4f * this.transform.localScale.x, -0.1f * this.transform.localScale.y);
        Vector2 collisionCheckOffset = new Vector2(0f * this.transform.localScale.x, -0.35f * this.transform.localScale.y);
        Vector2 topLeft = (Vector2)this.transform.position + collisionCheckOffset - 0.5f * collisionCheckSize;
        Vector2 bottomRight = (Vector2)this.transform.position + collisionCheckOffset + 0.5f * collisionCheckSize;

        Collider2D collider = Physics2D.OverlapArea(topLeft, bottomRight, DEATH_LAYERS);
        if(collider != null)
        {
            GameObject.Find("LevelManager").GetComponent<ReversableContainer>().startReversing();
        }
    }

    private void checkOnWall(int direction)
    {
        Vector2 collisionCheckSize = new Vector2(0.1f * this.transform.localScale.x, -0.25f * this.transform.localScale.y);
        Vector2 collisionCheckOffset = new Vector2(0.5f * this.transform.localScale.x, 0.5f * this.transform.localScale.y);
        Vector2 topLeft = (Vector2)this.transform.position + new Vector2(direction * collisionCheckOffset.x, collisionCheckOffset.y) - 0.5f * collisionCheckSize;
        Vector2 bottomRight = (Vector2)this.transform.position + new Vector2(direction * collisionCheckOffset.x, collisionCheckOffset.y) + 0.5f * collisionCheckSize;
        Collider2D collider = Physics2D.OverlapArea(topLeft, bottomRight, WALL_LAYERS);
        onWall = collider != null && !grounded;
        wallJumpDirection = -direction;
    }

    private void move(float direction)
    {

        direction = Mathf.Sign(direction); //make sure it is realy only 1 or -1;
        this.transform.Find("eye-container").localScale = new Vector3(-direction, 1, 1) * 0.8f;
        this.lookDirection = -1*direction;
        if(grounded)
        {
            rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
            checkCombine(direction);
        }
        else if(!isCurrentlyWalljumping)
        {
            rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
        }
    }

    private void checkCombine(float direction)
    {
        Vector2 collisionCheckSize = new Vector2(0.1f * this.transform.localScale.x, -0.25f * this.transform.localScale.y);
        Vector2 collisionCheckOffset = new Vector2(0.5f * this.transform.localScale.x, 0.4f * this.transform.localScale.y);
        LayerMask ground_layer = 1 << this.gameObject.layer;

        Vector2 topLeft = (Vector2)this.transform.position + new Vector2(direction * collisionCheckOffset.x, collisionCheckOffset.y) - 0.5f * collisionCheckSize;
        Vector2 bottomRight = (Vector2)this.transform.position + new Vector2(direction * collisionCheckOffset.x, collisionCheckOffset.y) + 0.5f * collisionCheckSize;
        Collider2D[] allColider = Physics2D.OverlapAreaAll(topLeft, bottomRight, ground_layer);
        Collider2D realColider = null;

        foreach(Collider2D c in allColider)
        {
            if (c.transform == this.transform)
                continue;
            realColider = c;
        }

        if(realColider != null && realColider.GetComponent<CharacterController>().Type == this.Type)
        {
            CharacterController target = realColider.GetComponent<CharacterController>();
            if (target == combineTarget && this.lastFrameWalkedInCombineTarget)
            {
                //Same Target
                float deltaTime = Time.time - tsStartWalkingInCombineTarget;
                if(deltaTime > combineTime)
                {
                   this.handleCombine();
                }
            }
            else
            {
                //New Target
                this.combineTarget = target;
                this.tsStartWalkingInCombineTarget = Time.time;
            }
            lastFrameWalkedInCombineTarget = true;
        }
        else
        {
            lastFrameWalkedInCombineTarget = false;
        }
    }

    private void stopMoving()
    {
        lastFrameWalkedInCombineTarget = false;
        if(grounded)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if(onWall && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

        }
    }

    private void jump()
    {
        if (grounded && !lastFrameJumpedPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(jumpPower * new Vector2(0, 1));
        }
        else if(!grounded && onWall && canWallJump && Time.time - lastWallJumpTs > wallJumpPause)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); //Kill current downfall
            rb.AddForce(wallJumpPowerModifier * jumpPower * new Vector2(wallJumpDirection, 2));
            lastWallJumpTs = Time.time;
            isCurrentlyWalljumping = true;
        }
        lastFrameJumpedPressed = true;
    }

    private void handleSplit()
    {
        CharacterType childrenType = CharacterType.Small;
        switch(this.Type)
        {
            case CharacterType.Large:
                childrenType = CharacterType.Medium;
                break;
            case CharacterType.Medium:
                childrenType = CharacterType.Small;
                break;
            case CharacterType.Small:
                return;
        }

        int count = 0;
        int c = (int) this.characterColor;
        int color_1 = 0;
        int color_2 = 0;
        for(int i = 3; i >= 0; i--)
        {
            int color = 1 << i;

            if (c >= color)
            {
                if (count % 2 == 0)
                    color_1 += (int)color;
                else
                    color_2 += (int)color;
                c -= color;
                count++;
            }
        }


        CharacterController child1 = ((GameObject)Instantiate(Resources.Load("Character"))).GetComponent<CharacterController>();
        child1.transform.position = this.transform.position + new Vector3(0.5f,0,0);
        child1.Type = childrenType;
        child1.active = true;
        child1.CharacterColor = (CharColor) color_1;


        CharacterController child2 = ((GameObject)Instantiate(Resources.Load("Character"))).GetComponent<CharacterController>();
        child2.transform.position = this.transform.position - new Vector3(0.5f, 0, 0);
        child2.Type = childrenType;
        child2.active = false;
        child2.CharacterColor = (CharColor) color_2;

        charContainer.removeCharacter(this);
        this.GetComponent<Reversable>().destroyObject();
        input.clickActionPerformed();
    }

    private void handleCombine()
    {
        if (combineTarget == null)
            return;

        if (this.Type != combineTarget.Type)
            return;

        CharacterType parentType = CharacterType.Small;
        switch (this.Type)
        {
            case CharacterType.Large:
                return;
            case CharacterType.Medium:
                parentType = CharacterType.Large;
                break;
            case CharacterType.Small:
                parentType = CharacterType.Medium;
                break;
        }   

        CharacterController child = ((GameObject)Instantiate(Resources.Load("Character"))).GetComponent<CharacterController>();
        child.transform.position = 0.5f * (this.transform.position + combineTarget.transform.position);
        child.Type = parentType;
        child.active = true;
        child.CharacterColor = (CharColor) ((int) this.characterColor + (int) combineTarget.characterColor);

        charContainer.removeCharacter(combineTarget);
        combineTarget.GetComponent<Reversable>().destroyObject();

        charContainer.removeCharacter(this);
        this.GetComponent<Reversable>().destroyObject();
    }

    public bool isGrounded()
    {
        return grounded;
    }
}
