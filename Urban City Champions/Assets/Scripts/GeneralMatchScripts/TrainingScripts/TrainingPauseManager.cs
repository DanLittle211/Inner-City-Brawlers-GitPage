using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

public class TrainingPauseManager : MonoBehaviour
{
    public enum pauseState { mainPauseScreen, TrainingSettings, EnemySettings, MovesList, InfoSettings, SoundSettings, notActive };
    public pauseState currentPauseState;
    public GameObject[] pauseScreens;
    public GameObject Menus;
    private PlayerMovement p1M, p2M;
     
    //0 = Main Pause Menu
    //1 = Training Settings
    //2 = Enemy Settings
    //3 = MoveList
    //4 = Info Settings
    //5 = Sound Settings
    public GameObject[] startingButtons;
   

    public GameMasterManager gMM;

    void Start()
    {
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
    }


    public void SetPause()
    {
        p1M.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        p1M.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
        SetMainPauseState();
        p1M.isDisabled = true;
    }
    public void unPause()
    {
        SetinactiveState();
        p1M.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Default", "Player1", true);
        p1M.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Default", "Player1", true);
        p1M.isDisabled = false;
    }
    public void SetinactiveState()
    {
        currentPauseState = pauseState.mainPauseScreen;
        pauseScreens[0].gameObject.SetActive(false);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[0]);

        GameObject player1 = GameObject.Find("Player1");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponentInChildren(typeof(PlayerMovement));
        p1.isDisabled = false;
    }

    public void SetMainPauseState()
    {
        currentPauseState = pauseState.mainPauseScreen;
        pauseScreens[0].gameObject.SetActive(true);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[0]);
        GameObject player1 = GameObject.Find("Player1");
        PlayerButtons p1 = (PlayerButtons)player1.GetComponentInChildren(typeof(PlayerButtons));
        p1.pM.isDisabled = true;
    }
    public void SetTrainingSettingsState()
    {

        currentPauseState = pauseState.TrainingSettings;
        pauseScreens[1].gameObject.SetActive(true);
        pauseScreens[0].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[1]);
    }
    public void SetEnemySettingsState()
    {
        currentPauseState = pauseState.EnemySettings;
        pauseScreens[2].gameObject.SetActive(true);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[0].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[2]);
    }
    public void SetMoveListState()
    {
        currentPauseState = pauseState.MovesList;
        pauseScreens[3].gameObject.SetActive(true);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[0].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[3]);
    }
    public void SetInfoSettingsState()
    {
        currentPauseState = pauseState.InfoSettings;
        pauseScreens[4].gameObject.SetActive(true);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[0].gameObject.SetActive(false);
        pauseScreens[5].gameObject.SetActive(false);
        SetActiveButton(startingButtons[4]);
    }
    public void SetSoundSettingsState()
    {
        currentPauseState = pauseState.SoundSettings;
        pauseScreens[5].gameObject.SetActive(true);
        pauseScreens[1].gameObject.SetActive(false);
        pauseScreens[2].gameObject.SetActive(false);
        pauseScreens[3].gameObject.SetActive(false);
        pauseScreens[4].gameObject.SetActive(false);
        pauseScreens[0].gameObject.SetActive(false);
        SetActiveButton(startingButtons[5]);
    }

    public void SetActiveButton(GameObject button)
    {
        UnityEngine.EventSystems.EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(button);
    }
    // Start is called before the first frame update
    public void GoToCharacterSelect()
    {
        Menus.gameObject.SetActive(true);
        gMM.SetGameStateCharacterSelect();
    }
    public void GoToMainMenu()
    {
        Menus.gameObject.SetActive(true);
        gMM.SetGameStateMain();
        /*GameObject player1 = GameObject.Find("Player1");
        PlayerButtons p1 = (PlayerButtons)player1.GetComponentInChildren(typeof(PlayerButtons));
        p1.pM.isDisabled = false;*/
    }
}