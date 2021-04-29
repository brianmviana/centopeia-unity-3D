using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float mouseSensitivy = 5f;


    private PlayerMotor motor;

    void Start() {
        motor = GetComponent<PlayerMotor>();    
    }

    void Update() {
        float _movementX = Input.GetAxisRaw("Horizontal");
        float _movementZ = Input.GetAxisRaw("Vertical");

        Vector3 _movementHorizontal = transform.right * _movementX;
        Vector3 _movementVertical = transform.forward * _movementZ;

        // final movement vector
        Vector3 _velocity = (_movementHorizontal + _movementVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);


        //Calculate rotation as a 3D vector
        float _rotationY = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _rotationY, 0f) * mouseSensitivy;

        //Apply rotation
        motor.Rotation(_rotation);

        //Calculate rotation as a 3D vector
        float _rotationX = Input.GetAxisRaw("Mouse Y");
        Vector3 _rotationCamera = new Vector3(_rotationX * -1f, 0f, 0f) * mouseSensitivy;

        //Apply rotation
        motor.RotationCamera(_rotationCamera);

    }

}
