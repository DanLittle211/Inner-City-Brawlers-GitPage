using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum GameState {LevelState, PauseState, LearnState}
    public GameState currentGameState;
    private bool isPaused;
   
    public GameObject Level; 
    public GameObject Pause;
    public GameObject Learn;
    

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.LevelState;
        ShowScreen(Level);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (isPaused)
            {
                isPaused = false;
                currentGameState = GameState.LevelState;
                ShowScreen(Level);
            }
            else
            {
                isPaused = true;
                currentGameState = GameState.PauseState;
                ShowScreen(Pause);
            }
        }

       /* if (Input.GetKeyDown(KeyCode.P))
        {
            if (isLearning)
            {
                isLearning = false;
                currentGameState = GameState.LevelState;
                ShowScreen(Level);
            }
            else
            {
                isLearning = true;
                currentGameState = GameState.LearnState;
                ShowScreen(Learn);
            }
        }*/
       
    }

    public void HowToPlay()
    {
        currentGameState = GameState.LearnState;
        ShowScreen(Learn);
    }

    public void TrainingMenu()
    {
        SceneManager.LoadScene("TrainingRoom");
    }

    public void BackToGame()
    {
        currentGameState = GameState.LevelState;
        ShowScreen(Level);
    }

    public void BackToPause()
    {
        currentGameState = GameState.PauseState;
        ShowScreen(Pause);
    }

    private void ShowScreen (GameObject gameObjectToShow)
    {
        Level.SetActive(false);
        Pause.SetActive(false);
        Learn.SetActive(false);

        gameObjectToShow.SetActive(true);
    }
}
