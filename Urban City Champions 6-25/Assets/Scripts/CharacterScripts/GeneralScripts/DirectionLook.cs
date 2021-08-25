using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLook : MonoBehaviour
{
    [SerializeField]private Transform player1Transform, player2Transform;

    public float blockDistance;
    //float to gauge distance from opponents

    public bool isFlipped;
    public bool isP1Flipped;
    public bool isP2Flipped;
    public bool blockAvailable;
    //related bools and information

    // Start is called before the first frame update
    void Start()
    {
        blockAvailable = false;
        player1Transform = GameObject.Find("Player1").transform;
        player2Transform = GameObject.Find("Player2").transform;
        isFlipped = false;
        isP1Flipped = false;
        isP2Flipped = true;
        //starting values for variables and object searching
    }

    private void Update()
    {
        Vector2 direction = player1Transform.position - player2Transform.position;
        direction.y = 0;
        if (Vector2.Distance(player1Transform.position, player2Transform.position) <= blockDistance)
        {
            blockAvailable = true;
        }
        else
        {
            blockAvailable = false;
        }
        //Takes the value of distance between the two characters
        //checks if they're close enough and allows for characters to block when they're attacking
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player1Transform.position.x >= player2Transform.position.x)
        {
            flipPlayers(player1Transform);
            unflipPlayers(player2Transform);
        }
        if (player2Transform.position.x >= player1Transform.position.x)
        {
            unflipPlayers(player1Transform);
            flipPlayers(player2Transform);
        }
        // when player sprite overlap or cross over oneanother, their personal orientation is flipped to constantly face the opponent.
        //below are the functions that flip the personal Y rotation of the sprites.
    }
    private void flipPlayers(Transform curObject)
    {
        isFlipped = true;
        isP1Flipped = true;
        isP2Flipped = false;
        curObject.rotation = Quaternion.Euler(0, -180f, 0);
        curObject.position = new Vector3(curObject.position.x, curObject.position.y, 15);
        Debug.Log("flipped");
    }
    private void unflipPlayers(Transform curObject)
    {
        isFlipped = false;
        isP1Flipped = false;
        isP2Flipped = true;
        curObject.rotation = Quaternion.Euler(0, 0f, 0);
        curObject.position = new Vector3(curObject.position.x, curObject.position.y, 15);
        Debug.Log("unflipped");
    }
}
