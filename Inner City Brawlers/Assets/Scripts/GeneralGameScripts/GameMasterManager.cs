using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMasterManager : MonoBehaviour
{
    [Header("Game Header Objects")]
    [SerializeField] public GameObject Menus;
    [SerializeField] public GameObject FightView;
    [SerializeField] public GameObject TrainingView;
    [SerializeField] public GameObject VersusView;

    [Header("SceneViewObjects")]
    public GameObject[] sceneView;
    [SerializeField] public enum GameState { StartScreen, MainMenu, InfoPage, SinglePlayer, Options, CharacterSelect, FightView, Controls };
    [SerializeField] public GameState currentGameState;
    public GameManagerScript gMS;

    [Header("EventSystemManipulation")]
    public GameObject[] newButton;
                        //0 = SinglePlayer
                        //1 = Arcade
                        //2 = Info Back button
                        //3 = Options main button
    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] private Player player;

    [Header("Master Game BoolState")]
    public bool isMultiActive;

    // Start is called before the first frame update
    void Start()
    {
        Menus.gameObject.SetActive(true);
        FightView.gameObject.SetActive(false);
        SetGameStateStart();
        player = ReInput.players.GetPlayer(playerID);
        SetControllerMapsForCurrentMode();
    }

    void Update()
    {
        if (currentGameState == GameState.StartScreen)
        {
            if (player.GetAnyButtonDown() || player.GetAnyNegativeButtonDown())
            {
                SetGameStateMain();
            }
        }
    }

    public void SetControllerMapsForCurrentMode()
    {
        player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
    }

    public void SetGameStateStart()
    {
        currentGameState = GameState.StartScreen;
        sceneView[0].gameObject.SetActive(true);

        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);
        sceneView[7].gameObject.SetActive(false);
        VersusView.SetActive(false);
        TrainingView.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    public void SetGameStateMain()
    {
        currentGameState = GameState.MainMenu;
        sceneView[1].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[0]);
        sceneView[7].gameObject.SetActive(false);
        FightView.SetActive(false);
        VersusView.SetActive(false);
        TrainingView.SetActive(false);
    }
    public void SetGameStateInfo()
    {
        currentGameState = GameState.InfoPage;
        sceneView[2].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);

        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[2]);
        sceneView[7].gameObject.SetActive(false);
    }
    public void SetGameStateSinglePlayer()
    {
        currentGameState = GameState.InfoPage;
        sceneView[3].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);

        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[1]);
        sceneView[7].gameObject.SetActive(false);
    }
    public void SetGameStateControls()
    {
        currentGameState = GameState.Controls;
        sceneView[7].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[4]);
    }
    public void SetGameStateOptions()
    {
        currentGameState = GameState.InfoPage;
        sceneView[4].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);

        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[3]);
        sceneView[7].gameObject.SetActive(false);
    }
    public void SetGameStateCharacterSelect()
    {
        currentGameState = GameState.CharacterSelect;
        sceneView[5].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);
        sceneView[7].gameObject.SetActive(false);
        FightView.SetActive(false);
        VersusView.SetActive(false);
        TrainingView.SetActive(false);
    }

    public void SetGameStateFightViewSingle()
    {
        currentGameState = GameState.FightView;
        sceneView[6].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);

        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p2.isDisabled = true;
        isMultiActive = false;
        gMS.Start();
        sceneView[7].gameObject.SetActive(false);
    }
    public void SetGameStateFightViewMulti()
    {
        currentGameState = GameState.FightView;
        sceneView[6].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);

        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p2.isDisabled = false;
        isMultiActive = true;
        gMS.Start();
        sceneView[7].gameObject.SetActive(false);
    }
   
}
