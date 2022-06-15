using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Transform Tower;
    public float rotSpeed = 15.0f;
    public float moveSpeed;
    private float gravity = -9.8f;
    
    public float terminalVel = -10.0f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;

    public float deceleration = 20.0f;
    public float targetBuffer = 1.5f;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private float _vertSpeed;
    private CharacterController _characterController;
    private ControllerColliderHit _contact;
    private float timer;
    private Touch _touch;
    private Touch _touch2;
    private int screenSpace;

    private void Start() {
        _vertSpeed = minFall;
        _characterController = GetComponent<CharacterController>();
        moveSpeed = Managers.Player.speed;
    }
    void Update() {

        Vector3 movement = Vector3.zero;
        float horInput = joystick.Direction.x;
        float verInput = joystick.Direction.y;

        if(horInput != 0 || verInput != 0) {

            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;


            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        movement.y = gravity;
        movement *= Time.deltaTime;
        _characterController.Move(movement);

    }
    
}
