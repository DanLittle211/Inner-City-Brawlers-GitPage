using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLook : MonoBehaviour
{
    public Transform player1Transform;
    public Transform player2Transform;

    public bool isFlipped;
    // Start is called before the first frame update
    void Start()
    {
        isFlipped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player1Transform.position.x >= player2Transform.position.x)
        { 
            flipPlayer1();
            flipPlayer2();
        }
        if (player2Transform.position.x >= player1Transform.position.x)
        {  
            unflipPlayer1();
            unflipPlayer2();
        }
    }

    private void flipPlayer1()
    {
        isFlipped = true;
        //player1RB.rotation = 180f;
        player1Transform.rotation = Quaternion.Euler(0, -180f,0); 
        Debug.Log("flipped p1");
    }
    private void flipPlayer2()
    {
        isFlipped = true;
        player2Transform.rotation = Quaternion.Euler(0, -180f, 0);
        //player2RB.rotation = 180f;
        Debug.Log("flipped p2");
    }

    private void unflipPlayer1()
    {
        isFlipped = false;
        player1Transform.rotation = Quaternion.Euler(0, 0f, 0);
        Debug.Log("unflipped p1");
    }
    private void unflipPlayer2()
    {
        isFlipped = false;
        player2Transform.rotation = Quaternion.Euler(0, 0f, 0);
        Debug.Log("unflipped p2");
    }
}
