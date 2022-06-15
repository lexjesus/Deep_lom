using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private InventoryPopup popup;
    [SerializeField] private Text levelEnding;
    [SerializeField] private Text debug;
    [SerializeField] private Text tikingPower;
    [SerializeField] private Text tikingSpeed;
    [SerializeField] private GameObject LevelCubePrefab;
    private GameObject _levelCube;

    

    private void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATE, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
        Messenger.AddListener(GameEvent.NO_ENEMY, OnNoEnemy);
        
        Messenger<bool>.AddListener(GameEvent.SPEED_CHANGE, OnSpeedRaise);
        Messenger<bool>.AddListener(GameEvent.POWER_CHANGE, OnPowerRaise);
        Messenger<string>.AddListener(GameEvent.DEBUG, OnDebug);
    }
    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATE, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
        Messenger.RemoveListener(GameEvent.NO_ENEMY, OnNoEnemy);
        Messenger<bool>.RemoveListener(GameEvent.SPEED_CHANGE, OnSpeedRaise);
        Messenger<bool>.RemoveListener(GameEvent.POWER_CHANGE, OnPowerRaise);
        Messenger<string>.RemoveListener(GameEvent.DEBUG, OnDebug);

    }
    private void OnNoEnemy() {
        _levelCube = Instantiate(LevelCubePrefab) as GameObject;
        _levelCube.transform.position = new Vector3(0, 0, 0);
    }
    private void OnSpeedRaise(bool b) {
        if(b) {
            tikingSpeed.text = "Speed is active";
        }
        else {
            tikingSpeed.text = "";
        }
    }

    private void OnPowerRaise(bool b) {
        if(b) {
            tikingPower.text = "Power is active";
        }
        else {
            tikingPower.text = "";
        }
    }

    private void OnDebug(string str) {
        debug.text = str;
    }

    void Start()
    {
        OnHealthUpdated();
        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    public void OpenMenu() {
        bool isShowing = popup.gameObject.activeSelf;
        popup.gameObject.SetActive(!isShowing);
        popup.Refresh();
    }
    private void OnLevelComplete() {
        StartCoroutine(CompleteLevel());
    }
    private IEnumerator CompleteLevel() {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";
        yield return new WaitForSeconds(2);

        Managers.Mission.GoToNext();
    }

    private void OnHealthUpdated() {
        string message = Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthLabel.text = message;
    }
    private void OnLevelFailed() {
        StartCoroutine(FailLevel());
    }

    public void SavaGame() {
        Managers.Data.SaveGameState();
    }
    public void LoadGame() {
        //OnDebug("tap");

        Managers.Data.LoadGameState();
    }

    public void OnGameComplete() {
        levelEnding.text = "Game complete!";
        levelEnding.gameObject.SetActive(true);
    }

    private IEnumerator FailLevel() {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed";

        yield return new WaitForSeconds(2);
        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent();
    }
}
