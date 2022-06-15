using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    //Vector3 movement;
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    private float gravity = -9.8f;
    public float jumpSpeed = 15.0f;
    public float terminalVel = -10.0f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;

    private float _vertSpeed;
    private CharacterController _characterController;
    private ControllerColliderHit _contact;
    private Animator _animator;

    private void Start() {
        _vertSpeed = minFall;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
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
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        
        bool hitGround = false;
        RaycastHit hit;
        if(_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check = (_characterController.height + _characterController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }


        if(hitGround) {
            if(Input.GetButtonDown("Jump")) {
                _vertSpeed = jumpSpeed;
            }
            else {
                _vertSpeed = -0.1f;
                _animator.SetBool("Jumping", false);
            }
        }
        else {
            _vertSpeed += gravity *5 * Time.deltaTime;
            if(_vertSpeed < terminalVel) {
                _vertSpeed = terminalVel;
            }

            if(_contact != null) {
                _animator.SetBool("Jumping", true);
            }

            if(_characterController.isGrounded) {
                if(Vector3.Dot(movement, _contact.normal) < 0) {
                    movement = _contact.normal * moveSpeed;
                }
                else {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }

        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _characterController.Move(movement);

    }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic) {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
