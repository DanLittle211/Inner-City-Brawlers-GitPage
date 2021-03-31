using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLook : MonoBehaviour
{
    private Transform player1Transform, player2Transform;

    public bool isFlipped;
    // Start is called before the first frame update
    void Start()
    {
        player1Transform = GameObject.Find("Player1").transform;
        player2Transform = GameObject.Find("Player2").transform;
        isFlipped = false;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (player1Transform.position.x >= player2Transform.position.x)
        {
            flipPlayers(player1Transform);
            flipPlayers(player2Transform);
        }
        if (player2Transform.position.x >= player1Transform.position.x)
        {
            unflipPlayers(player1Transform);
            unflipPlayers(player2Transform);
        }
    }
    private void flipPlayers(Transform curObject)
    {
        isFlipped = true;
        curObject.rotation = Quaternion.Euler(0, -180f, 0);
        Debug.Log("flipped");
    }
    private void unflipPlayers(Transform curObject)
    {
        isFlipped = false;
        curObject.rotation = Quaternion.Euler(0, 0f, 0);
        Debug.Log("unflipped");
    }
}
