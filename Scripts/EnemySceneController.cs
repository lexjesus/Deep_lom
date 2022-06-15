using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private List<GameObject> _enemy;
    
    public int curLevelEnemies { get; private set; }
    public int maxEnemies { get; private set; }
    public int maxCurEnem { get; private set; }
    public int curEnem { get; private set; }
    public int needCurEnem { get; private set; }

    private void Awake() {
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DIE, RemoveDeadEnemy);
    }
    private void OnDestroy() {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DIE, RemoveDeadEnemy);
    }


    private void Start() {
        int curLevel = Managers.Mission.curLevel;
        
        maxEnemies = curLevel * 2 + 1;
        maxCurEnem = curLevel + 1;
        if(Managers.Enemy.levelChange) {
            curLevelEnemies = maxEnemies;
        }
        else {
            if(Managers.Enemy.curLevelEnemies < maxEnemies) {
                curLevelEnemies = Managers.Enemy.GetCurLevelEnemies();
            }
            else {
                curLevelEnemies = maxEnemies;
            }
        }
        Managers.Enemy.levelChange = false;
        curEnem = 0;
        _enemy = new List<GameObject>(maxCurEnem);
        if(curLevelEnemies != 0) {
            for(int i = 0; i < maxCurEnem; i++) {

                GameObject e = Instantiate(enemyPrefab);
                _enemy.Add(e);
                int randX = Random.Range(-30, 14);
                int randZ = Random.Range(-30, 16);
                if(randZ > -8 && randX > 4) {
                    randX -= 17;
                    randZ -= 17;
                }
                e.transform.position = new Vector3(randX, 0, randZ);
                float angle = Random.Range(0, 360);
                e.transform.Rotate(0, angle, 0);
                curLevelEnemies--;
                if(curLevelEnemies == 0) {
                    break;
                }
            }
        }
        curEnem = _enemy.Count;
        Managers.Enemy.UpdateData(curLevelEnemies, curEnem);
        if(curLevelEnemies == 0 && curEnem == 0) {
            Managers.Enemy.UpdateData(curLevelEnemies, curEnem);
            Messenger.Broadcast(GameEvent.NO_ENEMY);
        }
        

    }

    public void RemoveDeadEnemy(GameObject deadEnemy) {
        _enemy.Remove(deadEnemy);
        curEnem = _enemy.Count;
        AddEnemyOnScene();
    }

    private void AddEnemyOnScene() {
        
        if(curEnem <= maxCurEnem && curLevelEnemies != 0) {
            GameObject e = Instantiate(enemyPrefab) as GameObject;
            _enemy.Add(e);
            int randX = Random.Range(-30, 14);
            int randZ = Random.Range(-30, 16);
            if(randZ > -8 && randX > 4) {
                randX -= 16;
                randZ -= 16;
            }
            e.transform.position = new Vector3(randX, 0, randZ);
            float angle = Random.Range(0, 360);
            e.transform.Rotate(0, angle, 0);
            curLevelEnemies--;
            Managers.Enemy.UpdateData(curLevelEnemies, curEnem);
            
        }
        if(curLevelEnemies == 0 && curEnem == 0) {
            Managers.Enemy.UpdateData(curLevelEnemies, curEnem);
            Messenger.Broadcast(GameEvent.NO_ENEMY);
        }
    }

    
}
