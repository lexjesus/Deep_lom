using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform Tower;
    //[SerializeField] private Text debug;
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    private float gravity = -9.8f;

    public float terminalVel = -10.0f;
    public float pushForce = 3.0f;

    public float deceleration = 20.0f;
    public float targetBuffer = 1.5f;

    private CharacterController _characterController;
    private ControllerColliderHit _contact;
    private GameObject _fireball;
    [SerializeField] private GameObject fireballPrefab2;
    private float timer;
    private Touch _touch;
    private Touch _touch2;
    private int screenSpace;

    private void Start() {
        timer = 1.0f;
        screenSpace = Screen.width / 3;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;
        Rigidbody body = hit.collider.attachedRigidbody;
        if(body != null && !body.isKinematic) {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    private void Shoot() {
        timer = 0;
        _fireball = Instantiate(fireballPrefab2) as GameObject;
        _fireball.transform.position = Tower.TransformPoint(new Vector3(0, 0, -6));
        _fireball.transform.rotation = Quaternion.Euler(0, Tower.eulerAngles.y + 180, 0);
    }

    private void Update() {
        if(timer <= 1.0f) {
            timer += Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if(Input.touchCount == 1 && Input.GetTouch(0).position.x > screenSpace) {
                _touch = Input.GetTouch(0);
                if(timer >= 1.0f) {
                    Shoot();
                }
            }
            /*else if(Input.touchCount == 2) {
                _touch = Input.GetTouch(0);
                _touch2 = Input.GetTouch(1);
                if((_touch.position.x > screenSpace &&
                EventSystem.current.IsPointerOverGameObject(_touch2.fingerId) ||
                _touch2.position.x > screenSpace &&
                EventSystem.current.IsPointerOverGameObject(_touch.fingerId))) {
                    if(timer >= 1.0f) {
                        Shoot();
                    }
                }

            }*/
        }
    }


}
