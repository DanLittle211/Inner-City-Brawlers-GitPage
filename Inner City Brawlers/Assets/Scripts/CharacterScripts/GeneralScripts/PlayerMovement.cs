using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerMovement : MonoBehaviour
{
    public PlayerButtons otherPlayer;
    private PlayerButtons pB;
    public GameObject groundCheck;
    public bool isGrounded;
   
    public float jumpStrength;
    public float movementSpeed;

    public enum playerState{Grounded, Crouch, Jump, Block, SoftKnockdown, HardKnockdown , Immobile};

    
    public playerState currentPlayState;
    public Rigidbody2D myRB2D;
    private Animator myAnim;

    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] public Player player;
    [SerializeField] public float moveHorizontal;
    [SerializeField] public  float moveUp;

    //[HideInInspector]
    public float x1, y1;
    [HideInInspector]
    public bool xDown1, yDown1;

    public bool isDisabled;
    public bool isBlockingHigh;
    public bool isBlockingLow;


    // Start is called before the first frame update
    void Start()
    {
        isBlockingHigh = false;
        isBlockingLow = false;
        pB = this.GetComponent<PlayerButtons>(); // this pB will be changed to one on active player once animations come in
        myAnim = this.GetComponent<Animator>();
        myRB2D = GetComponent<Rigidbody2D>();
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
                 myAnim.SetBool("IsCrouch", false);
                if (-moveUp >= 0.3)
                {
                    currentPlayState = playerState.Crouch;
                    Debug.Log("Pressed Down");
                }
                if (moveUp > 0.2)
                {
                    MoveFunction(0.0012f, 0f, jumpStrength);
                    currentPlayState = playerState.Jump;
                }

                if ((moveHorizontal <= 1 && moveHorizontal > 0.5) && Mathf.Round(myRB2D.velocity.x) <= 6)
                {
                    MoveFunction(0.0012f, movementSpeed, 0f);
                    if (moveUp <= 1 && moveUp > 0.2)
                    {
                        MoveFunction(0.0012f, 0f, jumpStrength);
                    }
                    if (-moveUp <= 0.7 && -moveUp > 0.2)
                    {
                        MoveFunction(0.0012f, 0f, 0f);
                        currentPlayState = playerState.Crouch;
                    }
                }

                if ((-moveHorizontal <= 1 && -moveHorizontal > 0.5) && Mathf.Abs(myRB2D.velocity.x) <= 6)
                {

                    MoveFunction(0.0012f, -movementSpeed, 0f);
                    if (moveUp <= 1 && moveUp > 0.2)
                    {
                        MoveFunction(0.0012f, 0f, jumpStrength);
                    }
                    if (-moveUp <= 0.7 && -moveUp > 0.2)
                    {
                        MoveFunction(0.0012f, 0f, 0f);
                        currentPlayState = playerState.Crouch;
                    }


                }
                else
                {
                    //currentPlayState = playerState.Grounded;
                    //isBlockingHigh = false;
                }
                break;
            case playerState.Crouch:
                {
                    myAnim.SetBool("IsCrouch", true);
                    if (-moveUp <= 1 && -moveUp > 0.4)
                    {
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
                    else
                    {
                        currentPlayState = playerState.Grounded;
                        Debug.Log("Current Player State: " + currentPlayState);
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
