using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MissionManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public int curLevel { get; private set; }
    public int maxLevel  {get; private set;}

    

    public void Startup() {
        Debug.Log("Mission manager starting...");
        maxLevel = 2;
        UpdateData(0);
        status = ManagerStatus.Started;
    }

    public void GoToNext() {
        Debug.Log("cur " + curLevel);
        if(curLevel < maxLevel) {
            curLevel++;
            string name = "Level" + curLevel.ToString();
            Debug.Log("Loading " + name);
            Managers.Enemy.levelChange = true;
            SceneManager.LoadScene(name);
        }
        else {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }
    }
    public void ReachObjective() {
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    public void RestartCurrent() {
        string name = "Level" + curLevel;
        SceneManager.LoadScene(name);
    }
    public void UpdateData(int curLevel) {
        this.curLevel = curLevel;
    }
}
