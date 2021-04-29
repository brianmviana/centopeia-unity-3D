using UnityEngine;

public class PlayerMoviment : MonoBehaviour {

    public CharacterController controller;
    
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start() {
    }


    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        /*if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        

        
        /*if (Input.GetKey(KeyCode.W)) {
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
        }*/

    }

    private void walking(float direction_forward, float direction_right) {
        
    }


}
    