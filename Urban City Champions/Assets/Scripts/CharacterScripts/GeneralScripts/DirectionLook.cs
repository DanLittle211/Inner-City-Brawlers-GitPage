using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLook : MonoBehaviour
{
    [SerializeField]private Transform player1Transform, player2Transform;

    public float blockDistance;

    public bool isFlipped;
    
    public bool isP1Flipped;
    public bool isP2Flipped;

    public bool blockAvailable;

    // Start is called before the first frame update
    void Start()
    {
        blockAvailable = false;
        player1Transform = GameObject.Find("Player1").transform;
        player2Transform = GameObject.Find("Player2").transform;
        isFlipped = false;
        isP1Flipped = false;
        isP2Flipped = true;
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
