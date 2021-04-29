using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 rotationCamera = Vector3.zero;

    void Start() {
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

    public void RotationCamera(Vector3 _rotationCamera) {
        rotationCamera = _rotationCamera;
    }

    private void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

    private void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {
            cam.transform.Rotate(rotationCamera);
        }
    }


}
