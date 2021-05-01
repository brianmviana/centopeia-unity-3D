using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float mouseSensitivy = 10f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring sttings:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJoinSettings(jointSpring);

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
        float _cameraRotationX = _rotationX * mouseSensitivy;

        //Apply rotation
        motor.RotationCamera(_cameraRotationX);

        //Calculate the thruster force;
        Vector3 _thusterForce = Vector3.zero;

        if (Input.GetButton("Jump")) {
            _thusterForce = Vector3.up * thrusterForce;
            SetJoinSettings(0f);
        }
        else {
            SetJoinSettings(jointSpring);
        }

        // Apply the thruster force
        motor.ApplyThruster(_thusterForce);

    }

    private void SetJoinSettings(float _jointSpring) {
        joint.yDrive = new JointDrive { 
            mode = jointMode, 
            positionSpring = _jointSpring, 
            maximumForce = jointMaxForce
        };
    }

}
