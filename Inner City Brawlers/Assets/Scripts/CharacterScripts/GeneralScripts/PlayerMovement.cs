using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class PlayerMovement : MonoBehaviour
{
    [Header("Script References")]
    public PlayerButtons otherPlayer;
    private PlayerButtons pB;

    [Header("Object References")]
    [HideInInspector]
    [SerializeField] public Rigidbody2D myRB2D;
    [SerializeField] private Animator myAnim;

    [Header("GameObject References")]
    public GameObject groundCheck;

    [Header("Booleans")]
    public bool isGrounded;
    public bool isDisabled;
    public bool isBlockingHigh;
    public bool isBlockingLow;

    [Header("Floats")]
    public float jumpStrength;
    public float movementSpeed;

    public enum playerState { Grounded, Crouch, Jump, Block, SoftKnockdown, HardKnockdown, Immobile };
    public playerState currentPlayState;


    //Rewired
    [Header("Rewired Variables")]
    [SerializeField] public int playerID;
    [SerializeField] public Player player;
    [SerializeField] public float moveHorizontal;
    [SerializeField] public float moveUp;
    public Transform head;

    [HideInInspector]
    public float x1, y1;
    public Vector2 walkMovement;
    [HideInInspector]
    public bool xDown1, yDown1;

    string currentState;
    const string IDLE = "idle";
    const string WalkF = "walk";
    const string WalkB = "wback";
    const string Jump = "jump";
    const string MidAir = "falling";
    const string Land = "land";
    const string toCrouch = "idleToCrouch";
    const string Crouch = "crouch";
    const string toIdle = "crouch2Idle";

    // Start is called before the first frame update
    void Start()
    {
        currentPlayState = playerState.Grounded;
        walkMovement = Vector2.zero;
        isBlockingHigh = false;
        isBlockingLow = false;
        pB = this.GetComponentInChildren<PlayerButtons>(); // this pB will be changed to one on active player once animations come in
        myAnim = this.GetComponentInChildren<Animator>();
        myRB2D = this.GetComponent<Rigidbody2D>();
        player = ReInput.players.GetPlayer(playerID);
        
        if (playerID == 0)
        {
            SetControllerMapsForCurrentModeP1();
        }
        if (playerID == 1)
        {
            SetControllerMapsForCurrentModeP2();
        }
        x1 = 0;
        y1 = 0;
        xDown1 = false;
        yDown1 = false;
    }
    void ChangeAnimationState(string thisState) 
    {
        //stop anim playing multiple times
        if (currentState == thisState)
        {
            return;
        }
        //plays animation
        myAnim.Play(thisState.ToString());

        currentState = thisState;
    }

    public void SetControllerMapsForCurrentModeP1()
    {
        player.controllers.maps.LoadMap(ControllerType.Keyboard, playerID, "Default", "Player1", true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, playerID, "Default", "Player1", true);
    }

    public void SetControllerMapsForCurrentModeP2()
    {
        player.controllers.maps.LoadMap(ControllerType.Keyboard, playerID, "Default", "Player2", true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, playerID, "Default", "Player2", true);
    }

    // Update is called once per frame
    public void Update()
    {
        //myAnim.SetFloat("yVelocity", this.transform.position.y);
        //myAnim.SetFloat("headVelocity", head.position.y);
        
        if (isDisabled != true)
        {
            GetInput(ref x1, ref xDown1, "Move Horizontal");
            GetInput(ref y1, ref yDown1, "Move Vertical");

        }
    }

    private void FixedUpdate()
    {
        if (isDisabled != true)
        {
           playerMovement();

        }
    }


    public void GetInput(ref float val, ref bool down, string axis)
    {
        float input = player.GetAxisRaw(axis);
        input = (input > 0.5f) ? 1 : ((input < -0.5f) ? -1 : 0);

        if (input != val)
        {
            val = input;
            if (val != 0)
            {
                down = true;
                
            }
            else
            {
                down = false;
            }
        }
        else
        {
            down = false;
        }
    }
    public void playerMovement()
    {
        DirectionLook dlook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
        moveHorizontal = player.GetAxisRaw("Move Horizontal");
        moveHorizontal = (moveHorizontal > 0.3f) ? 1 : ((moveHorizontal < -0.3f) ? -1 : 0);
        moveUp = player.GetAxisRaw("Move Vertical");
        moveUp = (moveUp >= 0.75f) ? 1 : ((moveUp <= -0.3f) ? -1 : 0); // Forces values of moveUp to be 1 or zero depending on moving direction

        switch (currentPlayState)
        {
            case playerState.Grounded:
                if (-moveUp >= 0.3)
                {
                    // myAnim.SetFloat("Yaxis", moveUp);
                    // myAnim.SetBool("isCrouch", true);
                    StartCoroutine(IdleToCrouchSequence(0f));
                    Debug.Log("Pressed Down");
                }
                if (moveUp > 0.2)
                {
                    StopAllCoroutines();
                    StartCoroutine(JumpSequence(1f));
                    MoveFunction(0.0012f, 0f, jumpStrength);
                    currentPlayState = playerState.Jump;
                }
                //myAnim.SetBool("isCrouch", false);
                if (isGrounded)
                {
                    if ((moveHorizontal <= 1 && moveHorizontal > 0.5) && Mathf.Round(myRB2D.velocity.x) <= 6)
                    {
                        
                        if (moveUp == 0)
                        {
                            currentPlayState = playerState.Grounded;
                            MoveFunction(0.0012f, movementSpeed, 0f);
                            if (playerID == 0)
                            {
                                if (dlook.isP1Flipped == true)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkF));

                                }
                                else if (dlook.isP1Flipped == false)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkB));

                                }
                            }
                            if (playerID == 1)
                            {
                                if (dlook.isP2Flipped == true)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkF));

                                }
                                else if (dlook.isP2Flipped == false)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkB));

                                }
                            }
                        }
                        else if (moveUp <= 1 && moveUp > 0.2)
                        {
                            MoveFunction(0.0012f, 0f, jumpStrength);

                        }
                        else if (-moveUp <= 1 && -moveUp > 0.2)
                        {
                            MoveFunction(0.0012f, 0f, 0f);
                            StartCoroutine(IdleToCrouchSequence(0f));
                        }
                        
                    }


                    else if ((-moveHorizontal <= 1 && -moveHorizontal > 0.5) && Mathf.Abs(myRB2D.velocity.x) <= 6)
                    {
                        
                        if (moveUp == 0)
                        {
                            currentPlayState = playerState.Grounded;
                            MoveFunction(0.0012f, -movementSpeed, 0f);
                            if (playerID == 0)
                            {
                                if (dlook.isP1Flipped == true)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkB));

                                }
                                else if (dlook.isP1Flipped == false)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkF));

                                }
                            }
                            if (playerID == 1)
                            {
                                if (dlook.isP2Flipped == true)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkB));
                                }
                                else if (dlook.isP2Flipped == false)
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(MoveAction(0f, WalkF));

                                }
                            }
                        }
                        else if (moveUp <= 1 && moveUp > 0.2)
                        {
                            MoveFunction(0.0012f, 0f, jumpStrength);
                        }
                        else if (-moveUp <= 1 && -moveUp > 0.2)
                        {
                            MoveFunction(0.0012f, 0f, 0f);
                            StartCoroutine(IdleToCrouchSequence(0f));
                        }
                        
                    }

                    if (moveHorizontal == 0 && moveUp == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(MoveAction(0f, IDLE));
                        currentPlayState = playerState.Grounded;

                    }
                }
                break;
            case playerState.Crouch:
                {
                    if (-moveUp <= 1 && -moveUp > 0.4)
                    {
                        ChangeAnimationState(Crouch);
                        if (moveHorizontal <= 0.5 && moveHorizontal > 0)
                        {
                            MoveFunction(0f, 0f, 0f);
                            currentPlayState = playerState.Crouch;
                            Debug.Log("Current Player State: " + currentPlayState + " Right");
                        }
                        if (-moveHorizontal <= 0.5 && -moveHorizontal > 0)
                        {
                            MoveFunction(0f, 0f, 0f);
                            currentPlayState = playerState.Crouch;
                            Debug.Log("Current Player State: " + currentPlayState + " Left");
                        }
                        MoveFunction(0f, 0f, 0f);
                        Debug.Log("Current Player State: " + currentPlayState);
                    }
                    else if(moveUp == 0)
                    {
                        StartCoroutine(CrouchToIdleSequence(0f));  
                    }

                    break;
                }
            case playerState.Jump:
                {
                    if (moveHorizontal >= 0.5 && Mathf.Round(myRB2D.velocity.x) <= 6)
                    {
                        MoveFunction(0.0012f, (movementSpeed / 2), 0f); // while in air, you move slightly to the right
                    }
                    if (-moveHorizontal >= 0.5 && Mathf.Abs(myRB2D.velocity.x) <= 6)
                    {
                        MoveFunction(0.0012f, (-movementSpeed / 2), 0f);  // while in air, you move slightly to the left
                    }
                    
                    break;
                }
            case playerState.SoftKnockdown:
                {

                    break;
                }
            case playerState.HardKnockdown:
                {

                    break;
                }
            case playerState.Immobile:
                {

                    break;
                }
        }
        if (playerID == 0)
        {
            if (dlook.isFlipped == false)
            {
                walkMovement.x = -(x1);
            }
            else
            {
                walkMovement.x = (x1);
            }
        }
        if (playerID == 1)
        {
            if (dlook.isFlipped == true)
            {
                walkMovement.x = -(x1);
            }
            else
            {
                walkMovement.x = (x1);
            }
        }

        Debug.Log("Current Player State: " + currentPlayState);
 
    }
    IEnumerator MoveAction(float time, string ActionString)
    {
        yield return new WaitForSeconds(0.01f);
        ChangeAnimationState(ActionString);

    }
    IEnumerator JumpSequence(float time)
    {
        ChangeAnimationState(Jump);
        yield return new WaitForSeconds(0.667f);
        ChangeAnimationState(MidAir);
        yield return new WaitForSeconds(0.007f);
        ChangeAnimationState(Land);
        yield return new WaitForSeconds(1f);
    }
    IEnumerator IdleToCrouchSequence(float time)
    {
        ChangeAnimationState(toCrouch);
        yield return new WaitForSeconds(0.1f);
        currentPlayState = playerState.Crouch;
        //CheckEquals();
    }
    IEnumerator CrouchToIdleSequence(float time)
    {
        ChangeAnimationState(toIdle);
        yield return new WaitForSeconds(0.1f);
        currentPlayState = playerState.Grounded;
        //CheckEquals();
    }
    public Vector2 GetInput()
    {
        return new Vector2(x1, y1);
    }
    public bool InputDownX1()
    {
        return xDown1;
    }
    public bool InputDownY1()
    {
        return yDown1;
    }
    public void MoveFunction(float jumpLimiter, float speed, float jumpForce)
    {
        if (isGrounded == true && Mathf.Abs(myRB2D.velocity.y) <= jumpLimiter)
        {
            myRB2D.AddForce(new Vector2(speed, jumpForce), ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            isGrounded = true;
            currentPlayState = playerState.Grounded;

            // myAnim.SetBool("Jump", !isGrounded);

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            isGrounded = false;
            currentPlayState = playerState.Jump;
        }
    }
}
