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

    [SerializeField]
    private float thrusterFuelBurnSpeed = 0.1f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;

    [SerializeField]
    private float thrusterFuelMax = 2f;

    [SerializeField]
    private float thrusterFuelAmount = 1f;

    [SerializeField]
    private LayerMask enviromentMask;

    [Header("Spring sttings: ")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;
    
    void Start() {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        thrusterFuelAmount = thrusterFuelMax;

        SetJoinSettings(jointSpring);

    }

    void Update() {

        if (PauseMenu.IsOn) {
            if (Cursor.lockState != CursorLockMode.None) {
                Cursor.lockState = CursorLockMode.None;
            }
            motor.Move(Vector3.zero);
            motor.Rotation(Vector3.zero);
            motor.RotationCamera(0f);

            return;
        }
        if (Cursor.lockState != CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.Locked;
        }


        // Setting targe position for spring
        // This makes the physics act right when it comes to applying gravity when flying over objects
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, enviromentMask)) {
            joint.targetPosition = new Vector3(0f, -(_hit.point.y), 0f);
        }
        else {
            joint.targetPosition = new Vector3(0f, 0, 0f);
        }

        float _movementX = Input.GetAxis("Horizontal");
        float _movementZ = Input.GetAxis("Vertical");

        Vector3 _movementHorizontal = transform.right * _movementX;
        Vector3 _movementVertical = transform.forward * _movementZ;

        // final movement vector
        Vector3 _velocity = (_movementHorizontal + _movementVertical).normalized * speed;


        //Animator movement
        animator.SetFloat("ForwardVelocity", _movementZ);

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
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f) {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            if (thrusterFuelAmount > 0.01f) {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJoinSettings(0f);
            }
        }
        else {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJoinSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        // Apply the thruster force
        motor.ApplyThruster(_thrusterForce);

    }

    public float GetThrusterFuelAmount() {
        return thrusterFuelAmount;
    }

    private void SetJoinSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring, 
            maximumForce = jointMaxForce
        };
    }

}
