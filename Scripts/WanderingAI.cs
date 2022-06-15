using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WanderingAI : MonoBehaviour {
    public float speed = 3.0f;
    public float obstacleRange = 3.0f;
    public bool _alive;
    public int enemyHealth { get; private set; }
    public int enemyMaxHealth { get; private set; }

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform Dulo;
    private GameObject _fireball;
    private EnemySceneController sceneController;

    private void Start() {
        sceneController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemySceneController>();
        enemyMaxHealth = Managers.Mission.curLevel * 5 + 10;
        enemyHealth = enemyMaxHealth;
        _alive = true;
    }
    void Update() {
        if(_alive) {
            transform.Translate(0, 0, speed * Time.deltaTime);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.SphereCast(ray, 0.75f, out hit)) {
                GameObject hitObject = hit.transform.gameObject;
                if(hitObject.GetComponent<PlayerCharacter>()) {

                    if(_fireball == null) {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                else if(hit.distance < obstacleRange && !hitObject.GetComponent<FireBall>()) {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }

    public void DieEnemy() {
        StartCoroutine(Die());
    }

    public void EnemyHurt(int damage) {
        enemyHealth -= damage;
        if(enemyHealth <= 0) {
            DieEnemy();
            Messenger<GameObject>.Broadcast(GameEvent.ENEMY_DIE, this.gameObject);
        }
    }

    private IEnumerator Die() {
        _alive = false;
        this.transform.Translate(0, 2, 0);
        this.transform.Rotate(0, 0, 120);
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
