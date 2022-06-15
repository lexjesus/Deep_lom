using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform Dulo;
    [SerializeField] private Text debug;

    public float rotSpeed = 1.5f;

    private Vector3 firstPoint;
    private Vector3 secondPoint;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;
    private Touch _touch;
    private Touch _touch2;
    private Vector3 _targetPos;
    private int _touchCount;
    private int screenSpace;
    void Start()
    {

        _rotY = transform.eulerAngles.x;
        _rotX = transform.eulerAngles.y;
        _offset = target.position - transform.position;
        _targetPos = target.position;
        screenSpace = Screen.width / 3;
        //targetTower = GameObject.Find("Tower").transform;
    }

    private void LateUpdate() {
        _touchCount = Input.touchCount;
       
        
        if(_touchCount <= 2 ) {
            _touch = Input.GetTouch(0);
            
            if(_touchCount == 2) {
                _touch2 = Input.GetTouch(1);
                if((_touch.phase == TouchPhase.Stationary && EventSystem.current.IsPointerOverGameObject(_touch.fingerId)) &&
                (_touch2.phase == TouchPhase.Moved && _touch2.position.x > screenSpace) ||
                ((_touch.phase == TouchPhase.Moved && EventSystem.current.IsPointerOverGameObject(_touch.fingerId)) &&
                (_touch2.phase == TouchPhase.Moved && _touch2.position.x > screenSpace))) {
                    _rotX -= _touch2.deltaPosition.y;
                    _rotX = Mathf.Clamp(_rotX, -60f, 60f);
                    _rotY += _touch2.deltaPosition.x;
                }
                if((_touch2.phase == TouchPhase.Stationary && EventSystem.current.IsPointerOverGameObject(_touch2.fingerId)) &&
                (_touch.phase == TouchPhase.Moved && _touch.position.x > screenSpace) ||
                ((_touch2.phase == TouchPhase.Moved && EventSystem.current.IsPointerOverGameObject(_touch2.fingerId)) &&
                (_touch.phase == TouchPhase.Moved && _touch.position.x > screenSpace))) {
                    _rotX -= _touch.deltaPosition.y;
                    _rotX = Mathf.Clamp(_rotX, -60f, 60f);
                    _rotY += _touch.deltaPosition.x;
                }
            }
            if(_touchCount == 1) {
                if(_touch.phase == TouchPhase.Moved && _touch.position.x > screenSpace) {
                    _rotX -= _touch.deltaPosition.y;
                    _rotX = Mathf.Clamp(_rotX, -60f, 60f);
                    _rotY += _touch.deltaPosition.x;
                }
            }

            Quaternion rotation = Quaternion.Euler(_rotX * 0.5f, _rotY * 0.5f, 0);
            //transform.rotation = rotation;
            //Dulo.rotation = Quaternion.Euler(0, transform.rotation.y + 180, 0);
            Dulo.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
            transform.position = target.position - (rotation * _offset);
            transform.LookAt(target);
        }
        
    }
    
    
}
