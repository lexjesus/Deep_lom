using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallPlayer : MonoBehaviour
{
    public float speed = 25f;
    public int damage;
    

    private void Start() {
        damage = Managers.Player.damage;
        //Messenger<string>.Broadcast(GameEvent.DEBUG, "power: " + damage.ToString());
    }

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Player") {
            if(other.CompareTag("Enemy")) {
                WanderingAI enemy = other.GetComponentInParent<WanderingAI>();
                if(enemy != null && enemy._alive) {
                    enemy.EnemyHurt(damage);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
