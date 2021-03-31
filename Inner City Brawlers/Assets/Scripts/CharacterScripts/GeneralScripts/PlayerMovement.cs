using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public GameObject groundCheck;
    public bool isGrounded;
   
    public float jumpStrength;
    public float movementSpeed;

    public enum playerState{Grounded, Crouch, Jump, Block, SoftKnockdown, HardKnockdown , Immobile};

    
    public playerState currentPlayState;
    public Rigidbody2D myRB2D;

    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] public Player player;
    [SerializeField] public float moveHorizontal;
    [SerializeField] public  float moveUp;

    public static float x, y;
    static bool xDown, yDown;

    public bool isDisabled;
    // Start is called before the first frame update
    void Start()
    {
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
        x = 0;
        y = 0;
        xDown = false;
        yDown = false;
        
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
            playerMovement();

            GetInput(ref x, ref xDown, "Move Horizontal");
            GetInput(ref y, ref yDown, "Move Vertical");
        }
        
    }

    public void GetInput(ref float val, ref bool down, string axis/*, int playerNum*/)
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
        
        moveHorizontal = player.GetAxisRaw("Move Horizontal");
        moveHorizontal = (moveHorizontal > 0.3f) ? 1 : ((moveHorizontal < -0.3f) ? -1 : 0);
        moveUp = player.GetAxisRaw("Move Vertical");
        moveUp = (moveUp > 0.75f) ? 1 : ((moveUp < -0.3f) ? -1 : 0); // Forces values of moveUp to be 1 or zero depending on moving direction

        switch (currentPlayState)
        {
            case playerState.Grounded:
                if (-moveUp >= 0.4)
                {
                    currentPlayState = playerState.Crouch;
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
                    if (-moveUp <= 1 && -moveUp > 0.2)
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
                    currentPlayState = playerState.Grounded;
                }
                break;
            case playerState.Crouch:
                {
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
                            MoveFunction(0f, 0f,0f);
                            currentPlayState = playerState.Crouch;
                            Debug.Log("Current Player State: " + currentPlayState + " Left");
                        }
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
                    if (moveHorizontal > 0.5 && Mathf.Round(myRB2D.velocity.x) <= 6)
                    {
                        MoveFunction(0.0012f, (movementSpeed / 2), 0f); // while in air, you move slightly to the right
                    }
                    if (-moveHorizontal > 0.5 && Mathf.Abs(myRB2D.velocity.x) <= 6)
                    {
                        MoveFunction(0.0012f, (-movementSpeed / 2), 0f);  // while in air, you move slightly to the left
                    }
                    break;
                }
        }
    }

    public static bool InputDownX()
    {
        return xDown;
    }
    public static bool InputDownY()
    {
        return yDown;
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
