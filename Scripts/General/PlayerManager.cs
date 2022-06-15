using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }
    public int damage { get; private set; }
    public float speed { get; private set; }
    private PointClickMovement moving;


    public void Startup() {
        Debug.Log("Player manager starting...");
        speed = 6.0f;
        maxHealth = 100;
        damage = 10;
        UpdateData(50);
        
        status = ManagerStatus.Started;
    }

    public void RaisePower() {
        StartCoroutine(PowerAction());
    }
    private IEnumerator PowerAction() {
        int tmp = damage;
        damage *= 2;
        Messenger<bool>.Broadcast(GameEvent.POWER_CHANGE, true);
        
        yield return new WaitForSeconds(15);

        damage = tmp;
        Messenger<bool>.Broadcast(GameEvent.POWER_CHANGE, false);

    }
    public void RaiseSpeed() {
        StartCoroutine(SpeedAction());
    }
    private IEnumerator SpeedAction() {
        moving = GameObject.FindGameObjectWithTag("Player").GetComponent<PointClickMovement>();
        float tmp = speed;
        speed += 5.0f;
        Messenger<bool>.Broadcast(GameEvent.SPEED_CHANGE, true);
        moving.moveSpeed = speed;

        yield return new WaitForSeconds(15);

        speed = tmp;
        moving.moveSpeed = speed;
        Messenger<bool>.Broadcast(GameEvent.SPEED_CHANGE, false);


    }


    public void ChangeHealth(int value) {
        health += value;
        if(health > maxHealth) {
            health = maxHealth;
        }else if(health < 0){
            health = 0;
        }
        if(health <= 0) {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }
        else {
            Messenger.Broadcast(GameEvent.HEALTH_UPDATE);
        }
    }

    public void UpdateData(int health) {
        this.health = health;
    }
    public void Respawn() {
        UpdateData(50);
    }


}
