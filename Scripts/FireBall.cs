using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.CompareTag("Player")) {
            Managers.Player.ChangeHealth(-damage);
        }
        Destroy(this.gameObject);
    }
}
