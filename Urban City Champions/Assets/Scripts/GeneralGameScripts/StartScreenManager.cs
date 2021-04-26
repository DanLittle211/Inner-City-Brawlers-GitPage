using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Rewired;

public class StartScreenManager : MonoBehaviour
{

    [Header("Game Header Objects")]
    [SerializeField] public GameObject StartMenu;
    [SerializeField] public GameObject StartMenuImages;
    [SerializeField] public GameObject blackFader;
    
    [SerializeField] public GameObject startButton;
    [SerializeField] public GameObject returnButton;
    [SerializeField] public GameObject quitObject;
    public Image BackgroundImage;
    public Sprite BrickBare;
    public Sprite BrickWithLogo;
    public Sprite BrickWithLogoAndSpray;

    [Header("SceneViewObjects")]
    public GameObject[] sceneView;
    [SerializeField] public enum GameState {StartScreenDisabled, StartScreenEnabled};
    [SerializeField] public GameState currentGameState;
    [Header("Animations")]
    [SerializeField] public Animator startImagesAnimator;
    [SerializeField] public Animator faderAnimator;

    

    [Header("Animation String Variables")]
    string currentState;
    const string StartUpAnim = "StartUpAnimation";
    const string SwitchButtonActive = "SwitchToButtonActive";
    const string SwitchButtonInactive = "SwitchToButtonInactive";

    const string fadeToClear = "BlackFadeToClear";
    const string staticBlack = "StayBlackAnim";
    const string fadeToBlack = "FadeToBlack";

    [Header("Rewired")]
    [SerializeField] public int playerID;
    [SerializeField] private Player player;
    bool QuitActive;

    public float waitTimer;
    public float maxWaitTimer;


    // Start is called before the first frame update
    void Start()
    {
        QuitActive = false;
        StartMenuImages.SetActive(false);
        quitObject.SetActive(false);
        waitTimer = maxWaitTimer;
        currentGameState = GameState.StartScreenDisabled;
        StartCoroutine(StartGame());
        startButton.SetActive(false);
        player = ReInput.players.GetPlayer(playerID);
        SetControllerMapsForCurrentMode();
    }
    void ChangeAnimationState(Animator thisAnim,string thisState)
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

    void Update()
    {
        
        switch (currentGameState)
        {
            case GameState.StartScreenDisabled:
                { 
                    if (player.GetAnyButtonDown() ^ player.GetAnyNegativeButtonDown() && (QuitActive ==  false && (!(player.GetButton("Escape")))))
                    {
                        DeactivateQuitMenu();
                        SetGameStateMainActive();
                    }

                    waitTimer = maxWaitTimer;
                    break;
                }
            case GameState.StartScreenEnabled:
                {
                    waitTimer -= Time.deltaTime;
                    if (waitTimer <= 0)
                    {
                        SetGameStateMainInactive();
                    }
                    if (player.GetButton("Back"))
                    {
                        SetGameStateMainInactive();
                    }
                    break;
                }
        }
        if (currentGameState == GameState.StartScreenDisabled)
        {
            if (player.GetButton("Escape"))
            {
                ActivateQuitMenu();
            }
        }
        
    }

    public void ActivateQuitMenu()
    {
        QuitActive = true;
        quitObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(returnButton);
    }
    public void DeactivateQuitMenu()
    {
        QuitActive = false;
        quitObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchImage(Sprite Image)
    {
        BackgroundImage.sprite = Image;
    }

    public void SetControllerMapsForCurrentMode()
    {
        player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
    }
    public void SetGameStateMainInactive()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchToInactive());   
    }

    public void SetGameStateMainActive()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchToActive());
    }

    public void MoveToMainGame()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchToMainGame());
    }
    IEnumerator StartGame()
    {
        ChangeAnimationState(faderAnimator, fadeToClear);
        yield return new WaitForSeconds(0.1f);
        StartMenuImages.SetActive(true);
        ChangeAnimationState(startImagesAnimator,StartUpAnim);
        yield return new WaitForSeconds(0.717f);
    }
    IEnumerator SwitchToActive()
    {
        ChangeAnimationState(startImagesAnimator, SwitchButtonActive);
        yield return new WaitForSeconds(0.717f);
        startButton.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(startButton);
        currentGameState = GameState.StartScreenEnabled;
    }
    IEnumerator SwitchToInactive()
    {
        startButton.SetActive(false);
        ChangeAnimationState(startImagesAnimator, SwitchButtonInactive);
        currentGameState = GameState.StartScreenDisabled;
        yield return new WaitForSeconds(0.333f);
    }
    IEnumerator SwitchToMainGame()
    {
        ChangeAnimationState(faderAnimator, fadeToBlack);
        yield return new WaitForSeconds(2.1f);
        ChangeAnimationState(faderAnimator, staticBlack);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainGameScene");
    }

}
