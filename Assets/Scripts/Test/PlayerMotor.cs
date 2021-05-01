using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float cameraCurrentRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimite = 85f;

    private void Start() {
        rb = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate() {
        PerformMovement();
        PerformRotation();
    }


    public void Move(Vector3 _velocity) {
        velocity = _velocity;
    }

    public void Rotation(Vector3 _rotation) {
        rotation = _rotation;
    }

    public void RotationCamera(float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }


    public void ApplyThruster(Vector3 _thrusterForce) {
        thrusterForce = _thrusterForce;

    }

    private void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }

        if (thrusterForce != Vector3.zero) {
            rb.AddForce(thrusterForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {

            //Set our rotation and clamp it
            cameraCurrentRotationX -= cameraRotationX;
            cameraCurrentRotationX = Mathf.Clamp(cameraCurrentRotationX, -cameraRotationLimite, cameraRotationLimite);

            //Apply our rotation to the tranform of our camera
            cam.transform.localEulerAngles = new Vector3(cameraCurrentRotationX, 0f, 0f);

        }
    }

}
