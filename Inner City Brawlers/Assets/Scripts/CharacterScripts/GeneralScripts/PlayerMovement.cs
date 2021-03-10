using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerMovement : MonoBehaviour
{

    public GameObject groundCheck;
    public bool isGrounded;
   
    public float jumpStrength;
    public float movementSpeed;

    public enum playerState{Grounded, Crouch, Jump, Block, SoftKnockdown, HardKnockdown, Defeated, Immobile};

    
    public playerState currentPlayState;
    public Rigidbody2D myRB2D;

    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] private Player player;

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
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if(isDisabled != true)
        {
            playerMovement();
        }  
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

    public void playerMovement()
    {
        
        float moveHorizontal = player.GetAxis("Move Horizontal");
        float moveUp = player.GetAxis("Move Vertical");
        //float moveDown = player.GetAxis("Move Down");

        if (currentPlayState == playerState.Grounded)
        {
            if  (moveUp <= 1 && moveUp > 0.2)
            {
                JumpFunction();
                Debug.Log("Current Player State: " + currentPlayState);
            }
            if (-moveUp <= 1 && -moveUp > 0.2)
            {
                currentPlayState = playerState.Crouch;
                Debug.Log("Current Player State: " + currentPlayState);
            }

            if (moveHorizontal <= 1 && moveHorizontal > 0.5)
            {
                myRB2D.AddForce(new Vector2(movementSpeed, 0), ForceMode2D.Impulse);
                if (moveUp <= 1 && moveUp > 0.2)
                {
                    JumpFunction();
                    Debug.Log("Current Player State: " + currentPlayState);
                }
                if (-moveUp <= 1 && -moveUp > 0.2)
                {
                    currentPlayState = playerState.Crouch;
                    myRB2D.AddForce(new Vector2(0, 0), ForceMode2D.Impulse);
                    Debug.Log("Current Player State: " + currentPlayState);
                }
                Debug.Log("RightWalk");
            }
            if (-moveHorizontal <= 1 && -moveHorizontal > 0.5)
            {
                myRB2D.AddForce(new Vector2(-movementSpeed, 0), ForceMode2D.Impulse);
                if (moveUp <= 1 && moveUp > 0.2)
                {
                    JumpFunction();
                    Debug.Log("Current Player State: " + currentPlayState);
                }
                if (-moveUp <= 1 && -moveUp > 0.2)
                {
                    currentPlayState = playerState.Crouch;
                    myRB2D.AddForce(new Vector2(0, 0), ForceMode2D.Impulse);
                    Debug.Log("Current Player State: " + currentPlayState);
                }
                Debug.Log("LeftWalk");
            }
            else 
            {
                currentPlayState = playerState.Grounded;
                Debug.Log("Current Player State: " + currentPlayState);
            }
        }
        if (currentPlayState == playerState.Crouch)
        {
            if (moveUp <= 1 && moveUp > 0.2)
            {
                JumpFunction();
                Debug.Log("Current Player State: " + currentPlayState);
            }
            if (-moveUp <= 1 && -moveUp > 0.2)
            {
                if (moveHorizontal <= 0.5 && moveHorizontal > 0.2)
                {
                    myRB2D.AddForce(new Vector2(0, 0), ForceMode2D.Impulse);
                    currentPlayState = playerState.Crouch;
                    Debug.Log("Current Player State: " + currentPlayState + " Right");
                }
                if (-moveHorizontal <= 0.5 && -moveHorizontal > 0.2)
                {
                    myRB2D.AddForce(new Vector2(0, 0), ForceMode2D.Impulse);
                    currentPlayState = playerState.Crouch;
                    Debug.Log("Current Player State: " + currentPlayState + " Left");
                }
            }
            else
            {
                currentPlayState = playerState.Grounded;
                Debug.Log("Current Player State: " + currentPlayState);
            }
        }
    }
    
    public void JumpFunction()
    {
        if (isGrounded == true && Mathf.Abs(myRB2D.velocity.y) < 0.001f)
        {
            myRB2D.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
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
