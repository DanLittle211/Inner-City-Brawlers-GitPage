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
    [SerializeField] public enum GameState {MainMenu, InfoPage, SinglePlayer, Options, CharacterSelect, FightView, Controls };
    [SerializeField] public GameState currentGameState;
    public GameManagerScript gMS;

    [Header("EventSystemManipulation")]
    public GameObject[] newButton;
    #region Button Number Order
    //0 = SinglePlayer
    //1 = Arcade
    //2 = Info Back button
    //3 = Options main button
    #endregion
    #region Menu Order
    //0 Main Menu
    //1 Single Player
    //2 Info Screen
    //3 Control Screen
    //4 Options Screen
    //5 Character Select
    //6 FightView
    #endregion
    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] private Player player;

    [Header("Master Game BoolState")]
    public bool isMultiActive;

    [Header("Info Screen Variables")]
    public GameObject controllerScreen;
    public GameObject keyboardScreen;

    [Header("Animation String Variables")]
    [SerializeField] public GameObject blackFader;
    [SerializeField] public Animator faderAnimator;
    string currentState;

    const string fadeToClear = "BlackFadeToClear";
    const string staticBlack = "StayBlackAnim";
    const string fadeToBlack = "FadeToBlack";

    // Start is called before the first frame update
    void Start()
    {
        ChangeAnimationState(faderAnimator, fadeToClear);
        Menus.gameObject.SetActive(true);
        FightView.gameObject.SetActive(false);
        SetGameStateMain();
        player = ReInput.players.GetPlayer(playerID);
        SetControllerMapsForCurrentMode();
        controllerScreen.SetActive(true);
        keyboardScreen.SetActive(false);
    }
    void ChangeAnimationState(Animator thisAnim, string thisState)
    {
        //stop anim playing multiple times
        if (currentState == thisState)
        {
            return;
        }
        //plays animation
        thisAnim.Play(thisState.ToString());
        currentState = thisState;
    }

    public void SetControllerMapsForCurrentMode()
    {
        player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
    }

    public void SetGameStateMain()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateMainScreen());
    }
    IEnumerator ActivateMainScreen()
    {
        yield return new WaitForSeconds(0.1f);
        currentGameState = GameState.MainMenu;
        sceneView[0].gameObject.SetActive(true);

        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[0]);
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
    }
    public void SetGameStateSinglePlayer()
    {
        currentGameState = GameState.InfoPage;
        sceneView[1].gameObject.SetActive(true);

        sceneView[0].gameObject.SetActive(false);  
        sceneView[2].gameObject.SetActive(false);
        sceneView[3].gameObject.SetActive(false);
        sceneView[4].gameObject.SetActive(false);
        sceneView[5].gameObject.SetActive(false);
        sceneView[6].gameObject.SetActive(false);

        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newButton[1]);
    }
    public void SetGameStateControls()
    {
        currentGameState = GameState.Controls;
        sceneView[3].gameObject.SetActive(true);
        SwitchToControllerScreen();
        sceneView[0].gameObject.SetActive(false);
        sceneView[1].gameObject.SetActive(false);
        sceneView[2].gameObject.SetActive(false);
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
        Menus.SetActive(false);
        p2.isDisabled = true;
        isMultiActive = false;
        gMS.Start();
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

        Menus.SetActive(false);
        p2.isDisabled = false;
        isMultiActive = true;
        gMS.Start();
    }
    public void SwitchToControllerScreen() 
    {
        controllerScreen.SetActive(true);
        keyboardScreen.SetActive(false);
    }
    public void SwitchToKeyboardScreen()
    {
        controllerScreen.SetActive(false);
        keyboardScreen.SetActive(true);
    }
    public void QuitGame()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToStartScreen());
    }
    IEnumerator ReturnToStartScreen()
    {
        ChangeAnimationState(faderAnimator, fadeToBlack);
        yield return new WaitForSeconds(2.1f);
        ChangeAnimationState(faderAnimator, staticBlack);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StartScreen");
    }
}