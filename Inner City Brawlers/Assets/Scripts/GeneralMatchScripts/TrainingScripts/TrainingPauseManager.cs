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
    //0 = Main Pause Menu
    //1 = Training Settings
    //2 = Enemy Settings
    //3 = MoveList
    //4 = Info Settings
    //5 = Sound Settings
    public GameObject[] startingButtons;
   

    public GameMasterManager gMM;

    public void SetPause()
    {
        GameObject player1 = GameObject.Find("Player1");
        PlayerButtons p1 = (PlayerButtons)player1.GetComponentInChildren(typeof(PlayerButtons));
        p1.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Menu", "Default", true);
        p1.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Menu", "Default", true);
        SetMainPauseState();
        p1.pM.isDisabled = true;
    }
    public void unPause()
    {
        GameObject player1 = GameObject.Find("Player1");
        SetinactiveState();
        PlayerButtons p1 = (PlayerButtons)player1.GetComponentInChildren(typeof(PlayerButtons));
        p1.player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Default", "Player1", true);
        p1.player.controllers.maps.LoadMap(ControllerType.Joystick, 0, "Default", "Player1", true);
        p1.pM.isDisabled = false;
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
        gMM.SetGameStateCharacterSelect();
    }
    public void GoToMainMenu()
    {
        gMM.SetGameStateMain();

        GameObject player1 = GameObject.Find("Player1");
        PlayerButtons p1 = (PlayerButtons)player1.GetComponentInChildren(typeof(PlayerButtons));
        p1.pM.isDisabled = false;
    }
}