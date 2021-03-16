using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour {

    public float MoveSpeed;
    public float TurnSpeed;

    public Rigidbody rigidbody;

    void Start() {
        MoveSpeed = 5f;
        TurnSpeed = 90f;
}

    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            walking(1f, 0f);
        }
        if (Input.GetKey(KeyCode.S)) {
            walking(-1f, 0f);
        }
        if (Input.GetKey(KeyCode.A)) {
            walking(0f, -1f);
        }
        if (Input.GetKey(KeyCode.D)) {
            walking(0f, 1f);
        }
    }

    private void walking(float direction_forward, float direction_right) {
        Vector3 targetMovement = new Vector3(
            (transform.right * MoveSpeed * Time.deltaTime * direction_right).x, 0f,
            (transform.forward * MoveSpeed * Time.deltaTime * direction_forward).z);        

        rigidbody.MovePosition(rigidbody.position + targetMovement);
    }
}
