using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set;}

    public int curLevelEnemies { get; private set; }
    public int curEnemiesOnScene { get; private set; }
    public bool levelChange;

    public void Startup() {
        Debug.Log("Enemy manager starting...");
        UpdateData(3, 0);
        status = ManagerStatus.Started;
    }

    public void UpdateData(int curLevelEnemies, int curEnemiesOnScene) {
        this.curLevelEnemies = curLevelEnemies;
        this.curEnemiesOnScene = curEnemiesOnScene;
    }
    public void LoadUpdateData(int c) {
        curLevelEnemies = c;
        curEnemiesOnScene = 0;
    }

    public int GetCurLevelEnemies() {
        return curEnemiesOnScene + curLevelEnemies;
    }

    public void SetCurLevelEnemies(int c) {
        curLevelEnemies = c;
    }
}
