using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManagement : MonoBehaviour
{
    public GameManagerScript gManager;

    //Round Win Variable
    public int TotalWinsInt;
    public int CurrentRoundInt;
    public int Player1currentRWInt;
    public int Player2currentRWInt;

    

    // Start is called before the first frame update
    void Start()
    {
 
        CurrentRoundInt = 1;
        Player1currentRWInt = 0;
        Player2currentRWInt = 0;
    }

    private void Update()
    {
        if (gManager.p1HealthFloat == 0 || gManager.p2HealthFloat == 0)
        {
            if (gManager.p2HealthFloat == gManager.p1HealthFloat)
            {
                //Debug.Log("Draw!!");
                //gManager.hasFunctionRun = true;
                //playersTie();
            }
            else if (gManager.p1HealthFloat >= gManager.p2HealthFloat)
            {
                //Debug.Log("Player 1 Wins!");
                //gManager.hasFunctionRun = true;
                //player1WinRound();
            }
            else if (gManager.p2HealthFloat >= gManager.p1HealthFloat)
            {
                //Debug.Log("Player 2 Wins!");
                //gManager.hasFunctionRun = true;
                //player2WinRound();
            }
            gManager.hasFunctionRun = false;
        }
    }

    public void player1WinRound()
    {
       
        GameObject player1 = GameObject.Find("Player1");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        //p1.currentPlayState = PlayerMovement.playerState.RoundOver;

        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        //p2.currentPlayState = PlayerMovement.playerState.RoundOver;
        Debug.Log("Round " + CurrentRoundInt + " is over!");

        if (gManager.hasFunctionRun != false)
        {
            Player1currentRWInt = Player1currentRWInt + 1;
            CurrentRoundInt = CurrentRoundInt + 1;
            gManager.hasFunctionRun = false;
            Debug.Log(" Player 1 Wins! Current Standings: Player 1 " + Player1currentRWInt + " to Player 2 " + Player2currentRWInt);
        }
        gManager.hasFunctionRun = false;
    }

    public void player2WinRound()
    {
        GameObject player1 = GameObject.Find("Player1");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        //p1.currentPlayState = PlayerMovement.playerState.RoundOver;

        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        //p2.currentPlayState = PlayerMovement.playerState.RoundOver;
        Debug.Log("Round " + CurrentRoundInt + " is over!");
        if (gManager.hasFunctionRun != false)
        {
            Player2currentRWInt = 1;
            CurrentRoundInt = 2;
            gManager.hasFunctionRun = false;
            Debug.Log(" Player 1 Wins! Current Standings: Player 1 " + Player1currentRWInt + " to Player 2 " + Player2currentRWInt);
        }
       
    }

    public void playersTie()
    {
        GameObject player1 = GameObject.Find("Player1");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        // p1.currentPlayState = PlayerMovement.playerState.RoundOver;

        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        // p2.currentPlayState = PlayerMovement.playerState.RoundOver;
        Debug.Log("Round " + CurrentRoundInt + " is over!");

        
    }
}
