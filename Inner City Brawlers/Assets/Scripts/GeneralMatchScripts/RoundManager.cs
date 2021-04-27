//created by Will McBride III
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Rewired;
public class RoundManager : MonoBehaviour
{
    //round States
    public enum roundState {preRound, duringRound, roundPaused, endRound, continueOption}
    public roundState currentState;
    #region Round Variables
    [Header("Round Win Variables")]
    public int currentRound;
    public int previousRound;
    public int totalRounds;
    public int p1CurrentWins;
    public int p2CurrentWins;
    #endregion
    #region Round Timer Variables
    [Header("Round Timers")]
    public float roundTimer;
    public float roundTimerStart;
    private float preroundTimer;
    public float preroundTimerStart;
    #endregion
    #region Menu/UI Variables
    [Header("Menu/UI Variables")]
    public GameObject GameConcludedPanel;
    public GameObject pauseMenu;
    public TMP_Text TimerText;
    public TMP_Text gameOverText;
    #endregion
    #region Script References
    [Header("Script references")]
    
    public GameManagerScript gManager;
    public GameMasterManager gMM;
    private PlayerHealth p1Health;
    private PlayerHealth p2Health;
    private PlayerMovement p1Movement;
    private PlayerMovement p2Movement;
    private PlayerButtons p1Buttons;
    private PlayerButtons p2Buttons;
    #endregion
    #region Player Object Referecnes
    [Header("Player References")]
    public GameObject player1;
    public GameObject player2;
    public GameObject TrainingRoom;
    public GameObject VersusRoom;
    #endregion
    #region Round Result Bools
    [Header("Round Results")]
    public bool p1Wins;
    public bool p2Wins;
    public bool haveTied;
    #endregion
    public GameObject multiPauseButton;
    public GameObject roundOverButton;
    public bool oneTime;
    

    public void Awake()
    {
        //Player script 
        p1Health = player1.GetComponentInChildren<PlayerHealth>();
        p2Health = player2.GetComponentInChildren<PlayerHealth>();
        p1Movement = player1.GetComponent<PlayerMovement>();
        p2Movement = player2.GetComponent<PlayerMovement>();
        p1Buttons = player1.GetComponentInChildren<PlayerButtons>();
        p2Buttons = player2.GetComponentInChildren<PlayerButtons>();
        //other variables

        if (gMM.isMultiActive == false)
        {
            gManager.trainingStart();
            pauseMenu.SetActive(false);
            GameConcludedPanel.SetActive(false);
            TrainingRoom.SetActive(true);
            VersusRoom.SetActive(false);
        }
        if (gMM.isMultiActive == true)
        {
            TrainingRoom.SetActive(false);
            VersusRoom.SetActive(true);
            gManager.LockStartPosition();
            roundTimer = roundTimerStart;
            preroundTimer = preroundTimerStart;
            currentRound = 1;
            p1CurrentWins = 0;
            p2CurrentWins = 0;
            currentState = roundState.preRound;
            GameConcludedPanel.SetActive(false);
            pauseMenu.SetActive(false);
        }
        
    }

    public void ResetGame()
    {
        Awake();
        gManager.StartMatch();
    }

    private void Update()
    {
        if (gMM.isMultiActive == true)
        {
            TimerText.text = "Time: " + (Mathf.Round(roundTimer)).ToString();
            Debug.Log(currentState);
            //preround countdown
            if (currentState == roundState.preRound)
            {
                //setting each player's movement state to immobile while in this round state
                p1Movement.isDisabled = true;
                p2Movement.isDisabled = true;
                oneTime = false;
                preroundTimer -= Time.deltaTime; //counts down timer while in this state
                if (preroundTimer <= 0)
                {
                    preroundTimer = 0;
                    previousRound = currentRound;

                    currentState = roundState.duringRound;
                    p1Movement.isDisabled = false;
                    p2Movement.isDisabled = false;
                }
            }
            //used for tracking round wins and others 
            if (currentState == roundState.duringRound)
            {
                //round timer functions
                roundTimer -= Time.deltaTime; //counts down timer while in this state
                                              //what happens when the timer runs out and no-one won
                p1Movement.isDisabled = false;
                p2Movement.isDisabled = false;
                if (roundTimer <= 0)
                {
                    roundTimer = 0;
                    //p1 wins
                    if (p1Health.currentHealth > p2Health.currentHealth)
                    {
                        p1Wins = true;
                        currentState = roundState.endRound;
                    }
                    //p2 wins
                    if (p1Health.currentHealth < p2Health.currentHealth)
                    {
                        p2Wins = true;
                        currentState = roundState.endRound;
                    }
                    //players tie
                    if (p1Health.currentHealth == p2Health.currentHealth)
                    {
                        haveTied = true;
                        currentState = roundState.endRound;
                    }

                }
                //if the round timer is > 0
                else
                {
                    //player 1 is knockedout
                    if (p1Health.currentHealth <= 0 && !p1Wins && !haveTied)
                    {
                        p2Wins = true;
                        currentState = roundState.endRound;
                    }
                    //player 2 is knockedout
                    if (p2Health.currentHealth <= 0 && !p2Wins && !haveTied)
                    {
                        p1Wins = true;
                        currentState = roundState.endRound;
                    }
                    //both are knocked out at same time
                    if ((p1Health.currentHealth == 0 && p2Health.currentHealth == 0) && (!p1Wins && !p2Wins))
                    {
                        haveTied = true;
                        currentState = roundState.endRound;
                    }
                }

            }
            //used for adding to round wins
            if (currentState == roundState.endRound)
            {  //setting each player's movement state to immobile while in this round state
                currentState = roundState.continueOption;
                //what happens when player 1 wins the round
            }
            //used for restarting rounds and tracking total rounds
            if (currentState == roundState.continueOption)
            {
                //setting each player's movement state to immobile while in this round state
                //p1Movement.isDisabled = true;
                //p2Movement.isDisabled = true;
                //works as a reset for the round
                if (currentRound == previousRound)
                {
                    setOneTime();
                    if (oneTime)
                    {
                        if (p1Wins)
                        {
                            p1CurrentWins = p1CurrentWins + 1;
                            p1Wins = false;
                            //currentState = roundState.continueOption;

                        }
                        //what happens when player 2 wins the round
                        if (p2Wins)
                        {
                            p2CurrentWins = p2CurrentWins + 1;
                            p2Wins = false;
                            //currentState = roundState.continueOption;

                        }
                        //what happens when the players have tied
                        if (haveTied)
                        {
                            haveTied = false;
                            totalRounds = totalRounds + 1;
                            //currentState = roundState.continueOption;
                        }
                        roundTimer = roundTimerStart;
                        preroundTimer = preroundTimerStart;
                        currentRound = currentRound + 1;
                        p1Wins = false;
                        p2Wins = false;
                        haveTied = false;
                        oneTime = false;
                        gManager.StartMatch();
                    }
                }
                else if (currentRound < totalRounds)
                {
                    currentState = roundState.preRound;
                }
                else if (currentRound >= totalRounds)
                {
                    //if p1 has more wins than p2 at the last round, for displaying who wins and giving options to restart or not
                    if (p1CurrentWins > p2CurrentWins)
                    {
                        GameConcludedPanel.SetActive(true);

                        p1Movement.isDisabled = true;
                        p2Movement.isDisabled = true;
                        gameOverText.text = "Game Over, Player 1 Wins";
                        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
                        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
                        Debug.Log("Player 1 Wins");
                    }
                    //if p2 has more wins than p1 at the last round, for displaying who wins and giving options to restart or not
                    if (p2CurrentWins > p1CurrentWins)
                    {
                        GameConcludedPanel.SetActive(true);

                        p1Movement.isDisabled = true;
                        p2Movement.isDisabled = true;
                        gameOverText.text = "Game Over, Player 2 Wins";
                        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
                        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
                        Debug.Log("Player 2 Wins");
                    }
                    //if both players have equal wins, for displaying who wins and giving options to restart or not
                    if (p1CurrentWins == p2CurrentWins)
                    {
                        GameConcludedPanel.SetActive(true);

                        p1Movement.isDisabled = true;
                        p2Movement.isDisabled = true;
                        gameOverText.text = "Game Over, Players Tied";
                        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
                        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
                        Debug.Log("The players tied");
                    }
                    //setOneTime();
                    if (oneTime == false)
                    {
                        GameOverButtonSet();
                        oneTime = true;
                    }
                }

            }
            //what happens if the game is paused
            if (currentState == roundState.roundPaused)
            {
                roundTimer -= 0;
                
                //setting each player's movement state to immobile while in this round state
                p1Movement.isDisabled = true;
                p2Movement.isDisabled = true;
                //setting pause menu to active 
            }
        }
    }
    public void SetActiveButton(GameObject button)
    {
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(button);
    }
    public void GameOverButtonSet()
    {
        SetActiveButton(roundOverButton);
    }
    public void PauseGame()
    {
        DisablePlayer(p2Movement);
        DisablePlayer(p1Movement);
        pauseMenu.SetActive(true);
        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 1, "Menu", "Default", true);
        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 1, "Menu", "Default", true);
        SetActiveButton(multiPauseButton);
    }
    public void UnpauseGame()
    {
        EnablePlayer(p2Movement);
        EnablePlayer(p1Movement);
        pauseMenu.SetActive(false);
        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Default", "Player1", true);
        p1Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Default", "Player1", true);
        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Keyboard, 1, "Default", "Player2", true);
        p2Buttons.player.controllers.maps.LoadMap(ControllerType.Joystick, 1, "Default", "Player2", true);
        currentState = roundState.duringRound;
    }
    void DisablePlayer(PlayerMovement thisPlayer)
    {
        thisPlayer.isDisabled = false;
        Debug.Log("Player Disabled");
    }
    void EnablePlayer(PlayerMovement thisPlayer)
    {
        thisPlayer.isDisabled = true;
        Debug.Log("Player Enabled");
    }
    void setOneTime()
    {
        oneTime = true;  
    }
}
